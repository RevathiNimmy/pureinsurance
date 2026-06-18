Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm.Focus()
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main interface
    '
    ' Edit History:
    ' RAW 08/07/2003 : CQ1335 : enable copy button for child screens that can be used by more than one parent (associated client, disclosure, peril)
    ' RKS 29/04/2005 : 354 - Standard Wording Control Enchancements
    ' RKS 11/05/2006 : PN28334 fix, Added ScrollBars to view screen design in any resolution
    ' ***************************************************************** '


    Private WithEvents m_frmSetListColumnOrder As frmSetListColumnOrder

    Private Const vbFormCode As Integer = 0
    Event closeStatusWindow()
    Event ChangeStatusMessage(ByRef sMsg As String)

    Private m_lSetTabOrderCount As Integer
    Public m_lSetTabOrderIndex As Integer



    Private formatLastValue As gPMConstants.PMEFormatStyle = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'maximum tabs per screen
    Private Const maxTabs As Integer = 10

    Private Const textBoxMinimumHeight As Integer = 285 ' CInt((285 / 15))
    Private Const textBoxMinimumWidth As Integer = 1695 'CInt((1695 / 15))

    'non database controls
    Private Const nonDatabaseElements As String = "Non Database Elements"
    Private Const freeFormatText As String = "Free Format Text"
    Private Const hyperlink As String = "Hyperlink"
    Private Const FindControl As String = "Find Control"

    'rule editor modes
    Private Const RuleEditorDefaults As Integer = 1
    Private Const RuleEditorDynamicValidation As Integer = 2
    Private Const RuleEditorPostSaveValidation As Integer = 3
    Private Const RuleEditorRisk As Integer = 4
    Private Const RuleEditorUnderwritingAuithorityLimits As Integer = 5

    Private Const cLabelNamePrefix As String = "Label."

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_lInitialControlsCount As Integer

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}

    Private m_vScreenHeader(,) As Object
    Private m_vScreenDetails(,) As Object
    Private m_vChildScreenDetails As Object

    Private CompiledRulesObject As String
    Private CompiledDefaultRuleClassName As String = "Defaults"
    Private CompiledValidationRuleClassName As String = "Validation"
    Private CompiledRules As Boolean = False

    Private m_sRuleFileName As String
    Private m_oCompiledRuleClassNameDefaults As Object
    Private m_oCompiledRuleClassNameValidation As Object

    Private Sub Ctx_mnuTab_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Ctx_mnuTab.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                cmdCancel.Focus()
                cmdCancel.PerformClick()
        End Select
    End Sub

    Private m_vInUse(GISDataModelType.GISDMTypeLast) As Object

    Private m_lGISDMType As Integer 'indicates which default/child DM type we are dealing with

    Private m_bMoving As Boolean

    Private Const TBD As Integer = 1


    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUMaintainScreenData.General

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRMaintainScreenData.Business
    'Private m_oBusiness As bSIRMaintainScreenData.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_lScreenId As Integer
    Private m_sScreenDesc As String = ""
    Private m_lScreenType As Integer
    Private ScreenCode As String = ""
    Private m_lGISObjectId As Integer
    Private m_lGISDataModelTypeId As Integer

    Private m_lGISDataModelId(GISDataModelType.GISDMTypeLast) As Integer
    Private m_sGISDataModelCode(GISDataModelType.GISDMTypeLast) As String

    Private m_lParentId As Integer

    Private m_sFrame As String = ""
    Private m_lTag As Integer

    Private m_iSourceId As Integer

    Private m_oNode As TreeNode

    Private lblmousedown As Integer
    Private m_lTextIndex As Integer
    Private m_lCheckIndex As Integer
    Private m_lComboIndex As Integer
    Private m_lFrameIndex As Integer
    Private m_lPanelIndex As Integer
    Private m_lCommandIndex As Integer
    Private m_lListViewIndex As Integer
    Private m_lStandardWordingIndex As Integer
    Private m_lSumInsuredIndex As Integer
    Private m_lAddressIndex As Integer
    Private m_lTabIndex As Integer
    Private m_lFindControlIndex As Integer
    Private m_lDateIndex As Integer

    Private m_lIndex As Integer

    Private m_lRight As Integer
    Private m_lBottom As Integer
    Private m_lHeight As Integer
    Private m_lWidth As Integer

    Private m_lMinimumWidth As Integer
    Private m_lMinimumHeight As Integer

    Private m_lListViewInThisFrame As Integer
    Private m_lStandardWordingInThisFrame As Integer
    Private m_lSumInsuredInThisFrame As Integer
    Private m_lAddressInThisFrame As Integer
    Private m_lClaimReserveInThisFrame As Boolean
    Private m_lClaimPaymentInThisFrame As Boolean
    Private m_lClaimPerilInThisFrame As Boolean
    Private m_lCaseClaimListInThisFrame As Boolean
    Private m_lCaseClaimHeaderInThisFrame As Boolean

    Private m_vTabArray(,) As Object
    Private m_vFrameArray(,) As Object
    Private m_vComboArray(,) As Object
    Private m_vTextArray(,) As Object
    Private m_vCheckArray(,) As Object
    Private m_vCommandArray(,) As Object
    Private m_vListViewArray(,) As Object
    Private m_vStandardWordingArray(,) As Object
    Private m_vSumInsuredArray(,) As Object
    Private m_vAddressArray(,) As Object
    Private m_vFindControlArray(,) As Object

    Private m_vGenericClickArray As Object
    Private m_vDateArray(,) As Object 'Swift calendar control

    Private m_sHelpText As String = ""
    Private m_lPreQuote As Integer
    Private m_lPostQuote As Integer
    Private m_lPurchase As Integer
    Private m_lIsValuation As Integer
    Private m_lIsRateAndPremium As Integer
    Private m_lIncludeInList As Integer
    Private m_lPMFormat As Object

    Private m_sFrameName As String = ""

    Private m_sRulePath As String = ""
    Private bIsCompileRuleEnabled As Boolean = False
    Private sAssemblyName As String = String.Empty

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_iSnapToGrid As Integer '0= no span 1=snap

    'moved from PBRiskScreenCommon
    Private g_vDataDictionary(GISDataModelType.GISDMTypeLast) As Object
    Private g_vScreenValues() As Object

    Private lVScrollMultiplier As Integer
    Private lHScrollMultiplier As Integer
    'Developer Guide No.181 
    Private bHasControl As Boolean = False

    Private m_cControlName As Object
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
    End Property
    Public WriteOnly Property ScreenType() As Integer
        Set(ByVal Value As Integer)
            m_lScreenType = Value
        End Set
    End Property

    Public WriteOnly Property GISDMType() As Integer
        Set(ByVal Value As Integer)
            m_lGISDMType = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}

    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property ScreenDesc() As String
        Get
            Return m_sScreenDesc
        End Get
        Set(ByVal Value As String)
            m_sScreenDesc = Value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property

    Public Property GISDataModelId() As Integer
        Get
            Return m_lGISDataModelId(GISDataModelType.GISDMTypeRisk)
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelId(GISDataModelType.GISDMTypeRisk) = Value
        End Set
    End Property

    Public Property GISDataModelCode() As String
        Get
            Return m_sGISDataModelCode(GISDataModelType.GISDMTypeRisk)
        End Get
        Set(ByVal Value As String)
            m_sGISDataModelCode(GISDataModelType.GISDMTypeRisk) = Value
        End Set
    End Property

    Public Property GISObjectId() As Integer
        Get
            Return m_lGISObjectId
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectId = Value
        End Set
    End Property

    Public Property ParentId() As Integer
        Get
            Return m_lParentId
        End Get
        Set(ByVal Value As Integer)
            m_lParentId = Value
        End Set
    End Property

    Public Property IsCompileRuleEnabled() As Boolean
        Get
            Return bIsCompileRuleEnabled
        End Get
        Set(ByVal value As Boolean)
            bIsCompileRuleEnabled = value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtScreenCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Error checking
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Error checking
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' Get an instance of the business object via
            ' the public object manager.
            m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRMaintainScreenData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}


            m_oBusiness.ScreenId = m_lScreenId


            m_oBusiness.SourceId = m_iSourceId


            m_oBusiness.GISDataModelId = m_lGISDataModelId(GISDataModelType.GISDMTypeRisk)


            m_oBusiness.GISDataModelCode = m_sGISDataModelCode(GISDataModelType.GISDMTypeRisk)


            m_oBusiness.GISObjectId = m_lGISObjectId

            If m_lGISDMType = 0 Then
                m_lGISDMType = GISDataModelType.GISDMTypeRisk
            End If

            'get the risk related data model details and the screen details

            m_lReturn = m_oBusiness.GetDetails(r_vDataDictionary:=g_vDataDictionary(m_lGISDMType), r_vScreenHeader:=m_vScreenHeader, r_vScreenDetails:=m_vScreenDetails, r_vChildScreenDetails:=m_vChildScreenDetails)


            If Not (m_vScreenDetails Is Nothing) Then
                TwipsToPixcelFormat(m_vScreenDetails, "DETAILS")

            End If

            If Not (m_vScreenHeader Is Nothing) Then
                TwipsToPixcelFormat(m_vScreenHeader, "HEADER")

            End If
            If Not (m_vChildScreenDetails Is Nothing) Then
                TwipsToPixcelFormat(m_vChildScreenDetails, "DETAILS")
            End If



            m_sGISDataModelCode(m_lGISDMType) = m_oBusiness.GISDataModelCode

            m_lGISDataModelId(m_lGISDMType) = m_oBusiness.GISDataModelId
            CompiledRules = m_oBusiness.CompiledRules


            m_lReturn = m_oBusiness.GetDataModelTypeId(v_sGISDataModelCode:=m_oBusiness.GISDataModelCode, r_lDataModelTypeId:=m_lGISDataModelTypeId)

            'CJR 17/3/2003 ISS2688 Replace Blank captions with Blank marker.
            If Information.IsArray(m_vScreenDetails) Then
                For iCnt As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)
                    If String.Compare(CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, iCnt)), "") = 0 Then
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, iCnt) = PBRiskScreenCommon2.ACBlankCaption
                    End If
                Next iCnt
            End If
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    'Don't exit, we need to terminate
                    '            Exit Function
                End If
            End If


            m_sRulePath = m_oBusiness.RulePath

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim nodTemp As TreeNode
        Dim lObjectId As Integer
        Dim sNodeTitle As String

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

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    m_lReturn = BuildScreen()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        BusinessToInterface = PMFalse
            '        Exit Function
            '    End If
            '
            '    m_lReturn = RejigControls()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        BusinessToInterface = PMFalse
            '        Exit Function
            '    End If
            '
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            Else
                If Information.IsArray(m_vScreenHeader) Then
                    txtScreenCode.Text = CStr(m_vScreenHeader(PBDatabaseConsts.ACHCode, 0))
                    txtDescription.Text = CStr(m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0))
                    ScreenCode = CStr(m_vScreenHeader(PBDatabaseConsts.ACHCode, 0))
                    IsCompileRuleEnabled = IIf(m_vScreenHeader(PBDatabaseConsts.ACHEnableCompiledRule, 0) = 3, True, False)
                    UctCompiledRuleDefaults.Text = CStr(m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0))
                    UctCompiledRuleValidation.Text = CStr(m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0))
                End If
            End If

            If IsCompileRuleEnabled Then
                chkEnableCompileRule.Checked = CheckState.Checked
                OnCompileRuleCheckChange()
            Else
                chkEnableCompileRule.Checked = CheckState.Unchecked
                OnCompileRuleCheckChange()
            End If

            'Clear it down
            tvwDataDictionary.Nodes.Clear()

            ' First node with 'Root' as text.
            If m_lGISDataModelTypeId = GISDataModelType.GISDMTypeCase Then

                sNodeTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaseDataDictionary, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                sNodeTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskDataDictionary, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If m_lGISDMType = GISDataModelType.GISDMTypeRisk Then
                nodTemp = tvwDataDictionary.Nodes.Add("r", sNodeTitle, ACOpenFolder)
                nodTemp.Collapse()
                nodTemp.Tag = CStr(-1)

                If Not Information.IsArray(g_vDataDictionary(m_lGISDMType)) Then
                    Return result
                End If


                addItemsToTreeView(tvwDataDictionary, g_vDataDictionary(m_lGISDMType), "r", m_lGISDMType)
            End If

            'if <> 0 indicates a child screen and so we don't need other models unless specifically identified

            'add the non database items
            nodTemp = tvwDataDictionary.Nodes.Add("r3", nonDatabaseElements, ACOpenFolder)
            nodTemp.Collapse()
            nodTemp.Tag = CStr(-3)

            'add the non database elements
            lObjectId = 999

            nodTemp.Tag = CStr(999)

            nodTemp = tvwDataDictionary.Nodes.Find("r3", True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000") & StringsHelper.Format(Math.Abs(PBRiskScreenCommon.ndcFreeFormatText), "000"), freeFormatText)

            nodTemp.Tag = New Object() {GISDataModelType.GISDMTypeNonDatabase, PBRiskScreenCommon.ndcFreeFormatText}

            nodTemp = tvwDataDictionary.Nodes.Find("r3", True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000") & StringsHelper.Format(Math.Abs(PBRiskScreenCommon.ndcHyperlink), "000"), hyperlink)

            nodTemp.Tag = New Object() {GISDataModelType.GISDMTypeNonDatabase, PBRiskScreenCommon.ndcHyperlink}

            'JES added for PB find control
            nodTemp = tvwDataDictionary.Nodes.Find("r3", True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000") & StringsHelper.Format(Math.Abs(PBRiskScreenCommon.ndcFindControl), "000"), FindControl)

            nodTemp.Tag = New Object() {GISDataModelType.GISDMTypeNonDatabase, PBRiskScreenCommon.ndcFindControl}

            nodTemp = Nothing


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ValidateForm()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Check mandatory properties have been added to the form.
            m_lReturn = CheckMandatoryProperties()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Start(Sriram P)CacheBug
    Public Function UpdateCache() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCache"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue




            Dim bSIRMaintainScreenData As bSIRMaintainScreenData.Business
            Dim temp_bSIRMaintainScreenData As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_bSIRMaintainScreenData, "bSIRMaintainScreenData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            bSIRMaintainScreenData = temp_bSIRMaintainScreenData

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of of the business component")
            End If


            m_lReturn = bSIRMaintainScreenData.UpdateCache(m_lScreenId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Update the Cache")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
    'End(Sriram P)CacheBug

    ' ***************************************************************** '
    '
    ' Name: UpdateBusiness
    '
    ' Description:
    '
    ' History: 13/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateBusiness() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String
        Dim bOK As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRMaintainScreenData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")

                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            m_lReturn = CheckCode(v_lScreenId:=m_lScreenId, v_sCode:=CStr(m_vScreenHeader(PBDatabaseConsts.ACHCode, 0)), r_bOK:=bOK)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If bOK Then


                    m_oBusiness.ScreenId = m_lScreenId


                    m_oBusiness.SourceId = m_iSourceId


                    m_oBusiness.GISDataModelId = m_lGISDataModelId(m_lGISDMType)


                    PixcelToTwipsFormat(m_vScreenHeader, "HEADER")
                    PixcelToTwipsFormat(m_vScreenDetails, "DETAILS")

                    m_lReturn = m_oBusiness.Update(r_vDataDictionary:=g_vDataDictionary(m_lGISDMType), r_vScreenHeader:=m_vScreenHeader, r_vScreenDetails:=m_vScreenDetails)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
                    End If


                    m_lScreenId = m_oBusiness.ScreenId
                End If
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckCode
    '
    ' Description:
    '
    ' History: 19/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckCode(ByVal v_lScreenId As Integer, ByVal v_sCode As String, ByRef r_bOK As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CheckCode(v_lScreenId:=v_lScreenId, v_sCode:=v_sCode, r_bOK:=r_bOK)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not r_bOK Then
                MessageBox.Show("This code is already in use", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ValidateForm
    '
    ' Description: Is the data on the interface ok
    '
    '
    ' ***************************************************************** '
    Public Function ValidateForm() As Integer

        Dim result As Integer = 0
        Dim bThere As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bThere = False

            If Information.IsArray(m_vFrameArray) Then
                For lTemp As Integer = m_vFrameArray.GetLowerBound(1) To m_vFrameArray.GetUpperBound(1)
                    If m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then

                        For Each ctlControl As Control In ContainerHelper.Controls(Me)
                            If ctlControl.HasChildren Then
                                bThere = GetControl(ctlControl, lTemp)
                                If bThere Then Exit For
                            End If
                            'Select Case ctlControl.Name
                            '    Case "chkYesNo", "uctStandardWording1", "cmdCommand", "cboCombo", "txtText", "uctAddress1", "lvwListView", "uctSumInsured1", "lblTextLabel", "uctClaimReserve1", "uctClaimPayment1", "uctCLMPerilDT1", "uctCLMCaseClaim1", "uctCLMCaseHeader1"
                            '        If ctlControl.Parent.Name = "fraFrame" Then
                            '            If ContainerHelper.GetControlIndex(ctlControl.Parent) = lTemp And ControlHelper.GetVisible(ctlControl) Then
                            '                bThere = True
                            '                Exit For
                            '            End If
                            '        End If
                            'End Select
                        Next ctlControl
                    End If
                    If bThere Then Exit For 'don't hang about
                Next lTemp
            End If

            If Not bThere Then
                MessageBox.Show("The screen is empty and cannot be saved", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lGISObjectId = 0 Then
                Return result
            End If

            'We're a sub screen, and it would be nice to have included something in the list...
            bThere = False

            If Information.IsArray(m_vCheckArray) Then
                For lTemp As Integer = m_vCheckArray.GetLowerBound(1) To m_vCheckArray.GetUpperBound(1)
                    If m_vCheckArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        If CDbl(m_vCheckArray(PBRiskScreenCommon.ACCIncludeInList, lTemp)) <> 0 Then
                            bThere = True
                            Exit For
                        End If
                    End If
                Next lTemp
            End If

            If bThere Then
                Return result
            End If

            If Information.IsArray(m_vComboArray) Then
                For lTemp As Integer = m_vComboArray.GetLowerBound(1) To m_vComboArray.GetUpperBound(1)
                    If m_vComboArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        If CDbl(m_vComboArray(PBRiskScreenCommon.ACCIncludeInList, lTemp)) <> 0 Then
                            bThere = True
                            Exit For
                        End If
                    End If
                Next lTemp
            End If

            If bThere Then
                Return result
            End If

            If Information.IsArray(m_vCommandArray) Then
                For lTemp As Integer = m_vCommandArray.GetLowerBound(1) To m_vCommandArray.GetUpperBound(1)
                    If m_vCommandArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        If CDbl(m_vCommandArray(PBRiskScreenCommon.ACCIncludeInList, lTemp)) <> 0 Then
                            bThere = True
                            Exit For
                        End If
                    End If
                Next lTemp
            End If

            If bThere Then
                Return result
            End If

            If Information.IsArray(m_vTextArray) Then
                For lTemp As Integer = m_vTextArray.GetLowerBound(1) To m_vTextArray.GetUpperBound(1)
                    If m_vTextArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        If CDbl(m_vTextArray(PBRiskScreenCommon.ACCIncludeInList, lTemp)) <> 0 Then
                            bThere = True
                            Exit For
                        End If
                    End If
                Next lTemp
            End If

            'if risk child then there must be some "include in list entries"
            'TR - Added check for Risk only GIS, as the other's don't need this message

            If Not bThere And (GISDataModelType.GISOTRisk = CDbl(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, 0)) Or GISDataModelType.GISOTCase = CDbl(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, 0))) Then
                MessageBox.Show("You must include at least one field in the list", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function GetControl(ByVal ctlControl As Control, ByVal lTemp As Integer) As Boolean
        Dim bThere As Boolean = False
        Dim strCtl As String() = New String() {"chkYesNo", "uctStandardWording1", "cmdCommand", "cboCombo", "txtText", "uctAddress1", "lvwListView", "uctSumInsured1", "lblTextLabel", "uctClaimReserve1", "uctClaimPayment1", "uctCLMPerilDT1", "uctCLMCaseClaim1", "uctCLMCaseHeader1"}
        Dim ctl As Control

        For i As Integer = 0 To strCtl.Length - 1
            ctl = FindControlName(ctlControl, strCtl(i).ToString())
            If Not ctl Is Nothing Then
                bThere = True
                Return bThere
            End If
        Next


        Return bThere


        'For Each ctlTabControl As Control In ctlControl.Controls
        '    'If ctlTabControl.HasChildren Then
        '    '    bThere = GetControl(ctlTabControl, lTemp)
        '    '    If (bThere) Then
        '    '        Return True
        '    '    End If
        '    'End If
        '    'reg=New System.Text.RegularExpressions.Regex(
        '    Dim ctlName As String
        '    If ctlTabControl.Name.Contains("_") Then
        '        ctlName = ctlTabControl.Name.Split("_")(1).ToString()
        '    Else
        '        ctlName = ctlTabControl.Name
        '    End If
        '    Select Case ctlName
        '        Case "chkYesNo", "uctStandardWording1", "cmdCommand", "cboCombo", "txtText", "uctAddress1", "lvwListView", "uctSumInsured1", "lblTextLabel", "uctClaimReserve1", "uctClaimPayment1", "uctCLMPerilDT1", "uctCLMCaseClaim1", "uctCLMCaseHeader1"
        '            If ctlTabControl.Parent.Name.Contains("fraFrame") Then
        '                If ContainerHelper.GetControlIndex(ctlTabControl.Parent) = lTemp And ControlHelper.GetVisible(ctlTabControl) Then
        '                    bThere = True
        '                    Return bThere
        '                End If
        '            End If
        '    End Select
        'Next ctlTabControl

        'Return bThere
    End Function
    ' ***************************************************************** '
    ' Description: To get the list of controls in the form.
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

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to display the lookup details", ACApp, ACClass, "DisplayLookupDetails", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: SpacifyName
    '
    ' Description:
    '
    ' History: 24/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function SpacifyName(ByRef sOriginal As String, ByRef sNew As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sNew = ""

            For iTemp As Integer = 1 To sOriginal.Length
                If sOriginal.Substring(iTemp - 1, 1) = "_" Then
                    sNew = sNew & " "
                Else
                    sNew = sNew & sOriginal.Substring(iTemp - 1, 1)
                End If

            Next iTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpacifyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpacifyName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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

            '    m_lReturn& = m_oBusiness.GetNext(r_vDataDictionary:=g_vDataDictionary, _
            'r_vScreenHeader:=m_vScreenHeader, _
            'r_vScreenDetails:=m_vScreenDetails, _
            'r_vChildScreenDetails:=m_vChildScreenDetails)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            For iLoop As Integer = 0 To GISDataModelType.GISDMTypeLast
                If Information.IsArray(g_vDataDictionary(iLoop)) Then


                    RedimSecondElement(m_vInUse(iLoop), g_vDataDictionary(iLoop).GetUpperBound(1))

                    For iTemp As Integer = 0 To m_vInUse(iLoop).GetUpperBound(0)

                        m_vInUse(iLoop)(iTemp) = gPMConstants.PMEReturnCode.PMFalse
                    Next iTemp
                End If
            Next
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' {* USER DEFINED CODE (End) *}

            'The plan - build up the screen header array, then the screen detail array
            'ReDim g_vDataDictionary(1, 14)

            'First the screen header
            If Not Information.IsArray(m_vScreenHeader) Then
                ReDim m_vScreenHeader(PBDatabaseConsts.ACHLastArrayPosition, 0)
                m_vScreenHeader(PBDatabaseConsts.ACHScriptDefaults, 0) = ""
                m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0) = ""
            End If
            'Developer Guide No. 57
            m_vScreenHeader(PBDatabaseConsts.ACHScreenId, 0) = m_lScreenId.ToString()
            m_vScreenHeader(PBDatabaseConsts.ACHGISDataModelId, 0) = m_lGISDataModelId(m_lGISDMType).ToString()
            m_vScreenHeader(PBDatabaseConsts.ACHCaptionId, 0) = "0"
            m_vScreenHeader(PBDatabaseConsts.ACHCode, 0) = txtScreenCode.Text.Trim().ToString()
            m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0) = txtDescription.Text.Trim().ToString()
            m_vScreenHeader(PBDatabaseConsts.ACHIsDeleted, 0) = "0"
            m_vScreenHeader(PBDatabaseConsts.ACHEffectiveDate, 0) = DateTime.Today.ToString()
            If m_lParentId = 0 Then

                m_vScreenHeader(PBDatabaseConsts.ACHParentId, 0) = DBNull.Value
            Else
                m_vScreenHeader(PBDatabaseConsts.ACHParentId, 0) = m_lParentId
            End If
            m_vScreenHeader(PBDatabaseConsts.ACHScreenType, 0) = m_lScreenType

            m_vScreenHeader(PBDatabaseConsts.ACHIsMaintainable, 0) = 1

            'Tomo151002
            m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0) = (TabStrip1.Height).ToString()
            'Developer Guide No. 74
            m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0) = (TabStrip1.Width).ToString()
            m_cControlName = PBRiskScreenCommon.g_sControlName

            m_vScreenHeader(PBDatabaseConsts.ACHEnableCompiledRule, 0) = IIf(chkEnableCompileRule.Checked, 3, 1)
            m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0) = IIf(chkEnableCompileRule.Checked, UctCompiledRuleDefaults.Text.Trim(), String.Format("{0}Def.rul", txtScreenCode.Text.Trim()))
            m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0) = IIf(chkEnableCompileRule.Checked, UctCompiledRuleValidation.Text.Trim(), String.Format("{0}Val.rul", txtScreenCode.Text.Trim()))

            m_lReturn = BuildDetailArray()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            If m_lScreenId = 0 Then
                cmdValidation.Enabled = False
                cmdDefaults.Enabled = False
                cmdCopy.Enabled = False
                cmdDynamicLogic.Enabled = False
            Else
                txtScreenCode.Enabled = False
            End If

            'Don't show copy if we're a child screen...
            ' RAW 08/07/2003 : CQ1335 : added screen type test
            ' unless it is a child screen that is not linked via the normal parent/child hierarchy and
            ' therefore excluded from CopyScreen process (eg associated client )
            If m_lParentId <> 0 Then

                Select Case m_lScreenType
                    Case GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTDisclosure, GISDataModelType.GISOTPeril
                        ' leave visible

                    Case Else
                        ' hide
                        cmdCopy.Visible = False
                End Select
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            ReDim m_ctlTabFirstLast(1, 0)

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

            m_ctlTabFirstLast(ACControlStart, 0) = tvwDataDictionary
            m_ctlTabFirstLast(ACControlEnd, 0) = tvwDataDictionary

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 55
            tabMainTab.TabPages(0).Text = iPMFunc.GetResData(g_iLanguageID, ACTabTitle1, gPMConstants.PMEResourseFileDataType.PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    lblScreen.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACLblScreen, _
            'iDataType:=PMResString)

            '    fraDataDictionary.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACFraStucture, _
            'iDataType:=PMResString)

            '    fraScreen.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACFraCommon, _
            'iDataType:=PMResString)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try


            ' Gets all of the lookup values.

            ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            ' Get all of the lookup values.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAll, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMEdit
            '            ' Get all of the lookup values with the correct
            '            ' effective date.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAllEffective, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMView
            '            ' Get lookup values for viewing only.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupSingle, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '    End Select
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetLookupValues = PMFalse
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get the lookup values from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetLookupValues"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow, lCntr As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    '
    ' Get the lookup values.
    '
    '    bFoundMatch = False
    ''
    '    For lRow& = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
    '        ' Check for a match of the table name.
    '        If (Trim$(m_vLookupValues(ACValueTableName, lRow&)) = _
    ''        Trim$(sLookupTable$)) Then
    '            ' Found a match
    '            bFoundMatch = True
    '            Exit For
    '        End If
    '    Next lRow&
    ''
    '    ' Check if there has been a table match.
    '    If (bFoundMatch = False) Then
    '        GetLookupDetails = PMFalse
    ''
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get details for the table, " & sLookupTable$, _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupDetails"
    ''
    '        Exit Function
    '    End If
    ''
    '    ' Using the lookup values, populate the control with
    '    ' the details from the lookup details array.
    ''
    '    For lCntr& = m_vLookupValues(ACValueStartPos, lRow&) To _
    ''    (m_vLookupValues(ACValueStartPos, lRow&) + m_vLookupValues(ACValueNumber, lRow&)) - 1
    '        ' Add the details to the control.
    '        ctlLookup.AddItem m_vLookupDetails(ACDetailDesc, lCntr&)
    '        ctlLookup.ItemData(ctlLookup.NewIndex) = CLng(m_vLookupDetails(ACDetailKey, lCntr&))
    ''
    '        'SP150998 - compare long value not string
    '        ' Check if this is the selected index.
    '        If (m_vLookupValues(ACValueID, lRow&) <> "") Then
    '            If (m_vLookupValues(ACValueID, lRow&) = _
    ''            CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
    '                ctlLookup.ListIndex = ctlLookup.NewIndex
    '            End If
    '        End If
    ''
    '    Next lCntr&
    ''
    '    ' Check if the selected index is blank. If so,
    '    ' we set the controls index to zero.
    '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
    '        ctlLookup.ListIndex = 0
    '    End If
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: BuildDetailArray
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function BuildDetailArray() As Integer

        Dim result As Integer = 0
        Dim lCount, lFrameCount, lTag As Integer
        Dim vFrameArray As Object
        Dim vTabArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First the tabs

            'Akash: Commented
            'm_vScreenDetails = VB6.CopyArray("")
            lCount = -1


            ReDim vTabArray(m_lTabIndex)

            For lTemp As Integer = 0 To m_lTabIndex + 1

                If PBTabStripCommon.IsTabVisible(TabStrip1, lTemp) Then
                    lCount += 1
                    If lCount = 0 Then
                        ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                    Else
                        ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                    End If

                    'If a tab has disappeared from the middle, this is the actual tab that
                    'a control on a later tab goes to...

                    vTabArray(lTemp) = lCount

                    'clear array
                    For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                        m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                    Next
                    'set other values
                    m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                    m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount
                    m_vScreenDetails(PBDatabaseConsts.ACDIsFrame, lCount) = gPMConstants.PMEReturnCode.PMFalse
                    m_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lCount) = lCount
                    'Akash: Modified the code
                    'm_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = TabStrip1.TabPages(lCount + 1)._ObjectDefault
                    m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = TabStrip1.TabPages(lCount).Text
                    m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lCount) = m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, lTemp)
                    m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = 0
                End If
            Next lTemp

            If Not Information.IsArray(m_vScreenDetails) Then
                'Nothing to do
                Return result
            End If

            'Then the frames

            ReDim vFrameArray(m_lFrameIndex)
            lFrameCount = -1

            If Information.IsArray(m_vFrameArray) Then
                For lTemp As Integer = m_vFrameArray.GetLowerBound(1) To m_vFrameArray.GetUpperBound(1)

                    If m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        lCount += 1
                        lFrameCount += 1
                        If lCount = 0 Then
                            ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        Else
                            ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        End If


                        vFrameArray(lTemp) = lFrameCount

                        'clear array
                        For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                            m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                        Next
                        'set other values
                        m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                        m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                        'Developer Guide No. 57 
                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = IIf(IsNothing(m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, lTemp)) OrElse m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, lTemp).ToString() = "", DBNull.Value, m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, lTemp))
                        m_vScreenDetails(PBDatabaseConsts.ACDIsFrame, lCount) = gPMConstants.PMEReturnCode.PMTrue


                        'Developer Guide No. 65
                        m_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lCount) = vTabArray(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lTemp))
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = fraFrame(lTemp).Text

                        If TypeOf (fraFrame(lTemp).Parent) Is GroupBox Then
                            m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (fraFrame(lTemp).Top)
                            m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (fraFrame(lTemp).Left)
                        Else
                            m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (fraFrame(lTemp).Top) + Math.Round(330 / 15)
                            m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (fraFrame(lTemp).Left) + Math.Round(90 / 15)

                        End If


                        If CDbl(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount)) < 0 Then
                            m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = CDbl(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount)) + Math.Round(75000 / 15)
                        End If
                        m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (fraFrame(lTemp).Height)
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (fraFrame(lTemp).Width)
                        If Not (Convert.IsDBNull(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp)) Or IsNothing(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp))) Then
                            m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp)
                        End If
                        m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFChildId, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, lTemp)

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(m_vFrameArray(PBRiskScreenCommon.ACFDataModelType, lTemp)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = CDbl(m_vFrameArray(PBRiskScreenCommon.ACFDataModelType, lTemp)) \ 10000
                        Else
                            '                    m_vScreenDetails(ACDDataModelType, lCount) = GISDMTypeRisk
                        End If
                    End If
                Next lTemp
            End If

            'Then the controls

            BuildDetail(lCount, vFrameArray, m_vTextArray, txtText, lblTextLabel) 'text
            'TBD BuildDetail lCount, vFrameArray, m_vDateArray, txtDate, lblDateLabel 'date

            BuildDetail(lCount, vFrameArray, m_vComboArray, cboCombo, lblComboLabel) 'combos

            BuildDetail(lCount, vFrameArray, m_vCheckArray, chkYesNo, lblCheckLabel) 'check boxes

            BuildDetail(lCount, vFrameArray, m_vCommandArray, cmdCommand, cmdCommand) 'Commands


            'Then the listviews
            'No need to bother - it's all handled in the frame.

            'Then the specials

            'First the address,
            If Information.IsArray(m_vAddressArray) Then
                For lTemp As Integer = m_vAddressArray.GetLowerBound(1) To m_vAddressArray.GetUpperBound(1)
                    If m_vAddressArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then

                        lCount += 1
                        If lCount = 0 Then
                            ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        Else
                            ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        End If

                        lTag = CInt(Convert.ToString(uctAddress1(lTemp).Tag))

                        'clear array
                        For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                            m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                        Next
                        'set other values
                        m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                        m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                        m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctAddress1(lTemp).Top)
                        m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctAddress1(lTemp).Left)
                        m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctAddress1(lTemp).Height)
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctAddress1(lTemp).Width)
                        m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctAddress1(lTemp).Parent))
                        m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""
                        m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vAddressArray(PBRiskScreenCommon.ACCTabSetIndex, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
                    End If
                Next lTemp
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(Convert.ToString(uctCLMPerilDT1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                lCount += 1
                If lCount = 0 Then
                    ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                Else
                    ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                End If

                lTag = CInt(Convert.ToString(uctCLMPerilDT1.Tag))

                'clear array
                For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                    m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                Next
                'set other values
                m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOGISObjectId, lTag Mod 10000)

                m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = DBNull.Value
                m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctCLMPerilDT1.Top)
                m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctCLMPerilDT1.Left)
                m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctCLMPerilDT1.Height)
                m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctCLMPerilDT1.Width)
                m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctCLMPerilDT1.Parent))
                m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctCLMPerilDT1.Parent))))
                m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
            End If


            'Then the standard wording
            If Information.IsArray(m_vStandardWordingArray) Then
                For lTemp As Integer = m_vStandardWordingArray.GetLowerBound(1) To m_vStandardWordingArray.GetUpperBound(1)
                    If m_vStandardWordingArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then

                        lCount += 1
                        If lCount = 0 Then
                            ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        Else
                            ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        End If

                        lTag = CInt(Convert.ToString(uctStandardWording1(lTemp).Tag))

                        'clear array
                        For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                            m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                        Next
                        'set other values
                        m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                        m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                        m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctStandardWording1(lTemp).Top)
                        m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctStandardWording1(lTemp).Left)
                        m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctStandardWording1(lTemp).Height)
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctStandardWording1(lTemp).Width)
                        m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctStandardWording1(lTemp).Parent))
                        m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                        m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctStandardWording1(lTemp).Parent))))
                        m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
                    End If
                Next lTemp
            End If

            '------------------------------------------------------------------------------
            '   12/07/2002 RVH  BEGIN
            '                   New claims specials : Reserve and Payment
            '------------------------------------------------------------------------------
            'Then the claim reserve
            Dim dbNumericTemp3 As Double
            If Double.TryParse(Convert.ToString(uctClaimReserve1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                lCount += 1
                If lCount = 0 Then
                    ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                Else
                    ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                End If

                lTag = CInt(Convert.ToString(uctClaimReserve1.Tag))

                'clear array
                For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                    m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                Next
                'set other values
                m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctClaimReserve1.Top)
                m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctClaimReserve1.Left)
                m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctClaimReserve1.Height)
                m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctClaimReserve1.Width)
                m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctClaimReserve1.Parent))
                m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctClaimReserve1.Parent))))
                m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
            End If

            'Then the claim payment
            Dim dbNumericTemp4 As Double
            If Double.TryParse(Convert.ToString(uctClaimPayment1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                lCount += 1
                If lCount = 0 Then
                    ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                Else
                    ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                End If

                lTag = CInt(Convert.ToString(uctClaimPayment1.Tag))

                'clear array
                For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                    m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                Next
                'set other values
                m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctClaimPayment1.Top)
                m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctClaimPayment1.Left)
                m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctClaimPayment1.Height)
                m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctClaimPayment1.Width)
                m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctClaimPayment1.Parent))
                m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctClaimPayment1.Parent))))
                m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
            End If

            'Then the Case Header
            Dim dbNumericTemp5 As Double
            If Double.TryParse(Convert.ToString(uctCLMCaseHeader1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                lCount += 1
                If lCount = 0 Then
                    ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                Else
                    ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                End If

                lTag = CInt(Convert.ToString(uctCLMCaseHeader1.Tag))

                'clear array
                For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                    m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                Next
                'set other values
                m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctCLMCaseHeader1.Top)
                m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctCLMCaseHeader1.Left)
                m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctCLMCaseHeader1.Height)
                m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctCLMCaseHeader1.Width)
                m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctCLMCaseHeader1.Parent))
                m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctCLMCaseHeader1.Parent))))
                m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
            End If


            'Then the Case claim List
            Dim dbNumericTemp6 As Double
            If Double.TryParse(Convert.ToString(uctCLMCaseClaim1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                lCount += 1
                If lCount = 0 Then
                    ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                Else
                    ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                End If

                lTag = CInt(Convert.ToString(uctCLMCaseClaim1.Tag))

                'clear array
                For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                    m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                Next
                'set other values
                m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctCLMCaseClaim1.Top)
                m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctCLMCaseClaim1.Left)
                m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctCLMCaseClaim1.Height)
                m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctCLMCaseClaim1.Width)
                m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(ContainerHelper.GetControlIndex(uctCLMCaseClaim1.Parent))
                m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = ""

                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(vFrameArray(ContainerHelper.GetControlIndex(uctCLMCaseClaim1.Parent))))
                m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
            End If
            '------------------------------------------------------------------------------
            '   12/07/2002 RVH  END
            '------------------------------------------------------------------------------

            'Finally the sums insured

            If Information.IsArray(m_vSumInsuredArray) Then
                For lTemp As Integer = m_vSumInsuredArray.GetLowerBound(1) To m_vSumInsuredArray.GetUpperBound(1)
                    If m_vSumInsuredArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        lCount += 1
                        If lCount = 0 Then
                            ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        Else
                            ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        End If

                        lTag = CInt(Convert.ToString(uctSumInsured1(lTemp).Tag))

                        'clear array
                        For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                            m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                        Next
                        'set other values
                        m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                        m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = ""
                        m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (uctSumInsured1(lTemp).Top)
                        m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (uctSumInsured1(lTemp).Left)
                        m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (uctSumInsured1(lTemp).Height)
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (uctSumInsured1(lTemp).Width)
                        m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(m_vSumInsuredArray(PBRiskScreenCommon.ACCFrameNumber, lTemp))
                        m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = m_vSumInsuredArray(PBRiskScreenCommon.ACCHelpText, lTemp)
                        If CBool(m_vSumInsuredArray(PBRiskScreenCommon.ACCIsValuation, lTemp)) Then
                            m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lCount) = gPMConstants.PMEReturnCode.PMTrue
                        Else
                            m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lCount) = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If CBool(m_vSumInsuredArray(PBRiskScreenCommon.ACCIsRateAndPremium, lTemp)) Then
                            m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lCount) = gPMConstants.PMEReturnCode.PMTrue
                        Else
                            m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lCount) = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, CInt(m_vSumInsuredArray(PBRiskScreenCommon.ACCFrameNumber, lTemp)))
                        m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
                    End If
                Next lTemp
            End If

            'FindControl
            If Information.IsArray(m_vFindControlArray) Then
                For lTemp As Integer = m_vFindControlArray.GetLowerBound(1) To m_vFindControlArray.GetUpperBound(1)
                    If m_vFindControlArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        lCount += 1
                        If lCount = 0 Then
                            ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        Else
                            ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, lCount)
                        End If

                        lTag = CInt(Convert.ToString(PBFindControl(lTemp).Tag))

                        'clear array
                        For iLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                            m_vScreenDetails(iLoopCount, lCount) = DBNull.Value
                        Next
                        'set other values
                        m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId
                        m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount) = lCount

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount) = DBNull.Value
                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount) = PBRiskScreenCommon.ndcFindControl
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount) = "Non Database Elements"
                        m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount) = (PBFindControl(lTemp).Top)
                        m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount) = (PBFindControl(lTemp).Left)
                        m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount) = (PBFindControl(lTemp).Height)
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount) = (PBFindControl(lTemp).Width)
                        m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lCount) = PBFindControl(lTemp).FindControlID 'store control id in PMFormat !
                        m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCPreQuote, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCPostQuote, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCPurchase, lTemp)

                        m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount) = vFrameArray(m_vFindControlArray(PBRiskScreenCommon.ACCFrameNumber, lTemp))
                        m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCHelpText, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCColumnPosition, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = m_vFindControlArray(PBRiskScreenCommon.ACCTabSetIndex, lTemp)
                        m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount) = lTag \ 10000
                    End If
                Next lTemp
            End If

            vTabArray = Nothing
            vFrameArray = Nothing

            Return result

        Catch excep As System.Exception



            '    Resume
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildDetailArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
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
    'VB6.TwipsToPixcel is removed 

    Private Function BuildScreen() As Integer

        Dim result As Integer = 0
        Dim lTag As Integer

        Dim sPropertyName, sObjectName As String
        Dim iDataType As Integer
        Dim bIsSpecial As Boolean
        Dim sColumnName As String = ""
        Dim lErrorCount As Integer
        Dim dtTimer As Date


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vScreenHeader) Or Not Information.IsArray(m_vTabArray) Then
                'if new screen create array for maximum amount of tabs
                ReDim m_vTabArray(PBRiskScreenCommon.ACFLastArrayPosition, 0)
                m_lTabIndex = 0
                m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, m_lTabIndex) = 0 'clear snap to grid
                If Not Information.IsArray(m_vScreenHeader) Then Return result
            End If

            m_sScreenDesc = CStr(m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0))

            'Tomo151002
            picScreen.Height = (IIf(CDbl(m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0)) > 0, CDbl(m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0)), VB6.TwipsToPixelsY(6000)))
            picScreen.Width = (IIf(CDbl(m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0)) > 0, CDbl(m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0)), VB6.TwipsToPixelsX(9015)))

            If Not Information.IsArray(m_vScreenDetails) Then
                Return result
            End If

            m_sFrameName = ""

            'Because of the way the data is written out, we can do this quite easily
            'First we've the tabs
            'Then the frames
            'Then the controls
            'Finally the specials

            Dim bSequenceControls As Boolean
            For lTemp As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)

                'do some fixups before continuing

                If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)) Then
                    m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp) = ""
                End If


                m_lPMFormat = m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lTemp)

                'This is null for tabs and frames and non DB bound controls

                'Developer Guide No. 56
                If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp)) Or String.IsNullOrEmpty(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp)) Then
                    If m_vScreenDetails(PBDatabaseConsts.ACDIsFrame, lTemp) <> Nothing AndAlso m_vScreenDetails(PBDatabaseConsts.ACDIsFrame, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                        'PMFalse for a tab
                        PBRiskScreenCommon2.addTabControl(m_lTabIndex, m_vTabArray, m_vScreenDetails, lTemp, TabStrip1)
                        'Developer Guide No. 74
                        TabStrip1.Width = picScreen.Width
                        'Developer Guide No. 59
                        TabStrip1.Controls(m_lTabIndex).AllowDrop = True
                        AddHandler TabStrip1.Controls(m_lTabIndex).DragDrop, AddressOf TabStrip1_DragDrop
                        AddHandler TabStrip1.Controls(m_lTabIndex).DragEnter, AddressOf TabStrip1_DragEnter
                    Else
                        'So it's a frame or a special
                        bIsSpecial = False

                        'Developer Guide No.56 
                        If Not (Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)) Or String.IsNullOrEmpty(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp))) Then

                            'Developer Guide No.56  
                            m_lReturn = PBRiskScreenCommon2.GetTag(CInt(IIf(String.IsNullOrEmpty(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)), 0, m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp))), -1, lTag, g_vDataDictionary(GISDataModelType.GISDMTypeRisk))


                            If CDbl(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, lTag)) = GISDataModelType.GISOTPeril Then
                                'OK, so here it's a claim payment control

                                m_lReturn = AddClaimPeril(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), sPropertyName, False, sObjectName, sPropertyName, , m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))
                                bIsSpecial = True
                            End If

                        End If
                        If Not bIsSpecial Then


                            If m_lFrameIndex >= 0 Then
                                ContainerHelper.LoadControl(Me, "fraFrame", m_lFrameIndex + 1, True)

                            End If

                            PBRiskScreenCommon2.addFrameControl(m_lFrameIndex, m_vFrameArray, m_vScreenDetails, lTemp, fraFrame)
                            If m_lFrameIndex = 0 Then
                                AddHandler fraFrame(m_lFrameIndex).MouseUp, AddressOf fraFrame_MouseUp
                                AddHandler fraFrame(m_lFrameIndex).MouseMove, AddressOf fraFrame_MouseMove
                                AddHandler fraFrame(m_lFrameIndex).MouseDown, AddressOf fraFrame_MouseDown
                                AddHandler fraFrame(m_lFrameIndex).DragEnter, AddressOf fraFrame_DragEnter
                                AddHandler fraFrame(m_lFrameIndex).DragDrop, AddressOf fraFrame_DragDrop
                            End If
                            If IsDBNull(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex)) Then
                                TabStrip1.TabPages(CInt(m_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lTemp))).Controls.Add(fraFrame(m_lFrameIndex))
                            End If

                            'Developer Guide No. 56
                            If Not (Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)) Or String.IsNullOrEmpty(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp))) Then
                                m_lListViewIndex += 1
                                If m_lListViewIndex > 0 Then

                                    ContainerHelper.LoadControl(Me, "lvwListView", m_lListViewIndex, True)
                                    ContainerHelper.LoadControl(Me, "cmdListViewAdd", m_lListViewIndex, True)
                                    ContainerHelper.LoadControl(Me, "cmdListViewEdit", m_lListViewIndex, True)
                                    ContainerHelper.LoadControl(Me, "cmdListViewDelete", m_lListViewIndex, True)
                                    ContainerHelper.LoadControl(Me, "cmdListViewSequenceUp", m_lListViewIndex, True)
                                    ContainerHelper.LoadControl(Me, "cmdListViewSequenceDown", m_lListViewIndex, True)

                                    ReDim Preserve m_vListViewArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lListViewIndex)
                                Else
                                    ReDim m_vListViewArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lListViewIndex)
                                End If

                                m_vFrameArray(PBRiskScreenCommon.ACFChildId, m_lFrameIndex) = m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)
                                m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, m_lFrameIndex) = m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)
                                m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, m_lFrameIndex) = m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp)

                                lvwListView(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)

                                m_vListViewArray(PBRiskScreenCommon.ACCFrameNumber, m_lListViewIndex) = m_lFrameIndex
                                m_vListViewArray(PBRiskScreenCommon.ACCIsDeleted, m_lListViewIndex) = gPMConstants.PMEReturnCode.PMFalse
                                m_vListViewArray(PBRiskScreenCommon.ACCHelpText, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCPreQuote, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCPostQuote, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCPurchase, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCIsValuation, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp)

                                m_vListViewArray(PBRiskScreenCommon.ACCIncludeInList, m_lListViewIndex) = DBNull.Value
                                m_vListViewArray(PBRiskScreenCommon.ACCChildId, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCGISObjectId, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCPMFormat, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lTemp)
                                m_vListViewArray(PBRiskScreenCommon.ACCTabSetIndex, m_lListViewIndex) = m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, m_lFrameIndex)


                                m_lReturn = PopulateListView(lListViewIndex:=m_lListViewIndex, lChildId:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)), bClear:=True, lvwListView:=lvwListView, m_vChildScreenDetails:=m_vChildScreenDetails, r_bSequenceControls:=bSequenceControls, cToolTip:=ToolTip1)


                                lvwListView(m_lListViewIndex).Top = VB6.TwipsToPixelsY(240)


                                lvwListView(m_lListViewIndex).Left = VB6.TwipsToPixelsX(120)
                                lvwListView(m_lListViewIndex).Width = lvwListView(m_lListViewIndex).Parent.Width - VB6.TwipsToPixelsX(240)
                                lvwListView(m_lListViewIndex).Height = lvwListView(m_lListViewIndex).Parent.Height - VB6.TwipsToPixelsY(720)
                                lvwListView(m_lListViewIndex).Visible = True


                                'if this list view has a sequence column then add the sequence buttons
                                If bSequenceControls Then
                                    lvwListView(m_lListViewIndex).Width = fraFrame(m_lFrameIndex).Width - VB6.TwipsToPixelsX(795)
                                    cmdListViewSequenceUp(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Top
                                    cmdListViewSequenceUp(m_lListViewIndex).Left = fraFrame(m_lFrameIndex).Width - VB6.TwipsToPixelsX(615)
                                    cmdListViewSequenceDown(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Top + lvwListView(m_lListViewIndex).Height - cmdListViewSequenceDown(m_lListViewIndex).Height
                                    cmdListViewSequenceDown(m_lListViewIndex).Left = cmdListViewSequenceUp(m_lListViewIndex).Left
                                    cmdListViewSequenceUp(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)
                                    cmdListViewSequenceDown(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)
                                    cmdListViewSequenceUp(m_lListViewIndex).Visible = True
                                    cmdListViewSequenceDown(m_lListViewIndex).Visible = True
                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return m_lReturn
                                End If

                                If lTag < 0 Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                lvwListView(m_lListViewIndex).Tag = CStr(GISDataModelType.GISDMTypeRisk * 10000 + lTag)


                                m_vInUse(GISDataModelType.GISDMTypeRisk)(lTag) = gPMConstants.PMEReturnCode.PMTrue

                                cmdListViewAdd(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)
                                cmdListViewEdit(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)
                                cmdListViewDelete(m_lListViewIndex).Parent = fraFrame(m_lFrameIndex)

                                cmdListViewAdd(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewEdit(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewDelete(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)

                                cmdListViewAdd(m_lListViewIndex).Left = VB6.TwipsToPixelsX(120)
                                cmdListViewEdit(m_lListViewIndex).Left = VB6.TwipsToPixelsX(1260)
                                cmdListViewDelete(m_lListViewIndex).Left = VB6.TwipsToPixelsX(2400)

                                cmdListViewAdd(m_lListViewIndex).Visible = True
                                cmdListViewEdit(m_lListViewIndex).Visible = True
                                cmdListViewDelete(m_lListViewIndex).Visible = True

                            End If
                        End If
                    End If
                Else

                    'So it's a control - but which one?

                    'this is needed for both object linked and non object linked controls

                    If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp)) Then
                        m_sHelpText = ""
                    Else
                        m_sHelpText = CStr(m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp))
                    End If


                    'Akash : 
                    'If Not (Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp))) Then
                    If Not (Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp) = String.Empty) Then


                        m_lReturn = PBRiskScreenCommon2.GetTag(lGISObjectId:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)), lGISPropertyId:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp)), lTag:=lTag, r_vDataDictionary:=g_vDataDictionary(GISDataModelType.GISDMTypeRisk), ACOGISObjectId:=MainModule.ACOGISObjectId, ACPGISPropertyId:=MainModule.ACPGISPropertyId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        If lTag < 0 Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        sPropertyName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPPropertyName, lTag))

                        sObjectName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOObjectName, lTag))

                        sColumnName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPColumnName, lTag))

                        iDataType = 0

                        If CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, lTag)) <> "" Then

                            iDataType = CInt(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, lTag))
                        End If

                        bIsSpecial = False

                        If CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, lTag)) <> "" Then

                            bIsSpecial = CBool(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, lTag))
                        End If


                        'Developer Guide No. 56
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDColumnWidth, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDColumnWidth, lTemp)) Or String.IsNullOrEmpty(m_vScreenDetails(PBDatabaseConsts.ACDColumnWidth, lTemp)) Then
                            m_lIncludeInList = 0
                        Else
                            m_lIncludeInList = 1
                        End If


                        'Developer Guide No. 56 
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lTemp) = String.Empty Then
                            m_lPreQuote = 1
                        Else
                            m_lPreQuote = CInt(m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lTemp))
                        End If


                        'Akash : 
                        'If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp)) Then
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp) = String.Empty Then
                            m_lPostQuote = 1
                        Else
                            m_lPostQuote = CInt(m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lTemp))
                        End If


                        'Akash : 
                        'If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp)) Then
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp) = String.Empty Then
                            m_lPurchase = 1
                        Else
                            m_lPurchase = CInt(m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lTemp))
                        End If


                        'Akash :
                        'If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp)) Then
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp) = String.Empty Then
                            m_lIsValuation = 0
                        Else
                            m_lIsValuation = (m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp) = gPMConstants.PMEReturnCode.PMTrue)
                        End If


                        'Akash : 
                        'If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp)) Then
                        If Convert.IsDBNull(m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp)) Or IsNothing(m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp)) Or m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp) = String.Empty Then
                            m_lIsRateAndPremium = 0
                        Else
                            m_lIsRateAndPremium = (m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp) = gPMConstants.PMEReturnCode.PMTrue)
                        End If
                        'Developer Guide No. 74
                        m_lWidth = CInt((m_vScreenDetails(PBDatabaseConsts.ACDWidth, lTemp)))
                        m_lHeight = CInt((m_vScreenDetails(PBDatabaseConsts.ACDHeight, lTemp)))

                        Select Case g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPSpecialsType, lTag)
                            Case GISSharedPropertyConstants.ACOGISListID
                                'OK, so here it's a Gemini combo, and a label
                                'In this program, we don't really care what type of combo it is...

                                m_lReturn = AddComboBox(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), vColumnPosition:=m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOPartyTypeID
                                'OK, so here it's a command button, and a panel

                                'Developer Guide No. 74
                                m_lReturn = AddCommand(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), vColumnPosition:=m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                                'OK, so here it's a combo box, and a panel

                                m_lReturn = AddComboBox(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), vColumnPosition:=m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOProductID
                                'OK, so here it's a command button, and a panel

                                'Developer Guide No. 74
                                m_lReturn = AddCommand(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), sObjectName, sPropertyName, m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn


                            Case GISSharedPropertyConstants.ACOPMLookupTableName
                                'OK, so here it's a combo box, and a panel

                                m_lReturn = AddComboBox(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), sPropertyName:=sPropertyName, sObjectName:=sObjectName, vColumnPosition:=m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                                'OK, so here it's a sum insured
                                m_lIsRateAndPremium = CInt(m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lTemp))
                                m_lIsValuation = CInt(m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lTemp))


                                'Developer Guide No. 74
                                m_lReturn = AddSumInsured(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp), sPropertyName:=sPropertyName, sObjectName:=sObjectName, bAddFrame:=False)

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOStdWordingType
                                'OK, so here it's a standard wording

                                'Developer Guide No. 74
                                m_lReturn = AddStandardWording(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=sPropertyName, sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp), bAddFrame:=False)

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOReserveID
                                'OK, so here it's a claim reserve control

                                'Developer Guide No. 74
                                m_lReturn = AddClaimReserve(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=sPropertyName, sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp), bAddFrame:=False)

                                result = m_lReturn

                            Case GISSharedPropertyConstants.ACOPaymentID
                                'OK, so here it's a claim payment control

                                'Developer Guide No. 74
                                m_lReturn = AddClaimPayment(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), sPropertyName, False, sObjectName, sPropertyName, , m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn
                                'TBD
                                '                            Case ACOPeril
                                '
                                '                                BuildScreen = m_lReturn
                                '
                                '                            Case ACOAssociatedClients
                                '                                'OK, so here it's a claim payment control
                                '                                m_lReturn = AddAssociatedClients(iFrameIndex:=CInt(m_vScreenDetails(ACDParentId, lTemp)), _
                                ''                                                        vTag:=Array(m_vScreenDetails(ACDDataModelType, lTemp), lTag), _
                                ''                                                               lX:=CLng(m_vScreenDetails(ACDLeft, lTemp)), _
                                ''                                                               lY:=CLng(m_vScreenDetails(ACDTop, lTemp)), _
                                ''                                                               sCaption:=sPropertyName, _
                                ''                                                        sPropertyName:=sPropertyName, sObjectName:=sObjectName, _
                                ''                                                        vTabSetIndex:=m_vScreenDetails(ACDTabSetIndex, lTemp), _
                                ''                                                          bAddFrame:=False)
                                '
                                '                                BuildScreen = m_lReturn

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Case GISSharedPropertyConstants.ACOCaseHeader
                                'OK, so here it's a claim reserve control

                                'Developer Guide No. 74
                                m_lReturn = AddClaimCaseHeader(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), sPropertyName, False, sObjectName, sPropertyName, , m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn
                            Case GISSharedPropertyConstants.ACOCaseClaimList
                                'OK, so here it's a claim reserve control

                                'Developer Guide No. 74
                                m_lReturn = AddCaseClaimList(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), sPropertyName, False, sObjectName, sPropertyName, , m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                result = m_lReturn
                            Case Else


                                If sPropertyName = "" Then
                                    'OK, so here it's a listview, with the buttons etc.

                                    m_lReturn = AddListView(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), sObjectName, sPropertyName, , m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                    result = m_lReturn
                                ElseIf (sColumnName.ToUpper().StartsWith("ADDRESS_CNT")) Then
                                    'OK, so here it's an address

                                    m_lReturn = AddAddress(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=sPropertyName, sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp), bAddFrame:=False)

                                    result = m_lReturn

                                Else
                                    'If we're here then it's a textbox or checkbox
                                    Select Case iDataType
                                        Case iGISSharedConstants.GISDataTypeOption
                                            'Checkbox

                                            'Developer Guide No. 74
                                            m_lReturn = AddCheckBox(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), vPMFormat:=m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lTemp), vColumnPosition:=m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp), sPropertyName:=sPropertyName, sObjectName:=sObjectName, vTabSetIndex:=m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                            result = m_lReturn

                                        Case Else
                                            'Textbox

                                            'Developer Guide No. 74
                                            m_lReturn = AddTextBox(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), New Object() {m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lTemp), lTag}, CInt((m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp))), CInt((m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp))), CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), sObjectName, sPropertyName, CObj(m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lTemp)), m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp))

                                            result = m_lReturn

                                    End Select
                                End If
                        End Select

                    Else
                        Select Case m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp)
                            Case ""
                                m_lReturn = 1
                            Case PBRiskScreenCommon.ndcFreeFormatText, PBRiskScreenCommon.ndcHyperlink

                                'Developer Guide No. 74
                                m_lReturn = AddLabel(iFrameIndex:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)), vTag:=New Object() {0, CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp))}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)))
                                'JES Adding Find Control
                            Case PBRiskScreenCommon.ndcFindControl

                                'Developer Guide No. 74
                                m_lReturn = AddFindControl(iFrameIndex:=gPMFunctions.ToSafeInteger(CInt(m_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp))), vTag:=New Object() {0, CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp))}, lX:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)), lY:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)), sCaption:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)), lId:=CInt(m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lTemp)))
                        End Select
                    End If
                End If
            Next lTemp

            PBTabStripCommon.SelectTab(TabStrip1, 0)
            If TabStrip1.TabCount > 0 Then
                TabStrip1_SelectedIndexChanged(Me.TabStrip1, Nothing)
                SSTabHelper.SetTabEnabled(TabStrip1, TabGetCurrentIndex(TabStrip1), True)
            End If

            Return result

        Catch excep As System.Exception



            'OK, so we try up to 10 times to wait a second and retry.
            If Information.Err().Number = -2147417848 Then
                lErrorCount += 1
                If lErrorCount < 10 Then
                    dtTimer = DateTime.Now.AddSeconds(1)
                    While dtTimer > DateTime.Now
                    End While

                End If
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    Private Sub RebuildArray(ByVal vScreenArray As Object(,), ByVal bsave As Boolean)

        For lTemp As Integer = vScreenArray.GetLowerBound(1) To vScreenArray.GetUpperBound(1)
            If bsave Then
                vScreenArray(PBDatabaseConsts.ACDWidth, lTemp) = (vScreenArray(PBDatabaseConsts.ACDWidth, lTemp))
            End If
        Next


    End Sub




    ' ***************************************************************** '
    '
    ' Name: ClearScreen
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ClearScreen() As Integer

        Dim result As Integer = 0
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue




            For iCnt As Integer = 0 To ContainerHelper.Controls(Me).Count - 1

                If HasVisibleProperty(ContainerHelper.Controls(Me)(iCnt)) Then

                    If Not ContainerHelper.Controls(Me)(iCnt).Parent Is Nothing Then
                        If ContainerHelper.Controls(Me)(iCnt).Parent.Name = "fraFrame" Then

                            ContainerHelper.Controls(Me)(iCnt).Parent = picScreen
                        End If
                    End If
                End If
            Next iCnt



            For iCnt As Integer = ContainerHelper.Controls(Me).Count - 1 To m_lInitialControlsCount Step -1
                ContainerHelper.UnloadControl(Me, "Controls", iCnt)
            Next iCnt

            'Reset the arrays
            ResetArrays()

            'Get rid of the other tabs
            Do While TabStrip1.TabPages.Count > 1
                TabStrip1.TabPages.RemoveAt(1)
            Loop

            'Reset the controls to be invisible
            HideControls()

            'Reset the indexes(again ?)

            m_lTabIndex = 0 'always at least one tab

            'Reset the arrays
            ResetArrays()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CallMe
    '
    ' Description:
    '
    ' History: 13/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CallMe(ByRef lIndex As Integer) As Integer

        Dim result As Integer = 0
        'Akash: Modified
        'Dim oScreen As ClassInterface
        Dim oScreen As Interface_Renamed
        Dim lTag, lChildId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the contact interface object via
            ' the public object manager.
            Dim temp_oScreen As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oScreen, sClassName:="iPMUMaintainScreenData.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oScreen = temp_oScreen

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get screen interface", vApp:=ACApp, vClass:=ACClass, vMethod:="CallMe", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            If PBRiskScreenCommon.g_sControlName = "uctCLMPerilDT1" Then
                lTag = CInt(CDbl(Convert.ToString(uctCLMPerilDT1.Tag)) Mod 10000)
            Else
                lTag = CInt(CDbl(Convert.ToString(lvwListView(lIndex).Tag)) Mod 10000)

                If Convert.IsDBNull(m_vListViewArray(PBRiskScreenCommon.ACCChildId, lIndex)) Or IsNothing(m_vListViewArray(PBRiskScreenCommon.ACCChildId, lIndex)) Then
                    lChildId = 0
                Else
                    lChildId = CInt(m_vListViewArray(PBRiskScreenCommon.ACCChildId, lIndex))
                End If
            End If

            'determine which screen id to use
            'for risks this is a single screen held in the listview
            'for others it needs to read a mapping table

            Select Case g_vDataDictionary(m_lGISDMType)(ACOIsNonGIS, lTag)
                Case GISDataModelType.GISOTRisk
                Case GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTDisclosure, GISDataModelType.GISOTPeril

                    SelectChildScreen(CInt(g_vDataDictionary(m_lGISDMType)(ACOIsNonGIS, lTag)), lChildId)
            End Select

            '    lvwListView(lIndex).SetFocus 'doesn't work

            If lChildId = -1 Then
                Return result
            ElseIf lChildId = 0 Then
                m_lReturn = oScreen.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            Else
                m_lReturn = oScreen.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            oScreen.GISObjectId = CInt(g_vDataDictionary(m_lGISDMType)(ACOGISObjectId, lTag))
            oScreen.ParentId = m_lScreenId

            oScreen.ScreenId = lChildId

            'pass in a (possibly) different data model code
            oScreen.GISDataModelCode = m_sGISDataModelCode(m_lGISDMType)
            oScreen.GISDataModelId = m_lGISDataModelId(m_lGISDMType)
            oScreen.GISDMType = m_lGISDMType

            oScreen.ScreenType = CInt(g_vDataDictionary(m_lGISDMType)(ACOIsNonGIS, lTag))


            'Akash: Commented
            'App.OleRequestPendingTimeout = 0

            'Akash: Commented
            'App.OleRequestPendingMsgTitle = "requesttext"

            'Akash: Commented
            'App.OleServerBusyMsgText = "busytext"

            'Akash: Commented
            'App.OleServerBusyTimeout = 0

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy) ' Set the mouse pointer to busy
            m_lReturn = oScreen.Start() 'load the child screen editor
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal) ' Set the mouse pointer to normal

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Me.Focus()
            'If not cancelled, add to grid
            If oScreen.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            '    LogMessage _
            'iType:=PMLogOnError, _
            'sMsg:="Screen Id is " & oScreen.ScreenId, _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="CallMe", _
            'vErrNo:=Err.Number, _
            'vErrDesc:=Err.Description

            'If PBRiskScreenCommon.g_sControlName = "uctCLMPerilDT1" Then
            If PBRiskScreenCommon.g_sControlName = "uctCLMPerilDT1" Then
            Else
                If oScreen.ScreenType <> 5 Then
                    'If m_cControlName <> Nothing Or m_cControlName <> "" Then
                    m_vListViewArray(PBRiskScreenCommon.ACCChildId, lIndex) = oScreen.ScreenId
                    m_vFrameArray(PBRiskScreenCommon.ACFChildId, ContainerHelper.GetControlIndex(lvwListView(lIndex).Parent)) = oScreen.ScreenId
                    'End If
                End If
            End If



            'May need to get more data to redo the listview.
            'PBE0007     Screen editor   The change in a child's list view column order is not displayed when returning to the parent.

            oScreen.Dispose()
            ' Destroy the instance of the conviction object
            ' from memory.
            oScreen = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallMe Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallMe", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyScreen
    '
    ' Description:
    '
    ' History: 14/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CopyScreen() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage, sCode, sDescription As String
        Dim bOK As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sCode = Interaction.InputBox("Please enter the new code: ", "Screen Designer")

            If sCode = "" Then
                Return result
            End If

            sDescription = Interaction.InputBox("Please enter the new description: ", "Screen Designer")

            If sDescription = "" Then
                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRMaintainScreenData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen")

                Return result
            End If

            m_lReturn = CheckCode(v_lScreenId:=0, v_sCode:=sCode, r_bOK:=bOK)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If bOK Then


                    m_oBusiness.ScreenId = m_lScreenId


                    m_oBusiness.GISDataModelCode = m_sGISDataModelCode(GISDataModelType.GISDMTypeRisk)


                    m_oBusiness.ScreenCode = txtScreenCode.Text.Trim()

                    ' RAW 08/07/2003 : CQ1335 : added

                    m_oBusiness.ScreenType = m_lScreenType

                    m_oBusiness.NewScreenCode = sCode


                    m_oBusiness.NewScreenDescription = sDescription


                    m_lReturn = m_oBusiness.CopyScreen

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get details.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy details in the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen")

                            'Don't exit, we need to terminate
                            '            Exit Function
                        End If
                    End If
                End If
            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Sub AddControlArray(ByVal cControl As String, ByVal lIndex As Integer)

        Dim lControlIndex As Integer
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
                'Case "txtMlText"
                '    ReDim Preserve txtMlText(lControlIndex)
                '    txtMlText(lControlIndex) = New TextBox
                '    txtMlText(lControlIndex).Name = "_txtMlText_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtMlText(lControlIndex))
            Case "chkYesNo"
                ReDim Preserve chkYesNo(lControlIndex)
                chkYesNo(lControlIndex) = New CheckBox
                chkYesNo(lControlIndex).Name = "_chkYesNo_" & CStr(lControlIndex)
                Me.Controls.Add(chkYesNo(lControlIndex))

            Case "lblTextLabel"
                ReDim Preserve lblTextLabel(lControlIndex)
                lblTextLabel(lControlIndex) = New Label
                lblTextLabel(lControlIndex).Name = "_lblText_" & CStr(lControlIndex)
                Me.Controls.Add(lblTextLabel(lControlIndex))
                'Case "lblMlText"
                '    ReDim Preserve lblMlText(lControlIndex)
                '    lblMlText(lControlIndex) = New Label
                '    lblMlText(lControlIndex).Name = "_lblMlText_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblMlText(lControlIndex))
            Case "lblCheckLabel"
                ReDim Preserve lblCheckLabel(lControlIndex)
                lblCheckLabel(lControlIndex) = New Label
                lblCheckLabel(lControlIndex).Name = "_lblCheckLabel_" & CStr(lControlIndex)
                Me.Controls.Add(lblCheckLabel(lControlIndex))
                'Case "lblPMLookup"
                '    ReDim Preserve lblPMLookup(lControlIndex)
                '    lblPMLookup(lControlIndex) = New Label
                '    lblPMLookup(lControlIndex).Name = "_lblPMLookup_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblPMLookup(lControlIndex))
                'Case "cboPMLookup"
                '    ReDim Preserve cboPMLookup(lControlIndex)
                '    cboPMLookup(lControlIndex) = New PMLookupControl.cboPMLookup
                '    cboPMLookup(lControlIndex).Name = "_cboPMLookup_" & CStr(lControlIndex)
                '    Me.Controls.Add(cboPMLookup(lControlIndex))
                'Case "cboGISLookup"
                '    ReDim Preserve cboGISLookup(lControlIndex)
                '    cboGISLookup(lControlIndex) = New uctGISUserDefLookupControl.cboGISLookup
                '    cboGISLookup(lControlIndex).Name = "_cboGISLookup_" & CStr(lControlIndex)
                '    Me.Controls.Add(cboGISLookup(lControlIndex))
                'Case "lblGISLookup"
                '    ReDim Preserve lblGISLookup(lControlIndex)
                '    lblGISLookup(lControlIndex) = New Label
                '    lblGISLookup(lControlIndex).Name = "_lblGISLookup_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblGISLookup(lControlIndex))
                'Case "txtAddress1"
                '    ReDim Preserve txtAddress1(lControlIndex)
                '    txtAddress1(lControlIndex) = New TextBox
                '    txtAddress1(lControlIndex).Name = "_txtAddress1_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress1(lControlIndex))
                'Case "txtAddress2"
                '    ReDim Preserve txtAddress2(lControlIndex)
                '    txtAddress2(lControlIndex) = New TextBox
                '    txtAddress2(lControlIndex).Name = "_txtAddress2_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress2(lControlIndex))
                'Case "txtAddress3"
                '    ReDim Preserve txtAddress3(lControlIndex)
                '    txtAddress3(lControlIndex) = New TextBox
                '    txtAddress3(lControlIndex).Name = "_txtAddress3_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress3(lControlIndex))
                'Case "txtAddress4"
                '    ReDim Preserve txtAddress4(lControlIndex)
                '    txtAddress4(lControlIndex) = New TextBox
                '    txtAddress4(lControlIndex).Name = "_txtAddress4_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress4(lControlIndex))
                'Case "txtAddress5"
                '    ReDim Preserve txtAddress5(lControlIndex)
                '    txtAddress5(lControlIndex) = New TextBox
                '    txtAddress5(lControlIndex).Name = "_txtAddress5_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress5(lControlIndex))
                'Case "txtAddress6"
                '    ReDim Preserve txtAddress6(lControlIndex)
                '    txtAddress6(lControlIndex) = New TextBox
                '    txtAddress6(lControlIndex).Name = "_txtAddress6_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtAddress6(lControlIndex))
                'Case "cmdAddress"
                '    ReDim Preserve cmdAddress(lControlIndex)
                '    cmdAddress(lControlIndex) = New Button
                '    cmdAddress(lControlIndex).Name = "_cmdAddress_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdAddress(lControlIndex))
                'Case "PBFindRT1"
                '    ReDim Preserve PBFindRT1(lControlIndex)
                '    PBFindRT1(lControlIndex) = New uctPBFindRT.PBFindRT
                '    PBFindRT1(lControlIndex).Name = "_PBFindRT1_" & CStr(lControlIndex)
                '    Me.Controls.Add(PBFindRT1(lControlIndex))
                'Case "cboList"
                '    ReDim Preserve cboList(lControlIndex)
                '    cboList(lControlIndex) = New PMListMgrDropdown.uctDropdown
                '    cboList(lControlIndex).Name = "_cboList_" & CStr(lControlIndex)
                '    Me.Controls.Add(cboList(lControlIndex))
                'Case "lblList"
                '    ReDim Preserve lblList(lControlIndex)
                '    lblList(lControlIndex) = New Label
                '    lblList(lControlIndex).Name = "_lblList_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblList(lControlIndex))
            Case "lvwListView"
                ReDim Preserve lvwListView(lControlIndex)
                lvwListView(lControlIndex) = New ListView
                lvwListView(lControlIndex).Name = "_lvwListView_" & CStr(lControlIndex)
                Me.Controls.Add(lvwListView(lControlIndex))
            Case "cmdListViewAdd"
                ReDim Preserve cmdListViewAdd(lControlIndex)
                cmdListViewAdd(lControlIndex) = New Button
                cmdListViewAdd(lControlIndex).Name = "_cmdListViewAdd_" & CStr(lControlIndex)
                'cmdListViewAdd(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACAddButton, gPMConstants.PMEResourseFileDataType.PMResString)
                Me.Controls.Add(cmdListViewAdd(lControlIndex))
            Case "cmdListViewEdit"
                ReDim Preserve cmdListViewEdit(lControlIndex)
                cmdListViewEdit(lControlIndex) = New Button
                cmdListViewEdit(lControlIndex).Name = "_cmdListViewEdit_" & CStr(lControlIndex)
                'cmdListViewEdit(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACEditButton, gPMConstants.PMEResourseFileDataType.PMResString)
                Me.Controls.Add(cmdListViewEdit(lControlIndex))
            Case "cmdListViewDelete"
                ReDim Preserve cmdListViewDelete(lControlIndex)
                cmdListViewDelete(lControlIndex) = New Button
                cmdListViewDelete(lControlIndex).Name = "_cmdListViewDelete_" & CStr(lControlIndex)
                'cmdListViewDelete(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString)
                Me.Controls.Add(cmdListViewDelete(lControlIndex))
            Case "cmdListViewSequenceUp"
                ReDim Preserve cmdListViewSequenceUp(lControlIndex)
                cmdListViewSequenceUp(lControlIndex) = New Button
                cmdListViewSequenceUp(lControlIndex).Name = "_cmdListViewSequenceUp_" & CStr(lControlIndex)
                Me.Controls.Add(cmdListViewSequenceUp(lControlIndex))
            Case "cmdListViewSequenceDown"
                ReDim Preserve cmdListViewSequenceDown(lControlIndex)
                cmdListViewSequenceDown(lControlIndex) = New Button
                cmdListViewSequenceDown(lControlIndex).Name = "_cmdListViewSequenceDown_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdListViewSequenceDown(lControlIndex))
                'Case "lvwSumInsured"
                '    ReDim Preserve lvwSumInsured(lControlIndex)
                '    lvwSumInsured(lControlIndex) = New ListView
                '    lvwSumInsured(lControlIndex).Name = "_lvwSumInsured_" & CStr(lControlIndex)
                '    Me.Controls.Add(lvwSumInsured(lControlIndex))
                'Case "cmdSumInsuredAdd"
                '    ReDim Preserve cmdSumInsuredAdd(lControlIndex)
                '    cmdSumInsuredAdd(lControlIndex) = New Button
                '    cmdSumInsuredAdd(lControlIndex).Name = "_cmdSumInsuredAdd_" & CStr(lControlIndex)
                '    cmdSumInsuredAdd(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACAddButton, gPMConstants.PMEResourseFileDataType.PMResString)
                '    Me.Controls.Add(cmdSumInsuredAdd(lControlIndex))
                'Case "cmdSumInsuredDelete"
                '    ReDim Preserve cmdSumInsuredDelete(lControlIndex)
                '    cmdSumInsuredDelete(lControlIndex) = New Button
                '    cmdSumInsuredDelete(lControlIndex).Name = "_cmdSumInsuredDelete_" & CStr(lControlIndex)
                '    cmdSumInsuredDelete(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString)
                '    Me.Controls.Add(cmdSumInsuredDelete(lControlIndex))
                'Case "cmdSumInsuredEdit"
                '    ReDim Preserve cmdSumInsuredEdit(lControlIndex)
                '    cmdSumInsuredEdit(lControlIndex) = New Button
                '    cmdSumInsuredEdit(lControlIndex).Name = "_cmdSumInsuredEdit_" & CStr(lControlIndex)
                '    cmdSumInsuredEdit(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACEditButton, gPMConstants.PMEResourseFileDataType.PMResString)
                '    Me.Controls.Add(cmdSumInsuredEdit(lControlIndex))
                'Case "lblTotalSumInsured"
                '    ReDim Preserve lblTotalSumInsured(lControlIndex)
                '    lblTotalSumInsured(lControlIndex) = New Label
                '    lblTotalSumInsured(lControlIndex).Name = "_lblTotalSumInsured_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblTotalSumInsured(lControlIndex))
                'Case "pnlTotalSumInsured"
                '    ReDim Preserve pnlTotalSumInsured(lControlIndex)
                '    pnlTotalSumInsured(lControlIndex) = New Label
                '    pnlTotalSumInsured(lControlIndex).Name = "_pnlTotalSumInsured_" & CStr(lControlIndex)
                '    Me.Controls.Add(pnlTotalSumInsured(lControlIndex))
                'Case "lblRate"
                '    ReDim Preserve lblRate(lControlIndex)
                '    lblRate(lControlIndex) = New Label
                '    lblRate(lControlIndex).Name = "_lblRate_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblRate(lControlIndex))
                'Case "txtRate"
                '    ReDim Preserve txtRate(lControlIndex)
                '    txtRate(lControlIndex) = New TextBox
                '    txtRate(lControlIndex).Name = "_txtRate_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtRate(lControlIndex))
                'Case "lblPremium"
                '    ReDim Preserve lblPremium(lControlIndex)
                '    lblPremium(lControlIndex) = New Label
                '    lblPremium(lControlIndex).Name = "_lblPremium_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblPremium(lControlIndex))
                'Case "txtPremium"
                '    ReDim Preserve txtPremium(lControlIndex)
                '    txtPremium(lControlIndex) = New TextBox
                '    txtPremium(lControlIndex).Name = "_txtPremium_" & CStr(lControlIndex)
                '    Me.Controls.Add(txtPremium(lControlIndex))
                'Case "cmdPolicyCommand"
                '    ReDim Preserve cmdPolicyCommand(lControlIndex)
                '    cmdPolicyCommand(lControlIndex) = New Button
                '    cmdPolicyCommand(lControlIndex).Name = "_cmdPolicyCommand_" & CStr(lControlIndex)
                '    'cmdPolicyCommand(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, , gPMConstants.PMEResourseFileDataType.PMResString)
                '    Me.Controls.Add(cmdPolicyCommand(lControlIndex))
                'Case "pnlPolicyPanel"
                '    ReDim Preserve pnlPolicyPanel(lControlIndex)
                '    pnlPolicyPanel(lControlIndex) = New Label
                '    pnlPolicyPanel(lControlIndex).Name = "_pnlPolicyPanel_" & CStr(lControlIndex)
                '    Me.Controls.Add(pnlPolicyPanel(lControlIndex))
                'Case "cmdPartyCommand"
                '    ReDim Preserve cmdPartyCommand(lControlIndex)
                '    cmdPartyCommand(lControlIndex) = New Button
                '    cmdPartyCommand(lControlIndex).Name = "_cmdPartyCommand_" & CStr(lControlIndex)
                '    'cmdPolicyCommand(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, , gPMConstants.PMEResourseFileDataType.PMResString)
                '    Me.Controls.Add(cmdPartyCommand(lControlIndex))
                'Case "pnlPartyPanel"
                '    ReDim Preserve pnlPartyPanel(lControlIndex)
                '    pnlPartyPanel(lControlIndex) = New Label
                '    pnlPartyPanel(lControlIndex).Name = "_pnlPartyPanel_" & CStr(lControlIndex)
                '    Me.Controls.Add(pnlPartyPanel(lControlIndex))
                'Case "lvwStandardWording"
                '    ReDim Preserve lvwStandardWording(lControlIndex)
                '    lvwStandardWording(lControlIndex) = New ListView
                '    lvwStandardWording(lControlIndex).Name = "_lvwStandardWording_" & CStr(lControlIndex)
                '    Me.Controls.Add(lvwStandardWording(lControlIndex))
                'Case "cmdStandardWordingAdd"
                '    ReDim Preserve cmdStandardWordingAdd(lControlIndex)
                '    cmdStandardWordingAdd(lControlIndex) = New Button
                '    cmdStandardWordingAdd(lControlIndex).Name = "_cmdStandardWordingAdd_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdStandardWordingAdd(lControlIndex))
                'Case "cmdStandardWordingEdit"
                '    ReDim Preserve cmdStandardWordingEdit(lControlIndex)
                '    cmdStandardWordingEdit(lControlIndex) = New Button
                '    cmdStandardWordingEdit(lControlIndex).Name = "_cmdStandardWordingEdit_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdStandardWordingEdit(lControlIndex))
                'Case "cmdStandardWordingDelete"
                '    ReDim Preserve cmdStandardWordingDelete(lControlIndex)
                '    cmdStandardWordingDelete(lControlIndex) = New Button
                '    cmdStandardWordingDelete(lControlIndex).Name = "_cmdStandardWordingDelete_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdStandardWordingDelete(lControlIndex))
                'Case "cmdStandardWordingUp"
                '    ReDim Preserve cmdStandardWordingUp(lControlIndex)
                '    cmdStandardWordingUp(lControlIndex) = New Button
                '    cmdStandardWordingUp(lControlIndex).Name = "_cmdStandardWordingUp_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdStandardWordingUp(lControlIndex))
                'Case "cmdStandardWordingDown"
                '    ReDim Preserve cmdStandardWordingDown(lControlIndex)
                '    cmdStandardWordingDown(lControlIndex) = New Button
                '    cmdStandardWordingDown(lControlIndex).Name = "_cmdStandardWordingDown_" & CStr(lControlIndex)
                '    Me.Controls.Add(cmdStandardWordingDown(lControlIndex))
                'Case "lblStandardWordingMove"
                '    ReDim Preserve lblStandardWordingMove(lControlIndex)
                '    lblStandardWordingMove(lControlIndex) = New Label
                '    lblStandardWordingMove(lControlIndex).Name = "_lblStandardWordingMove_" & CStr(lControlIndex)
                '    Me.Controls.Add(lblStandardWordingMove(lControlIndex))
        End Select

    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddControl
    '
    ' Description:
    '
    ' History: 23/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddControl(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single) As Integer

        Dim result As Integer = 0
        Dim sName, sPropertyName, sObjectName, sColumnName As String
        Dim iDataType As Integer
        Dim bIsSpecial, bIsComment As Boolean




        result = gPMConstants.PMEReturnCode.PMTrue


        m_lClaimReserveInThisFrame = False
        m_lClaimPaymentInThisFrame = False
        m_lClaimPerilInThisFrame = False
        m_lStandardWordingInThisFrame = -1
        m_lSumInsuredInThisFrame = -1
        m_lAddressInThisFrame = -1
        m_lListViewInThisFrame = -1
        m_lCaseClaimListInThisFrame = False

        'Default these to "optional"
        m_lPreQuote = 1
        m_lPostQuote = 1
        m_lPurchase = 1

        m_sFrameName = ""



        m_lPMFormat = DBNull.Value


        Try
            Dim arrList As ArrayList = New ArrayList
            ControlList(Me, arrList)

            For ctl As Integer = 0 To arrList.Count - 1
                Dim ctlControl As Control = arrList(ctl)
                If HasVisibleProperty(ctlControl) Then
                    If Not IsNothing(ctlControl.Parent) Then
                        'Developer Guide No.62 
                        If ctlControl.Parent.Name.Contains("fraFrame") Then
                            If ContainerHelper.GetControlIndex(ctlControl.Parent) = iFrameIndex Then
                                Select Case ctlControl.Name
                                    Case "uctStandardWording1"
                                        m_lStandardWordingInThisFrame = iFrameIndex
                                    Case "uctSumInsured1"
                                        m_lSumInsuredInThisFrame = iFrameIndex
                                    Case "uctAddress1"
                                        m_lAddressInThisFrame = iFrameIndex
                                    Case "lvwListView"
                                        m_lListViewInThisFrame = iFrameIndex
                                    Case "uctClaimReserve1"
                                        m_lClaimReserveInThisFrame = True
                                    Case "uctClaimPayment1"
                                        m_lClaimPaymentInThisFrame = True
                                    Case "uctCLMPerilDT1"
                                        m_lClaimPerilInThisFrame = True
                                    Case "uctCLMCaseClaim1"
                                        m_lCaseClaimListInThisFrame = True
                                    Case "uctCLMCaseHeader1"
                                        m_lCaseClaimHeaderInThisFrame = True
                                End Select

                            End If
                        End If
                    End If
                End If

            Next

            If m_lStandardWordingInThisFrame > -1 Or (m_lSumInsuredInThisFrame > -1) Or (m_lAddressInThisFrame > -1) Or (m_lClaimReserveInThisFrame) Or (m_lClaimPaymentInThisFrame) Or (m_lClaimPerilInThisFrame) Then
                MessageBox.Show("Cannot add control to frame containing a special", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            If m_lListViewInThisFrame > -1 Then
                MessageBox.Show("Cannot add control to frame containing a listview", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            If CDbl(vTag(1)) >= 0 Then 'check if attribute bound control

                'Is this one already in use?

                If m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("You're already using this one", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If

                sName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPPropertyName, vTag(1)))

                m_lReturn = SpacifyName(sOriginal:=sName, sNew:=sPropertyName)

                sColumnName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPColumnName, vTag(1)))
                sObjectName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOObjectName, vTag(1)))

                iDataType = 0
                If CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, vTag(1))) <> "" Then
                    iDataType = CInt(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, vTag(1)))
                End If
                'Developer Guide No. 74
                m_lWidth = VB6.TwipsToPixelsX(textBoxMinimumWidth)

                Select Case g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPSpecialsType, vTag(1))
                    Case GISSharedPropertyConstants.ACOGISListID
                        'OK, so here it's a Gemini combo, and a label
                        'In this program, we don't really care what type of combo it is...
                        m_lReturn = AddComboBox(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOPMLookupTableName
                        'OK, so here it's a combo box, and a panel
                        m_lReturn = AddComboBox(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOPartyTypeID
                        'OK, so here it's a command button, and a panel
                        m_lReturn = AddCommand(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                        'OK, so here it's a sum insured
                        m_lIsRateAndPremium = gPMConstants.PMEReturnCode.PMTrue
                        m_lIsValuation = gPMConstants.PMEReturnCode.PMTrue
                        m_sFrameName = sPropertyName
                        m_lReturn = AddSumInsured(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOStdWordingType
                        'OK, so here it's a standard wording
                        m_sFrameName = sPropertyName
                        m_lReturn = AddStandardWording(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                        m_lReturn = AddComboBox(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOProductID
                        m_lReturn = AddCommand(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOReserveID
                        'OK, so here it's a claim reserve
                        m_sFrameName = sPropertyName
                        m_lReturn = AddClaimReserve(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOPaymentID
                        'OK, so here it's a claim payment
                        m_sFrameName = sPropertyName
                        m_lReturn = AddClaimPayment(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOCaseHeader
                        'OK, so here it's a claim Case Header
                        m_sFrameName = sPropertyName
                        m_lReturn = AddClaimCaseHeader(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case GISSharedPropertyConstants.ACOCaseClaimList
                        'OK, so here it's a claim Case Header
                        m_sFrameName = sPropertyName
                        m_lReturn = AddCaseClaimList(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)
                        result = m_lReturn
                    Case Else

                        bIsSpecial = Conversion.Val(CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, vTag(1)))) > 0
                        bIsComment = Conversion.Val(CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, vTag(1)))) = iGISSharedConstants.GISDataTypeComment
                        If bIsComment Then
                            m_lPMFormat = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine
                        End If

                        m_lHeight = VB6.TwipsToPixelsY(textBoxMinimumHeight)

                        If CDbl(vTag(1)) >= 0 Then 'check if attribute bound control

                            If sPropertyName = "" Then
                                'OK, so here it's a listview, with the buttons etc.

                                If CDbl(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOIsNonGIS, vTag(1))) = GISDataModelType.GISOTPeril Then

                                    m_lReturn = AddClaimPeril(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=sPropertyName, sObjectName:=sObjectName, sPropertyName:=sPropertyName, bAddFrame:=True)


                                Else
                                    'standard list view
                                    m_lReturn = AddListView(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName)

                                End If

                                Return m_lReturn
                            End If

                            '    Select Case sColumnName
                            '    Case "address_cnt"
                            If sColumnName.ToUpper().StartsWith("ADDRESS_CNT") Then
                                'OK, so here it's an address
                                m_sFrameName = sPropertyName
                                m_lReturn = AddAddress(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName, bAddFrame:=True)


                                Return m_lReturn

                                '    End Select
                            End If

                            'If we're here then it's a textbox or checkbox
                            Select Case iDataType
                                '    Case PMBoolean
                                Case iGISSharedConstants.GISDataTypeOption
                                    'Checkbox

                                    'vTag(0) check data format
                                    m_lReturn = AddCheckBox(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName, vPMFormat:=0)

                                    result = m_lReturn

                                Case Else
                                    m_lReturn = AddTextBox(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sObjectName:=sObjectName, sPropertyName:=sPropertyName, sCaption:=sPropertyName, r_bIsComment:=bIsComment)


                                    result = m_lReturn
                            End Select
                        End If
                End Select
            Else
                Select Case vTag(1)
                    Case PBRiskScreenCommon.ndcFreeFormatText
                        m_lReturn = AddLabel(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=freeFormatText)
                    Case PBRiskScreenCommon.ndcHyperlink
                        m_lReturn = AddLabel(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=hyperlink)
                    Case PBRiskScreenCommon.ndcFindControl
                        m_lReturn = AddFindControl(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, sCaption:=FindControl, lId:=-1)
                End Select
            End If
            Return result

Err_AddControl:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddControl", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result


        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
	End Function
	
	Private Function AddFindControl(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef lId As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			'add frame
            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
			
			m_lFindControlIndex += 1
			
            If m_lFindControlIndex > 0 Then
                'Developer Guide No.94
                ContainerHelper.LoadControl(Me, "PBFindControl", m_lFindControlIndex, True)

                ReDim Preserve m_vFindControlArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lFindControlIndex)
            Else
                ReDim m_vFindControlArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lFindControlIndex)
            End If

            m_vFindControlArray(PBRiskScreenCommon.ACCFrameNumber, m_lFindControlIndex) = iFrameIndex
            m_vFindControlArray(PBRiskScreenCommon.ACCIsDeleted, m_lFindControlIndex) = gPMConstants.PMEReturnCode.PMFalse

            m_vFindControlArray(PBRiskScreenCommon.ACCHelpText, m_lFindControlIndex) = DBNull.Value
            m_vFindControlArray(PBRiskScreenCommon.ACCPreQuote, m_lFindControlIndex) = m_lPreQuote
            m_vFindControlArray(PBRiskScreenCommon.ACCPostQuote, m_lFindControlIndex) = m_lPostQuote
            m_vFindControlArray(PBRiskScreenCommon.ACCPurchase, m_lFindControlIndex) = m_lPurchase
            m_vFindControlArray(PBRiskScreenCommon.ACCIncludeInList, m_lFindControlIndex) = m_lIncludeInList

            m_vFindControlArray(PBRiskScreenCommon.ACCChildId, m_lFindControlIndex) = DBNull.Value

            m_vFindControlArray(PBRiskScreenCommon.ACCGISObjectId, m_lFindControlIndex) = DBNull.Value

            m_vFindControlArray(PBRiskScreenCommon.ACCPMFormat, m_lFindControlIndex) = DBNull.Value

            m_vFindControlArray(PBRiskScreenCommon.ACCTabSetIndex, m_lFindControlIndex) = m_lPMFormat
            m_vFindControlArray(PBRiskScreenCommon.ACCColumnPosition, m_lFindControlIndex) = 0

            PBFindControl(m_lFindControlIndex).Parent = fraFrame(iFrameIndex)

            'Developer Guide No. 74
            PBFindControl(m_lFindControlIndex).Top = (lY)
            PBFindControl(m_lFindControlIndex).Left = (lX)
            PBFindControl(m_lFindControlIndex).Visible = True


            PBFindControl(m_lFindControlIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))

            PBFindControl(m_lFindControlIndex).FindControlID = lId
            AddHandler PBFindControl(m_lFindControlIndex).MouseDown, AddressOf PBFindControl_MouseDown
            AddHandler PBFindControl(m_lFindControlIndex).btnFind_renamed, AddressOf PBFindControl_btnFind
            AddHandler PBFindControl(m_lFindControlIndex).MouseUp, AddressOf PBFindControl_MouseUp
            AddHandler PBFindControl(m_lFindControlIndex).MouseMove, AddressOf PBFindControl_MouseMove

            'get text controls in this frame, and set ID moved into PBFindControl_btnFind event

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddFindControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFindControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddAddress
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddAddress(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            m_lAddressIndex += 1
            If m_lAddressIndex > 0 Then
                ContainerHelper.LoadControl(Me, "uctAddress1", m_lAddressIndex, True)
                ReDim Preserve m_vAddressArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lAddressIndex)
            Else
                ReDim m_vAddressArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lAddressIndex)
            End If

            m_vAddressArray(PBRiskScreenCommon.ACCFrameNumber, m_lAddressIndex) = iFrameIndex
            m_vAddressArray(PBRiskScreenCommon.ACCIsDeleted, m_lAddressIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vAddressArray(PBRiskScreenCommon.ACCHelpText, m_lAddressIndex) = m_sHelpText
            m_vAddressArray(PBRiskScreenCommon.ACCPreQuote, m_lAddressIndex) = m_lPreQuote
            m_vAddressArray(PBRiskScreenCommon.ACCPostQuote, m_lAddressIndex) = m_lPostQuote
            m_vAddressArray(PBRiskScreenCommon.ACCPurchase, m_lAddressIndex) = m_lPurchase
            m_vAddressArray(PBRiskScreenCommon.ACCIsValuation, m_lAddressIndex) = m_lIsValuation
            m_vAddressArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lAddressIndex) = m_lIsRateAndPremium
            m_vAddressArray(PBRiskScreenCommon.ACCIncludeInList, m_lAddressIndex) = m_lIncludeInList

            m_vAddressArray(PBRiskScreenCommon.ACCChildId, m_lAddressIndex) = DBNull.Value

            m_vAddressArray(PBRiskScreenCommon.ACCGISObjectId, m_lAddressIndex) = DBNull.Value

            m_vAddressArray(PBRiskScreenCommon.ACCPMFormat, m_lAddressIndex) = DBNull.Value

            m_vAddressArray(PBRiskScreenCommon.ACCTabSetIndex, m_lAddressIndex) = vTabSetIndex


            uctAddress1(m_lAddressIndex).Parent = fraFrame(iFrameIndex)

            uctAddress1(m_lAddressIndex).Top = VB6.TwipsToPixelsY(240)
            uctAddress1(m_lAddressIndex).Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctAddress1(m_lAddressIndex).Left = VB6.TwipsToPixelsX(240)
            uctAddress1(m_lAddressIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(300)
            uctAddress1(m_lAddressIndex).Visible = True


            uctAddress1(m_lAddressIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctAddress1(m_lAddressIndex), fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddCheckBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddCheckBox(ByRef iFrameIndex As Integer, ByRef vTag As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByVal vPMFormat As Object, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef vColumnPosition As Object = 0, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            m_lCheckIndex += 1

            If m_lCheckIndex > 0 Then
                'Developer Guide No. 94
                ContainerHelper.LoadControl(Me, "chkYesNo", m_lCheckIndex, True)
                ContainerHelper.LoadControl(Me, "lblCheckLabel", m_lCheckIndex, True)
                ReDim Preserve m_vCheckArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lCheckIndex)
            Else
                ReDim m_vCheckArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lCheckIndex)
            End If

            ' RAW 08/07/2003 : CQ1335 : added DataDictionary and ScreenValues arguments to get it to compile

            PBRiskScreenCommon2.SetInitialControlValues(m_lCheckIndex, m_vCheckArray, lblCheckLabel, chkYesNo, fraFrame, iFrameIndex, gPMConstants.PMEComponentAction.PMEdit, vPMFormat, vTabSetIndex, vColumnPosition, CInt(lX), CInt(lY), sCaption, vTag, vTag, False, 0, 0, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, 0, m_oFormFields, 0, g_vDataDictionary(GISDataModelType.GISDMTypeRisk), g_vScreenValues, ACPDataType:=MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)

            'add object and property name as tooltip
            SetToolTip(chkYesNo(m_lCheckIndex), lblCheckLabel(m_lCheckIndex), sObjectName, sPropertyName)


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue
            'To Set the height of the checkbox
            chkYesNo(m_lCheckIndex).Height = 18
            'call this to set position!
            'Developer Guide No.64 
            textLabelMouseMove("chkYesNo", MouseButtons.Left, PBRiskScreenCommon.g_lx, PBRiskScreenCommon.g_ly, chkYesNo(m_lCheckIndex), lblCheckLabel(m_lCheckIndex), pnlPosition, m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, CInt(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, iFrameIndex))))

            'Developer Guide No.74 
            If ((chkYesNo(m_lCheckIndex).Left) + (chkYesNo(m_lCheckIndex).Width)) > ((fraFrame(iFrameIndex).Width) - Math.Round(90 / 15)) Then
                chkYesNo(m_lCheckIndex).Left = (fraFrame(iFrameIndex).Width - chkYesNo(m_lCheckIndex).Width - Math.Round(90 / 15))
            End If

            lblCheckLabel(m_lCheckIndex).Visible = True
            chkYesNo(m_lCheckIndex).Visible = True
            'Developer Guide No.59
            chkYesNo(m_lCheckIndex).Width = VB6.TwipsToPixelsX(PBRiskScreenCommon.cCheckBoxCaptionWidth)

            If m_lCheckIndex = 0 Then
                AddHandler chkYesNo(m_lCheckIndex).MouseClick, AddressOf chkYesNo_CheckedChanged
                AddHandler chkYesNo(m_lCheckIndex).MouseDown, AddressOf chkYesNo_MouseDown
                AddHandler chkYesNo(m_lCheckIndex).MouseUp, AddressOf chkYesNo_MouseUp

            End If


            'set the caption
            'now, and only now make it look like a try state
            If vPMFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                chkYesNo(m_lCheckIndex).Width = VB6.TwipsToPixelsX(PBRiskScreenCommon.cCheckBoxTriStateCaptionWidth)
                PBRiskScreenCommon.g_iCheckBoxValue = 1 ' so move to 2!
                PBRiskScreenCommon2.simulateTriStateCheckBox(1, chkYesNo(m_lCheckIndex))
            Else
                chkYesNo(m_lCheckIndex).CheckState = CheckState.Unchecked
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCheckBox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCheckBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddComboBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddComboBox(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef vColumnPosition As Object = 0, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            'OK, so here it's a combo, and a label
            'Is it one of ours of one of theirs?  Can have an effect on the control we use.
            'For now let's use a standard lookup
            m_lComboIndex += 1
            If m_lComboIndex > 0 Then
                ContainerHelper.LoadControl(Me, "cboCombo", m_lComboIndex, True)
                ContainerHelper.LoadControl(Me, "lblComboLabel", m_lComboIndex, True)
                ReDim Preserve m_vComboArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lComboIndex)
            Else
                ReDim m_vComboArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lComboIndex)
            End If

            m_vComboArray(PBRiskScreenCommon.ACCFrameNumber, m_lComboIndex) = iFrameIndex
            m_vComboArray(PBRiskScreenCommon.ACCIsDeleted, m_lComboIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vComboArray(PBRiskScreenCommon.ACCHelpText, m_lComboIndex) = m_sHelpText
            m_vComboArray(PBRiskScreenCommon.ACCPreQuote, m_lComboIndex) = m_lPreQuote
            m_vComboArray(PBRiskScreenCommon.ACCPostQuote, m_lComboIndex) = m_lPostQuote
            m_vComboArray(PBRiskScreenCommon.ACCPurchase, m_lComboIndex) = m_lPurchase
            m_vComboArray(PBRiskScreenCommon.ACCIsValuation, m_lComboIndex) = m_lIsValuation
            m_vComboArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lComboIndex) = m_lIsRateAndPremium
            m_vComboArray(PBRiskScreenCommon.ACCIncludeInList, m_lComboIndex) = m_lIncludeInList

            m_vComboArray(PBRiskScreenCommon.ACCChildId, m_lComboIndex) = DBNull.Value

            m_vComboArray(PBRiskScreenCommon.ACCGISObjectId, m_lComboIndex) = DBNull.Value

            m_vComboArray(PBRiskScreenCommon.ACCPMFormat, m_lComboIndex) = DBNull.Value

            m_vComboArray(PBRiskScreenCommon.ACCTabSetIndex, m_lComboIndex) = vTabSetIndex

            m_vComboArray(PBRiskScreenCommon.ACCColumnPosition, m_lComboIndex) = vColumnPosition

            cboCombo(m_lComboIndex).Parent = fraFrame(iFrameIndex)


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            lblComboLabel(m_lComboIndex).Parent = fraFrame(iFrameIndex)

            ''Developer Guide No. 74
            lblComboLabel(m_lComboIndex).Top = (lY)
            lblComboLabel(m_lComboIndex).Left = (lX)
            lblComboLabel(m_lComboIndex).Height = (PBRiskScreenCommon2.CalculateLinesInCaption(sCaption) * VB6.TwipsToPixelsY(240)) + VB6.TwipsToPixelsY(195)
            lblComboLabel(m_lComboIndex).Visible = True
            lblComboLabel(m_lComboIndex).Text = sCaption

            'Developer Guide No. 74
            cboCombo(m_lComboIndex).Width = m_lWidth
            cboCombo(m_lComboIndex).Visible = True
            cboCombo(m_lComboIndex).Tag = CStr(vTag(0) * 10000 + vTag(1))
            'Developer Guide No. 74 For display position
            'call this to set position!
            textLabelMouseMove("", Windows.Forms.MouseButtons.Left, PBRiskScreenCommon.g_lx, PBRiskScreenCommon.g_ly, cboCombo(m_lComboIndex), lblComboLabel(m_lComboIndex), pnlPosition, m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, CInt(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, iFrameIndex))))

            'add object and property name as tooltip
            SetToolTip(cboCombo(m_lComboIndex), lblComboLabel(m_lComboIndex), sObjectName, sPropertyName)

            'Developer Guide No.  183
            AddHandler cboCombo(m_lComboIndex).MouseMove, AddressOf cboCombo_MouseMove
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddComboBox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddComboBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddCommand
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddCommand(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef vColumnPosition As Object = 0, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            'OK, so here it's a command button, and a panel
            m_lCommandIndex += 1
            If m_lCommandIndex > 0 Then
                ContainerHelper.LoadControl(Me, "cmdCommand", m_lCommandIndex, True)
                ContainerHelper.LoadControl(Me, "pnlPanel", m_lCommandIndex, True)
                ReDim Preserve m_vCommandArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lCommandIndex)
            Else
                ReDim m_vCommandArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lCommandIndex)
            End If

            m_vCommandArray(PBRiskScreenCommon.ACCFrameNumber, m_lCommandIndex) = iFrameIndex
            m_vCommandArray(PBRiskScreenCommon.ACCIsDeleted, m_lCommandIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vCommandArray(PBRiskScreenCommon.ACCHelpText, m_lCommandIndex) = m_sHelpText
            m_vCommandArray(PBRiskScreenCommon.ACCPreQuote, m_lCommandIndex) = m_lPreQuote
            m_vCommandArray(PBRiskScreenCommon.ACCPostQuote, m_lCommandIndex) = m_lPostQuote
            m_vCommandArray(PBRiskScreenCommon.ACCPurchase, m_lCommandIndex) = m_lPurchase
            m_vCommandArray(PBRiskScreenCommon.ACCIsValuation, m_lCommandIndex) = m_lIsValuation
            m_vCommandArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lCommandIndex) = m_lIsRateAndPremium
            m_vCommandArray(PBRiskScreenCommon.ACCIncludeInList, m_lCommandIndex) = m_lIncludeInList

            m_vCommandArray(PBRiskScreenCommon.ACCChildId, m_lCommandIndex) = DBNull.Value

            m_vCommandArray(PBRiskScreenCommon.ACCGISObjectId, m_lCommandIndex) = DBNull.Value

            m_vCommandArray(PBRiskScreenCommon.ACCPMFormat, m_lCommandIndex) = DBNull.Value

            m_vCommandArray(PBRiskScreenCommon.ACCTabSetIndex, m_lCommandIndex) = vTabSetIndex

            m_vCommandArray(PBRiskScreenCommon.ACCColumnPosition, m_lCommandIndex) = vColumnPosition

            cmdCommand(m_lCommandIndex).Parent = fraFrame(iFrameIndex)

            'Developer Guide No. 74
            cmdCommand(m_lCommandIndex).Top = (lY)
            cmdCommand(m_lCommandIndex).Left = (lX)
            cmdCommand(m_lCommandIndex).Visible = True
            cmdCommand(m_lCommandIndex).Tag = CStr(vTag(0) * 10000 + vTag(1))
            cmdCommand(m_lCommandIndex).Text = sCaption

            'Developer Guide No. 74
            pnlPanel(m_lCommandIndex).Width = (m_lWidth)

            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            pnlPanel(m_lCommandIndex).Parent = fraFrame(iFrameIndex)

            'Developer Guide No. 74
            pnlPanel(m_lCommandIndex).Top = (lY)
            pnlPanel(m_lCommandIndex).Left = (lX) + cmdCommand(m_lCommandIndex).Width
            pnlPanel(m_lCommandIndex).Visible = True
            pnlPanel(m_lCommandIndex).Text = sCaption

            'Developer Guide No. 74
            If ((pnlPanel(m_lCommandIndex).Left) + (pnlPanel(m_lCommandIndex).Width)) > ((fraFrame(iFrameIndex).Width) - (120 / 20)) Then
                pnlPanel(m_lCommandIndex).Left = (fraFrame(iFrameIndex).Width) - pnlPanel(m_lCommandIndex).Width - (120 / 20)
                cmdCommand(m_lCommandIndex).Left = pnlPanel(m_lCommandIndex).Left - cmdCommand(m_lCommandIndex).Width
            End If

            'add object and property name as tooltip
            SetToolTip(cmdCommand(m_lCommandIndex), pnlPanel(m_lCommandIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCommand Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddFrame
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddFrame(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef lAddIfLessThan As Integer) As Integer

        Dim result As Integer = 0
        Dim sName As String = ""
        Dim lTop, lBottom, lLeft, lRight As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If iFrameIndex >= lAddIfLessThan Then
                Return result
            End If

            sName = ""


            If CDbl(vTag(1)) >= 0 Then 'check if attribute bound control
                Dim auxVar As Object = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOObjectName, vTag(1))


                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                    sName = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOObjectName, vTag(1)))
                End If

                If m_sFrameName <> "" Then
                    sName = m_sFrameName
                End If
            Else
                sName = nonDatabaseElements
            End If
            m_lFrameIndex += 1

            If m_lFrameIndex > 0 Then
                ContainerHelper.LoadControl(Me, "fraFrame", m_lFrameIndex, True)
                ReDim Preserve m_vFrameArray(PBRiskScreenCommon.ACFLastArrayPosition, m_lFrameIndex)
            Else
                ReDim m_vFrameArray(PBRiskScreenCommon.ACFLastArrayPosition, m_lFrameIndex)
            End If

            'store data model type and dictionary offset


            m_vFrameArray(PBRiskScreenCommon.ACFDataModelType, m_lFrameIndex) = CDbl(vTag(0)) * 10000 + CDbl(vTag(1))

            m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, m_lFrameIndex) = TabGetCurrentIndex(TabStrip1)

            m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex) = DBNull.Value
            m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, m_lFrameIndex) = gPMConstants.PMEReturnCode.PMFalse

            m_vFrameArray(PBRiskScreenCommon.ACFChildId, m_lFrameIndex) = DBNull.Value

            m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, m_lFrameIndex) = DBNull.Value

            m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, m_lFrameIndex) = DBNull.Value
            TabStrip1.TabPages(CInt(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, m_lFrameIndex))).Controls.Add(fraFrame(m_lFrameIndex))

            If lAddIfLessThan > 1000 Then
                If iFrameIndex > -1 Then
                    fraFrame(m_lFrameIndex).Parent = fraFrame(iFrameIndex)
                    m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex) = iFrameIndex
                End If
            End If

            'Set the defaults...
            'lTop = lX 'VB6.TwipsToPixelsY(360)
            'lLeft = lY 'VB6.TwipsToPixelsX(120)
            'Developer Guide No. 74
            If TypeOf fraFrame(m_lFrameIndex).Parent Is GroupBox Then
                'Developer Guide No.74 
                lBottom = CInt((fraFrame(m_lFrameIndex).Parent.Height) - Math.Round(120 / 15))
                lRight = CInt((fraFrame(m_lFrameIndex).Parent.Width) - Math.Round(120 / 15))
            Else
                'Not a frame in a frame
                'Developer Guide No. 74
                lBottom = CInt(TabStrip1.Height - Math.Round(120 / 15))
                lRight = CInt(TabStrip1.Width - Math.Round(120 / 15))
            End If

            If m_lFrameIndex <> 0 Then
                'We've got a position on the screen - let's loop around the frames and see what
                'the top and bottom can be...
                'Ignore this one...
                ' For lTemp As Integer = 0 To m_lFrameIndex - 1
                For lTemp As Integer = 0 To m_lFrameIndex
                    'On this tab...
                    If CDbl(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lTemp)) = TabGetCurrentIndex(TabStrip1) Then


                        If Convert.IsDBNull(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp)) Or IsNothing(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp)) Then

                            'Both directly on the tab...

                            If Convert.IsDBNull(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex)) Or IsNothing(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex)) Then

                                'And the compared frame hasn't been deleted...
                                If fraFrame(lTemp).Visible Then
                                    ' Developer Guide No. 74
                                    If (fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) < lY Then
                                        If (fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) > lTop Then
                                            lTop = CInt((fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) + Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Top) > lY Then
                                        If (fraFrame(lTemp).Top) < lBottom Then
                                            lBottom = CInt((fraFrame(lTemp).Top) - Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) < lX Then
                                        If (fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) > lLeft Then
                                            lLeft = CInt((fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) + Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Left) > lX Then
                                        If (fraFrame(lTemp).Left) < lRight Then
                                            lRight = CInt((fraFrame(lTemp).Left) - Math.Round(120 / 15))
                                        End If
                                    End If
                                End If
                            End If
                        Else

                            'Both in this frame...
                            If m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, lTemp).Equals(m_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, m_lFrameIndex)) Then
                                'And the compared frame hasn't been deleted...

                                If fraFrame(lTemp).Visible Then
                                    lTop = lX 'VB6.TwipsToPixelsY(360)
                                    'lLeft = lY 'VB6.TwipsToPixelsX(120)
                                    If (fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) < lY Then
                                        If (fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) > lTop Then
                                            lTop = CInt((fraFrame(lTemp).Top) + (fraFrame(lTemp).Height) + Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Top) > lY Then
                                        If (fraFrame(lTemp).Top) < lBottom Then
                                            lBottom = CInt((fraFrame(lTemp).Top) - Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) < lX Then
                                        If (fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) > lLeft Then
                                            lLeft = CInt((fraFrame(lTemp).Left) + (fraFrame(lTemp).Width) + Math.Round(120 / 15))
                                        End If
                                    End If

                                    If (fraFrame(lTemp).Left) > lX Then
                                        If (fraFrame(lTemp).Left) < lRight Then
                                            lRight = CInt((fraFrame(lTemp).Left) - Math.Round(120 / 15))
                                        End If
                                    End If

                                End If
                            End If
                        End If
                    End If
                Next lTemp
            End If

            'Developer Guide No. 74
            fraFrame(m_lFrameIndex).Top = lTop
            lY -= lTop
            'Developer Guide No. 74
            fraFrame(m_lFrameIndex).Height = lBottom - fraFrame(m_lFrameIndex).Top - 20
            fraFrame(m_lFrameIndex).Left = (lLeft)
            lX -= lLeft
            'Developer Guide No. 74
            fraFrame(m_lFrameIndex).Width = (lRight - lLeft)

            If sName <> "" Then
                fraFrame(m_lFrameIndex).Text = sName
            End If
            fraFrame(m_lFrameIndex).Visible = True

            ''Developer Guide No. 59
            fraFrame(m_lFrameIndex).AllowDrop = True

            fraFrame(m_lFrameIndex).BringToFront()

            iFrameIndex = m_lFrameIndex

            ''Developer Guide No.183 
            If m_lFrameIndex = 0 Then
                AddHandler fraFrame(m_lFrameIndex).MouseUp, AddressOf fraFrame_MouseUp
                AddHandler fraFrame(m_lFrameIndex).MouseMove, AddressOf fraFrame_MouseMove
                AddHandler fraFrame(m_lFrameIndex).MouseDown, AddressOf fraFrame_MouseDown
                AddHandler fraFrame(m_lFrameIndex).DragEnter, AddressOf fraFrame_DragEnter
                AddHandler fraFrame(m_lFrameIndex).DragDrop, AddressOf fraFrame_DragDrop
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddFrame Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFrame", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddListView
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddListView(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = 0) As Integer


        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'OK, so here it's a listview in a frame, with the standard add, edit and delete buttons

            'Add the frame
            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            m_lListViewIndex += 1
            If m_lListViewIndex > 0 Then
                ContainerHelper.LoadControl(Me, "lvwListView", m_lListViewIndex, True)
                ContainerHelper.LoadControl(Me, "cmdListViewAdd", m_lListViewIndex, True)
                ContainerHelper.LoadControl(Me, "cmdListViewEdit", m_lListViewIndex, True)
                ContainerHelper.LoadControl(Me, "cmdListViewDelete", m_lListViewIndex, True)
                ReDim Preserve m_vListViewArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lListViewIndex)
            Else
                ReDim m_vListViewArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lListViewIndex)
            End If

            m_vListViewArray(PBRiskScreenCommon.ACCFrameNumber, m_lListViewIndex) = iFrameIndex
            m_vListViewArray(PBRiskScreenCommon.ACCIsDeleted, m_lListViewIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vListViewArray(PBRiskScreenCommon.ACCHelpText, m_lListViewIndex) = m_sHelpText
            m_vListViewArray(PBRiskScreenCommon.ACCPreQuote, m_lListViewIndex) = m_lPreQuote
            m_vListViewArray(PBRiskScreenCommon.ACCPostQuote, m_lListViewIndex) = m_lPostQuote
            m_vListViewArray(PBRiskScreenCommon.ACCPurchase, m_lListViewIndex) = m_lPurchase
            m_vListViewArray(PBRiskScreenCommon.ACCIsValuation, m_lListViewIndex) = m_lIsValuation
            m_vListViewArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lListViewIndex) = m_lIsRateAndPremium
            m_vListViewArray(PBRiskScreenCommon.ACCIncludeInList, m_lListViewIndex) = m_lIncludeInList
            m_vListViewArray(PBRiskScreenCommon.ACCChildId, m_lListViewIndex) = 0

            m_vListViewArray(PBRiskScreenCommon.ACCGISObjectId, m_lListViewIndex) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOGISObjectId, vTag(1))

            m_vListViewArray(PBRiskScreenCommon.ACCPMFormat, m_lListViewIndex) = DBNull.Value

            m_vListViewArray(PBRiskScreenCommon.ACCPMFormat, m_lListViewIndex) = vTabSetIndex

            m_vFrameArray(PBRiskScreenCommon.ACFChildId, iFrameIndex) = 0

            m_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, iFrameIndex) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOGISObjectId, vTag(1))

            lvwListView(m_lListViewIndex).Parent = fraFrame(iFrameIndex)

            lvwListView(m_lListViewIndex).Top = VB6.TwipsToPixelsY(240)
            lvwListView(m_lListViewIndex).Left = VB6.TwipsToPixelsX(120)
            lvwListView(m_lListViewIndex).Width = lvwListView(m_lListViewIndex).Parent.Width - VB6.TwipsToPixelsX(240)
            lvwListView(m_lListViewIndex).Height = lvwListView(m_lListViewIndex).Parent.Height - VB6.TwipsToPixelsY(720)
            lvwListView(m_lListViewIndex).Visible = True


            lvwListView(m_lListViewIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))

            'We inherit the columns from the last one, so let's get rid of them...
            For lTemp As Integer = lvwListView(m_lListViewIndex).Columns.Count To 1 Step -1
                lvwListView(m_lListViewIndex).Columns.RemoveAt(lTemp - 1)
            Next lTemp

            If (lvwListView(m_lListViewIndex).Columns.Count) = 0 Then
                lvwListView(m_lListViewIndex).Columns.Add("", 94)
            End If

            lvwListView(m_lListViewIndex).Columns.Item(0).Text = fraFrame(iFrameIndex).Text


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            cmdListViewAdd(m_lListViewIndex).Parent = fraFrame(iFrameIndex)
            cmdListViewEdit(m_lListViewIndex).Parent = fraFrame(iFrameIndex)
            cmdListViewDelete(m_lListViewIndex).Parent = fraFrame(iFrameIndex)

            cmdListViewAdd(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)
            cmdListViewEdit(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)
            cmdListViewDelete(m_lListViewIndex).Top = lvwListView(m_lListViewIndex).Height + VB6.TwipsToPixelsY(300)

            cmdListViewAdd(m_lListViewIndex).Left = VB6.TwipsToPixelsX(120)
            cmdListViewEdit(m_lListViewIndex).Left = VB6.TwipsToPixelsX(1260)
            cmdListViewDelete(m_lListViewIndex).Left = VB6.TwipsToPixelsX(2400)

            cmdListViewAdd(m_lListViewIndex).Visible = True
            cmdListViewEdit(m_lListViewIndex).Visible = True
            cmdListViewDelete(m_lListViewIndex).Visible = True

            'add object and property name as tooltip
            SetToolTip(lvwListView(m_lListViewIndex), cmdListViewAdd(m_lListViewIndex), sObjectName, sPropertyName)
            SetToolTip(cmdListViewEdit(m_lListViewIndex), cmdListViewDelete(m_lListViewIndex), sObjectName, sPropertyName)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddStandardWording
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddStandardWording(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            m_lStandardWordingIndex += 1
            If m_lStandardWordingIndex > 0 Then
                ContainerHelper.LoadControl(Me, "uctStandardWording1", m_lStandardWordingIndex, True)
                ReDim Preserve m_vStandardWordingArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lStandardWordingIndex)
            Else
                ReDim m_vStandardWordingArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lStandardWordingIndex)
            End If

            m_vStandardWordingArray(PBRiskScreenCommon.ACCFrameNumber, m_lStandardWordingIndex) = iFrameIndex
            m_vStandardWordingArray(PBRiskScreenCommon.ACCIsDeleted, m_lStandardWordingIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vStandardWordingArray(PBRiskScreenCommon.ACCHelpText, m_lStandardWordingIndex) = m_sHelpText
            m_vStandardWordingArray(PBRiskScreenCommon.ACCPreQuote, m_lStandardWordingIndex) = m_lPreQuote
            m_vStandardWordingArray(PBRiskScreenCommon.ACCPostQuote, m_lStandardWordingIndex) = m_lPostQuote
            m_vStandardWordingArray(PBRiskScreenCommon.ACCPurchase, m_lStandardWordingIndex) = m_lPurchase
            m_vStandardWordingArray(PBRiskScreenCommon.ACCIncludeInList, m_lStandardWordingIndex) = m_lIncludeInList

            m_vStandardWordingArray(PBRiskScreenCommon.ACCChildId, m_lStandardWordingIndex) = DBNull.Value

            m_vStandardWordingArray(PBRiskScreenCommon.ACCGISObjectId, m_lStandardWordingIndex) = DBNull.Value

            m_vStandardWordingArray(PBRiskScreenCommon.ACCPMFormat, m_lStandardWordingIndex) = DBNull.Value

            m_vStandardWordingArray(PBRiskScreenCommon.ACCTabSetIndex, m_lStandardWordingIndex) = vTabSetIndex


            uctStandardWording1(m_lStandardWordingIndex).Parent = fraFrame(iFrameIndex)

            uctStandardWording1(m_lStandardWordingIndex).Top = VB6.TwipsToPixelsY(240)
            uctStandardWording1(m_lStandardWordingIndex).Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctStandardWording1(m_lStandardWordingIndex).Left = VB6.TwipsToPixelsX(120)
            uctStandardWording1(m_lStandardWordingIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctStandardWording1(m_lStandardWordingIndex).Visible = True


            uctStandardWording1(m_lStandardWordingIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctStandardWording1(m_lStandardWordingIndex), fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddStandardWording Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddStandardWording", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddClaimReserve
    '
    ' Description: Add claim reserve control
    '
    ' History: 12/07/2002 RVH Created
    '
    ' ***************************************************************** '
    Private Function AddClaimReserve(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            uctClaimReserve1.Parent = fraFrame(iFrameIndex)

            uctClaimReserve1.Top = VB6.TwipsToPixelsY(200)
            uctClaimReserve1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctClaimReserve1.Left = VB6.TwipsToPixelsX(100)
            uctClaimReserve1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctClaimReserve1.Visible = True


            uctClaimReserve1.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctClaimReserve1, fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimReserve Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimReserve", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddClaimPayment
    '
    ' Description: Add claim payment control
    '
    ' History: 12/07/2002 RVH Created
    '
    ' ***************************************************************** '
    Private Function AddClaimPayment(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oContainer As Control
        Dim oNewContainer As Control
        Dim bStop As Boolean
        Dim arr As New ArrayList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            uctClaimPayment1.Parent = fraFrame(iFrameIndex)

            uctClaimPayment1.Top = VB6.TwipsToPixelsY(200)

            'Developer Guide No. 74
            uctClaimPayment1.Height = CInt(fraFrame(iFrameIndex).Height - (300 / 20))
            fraFrame(iFrameIndex).Height = uctClaimPayment1.Height + (300 / 20)

            uctClaimPayment1.Width = CInt((fraFrame(iFrameIndex).Width) - (180 / 20))
            fraFrame(iFrameIndex).Width = uctClaimPayment1.Width + (180 / 20)

            ' set the container to be the claim payment controls parent
            oContainer = uctClaimPayment1.Parent


            ' set stop processing indicator
            bStop = False
            Do Until bStop

                oNewContainer = oContainer.Parent
                If oNewContainer.Name.ToLower() <> "picscreenmain" Then
                    arr.Add(oNewContainer)
                Else
                    ' we have gone to far
                    bStop = True
                End If

                If Not bStop Then
                    oContainer = oNewContainer
                End If
            Loop

            arr.Reverse()
            For ctlcount As Integer = 0 To arr.Count - 1
                arr(ctlcount).Width = uctClaimPayment1.Width + VB6.TwipsToPixelsX(180) * (arr.Count - ctlcount)
                arr(ctlcount).Height = uctClaimPayment1.Height + VB6.TwipsToPixelsY(300) * (arr.Count - ctlcount)
            Next

            uctClaimPayment1.Left = VB6.TwipsToPixelsX(100)
            uctClaimPayment1.Visible = True


            uctClaimPayment1.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue
            'uctClaimPayment1.Parent.Parent.Width = oNewContainer.Width
            'uctClaimPayment1.Parent.Parent.Height = oNewContainer.Height
            fraFrame(iFrameIndex).BringToFront()
            'add object and property name as tooltip
            SetToolTip(uctClaimPayment1, fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPayment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddClaimPeril
    '
    ' Description: Add claim Peril control
    '
    ' History: 12/07/2002 RVH Created
    '
    ' ***************************************************************** '
    Private Function AddClaimPeril(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            uctCLMPerilDT1.Parent = fraFrame(iFrameIndex)

            uctCLMPerilDT1.Top = VB6.TwipsToPixelsY(200)
            uctCLMPerilDT1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctCLMPerilDT1.Left = VB6.TwipsToPixelsX(100)
            uctCLMPerilDT1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctCLMPerilDT1.Visible = True


            uctCLMPerilDT1.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctCLMPerilDT1, fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimPeril Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' Name: AddAssociatedClients
    '
    ' Description: Add claim payment control
    '
    ' History: 12/07/2002 RVH Created
    '
    ' ***************************************************************** '

    'Private Function AddAssociatedClients(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Byte = 0) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If bAddFrame Then
    'm_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'Return m_lReturn
    'End If
    'End If
    '
    'uctAssociatedClients.Parent = fraFrame(iFrameIndex)
    '
    'uctAssociatedClients.Top = VB6.TwipsToPixelsY(200)
    'uctAssociatedClients.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
    'uctAssociatedClients.Left = VB6.TwipsToPixelsX(100)
    'uctAssociatedClients.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
    'uctAssociatedClients.Visible = True


    'uctAssociatedClients.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))
    '

    'm_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue
    '
    'add object and property name as tooltip
    'SetToolTip(uctAssociatedClients, fraFrame(iFrameIndex), sObjectName, sPropertyName)
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAssociatedClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAssociatedClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function



    ' ***************************************************************** '
    '
    ' Name: AddSumInsured
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddSumInsured(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If
            End If

            m_lSumInsuredIndex += 1
            If m_lSumInsuredIndex > 0 Then
                'Developer Guide No.94
                ContainerHelper.LoadControl(Me, "uctSumInsured1", m_lSumInsuredIndex, True)
                ReDim Preserve m_vSumInsuredArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lSumInsuredIndex)
            Else
                ReDim m_vSumInsuredArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lSumInsuredIndex)
            End If

            m_vSumInsuredArray(PBRiskScreenCommon.ACCFrameNumber, m_lSumInsuredIndex) = iFrameIndex
            m_vSumInsuredArray(PBRiskScreenCommon.ACCIsDeleted, m_lSumInsuredIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vSumInsuredArray(PBRiskScreenCommon.ACCHelpText, m_lSumInsuredIndex) = m_sHelpText
            m_vSumInsuredArray(PBRiskScreenCommon.ACCPreQuote, m_lSumInsuredIndex) = m_lPreQuote
            m_vSumInsuredArray(PBRiskScreenCommon.ACCPostQuote, m_lSumInsuredIndex) = m_lPostQuote
            m_vSumInsuredArray(PBRiskScreenCommon.ACCPurchase, m_lSumInsuredIndex) = m_lPurchase
            m_vSumInsuredArray(PBRiskScreenCommon.ACCIsValuation, m_lSumInsuredIndex) = m_lIsValuation
            m_vSumInsuredArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lSumInsuredIndex) = m_lIsRateAndPremium
            m_vSumInsuredArray(PBRiskScreenCommon.ACCIncludeInList, m_lSumInsuredIndex) = m_lIncludeInList

            m_vSumInsuredArray(PBRiskScreenCommon.ACCChildId, m_lSumInsuredIndex) = DBNull.Value

            m_vSumInsuredArray(PBRiskScreenCommon.ACCGISObjectId, m_lSumInsuredIndex) = DBNull.Value

            m_vSumInsuredArray(PBRiskScreenCommon.ACCPMFormat, m_lSumInsuredIndex) = DBNull.Value

            m_vSumInsuredArray(PBRiskScreenCommon.ACCTabSetIndex, m_lSumInsuredIndex) = vTabSetIndex

            uctSumInsured1(m_lSumInsuredIndex).Parent = fraFrame(iFrameIndex)

            uctSumInsured1(m_lSumInsuredIndex).ShowRateAndPremium = (m_lIsRateAndPremium = gPMConstants.PMEReturnCode.PMTrue)
            uctSumInsured1(m_lSumInsuredIndex).ShowValuation = (m_lIsValuation = gPMConstants.PMEReturnCode.PMTrue)
            uctSumInsured1(m_lSumInsuredIndex).Top = VB6.TwipsToPixelsY(240)
            uctSumInsured1(m_lSumInsuredIndex).Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctSumInsured1(m_lSumInsuredIndex).Left = VB6.TwipsToPixelsX(120)
            uctSumInsured1(m_lSumInsuredIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctSumInsured1(m_lSumInsuredIndex).Visible = True


            uctSumInsured1(m_lSumInsuredIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctSumInsured1(m_lSumInsuredIndex), fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSumInsured Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSumInsured", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddLabel
    '
    ' Description:
    '
    ' History: 04/07/2001 CLG - Created.
    '          18/07/2001 CLG - Added support for hyperlinks
    ' ***************************************************************** '
    Private Function AddLabel(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String) As Integer
        Dim result As Integer = 0
        Dim lCharPos As Integer

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddLabel")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            m_lTextIndex += 1

            If m_lTextIndex > 0 Then
                ContainerHelper.LoadControl(Me, "lblTextLabel", m_lTextIndex, True)
                ContainerHelper.LoadControl(Me, "txtText", m_lTextIndex, True)
                ReDim Preserve m_vTextArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lTextIndex)
            Else
                ReDim m_vTextArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lTextIndex)
            End If

            m_vTextArray(PBRiskScreenCommon.ACCFrameNumber, m_lTextIndex) = iFrameIndex
            m_vTextArray(PBRiskScreenCommon.ACCIsDeleted, m_lTextIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vTextArray(PBRiskScreenCommon.ACCHelpText, m_lTextIndex) = m_sHelpText
            m_vTextArray(PBRiskScreenCommon.ACCPreQuote, m_lTextIndex) = m_lPreQuote
            m_vTextArray(PBRiskScreenCommon.ACCPostQuote, m_lTextIndex) = m_lPostQuote
            m_vTextArray(PBRiskScreenCommon.ACCPurchase, m_lTextIndex) = m_lPurchase

            '** JB START change **
            '** Change (462) to ensure that no labels will be
            '** displayed in the Set List Column Order screen
            'm_vTextArray(ACCIncludeInList, m_lTextIndex) = m_lIncludeInList
            m_vTextArray(PBRiskScreenCommon.ACCIncludeInList, m_lTextIndex) = 0
            '** JB END Change


            m_vTextArray(PBRiskScreenCommon.ACCChildId, m_lTextIndex) = DBNull.Value

            m_vTextArray(PBRiskScreenCommon.ACCGISObjectId, m_lTextIndex) = DBNull.Value

            m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lTextIndex) = m_lPMFormat

            m_vTextArray(PBRiskScreenCommon.ACCTabSetIndex, m_lTextIndex) = m_lPMFormat
            m_vTextArray(PBRiskScreenCommon.ACCColumnPosition, m_lTextIndex) = 0

            lblTextLabel(m_lTextIndex).Parent = fraFrame(iFrameIndex)
            txtText(m_lTextIndex).Parent = fraFrame(iFrameIndex)

            'Developer Guide No. 74
            lblTextLabel(m_lTextIndex).Top = (lY)
            lblTextLabel(m_lTextIndex).Left = (lX)
            lblTextLabel(m_lTextIndex).Visible = True
            lblTextLabel(m_lTextIndex).Text = sCaption

            ' RAW 20/10/2004 : CQ1814 : added

            If CDbl(vTag(1)) = PBRiskScreenCommon.ndcHyperlink Then
                lCharPos = (sCaption.IndexOf("|"c) + 1)
                If lCharPos > 0 Then
                    ' this is what the user sees
                    lblTextLabel(m_lTextIndex).Text = sCaption.Substring(0, lCharPos - 1)
                    ' this is the target address
                    txtText(m_lTextIndex).Text = sCaption.Substring(sCaption.Length - (sCaption.Length - lCharPos))
                End If
            End If
            txtText(m_lTextIndex).Visible = False


            txtText(m_lTextIndex).Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))

            SetLabelAppearance(txtText(m_lTextIndex), lblTextLabel(m_lTextIndex)) ' RAW 20/10/2004 : CQ1814 : renamed function

            'set tooltip to label name
            Do While (m_sHelpText.IndexOf("."c) + 1)
                m_sHelpText = m_sHelpText.Substring(m_sHelpText.Length - (m_sHelpText.Length - (m_sHelpText.IndexOf("."c) + 1)))
            Loop
            SetToolTip(txtText(m_lTextIndex), lblTextLabel(m_lTextIndex), "Label", m_sHelpText)

            If m_lTextIndex = 0 Then
                AddHandler lblTextLabel(m_lTextIndex).MouseMove, AddressOf lblTextLabel_MouseMove
                AddHandler lblTextLabel(m_lTextIndex).MouseDown, AddressOf lblTextLabel_MouseDown
                AddHandler lblTextLabel(m_lTextIndex).MouseUp, AddressOf lblTextLabel_MouseUp
                AddHandler lblTextLabel(m_lTextIndex).TextChanged, AddressOf lblTextLabel_TextChanged
            End If
            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddLabel")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddLabel")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLabel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLabel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddTextBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddTextBox(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef vColumnPosition As Object = 0, Optional ByRef vTabSetIndex As Object = 0, Optional ByRef r_bIsComment As Boolean = False) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return m_lReturn
            End If

            m_lTextIndex += 1

            If m_lTextIndex > 0 Then
                ContainerHelper.LoadControl(Me, "txtText", m_lTextIndex, True)
                ContainerHelper.LoadControl(Me, "lblTextLabel", m_lTextIndex, True)
                ReDim Preserve m_vTextArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lTextIndex)
            Else
                ReDim m_vTextArray(PBRiskScreenCommon.ACCLastArrayPosition, m_lTextIndex)
            End If

            m_vTextArray(PBRiskScreenCommon.ACCFrameNumber, m_lTextIndex) = iFrameIndex
            m_vTextArray(PBRiskScreenCommon.ACCIsDeleted, m_lTextIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vTextArray(PBRiskScreenCommon.ACCHelpText, m_lTextIndex) = m_sHelpText
            m_vTextArray(PBRiskScreenCommon.ACCPreQuote, m_lTextIndex) = m_lPreQuote
            m_vTextArray(PBRiskScreenCommon.ACCPostQuote, m_lTextIndex) = m_lPostQuote
            m_vTextArray(PBRiskScreenCommon.ACCPurchase, m_lTextIndex) = m_lPurchase

            'Dont allow comment fields to be in lists this will look daft.
            m_vTextArray(PBRiskScreenCommon.ACCIncludeInList, m_lTextIndex) = IIf(r_bIsComment, 0, m_lIncludeInList)


            m_vTextArray(PBRiskScreenCommon.ACCChildId, m_lTextIndex) = DBNull.Value

            m_vTextArray(PBRiskScreenCommon.ACCGISObjectId, m_lTextIndex) = DBNull.Value

            m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lTextIndex) = m_lPMFormat

            m_vTextArray(PBRiskScreenCommon.ACCTabSetIndex, m_lTextIndex) = vTabSetIndex

            m_vTextArray(PBRiskScreenCommon.ACCColumnPosition, m_lTextIndex) = vColumnPosition

            txtText(m_lTextIndex).Parent = fraFrame(iFrameIndex)


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            lblTextLabel(m_lTextIndex).Parent = fraFrame(iFrameIndex)

            'Developer Guide No. 74
            lblTextLabel(m_lTextIndex).Top = (lY)
            lblTextLabel(m_lTextIndex).Left = (lX)
            lblTextLabel(m_lTextIndex).Height = (PBRiskScreenCommon2.CalculateLinesInCaption(sCaption) * VB6.TwipsToPixelsY(240)) + VB6.TwipsToPixelsY(195)
            lblTextLabel(m_lTextIndex).Visible = True
            lblTextLabel(m_lTextIndex).Text = sCaption
            lblTextLabel(m_lTextIndex).Font = VB6.FontChangeUnderline(lblTextLabel(m_lTextIndex).Font, False)
            lblTextLabel(m_lTextIndex).ForeColor = Color.FromArgb(0, 0, 0)

            With txtText(m_lTextIndex)
                If r_bIsComment Then
                    'Developer Guide No. 74
                    .Width = m_lWidth * 3
                    .Height = m_lHeight * 3
                Else
                    .Width = m_lWidth
                    .Height = m_lHeight
                End If
                .Visible = True
                .Tag = CStr(vTag(0) * 10000 + vTag(1))
                ''Developer Guide No. 60
                .BackColor = IIf(PBRiskScreenCommon2.IsInputControl(txtText(m_lTextIndex), g_vDataDictionary, 15), Color.White, SystemColors.GrayText)
                .Text = ""
            End With
            textLabelMouseMove("", MouseButtons.Left, PBRiskScreenCommon.g_lx, PBRiskScreenCommon.g_ly, txtText(m_lTextIndex), lblTextLabel(m_lTextIndex), pnlPosition, m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, CInt(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, iFrameIndex))))
            If ((txtText(m_lTextIndex).Left) + (txtText(m_lTextIndex).Width)) > ((fraFrame(iFrameIndex).Width) - VB6.TwipsToPixelsX(120)) Then
                txtText(m_lTextIndex).Left = ((fraFrame(iFrameIndex).Width) - (txtText(m_lTextIndex).Width) - VB6.TwipsToPixelsX(120))
                lblTextLabel(m_lTextIndex).Left = ((txtText(m_lTextIndex).Left) - (lblTextLabel(m_lTextIndex).Width) - VB6.TwipsToPixelsX(PBRiskScreenCommon.cControlHorizontalOffset))
            End If

            ''Developer Guide No.  74
            SetToolTip(txtText(m_lTextIndex), lblTextLabel(m_lTextIndex), sObjectName, sPropertyName)

            'Developer Guide No.  183
            If m_lTextIndex = 0 Then
                AddHandler lblTextLabel(m_lTextIndex).MouseMove, AddressOf lblTextLabel_MouseMove
                AddHandler lblTextLabel(m_lTextIndex).MouseDown, AddressOf lblTextLabel_MouseDown
                AddHandler lblTextLabel(m_lTextIndex).MouseUp, AddressOf lblTextLabel_MouseUp
                AddHandler lblTextLabel(m_lTextIndex).TextChanged, AddressOf lblTextLabel_TextChanged

                AddHandler txtText(m_lTextIndex).MouseMove, AddressOf txtText_MouseMove
                AddHandler txtText(m_lTextIndex).KeyDown, AddressOf txtText_KeyDown
                AddHandler txtText(m_lTextIndex).MouseUp, AddressOf txtText_MouseUp
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTextBox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTextBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            '    Resume
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: EntryRequirements
    '
    ' Description:
    '
    ' History: 27/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function EntryRequirements() As Integer

        Dim result As Integer = 0
        Dim lTag, lPreQuote, lPostQuote, lPurchase As Integer
        Dim sCaption As String = ""
        Dim oFrmEntry As frmEntry

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case PBRiskScreenCommon.g_sControlName
                Case "chkYesNo"
                    lTag = CInt(Convert.ToString(chkYesNo(m_lIndex).Tag))
                    lPreQuote = CInt(m_vCheckArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex))
                    lPostQuote = CInt(m_vCheckArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex))
                    lPurchase = CInt(m_vCheckArray(PBRiskScreenCommon.ACCPurchase, m_lIndex))
                    sCaption = lblCheckLabel(m_lIndex).Text
                Case "cmdCommand"
                    lTag = CInt(Convert.ToString(cmdCommand(m_lIndex).Tag))
                    lPreQuote = CInt(m_vCommandArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex))
                    lPostQuote = CInt(m_vCommandArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex))
                    lPurchase = CInt(m_vCommandArray(PBRiskScreenCommon.ACCPurchase, m_lIndex))
                    sCaption = cmdCommand(m_lIndex).Text
                Case "lblComboLabel"
                    lTag = CInt(Convert.ToString(cboCombo(m_lIndex).Tag))
                    lPreQuote = CInt(m_vComboArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex))
                    lPostQuote = CInt(m_vComboArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex))
                    lPurchase = CInt(m_vComboArray(PBRiskScreenCommon.ACCPurchase, m_lIndex))
                    sCaption = lblComboLabel(m_lIndex).Text
                Case "lblTextLabel"
                    lTag = CInt(Convert.ToString(txtText(m_lIndex).Tag))
                    lPreQuote = CInt(m_vTextArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex))
                    lPostQuote = CInt(m_vTextArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex))
                    lPurchase = CInt(m_vTextArray(PBRiskScreenCommon.ACCPurchase, m_lIndex))
                    sCaption = lblTextLabel(m_lIndex).Text
                Case "lblDateLabel"
                    ' TBD lTag = txtDate(m_lIndex).Tag
                    lPreQuote = CInt(m_vDateArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex))
                    lPostQuote = CInt(m_vDateArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex))
                    lPurchase = CInt(m_vDateArray(PBRiskScreenCommon.ACCPurchase, m_lIndex))
                    sCaption = lblDateLabel(m_lIndex).Text

            End Select

            oFrmEntry = New frmEntry()


            'Get the other things as well
            oFrmEntry.PreQuote = lPreQuote
            oFrmEntry.PostQuote = lPostQuote
            oFrmEntry.Purchase = lPurchase
            '    oFrmEntry.ControlName = g_vDataDictionary(ACPPropertyName, lTag)
            oFrmEntry.ControlName = PBRiskScreenCommon.StripColonFromCaption(sCaption)


            'Akash: commented because ShowDialog automaticaaly call the form_load event
            'Load(oFrmEntry)
            oFrmEntry.ShowDialog()

            If oFrmEntry.Status = gPMConstants.PMEReturnCode.PMOK Then
                lPreQuote = oFrmEntry.PreQuote
                lPostQuote = oFrmEntry.PostQuote
                lPurchase = oFrmEntry.Purchase

                Select Case PBRiskScreenCommon.g_sControlName
                    Case "chkYesNo"
                        m_vCheckArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex) = lPreQuote
                        m_vCheckArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex) = lPostQuote
                        m_vCheckArray(PBRiskScreenCommon.ACCPurchase, m_lIndex) = lPurchase
                    Case "cmdCommand"
                        m_vCommandArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex) = lPreQuote
                        m_vCommandArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex) = lPostQuote
                        m_vCommandArray(PBRiskScreenCommon.ACCPurchase, m_lIndex) = lPurchase
                    Case "lblComboLabel"
                        m_vComboArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex) = lPreQuote
                        m_vComboArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex) = lPostQuote
                        m_vComboArray(PBRiskScreenCommon.ACCPurchase, m_lIndex) = lPurchase
                    Case "lblTextLabel"
                        m_vTextArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex) = lPreQuote
                        m_vTextArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex) = lPostQuote
                        m_vTextArray(PBRiskScreenCommon.ACCPurchase, m_lIndex) = lPurchase
                    Case "lblDateLabel"
                        m_vDateArray(PBRiskScreenCommon.ACCPreQuote, m_lIndex) = lPreQuote
                        m_vDateArray(PBRiskScreenCommon.ACCPostQuote, m_lIndex) = lPostQuote
                        m_vDateArray(PBRiskScreenCommon.ACCPurchase, m_lIndex) = lPurchase

                End Select
            End If

            'Do we need to do this?
            oFrmEntry.Close()
            oFrmEntry = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EntryRequirements Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EntryRequirements", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteTabContents
    '
    ' Description:
    '
    ' History: 12/07/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteTabContents(ByRef lTab As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vFrameArray) Then
                Return result
            End If

            For lTemp As Integer = m_vFrameArray.GetLowerBound(1) To m_vFrameArray.GetUpperBound(1)
                If (CDbl(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lTemp)) = lTab) And m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, lTemp) <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = DeleteFrameContents(lIndex:=lTemp)
                    m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMTrue
                End If
            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTabContents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTabContents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DeleteFrameContents
    '
    ' Description:
    '
    ' History: 27/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteFrameContents(ByRef lIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lTag As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 181
            For Each ctlControl As Control In Me.Controls
                result = CheckControl(ctlControl, lIndex)
            Next ctlControl




            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DeleteFrameContents Failed", ACApp, ACClass, "DeleteFrameContents", Information.Err().Number, excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    'Developer Guide No. 181
    Private Function CheckControl(ByVal ctlControl As Control, ByRef lIndex As Integer) As Integer
        Dim result As Integer = 0
        Dim lTag As String = ""
        Dim arrControlList As ArrayList = New ArrayList
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            If ctlControl.HasChildren Then
                ControlList(ctlControl, arrControlList)
            End If

            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim ctlTabControl As Control = arrControlList(arrItem)
                If ctlTabControl.Parent.Name.Contains("fraFrame") Then
                    If ContainerHelper.GetControlIndex(ctlTabControl.Parent) = lIndex Then
                        'If IndexOf(ctlTabControl.Parent) = lIndex Then
                        ControlHelper.SetVisible(ctlTabControl, False)
                        lTag = CStr(-1)
                        Dim checkST As String = ""
                        checkST = ctlTabControl.Name
                        If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
                            checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                        ElseIf checkST.StartsWith("_") Then
                            checkST = checkST.Substring(1, checkST.Length - 2)
                        End If
                        Select Case checkST
                            'Select Case ctlTabControl.Name
                            Case "fraFrame"
                                m_lReturn = CheckControl(ctlTabControl, ContainerHelper.GetControlIndex(ctlTabControl))
                                m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "chkYesNo"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vCheckArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "cmdCommand"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vCommandArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "cboCombo"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vComboArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "txtText"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vTextArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "txtDate"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vDateArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "lvwListView"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vListViewArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "uctStandardWording1"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vStandardWordingArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "uctSumInsured1"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vSumInsuredArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue
                            Case "uctAddress1"
                                lTag = Convert.ToString(ControlHelper.GetTag(ctlTabControl))
                                m_vAddressArray(PBRiskScreenCommon.ACCIsDeleted, ContainerHelper.GetControlIndex(ctlTabControl)) = gPMConstants.PMEReturnCode.PMTrue

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  BEGIN
                                '                   New claims specials : Reserve and Payment
                                '------------------------------------------------------------------------------
                            Case "uctClaimReserve1"
                                lTag = Convert.ToString(uctClaimReserve1.Tag)
                                uctClaimReserve1.Tag = ""
                            Case "uctClaimPayment1"
                                lTag = Convert.ToString(uctClaimPayment1.Tag)
                                uctClaimPayment1.Tag = ""
                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Case "uctCLMPerilDT1"
                                lTag = Convert.ToString(uctCLMPerilDT1.Tag)
                                uctCLMPerilDT1.Tag = ""
                            Case "uctCLMCaseHeader1"
                                lTag = Convert.ToString(uctCLMCaseHeader1.Tag)
                                uctCLMCaseHeader1.Tag = ""
                            Case "uctCLMCaseClaim1"
                                lTag = Convert.ToString(uctCLMCaseClaim1.Tag)
                                uctCLMCaseClaim1.Tag = ""
                        End Select

                        If StringsHelper.ToDoubleSafe(lTag) >= 0 Then 'check if attribute bound control
                            m_vInUse(GISDataModelType.GISDMTypeRisk)(CInt(CDbl(lTag) Mod 10000)) = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
                'End If
                'End If
            Next
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DeleteFrameContents Failed", ACApp, ACClass, "DeleteFrameContents", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try

    End Function
    'Developer Guide No. 181
    Private Sub cboCombo_MouseMove(ByVal Sender As Object, ByVal e As uctSimulateCombo.MouseMoveEventArgs)
        Dim Index As Integer = Array.IndexOf(cboCombo, Sender)
        Dim Button As Integer = e.Button
        Dim Shift As Integer = e.Shift
        Dim x As Single = e.x
        Dim y As Single = e.y
        Dim iPointer As Cursor
        Static bSizing, bNorth, bEast As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        'Developer Guide No. 74
        Dim lTop As Integer = CInt((cboCombo(Index).Top))
        Dim lHeight As Integer = CInt((cboCombo(Index).Height))
        Dim lWidth As Integer = CInt((cboCombo(Index).Width))

        If Not bSizing Then
            bEast = (x > (lWidth - CInt(VB6.TwipsToPixelsX(90))))

            If bEast Then
                iPointer = Cursors.SizeWE
                'Akash: added
                cboCombo(Index).Cursor = iPointer
            End If

            'Akash: commented
            'cboCombo(Index).MousePointer = iPointer

        End If

        Select Case Button
            ' Case MouseButtonConstants.LeftButton
            Case MouseButtons.Left
                bSizing = True
                If bNorth Then
                    lTop = CInt(lTop + y)
                    lHeight = CInt(lHeight - y)
                End If

                'Developer Guide No. 74
                If bEast Then
                    lWidth = CInt(x)
                    If lWidth > CInt((Math.Round(100 / 15))) And (cboCombo(Index).Left + lWidth) < (cboCombo(Index).Parent.Width - CInt(VB6.TwipsToPixelsX(100))) Then
                        cboCombo(Index).Width = (lWidth)
                    End If
                End If

                lblComboLabel(Index).Top = cboCombo(Index).Top + VB6.TwipsToPixelsY(45)

                With cboCombo(Index)
                    SetSizeAndPosition(CInt(.Left), CInt(.Top), CInt(.ComboWidth), CInt(.Height))
                End With

            Case Else
                bSizing = False
        End Select


    End Sub

    'Developer Guide No. 181
    'Private Sub chkYesNo_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
    Private Sub chkYesNo_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) ' Handles _chkYesNo_0.CheckedChanged

        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)
        PBRiskScreenCommon2.simulateTriStateCheckBox(PBRiskScreenCommon.g_iCheckBoxValue, chkYesNo(Index))
    End Sub
    'Developer Guide No.181 
    Private Sub chkYesNo_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)
        PBRiskScreenCommon2.simulateTriStateCheckBox(PBRiskScreenCommon.g_iCheckBoxValue, chkYesNo(Index))

        PBRiskScreenCommon.g_iCheckBoxValue = chkYesNo(Index).CheckState
        GenericClickEvent("chkYesNo", m_vCheckArray, chkYesNo, lblCheckLabel, Index, Button, Shift, x, y)
    End Sub


    Private Sub cmdDynamicLogic_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDynamicLogic.Click
        RuleEditor(RuleEditorDynamicValidation)
    End Sub
    
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        m_lInitialControlsCount = ContainerHelper.Controls(Me).Count

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            'Dim lButtonOffset As Integer = CInt(VB6.PixelsToTwipsX(cmdCancel.Width) + 100)
            'Dim lButtonTop As Integer = CInt(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(cmdCancel.Height) - 660)

            'tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(330)
            'tabMainTab.Height = VB6.TwipsToPixelsY(lButtonTop - 330)

            'picScreenMain.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMainTab.Width) - VB6.PixelsToTwipsX(tvwDataDictionary.Width) - 860)
            'picScreenMain.Height = tabMainTab.Height - VB6.TwipsToPixelsY(1660)
            'tvwDataDictionary.Height = picScreenMain.Height

            'HScroll1.Left = 0
            'HScroll1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(picScreenMain.Height) - VB6.PixelsToTwipsY(HScroll1.Height) - 60)
            'HScroll1.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picScreenMain.Width) - VB6.PixelsToTwipsX(VScroll1.Width) - 60)

            'VScroll1.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picScreenMain.Width) - VB6.PixelsToTwipsX(VScroll1.Width) - 60)
            'VScroll1.Top = 0
            'VScroll1.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(picScreenMain.Height) - VB6.PixelsToTwipsY(HScroll1.Height) - 60)

            'picScreen_Resize(picScreen, New EventArgs())

            'cmdCopy.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 8)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdDefaults.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 7)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdDynamicLogic.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 6)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdValidation.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 5)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdSave.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 4)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdOK.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 3)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdCancel.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - (lButtonOffset * 2)), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdHelp.SetBounds(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 230 - lButtonOffset), VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdNavigate.SetBounds(cmdNavigate.Left, VB6.TwipsToPixelsY(lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)





            Dim lButtonOffset As Integer = (cmdCancel.Width) + CInt(VB6.TwipsToPixelsX(100))
            'Dim lButtonTop As Integer = (Me.Height) - (cmdCancel.Height) - CInt(VB6.TwipsToPixelsY(660))
            Dim lButtonTop As Integer = (Me.Height) - (cmdCancel.Height) - 45 ' CInt(VB6.TwipsToPixelsY(460))

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(330)
            tabMainTab.Height = lButtonTop - 70 ' VB6.TwipsToPixelsY(330)

            picScreenMain.Width = tabMainTab.Width - (tvwDataDictionary.Width) - VB6.TwipsToPixelsX(860)
            picScreenMain.Height = tabMainTab.Height - VB6.TwipsToPixelsY(1660)
            tvwDataDictionary.Height = picScreenMain.Height

            HScroll1.Left = 0
            HScroll1.Top = (picScreenMain.Height) - (HScroll1.Height) - VB6.TwipsToPixelsY(60)
            HScroll1.Width = (picScreenMain.Width) - (VScroll1.Width) - VB6.TwipsToPixelsX(60)

            VScroll1.Left = (picScreenMain.Width) - (VScroll1.Width) - VB6.TwipsToPixelsX(60)
            VScroll1.Top = 0
            VScroll1.Height = (picScreenMain.Height) - (HScroll1.Height) - VB6.TwipsToPixelsY(60)

            picScreen_Resize(picScreen, New EventArgs())

            'cmdCopy.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 8)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdDefaults.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 7)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdDynamicLogic.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 6)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdValidation.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 5)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdSave.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 4)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdOK.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 3)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdCancel.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - (lButtonOffset * 2)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdHelp.SetBounds((Me.Width - VB6.TwipsToPixelsX(230) - lButtonOffset), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            'cmdNavigate.SetBounds(cmdNavigate.Left, (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

            cmdCopy.SetBounds((Me.Width - 12 - (lButtonOffset * 8)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdDefaults.SetBounds((Me.Width - 12 - (lButtonOffset * 7)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdDynamicLogic.SetBounds((Me.Width - 12 - (lButtonOffset * 6)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdValidation.SetBounds((Me.Width - 12 - (lButtonOffset * 5)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdSave.SetBounds((Me.Width - 12 - (lButtonOffset * 4)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdOK.SetBounds((Me.Width - 12 - (lButtonOffset * 3)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdCancel.SetBounds((Me.Width - 12 - (lButtonOffset * 2)), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdHelp.SetBounds((Me.Width - 12 - lButtonOffset), (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdNavigate.SetBounds(cmdNavigate.Left, (lButtonTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)


        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    'Developer Guide No. 181
    Private Sub HScroll1_Change(ByVal newScrollValue As Integer)
        picScreen.Left = ((-1) * (newScrollValue / (HScroll1.Maximum - (HScroll1.LargeChange + 1))) * lHScrollMultiplier)
    End Sub

    Private Sub lblCheckLabel_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblCheckLabel_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblCheckLabel, eventSender)
        'make sure control is snapped to grid (if required)
        textLabelMouseMove("chkYesNo", Button, x, y, chkYesNo(Index), lblCheckLabel(Index), pnlPosition, m_iSnapToGrid)
        SetSizeAndPosition()
    End Sub
    'Developer Guide No. 181
    Private Sub chkYesNo_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        SetSizeAndPosition()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            If m_lSetTabOrderCount <> 0 Then
                m_lSetTabOrderCount = 0
                EnableControlsAfterTabSet(True)
                MessageBox.Show("Tab order set has been cancelled", "Set Control's tab order", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCommand_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _cmdCommand_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(cmdCommand, eventSender)

        PBRiskScreenCommon.g_lx = CInt(x)
        PBRiskScreenCommon.g_ly = CInt(y)

        'Developer Guide No. 74
        SetSizeAndPosition((pnlPanel(Index).Left), (pnlPanel(Index).Top))

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                SetControlTabIndex(m_vCommandArray(PBRiskScreenCommon.ACCTabSetIndex, Index), pnlPanel(Index), 1)

                'Developer Guide No. 64
            Case MouseButtons.Right

                If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

                'Menu time
                PBRiskScreenCommon.g_sControlName = "cmdCommand"
                m_lIndex = Index

                If m_lGISObjectId = 0 Then
                    mnuControlIncludeInList.Available = False
                Else
                    mnuControlIncludeInList.Available = True
                    mnuControlIncludeInList.Checked = CBool(m_vCommandArray(PBRiskScreenCommon.ACCIncludeInList, Index))
                End If
                mnuControlSetListColumnOrder.Available = mnuControlIncludeInList.Available

                mnuControlFormat.Available = False
                Ctx_mnuControl.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                SetSizeAndPosition()
        End Select

    End Sub

    Private Sub cmdCommand_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _cmdCommand_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(cmdCommand, eventSender)

        Dim lTop, lLeft, lRight As Integer

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                'Developer Guide No. 74
                lTop = CInt((cmdCommand(Index).Top) - PBRiskScreenCommon.g_ly + y)
                lLeft = CInt((cmdCommand(Index).Left) - PBRiskScreenCommon.g_lx + x)
                lRight = CInt(lLeft + (cmdCommand(Index).Width) + (pnlPanel(Index).Width))

                If lTop > VB6.TwipsToPixelsY(120) Then
                    If lTop < ((cmdCommand(Index).Parent.Height) - VB6.TwipsToPixelsY(420)) Then
                        cmdCommand(Index).Top = (cmdCommand(Index).Top - PBRiskScreenCommon.g_ly + y)
                        pnlPanel(Index).Top = cmdCommand(Index).Top

                    End If
                End If

                If lLeft > VB6.TwipsToPixelsY(120) Then
                    If lRight < (cmdCommand(Index).Parent.Width - VB6.TwipsToPixelsX(120)) Then
                        cmdCommand(Index).Left = (cmdCommand(Index).Left - PBRiskScreenCommon.g_lx + x)
                        pnlPanel(Index).Left = cmdCommand(Index).Left + cmdCommand(Index).Width
                    End If
                End If

                SetSizeAndPosition(CInt(pnlPanel(Index).Left), CInt(pnlPanel(Index).Top))

        End Select

    End Sub

    Private Sub cmdCommand_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _cmdCommand_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        SetSizeAndPosition()
    End Sub


    Private Sub cmdCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopy.Click
        m_lReturn = CopyScreen()
    End Sub

    Private Sub cmdDefaults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefaults.Click
        RuleEditor(RuleEditorDefaults)
    End Sub


    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate


            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CheckCompiledRuleValues()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSave.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CheckCompiledRuleValues()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_iTask = gPMConstants.PMEComponentAction.PMEdit

                cmdValidation.Enabled = True
                cmdDefaults.Enabled = True
                cmdCopy.Enabled = True
                cmdDynamicLogic.Enabled = True
                'Start(Sriram P)CacheBug

                m_lReturn = UpdateCache()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
                'End(Sriram P)CacheBug
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Save command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSave_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdValidation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdValidation.Click
        RuleEditor(RuleEditorPostSaveValidation)
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUMaintainScreenData.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(r_frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'Set this up else we always lose the first description and caption
            m_lTag = -1

            m_lTextIndex = -1
            m_lCheckIndex = -1
            m_lComboIndex = -1
            m_lFrameIndex = -1
            m_lPanelIndex = -1
            m_lCommandIndex = -1
            m_lListViewIndex = -1
            m_lStandardWordingIndex = -1
            m_lSumInsuredIndex = -1
            m_lAddressIndex = -1
            m_lFindControlIndex = -1

            m_lTabIndex = 0

            ReDim m_vTabArray(PBRiskScreenCommon.ACFLastArrayPosition, m_lTabIndex)

            m_vTabArray(PBRiskScreenCommon.ACFTabNumber, m_lTabIndex) = m_lTabIndex
            m_vTabArray(PBRiskScreenCommon.ACFIsDeleted, m_lTabIndex) = gPMConstants.PMEReturnCode.PMFalse

            m_vTabArray(PBRiskScreenCommon.ACFChildId, m_lTabIndex) = DBNull.Value

            m_vTabArray(PBRiskScreenCommon.ACFGISObjectId, m_lTabIndex) = DBNull.Value


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Function Initialise() As Object

        ' Forms load event.
        iPMFunc.ForceForegroundWindow(Me.Handle.ToInt32())

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Function
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Function
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Function
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Function
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function
        End Try

    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                Status = gPMConstants.PMEReturnCode.PMCancel
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

                m_oBusiness.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing
            End If

            If Not (m_oFormFields Is Nothing) Then

                ' Terminate the form control object.
                m_oFormFields.Dispose()
                ' Destroy the instance of the form control object
                ' from memory.
                m_oFormFields = Nothing

            End If

            'Reset the arrays
            ResetArrays()

            ' Reset the mouse pointer to normal.

            m_lReturn = ClearScreen()
            Erase m_ctlTabFirstLast

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                    Case Keys.Escape
                        cmdCancel.Focus()
                        cmdCancel.PerformClick()

                End Select
            End With

        Catch




            Exit Sub
        End Try


    End Sub


    'Developer Guide No.  181
    Public Sub fraFrame_DragDrop(ByVal eventSender As Object, ByVal EventArgs As System.Windows.Forms.DragEventArgs) 'Handles _fraFrame_0.DragDrop, _fraFrame_0.DragOver

        'Set m_oNode = Nothing
        Dim Index As Integer = Array.IndexOf(fraFrame, eventSender)
        'Developer Guide No. 74
        Dim pt As New Point
        pt = CType(eventSender, GroupBox).PointToClient(New Point(EventArgs.X, EventArgs.Y))


        Dim x As Single = pt.X 'EventArgs.X
        Dim y As Single = pt.Y ' EventArgs.Y

        'EventArgs.Effect = DragDropEffects.Copy
        'Developer Guide No. 63
        m_lReturn = AddControl(Index, m_oNode.Tag, x, y)

    End Sub


    Public Sub fraFrame_DragEnter(ByVal eventSender As Object, ByVal EventArgs As System.Windows.Forms.DragEventArgs) 'Handles _fraFrame_0.DragDrop, _fraFrame_0.DragOver
        EventArgs.Effect = DragDropEffects.Move
        iPMFunc.SetMousePointer(2)
    End Sub

    'Developer Guide No. 181
    Public Sub fraFrame_MouseDown(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) 'Handles _fraFrame_0.MouseDown

        Try
            Dim Index As Integer = ContainerHelper.GetControlIndex(eventSender)

            Dim x As Single = eventArgs.X 'e.X
            Dim y As Single = eventArgs.Y ' e.Y

            SetSizeAndPosition()

            Dim iTabIncrement As Integer

            ' Developer Guide No. Added to get control list 
            Dim arrControl As ArrayList = New ArrayList()
            ControlList(Me, arrControl)


            Select Case eventArgs.Button 'Button
                Case MouseButtons.Left  'MouseButtonConstants.LeftButton

                    If m_lSetTabOrderCount > 0 Then

                        For ctlControl As Integer = 0 To arrControl.Count - 1

                            If HasVisibleProperty(arrControl(ctlControl)) Then
                                'Developer Guide No. 62
                                If Not IsNothing(arrControl(ctlControl).Parent) Then
                                    If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                        If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                            If ControlHelper.GetVisible(arrControl(ctlControl)) Then
                                                Dim checkST As String = ""
                                                checkST = arrControl(ctlControl).Name
                                                If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
                                                    checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                                                ElseIf checkST.StartsWith("_") Then
                                                    checkST = checkST.Substring(1, checkST.Length - 2)
                                                End If
                                                Select Case checkST
                                                    Case "lvwListView"
                                                        iTabIncrement = 4
                                                    Case "uctStandardWording1"
                                                        iTabIncrement = 6
                                                    Case "uctSumInsured1"
                                                        iTabIncrement = 7
                                                    Case "uctAddress1"
                                                        iTabIncrement = 1
                                                    Case "uctCLMPerilDT1", "uctClaimReserve1", "uctClaimPayment1", "uctCLMCaseHeader1", "uctCLMCaseClaim1"
                                                        iTabIncrement = 2
                                                End Select

                                                If iTabIncrement > 0 Then
                                                    SetControlTabIndex(m_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, Index), fraFrame(Index), iTabIncrement)
                                                    Exit For
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        'Next ctlControl

                    Else



                        PBRiskScreenCommon.g_lx = CInt(x)
                        PBRiskScreenCommon.g_ly = CInt(y)

                        ''Developer Guide No. 74
                        m_lBottom = CInt(fraFrame(Index).Top + fraFrame(Index).Height)
                        m_lRight = CInt(fraFrame(Index).Left + fraFrame(Index).Width)
                        m_lHeight = CInt(fraFrame(Index).Height)
                        m_lWidth = CInt(fraFrame(Index).Width)


                        m_lListViewInThisFrame = -1
                        m_lStandardWordingInThisFrame = -1
                        m_lSumInsuredInThisFrame = -1

                        m_lAddressInThisFrame = -1

                        m_lClaimReserveInThisFrame = False
                        m_lClaimPaymentInThisFrame = False
                        m_lClaimPerilInThisFrame = False

                        ''Developer Guide No. 74
                        m_lMinimumWidth = VB6.TwipsToPixelsX(1000)
                        m_lMinimumHeight = VB6.TwipsToPixelsY(1000)

                        For ctlControl As Integer = 0 To arrControl.Count - 1
                            If HasVisibleProperty(ctlControl) Then
                                If ControlHelper.GetVisible(arrControl(ctlControl)) Then
                                    If Information.Err().Number <> 0 Then
                                        Information.Err().Clear()
                                    Else
                                        Dim checkST As String = ""
                                        checkST = arrControl(ctlControl).Name
                                        If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
                                            checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                                        ElseIf checkST.StartsWith("_") Then
                                            checkST = checkST.Substring(1, checkST.Length - 2)
                                        End If
                                        Select Case checkST
                                            Case "lvwListView"
                                                'If ctlControl.Parent.Name = "fraFrame" Then
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lListViewInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                                        'Developer Guide No. 74
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(1500)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(3000)
                                                    End If
                                                End If
                                            Case "uctStandardWording1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lStandardWordingInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctStandardWording1(m_lStandardWordingInThisFrame).MinimumHeight()) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctStandardWording1(m_lStandardWordingInThisFrame).MinimumWidth()) + VB6.TwipsToPixelsX(180)
                                                    End If
                                                End If
                                            Case "uctSumInsured1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lSumInsuredInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                                        'Developer Guide No. 74
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctSumInsured1(m_lSumInsuredInThisFrame).MinimumHeight()) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctSumInsured1(m_lSumInsuredInThisFrame).MinimumWidth()) + VB6.TwipsToPixelsX(180)
                                                    End If
                                                End If
                                            Case "uctAddress1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lAddressInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctAddress1(m_lAddressInThisFrame).MinimumHeight()) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctAddress1(m_lAddressInThisFrame).MinimumWidth()) + VB6.TwipsToPixelsX(300)
                                                    End If
                                                End If

                                                '------------------------------------------------------------------------------
                                                '   12/07/2002 RVH  BEGIN
                                                '                   New special controls for claims reserve and payment
                                                '------------------------------------------------------------------------------
                                            Case "uctClaimReserve1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lClaimReserveInThisFrame = True
                                                        'Developer Guide No. 74
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctClaimReserve1.MinimumHeight) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctClaimReserve1.MinimumWidth) + VB6.TwipsToPixelsX(180)
                                                    End If
                                                End If
                                            Case "uctClaimPayment1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lClaimPaymentInThisFrame = True
                                                        'Developer Guide No. 74
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctClaimPayment1.MinimumHeight) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctClaimPayment1.MinimumWidth) + VB6.TwipsToPixelsX(180)
                                                    End If
                                                End If
                                            Case "uctCLMPerilDT1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lClaimPerilInThisFrame = True
                                                        'Developer Guide No. 74
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctCLMPerilDT1.MinimumHeight) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctCLMPerilDT1.MinimumWidth) + VB6.TwipsToPixelsY(180)
                                                    End If
                                                End If
                                            Case "uctCLMCaseClaim1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lCaseClaimListInThisFrame = True
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctCLMCaseClaim1.MinimumHeight) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctCLMCaseClaim1.MinimumWidth) + VB6.TwipsToPixelsY(180)
                                                    End If
                                                End If
                                            Case "uctCLMCaseHeader1"
                                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                        m_lCaseClaimHeaderInThisFrame = True
                                                        m_lMinimumHeight = VB6.TwipsToPixelsY(uctCLMCaseHeader1.MinimumHeight) + VB6.TwipsToPixelsY(300)
                                                        m_lMinimumWidth = VB6.TwipsToPixelsX(uctCLMCaseHeader1.MinimumWidth) + VB6.TwipsToPixelsY(180)
                                                    End If
                                                End If
                                                '------------------------------------------------------------------------------
                                                '   12/07/2002 RVH  END
                                                '------------------------------------------------------------------------------
                                            Case Else
                                                'Developer Guide No. 62
                                                If Not IsNothing(arrControl(ctlControl).Parent) Then
                                                    If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                                        If Information.Err().Number <> 0 Then
                                                            Information.Err().Clear()
                                                        Else
                                                            If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                                                'Don't worry about the list view buttons, it's
                                                                'all taken care of above...
                                                                Select Case checkST
                                                                    Case "cmdListViewAdd", "cmdListViewEdit", "cmdListViewDelete", "", ""
                                                                    Case Else
                                                                        If ((arrControl(ctlControl).Left) + (arrControl(ctlControl).Width)) > m_lMinimumWidth Then
                                                                            m_lMinimumWidth = CInt((arrControl(ctlControl).Left) + (arrControl(ctlControl).Width) + Math.Round(180 / 15))
                                                                        End If
                                                                        If ((arrControl(ctlControl).Top) + (arrControl(ctlControl).Height)) > m_lMinimumHeight Then
                                                                            m_lMinimumHeight = CInt((arrControl(ctlControl).Top) + (arrControl(ctlControl).Height) + Math.Round(300 / 15))
                                                                        End If
                                                                End Select
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                    End If
                                End If
                            End If
                        Next ctlControl
                    End If

                Case MouseButtons.Right 'MouseButtonConstants.RightButton

                    If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

                    'Menu time
                    PBRiskScreenCommon.g_sControlName = "fraFrame"
                    m_lIndex = Index

                    m_lSumInsuredInThisFrame = -1
                    m_lListViewInThisFrame = -1

                    For ctlControl As Integer = 0 To arrControl.Count - 1
                        Dim checkST As String = ""
                        checkST = arrControl(ctlControl).Name
                        If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
                            checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                        ElseIf checkST.StartsWith("_") Then
                            checkST = checkST.Substring(1, checkST.Length - 2)
                        End If
                        Select Case checkST
                            Case "uctSumInsured1"
                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                        m_lSumInsuredInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                    End If
                                End If
                            Case "lvwListView"
                                If arrControl(ctlControl).Parent.Name.Contains("fraFrame") Then
                                    If ContainerHelper.GetControlIndex(arrControl(ctlControl).Parent) = Index Then
                                        m_lListViewInThisFrame = ContainerHelper.GetControlIndex(arrControl(ctlControl))
                                    End If
                                End If
                        End Select
                    Next ctlControl
                    If m_lSumInsuredInThisFrame > -1 Then
                        mnuFrameShowRateAndPremium.Available = True
                        mnuFrameShowRateAndPremium.Checked = uctSumInsured1(m_lSumInsuredInThisFrame).ShowRateAndPremium
                        mnuFrameShowValuation.Available = True
                        mnuFrameShowValuation.Checked = uctSumInsured1(m_lSumInsuredInThisFrame).ShowValuation
                    Else
                        mnuFrameShowRateAndPremium.Available = False
                        mnuFrameShowValuation.Available = False
                    End If

                    '        If (m_lListViewInThisFrame > -1) Then
                    '            mnuFrameShowDetails.Visible = True
                    '        Else
                    '            mnuFrameShowDetails.Visible = False
                    '        End If
                    Ctx_mnuFrame.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            End Select

        Catch exc As System.Exception
            'Developer Guide No.32
        End Try

    End Sub


    'Developer Guide No. 181
    Public Sub fraFrame_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)

        'Developer Guide No. 181
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Index As Integer = Array.IndexOf(fraFrame, eventSender)
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        Dim lOldValue, lHeight, lWidth As Integer
        Dim iPointer As Cursor
        Dim bHasSequenceButtons As Boolean

        Static bSizing, bEast, bNorth, bSouth, bWest As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        If Not bSizing Then
            'Developer Guide No. 74
            bNorth = (y < CInt(VB6.TwipsToPixelsY(200)))
            bSouth = (y > (fraFrame(Index).Height - CInt(VB6.TwipsToPixelsY(200))))
            bWest = (x < 200 / 20)
            bEast = (x > (fraFrame(Index).Width - CInt(VB6.TwipsToPixelsX(200))))


            If (bEast And bNorth) Or (bWest And bSouth) Then
                iPointer = Cursors.SizeNESW
            ElseIf ((bEast And bSouth) Or (bWest And bNorth)) Then
                iPointer = Cursors.SizeNWSE
            ElseIf (bEast Or bWest) Then
                iPointer = Cursors.SizeWE
            ElseIf (bSouth Or bNorth) Then
                iPointer = Cursors.SizeNS
            End If

            fraFrame(Index).Cursor = iPointer
        End If

        If m_lListViewInThisFrame <> -1 Then
            'Developer Guide No. 74
            bHasSequenceButtons = (fraFrame(Index).Width - lvwListView(m_lListViewInThisFrame).Width > VB6.TwipsToPixelsX(495))
        End If

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                bSizing = True
                If bNorth Then
                    'Developer Guide No. 74
                    lOldValue = CInt(fraFrame(Index).Top)
                    lHeight = CInt(m_lBottom - lOldValue + PBRiskScreenCommon.g_ly - y)

                    If (lOldValue - PBRiskScreenCommon.g_ly + y) < VB6.TwipsToPixelsX(360) Then
                    Else
                        If lHeight < m_lMinimumHeight Then
                        Else
                            If m_lListViewInThisFrame > -1 Then

                                'Developer Guide No. 74
                                bHasSequenceButtons = (fraFrame(Index).Width - lvwListView(m_lListViewInThisFrame).Width > Math.Round(495 / 15))

                                'Developer Guide No. 74
                                fraFrame(Index).Top = lOldValue - PBRiskScreenCommon.g_ly + y
                                fraFrame(Index).Height = m_lBottom - fraFrame(Index).Top
                                lvwListView(m_lListViewInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(720)
                                cmdListViewAdd(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewEdit(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewDelete(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)


                                If bHasSequenceButtons Then 'if we have sequence buttons
                                    cmdListViewSequenceUp(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Top
                                    cmdListViewSequenceDown(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Top + lvwListView(m_lListViewInThisFrame).Height - cmdListViewSequenceDown(m_lListViewInThisFrame).Height
                                End If
                            ElseIf (m_lStandardWordingInThisFrame > -1) Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctStandardWording1(m_lStandardWordingInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf (m_lSumInsuredInThisFrame > -1) Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctSumInsured1(m_lSumInsuredInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf (m_lAddressInThisFrame > -1) Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctAddress1(m_lAddressInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  BEGIN
                                '                   New special controls for claims reserve and payment
                                '------------------------------------------------------------------------------
                            ElseIf m_lClaimReserveInThisFrame Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctClaimReserve1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lClaimPaymentInThisFrame Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctClaimPayment1.Height = CInt((fraFrame(Index).Height) - VB6.TwipsToPixelsY(300))
                            ElseIf m_lClaimPerilInThisFrame Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctCLMPerilDT1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lCaseClaimListInThisFrame Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctCLMCaseClaim1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lCaseClaimHeaderInThisFrame Then
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                                uctCLMCaseHeader1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Else
                                'Developer Guide No. 74
                                fraFrame(Index).Top = (lOldValue - PBRiskScreenCommon.g_ly + y)
                                fraFrame(Index).Height = (m_lBottom) - fraFrame(Index).Top
                            End If
                        End If
                    End If
                End If

                If bSouth Then
                    'Developer Guide No. 74
                    lOldValue = CInt(fraFrame(Index).Height)

                    lHeight = CInt(m_lHeight - PBRiskScreenCommon.g_ly + y)
                    'Developer Guide No. 74
                    If (fraFrame(Index).Top + lOldValue - PBRiskScreenCommon.g_ly + y) > (fraFrame(Index).Parent.Height - VB6.TwipsToPixelsY(120)) Then
                    Else
                        If lHeight < m_lMinimumHeight Then
                        Else
                            If m_lListViewInThisFrame > -1 Then
                                'Developer Guide No. 74
                                fraFrame(Index).Height = lHeight

                                lvwListView(m_lListViewInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(720)
                                cmdListViewAdd(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewEdit(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)
                                cmdListViewDelete(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Height + VB6.TwipsToPixelsY(300)


                                If bHasSequenceButtons Then 'if we have sequence buttons
                                    cmdListViewSequenceUp(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Top
                                    cmdListViewSequenceDown(m_lListViewInThisFrame).Top = lvwListView(m_lListViewInThisFrame).Top + lvwListView(m_lListViewInThisFrame).Height - cmdListViewSequenceDown(m_lListViewInThisFrame).Height
                                End If
                            ElseIf (m_lStandardWordingInThisFrame > -1) Then
                                'Developer Guide No. 74
                                fraFrame(Index).Height = lHeight
                                uctStandardWording1(m_lStandardWordingInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf (m_lSumInsuredInThisFrame > -1) Then
                                'Developer Guide No. 74
                                fraFrame(Index).Height = lHeight
                                uctSumInsured1(m_lSumInsuredInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf (m_lAddressInThisFrame > -1) Then
                                'Developer Guide No. 74

                                fraFrame(Index).Height = (lHeight)
                                uctAddress1(m_lAddressInThisFrame).Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  BEGIN
                                '                   New special controls for claims reserve and payment
                                '------------------------------------------------------------------------------
                                'Developer Guide No. 74
                            ElseIf m_lClaimReserveInThisFrame Then
                                fraFrame(Index).Height = (lHeight)
                                uctClaimReserve1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lClaimPaymentInThisFrame Then
                                fraFrame(Index).Height = (lHeight)
                                uctClaimPayment1.Height = CInt((fraFrame(Index).Height) - VB6.TwipsToPixelsY(300))
                            ElseIf m_lClaimPerilInThisFrame Then
                                fraFrame(Index).Height = (lHeight)
                                uctCLMPerilDT1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lCaseClaimListInThisFrame Then
                                fraFrame(Index).Height = (lHeight)
                                uctCLMCaseClaim1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                            ElseIf m_lCaseClaimHeaderInThisFrame Then
                                fraFrame(Index).Height = (lHeight)
                                uctCLMCaseHeader1.Height = fraFrame(Index).Height - VB6.TwipsToPixelsY(300)
                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Else
                                'Developer Guide No. 74
                                fraFrame(Index).Height = lHeight
                            End If
                        End If
                    End If
                End If

                If bWest Then
                    'Developer Guide No. 74
                    lOldValue = CInt(fraFrame(Index).Left)
                    lWidth = CInt(m_lRight - lOldValue + PBRiskScreenCommon.g_lx - x)

                    'Developer Guide No. 74
                    If (lOldValue - PBRiskScreenCommon.g_lx + x) < 1 Then 'CInt(VB6.TwipsToPixelsX(120)) Then
                    Else
                        If lWidth < m_lMinimumWidth Then
                        Else
                            'Developer Guide No. 74
                            If m_lListViewInThisFrame > -1 Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                If bHasSequenceButtons Then 'if we have sequence buttons
                                    lWidth = CInt((fraFrame(Index).Width))
                                    lvwListView(m_lListViewInThisFrame).Width = (lWidth - VB6.TwipsToPixelsX(795))
                                    cmdListViewSequenceUp(m_lListViewInThisFrame).Left = (lWidth - VB6.TwipsToPixelsX(615))
                                    cmdListViewSequenceDown(m_lListViewInThisFrame).Left = cmdListViewSequenceUp(m_lListViewInThisFrame).Left
                                Else
                                    lvwListView(m_lListViewInThisFrame).Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(240)
                                End If

                            ElseIf (m_lStandardWordingInThisFrame > -1) Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctStandardWording1(m_lStandardWordingInThisFrame).Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                            ElseIf (m_lSumInsuredInThisFrame > -1) Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctSumInsured1(m_lSumInsuredInThisFrame).Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                            ElseIf (m_lAddressInThisFrame > -1) Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctAddress1(m_lAddressInThisFrame).Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(300)

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  BEGIN
                                '                   New special controls for claims reserve and payment
                                '------------------------------------------------------------------------------
                            ElseIf m_lClaimReserveInThisFrame Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctClaimReserve1.Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                            ElseIf m_lClaimPaymentInThisFrame Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctClaimPayment1.Width = CInt(fraFrame(Index).Width - VB6.PixelsToTwipsX(180))
                            ElseIf m_lClaimPerilInThisFrame Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctCLMPerilDT1.Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                            ElseIf m_lCaseClaimListInThisFrame Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctCLMCaseClaim1.Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                            ElseIf m_lCaseClaimHeaderInThisFrame Then
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                                uctCLMCaseHeader1.Width = fraFrame(Index).Width - VB6.TwipsToPixelsX(180)
                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Else
                                fraFrame(Index).Left = (lOldValue - PBRiskScreenCommon.g_lx + x)
                                fraFrame(Index).Width = (m_lRight) - fraFrame(Index).Left
                            End If

                        End If
                    End If
                End If

                If bEast Then
                    lWidth = CInt(m_lWidth - PBRiskScreenCommon.g_lx + x)

                    'Developer Guide No.74 
                    If ((fraFrame(Index).Left) + lWidth) > ((fraFrame(Index).Parent.Width)) Then
                    Else
                        If lWidth < m_lMinimumWidth Then
                        Else
                            If m_lListViewInThisFrame > -1 Then
                                fraFrame(Index).Width = (lWidth)
                                If bHasSequenceButtons Then 'if we have sequence buttons
                                    lvwListView(m_lListViewInThisFrame).Width = (lWidth - VB6.TwipsToPixelsX(795))
                                    cmdListViewSequenceUp(m_lListViewInThisFrame).Left = (lWidth - VB6.TwipsToPixelsX(615))
                                    cmdListViewSequenceDown(m_lListViewInThisFrame).Left = cmdListViewSequenceUp(m_lListViewInThisFrame).Left
                                Else
                                    lvwListView(m_lListViewInThisFrame).Width = lWidth - VB6.TwipsToPixelsX(240)
                                End If
                            ElseIf (m_lStandardWordingInThisFrame > -1) Then
                                fraFrame(Index).Width = lWidth
                                uctStandardWording1(m_lStandardWordingInThisFrame).Width = (lWidth - VB6.TwipsToPixelsX(180))
                            ElseIf (m_lSumInsuredInThisFrame > -1) Then
                                fraFrame(Index).Width = (lWidth)
                                uctSumInsured1(m_lSumInsuredInThisFrame).Width = (lWidth - VB6.TwipsToPixelsX(180))
                            ElseIf (m_lAddressInThisFrame > -1) Then
                                fraFrame(Index).Width = (lWidth)
                                uctAddress1(m_lAddressInThisFrame).Width = (lWidth - VB6.TwipsToPixelsX(300))

                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  BEGIN
                                '                   New special controls for claims reserve and payment
                                '------------------------------------------------------------------------------
                            ElseIf m_lClaimReserveInThisFrame Then
                                fraFrame(Index).Width = (lWidth)
                                uctClaimReserve1.Width = (lWidth - VB6.TwipsToPixelsX(180))
                            ElseIf m_lClaimPaymentInThisFrame Then
                                fraFrame(Index).Width = (lWidth)
                                uctClaimPayment1.Width = lWidth - VB6.TwipsToPixelsX(180)
                            ElseIf m_lClaimPerilInThisFrame Then
                                fraFrame(Index).Width = (lWidth)
                                uctCLMPerilDT1.Width = (lWidth - VB6.TwipsToPixelsX(180))
                            ElseIf m_lCaseClaimListInThisFrame Then
                                fraFrame(Index).Width = (lWidth)
                                uctCLMCaseClaim1.Width = (lWidth - VB6.TwipsToPixelsX(180))
                            ElseIf m_lCaseClaimHeaderInThisFrame Then
                                fraFrame(Index).Width = (lWidth)
                                uctCLMCaseHeader1.Width = (lWidth - VB6.TwipsToPixelsX(180))
                                '------------------------------------------------------------------------------
                                '   12/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                            Else
                                fraFrame(Index).Width = (m_lWidth - PBRiskScreenCommon.g_lx + x)
                            End If
                        End If
                    End If
                End If

                With fraFrame(Index)
                    'Developer Guide No. 74
                    SetSizeAndPosition(CInt(.Left), CInt(.Top), CInt(.Width), CInt(.Height))
                End With

            Case Else
                bSizing = False
        End Select


    End Sub


    'Developer Guide No. 181
    Public Sub fraFrame_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        'Developer Guide No. 181
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Index As Integer = Array.IndexOf(fraFrame, eventSender)
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                fraFrame(Index).Cursor = Cursors.Default
                picScreen.Cursor = Cursors.Default
        End Select

    End Sub

    Private Sub lblCheckLabel_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _lblCheckLabel_0.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim Index As Integer = Array.IndexOf(lblCheckLabel, eventSender)
        GenericLabelChangeEvent(lblCheckLabel(Index))
    End Sub

    Private Sub lblComboLabel_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _lblComboLabel_0.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim Index As Integer = Array.IndexOf(lblComboLabel, eventSender)
        GenericLabelChangeEvent(lblComboLabel(Index))
    End Sub

    'Developer Guide No. 181
    Private Sub lblTextLabel_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim Index As Integer = Array.IndexOf(lblTextLabel, eventSender)
        GenericLabelChangeEvent(lblTextLabel(Index))
    End Sub
    'Developer Guide No. 181
    Private Sub lblCheckLabel_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblCheckLabel_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblCheckLabel, eventSender)
        GenericClickEvent("chkYesNo", m_vCheckArray, chkYesNo, lblCheckLabel, Index, Button, Shift, x, y)
    End Sub
    Private Sub lblComboLabel_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblComboLabel_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblComboLabel, eventSender)
        GenericClickEvent("lblComboLabel", m_vComboArray, cboCombo, lblComboLabel, Index, Button, Shift, x, y)
    End Sub
    Private Sub lblComboLabel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblComboLabel_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblComboLabel, eventSender)
        textLabelMouseMove("lblComboLabel", Button, x, y, cboCombo(Index), lblComboLabel(Index), pnlPosition, m_iSnapToGrid)
    End Sub
    Private Sub lblComboLabel_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblComboLabel_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblComboLabel, eventSender)
        'make sure control is snapped to grid (if required)
        textLabelMouseMove("lblComboLabel", Button, x, y, cboCombo(Index), lblComboLabel(Index), pnlPosition, m_iSnapToGrid)
        SetSizeAndPosition()
    End Sub
    Private Sub lblDateLabel_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblDateLabel_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
    End Sub

    Private Sub lblDateLabel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblDateLabel_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
    End Sub
    Private Sub lblTextLabel_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblTextLabel, eventSender)
        lblmousedown += 1
        GenericClickEvent("lblTextLabel", m_vTextArray, txtText, lblTextLabel, Index, Button, Shift, x, y)
    End Sub
    'Developer Guide No. 181
    Private Sub lblTextLabel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblTextLabel, eventSender)
        textLabelMouseMove("lblTextLabel", Button, x, y, txtText(Index), lblTextLabel(Index), pnlPosition, m_iSnapToGrid)
    End Sub
    'Developer Guide No. 181
    Private Sub lblTextLabel_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblTextLabel, eventSender)
        'make sure control is snapped to grid (if required)
        textLabelMouseMove("lblTextLabel", Button, x, y, txtText(Index), lblTextLabel(Index), pnlPosition, m_iSnapToGrid)
        SetSizeAndPosition()
    End Sub
    Private Sub lblCheckLabel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lblCheckLabel_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lblCheckLabel, eventSender)
        textLabelMouseMove("chkYesNo", Button, x, y, chkYesNo(Index), lblCheckLabel(Index), pnlPosition, m_iSnapToGrid)
    End Sub

    'Developer Guide No. 181
    Private Sub lvwListView_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _lvwListView_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(lvwListView, eventSender)

        SetSizeAndPosition()

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
            Case MouseButtons.Right

                If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

                'Menu time
                PBRiskScreenCommon.g_sControlName = "lvwListView"
                m_lIndex = Index

                If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                    Ctx_mnuListView.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                Else
                    MessageBox.Show("Please save this screen before editing the Drill Down", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

        End Select

    End Sub


    Private Sub m_frmSetListColumnOrder_ClickOK() Handles m_frmSetListColumnOrder.ClickOK

        Select Case m_frmSetListColumnOrder.Text
            Case "Set List Column Order"
                SetListColumnPositionFromScreen()
            Case "Move Tab"
                MoveTab()
        End Select

    End Sub


    Public Sub mnuControlCaption_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlCaption.Click

        Dim sCaption, sNewCaption, sVisibleCaption, sCaptionExtension As String
        Dim iTemp As Integer


        SetSizeAndPosition()

        'Add new functionality to allow multiline captions, but not on command buttons...


        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                sCaption = lblCheckLabel(m_lIndex).Text
            Case "cmdCommand"
                sCaption = cmdCommand(m_lIndex).Text
            Case "lblComboLabel"
                sCaption = lblComboLabel(m_lIndex).Text
            Case "lblTextLabel"
                ' RAW 20/10/2004 : CQ1814 : add test for hyperlink

                If m_vGenericClickArray(m_lIndex).Tag = PBRiskScreenCommon.ndcHyperlink Then
                    ' note that txtText holds the target address - but only when different to the caption
                    If txtText(m_lIndex).Text <> "" Then
                        sCaption = lblTextLabel(m_lIndex).Text & Strings.Chr(13) & Strings.Chr(10) & txtText(m_lIndex).Text
                    Else
                        sCaption = lblTextLabel(m_lIndex).Text
                    End If
                Else
                    sCaption = lblTextLabel(m_lIndex).Text
                End If
        End Select


        'RVH 26/9/2003 CQ1946 - Use array set at time of click event - original code
        '                       used one specific array which was incorrect.

        If g_sControlName = "cmdCommand" Then 'if attribute bound control

            sNewCaption = InputBoxEx("Enter new caption", "Screen Designer", sCaption)

            ' RAW 20/10/2004 : CQ1814 : added
            If sNewCaption = sCaption Then
                ' we must assume that the user pressed cancel
                Exit Sub
            End If

            If (sNewCaption = "") Then sNewCaption = ACBlankCaption

            sVisibleCaption = sNewCaption

        ElseIf m_vGenericClickArray(m_lIndex).Tag >= 0 Then 'if attribute bound control

            sNewCaption = InputBoxEx("Enter new caption", "Screen Designer", sCaption)

            ' RAW 20/10/2004 : CQ1814 : added
            If sNewCaption = sCaption Then
                ' we must assume that the user pressed cancel
                Exit Sub
            End If

            If (sNewCaption = "") Then sNewCaption = PBRiskScreenCommon2.ACBlankCaption

            sVisibleCaption = sNewCaption ' RAW 20/10/2004 : CQ1814 : added

            ' RAW 20/10/2004 : CQ1814 : removed counting of lines
        Else
            ' not in database (eg hyperlink, label etc)

            ' RAW 20/10/2004 : CQ1814 : completely reworked this bit
            While sNewCaption = ""
                sNewCaption = InputBoxEx("Enter new caption", "Screen Designer", sCaption)

                If sNewCaption = sCaption Then
                    ' we must assume that the user pressed cancel
                    Exit Sub
                End If

                sVisibleCaption = sNewCaption
                sCaptionExtension = ""


                If m_vGenericClickArray(m_lIndex).Tag = PBRiskScreenCommon.ndcHyperlink Then
                    'strip trailing vbCrLf characters
                    Do While (sNewCaption.Substring(sNewCaption.Length - 2) = Strings.Chr(13) & Strings.Chr(10))
                        sNewCaption = sNewCaption.Substring(0, sNewCaption.Length - 2)
                    Loop
                    sVisibleCaption = sNewCaption
                    'extract visible part of caption (ie everyting to the left of the last vbCrLf)
                    iTemp = IIf(sNewCaption = "" And Strings.Chr(13) & Strings.Chr(10) = "", 0, (sNewCaption.LastIndexOf(Strings.Chr(13) & Strings.Chr(10)) + 1))
                    If iTemp > 0 Then
                        ' this is what the user sees
                        sVisibleCaption = sNewCaption.Substring(0, iTemp - 1)
                        ' this is the target address
                        sCaptionExtension = sNewCaption.Substring(sNewCaption.Length - (sNewCaption.Length - iTemp - 1)).TrimStart()
                    End If
                End If

                If sVisibleCaption = "" Then
                    ' we must have something to display
                    MessageBox.Show("caption cannot be empty", "Set Caption", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    sNewCaption = ""
                End If

            End While
            ' RAW 20/10/2004 : CQ1814 : end

        End If

        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                ' is this really necessary - why not rely on autosize ?
                lblCheckLabel(m_lIndex).Height = (PBRiskScreenCommon2.CalculateLinesInCaption(sVisibleCaption) * VB6.TwipsToPixelsY(240)) + VB6.TwipsToPixelsY(195) ' RAW 20/10/2004 : CQ1814 : added
                lblCheckLabel(m_lIndex).Text = sNewCaption
                'call this to set position!
                textLabelMouseMove("chkYesNo", MouseButtons.Left, PBRiskScreenCommon.g_lx, PBRiskScreenCommon.g_ly, chkYesNo(m_lIndex), lblCheckLabel(m_lIndex), pnlPosition, m_iSnapToGrid)

            Case "cmdCommand"
                cmdCommand(m_lIndex).Text = sNewCaption
                pnlPanel(m_lIndex).Text = sNewCaption

            Case "lblComboLabel"
                ' is this really necessary - why not rely on autosize ?
                lblComboLabel(m_lIndex).Height = (PBRiskScreenCommon2.CalculateLinesInCaption(sVisibleCaption) * VB6.TwipsToPixelsY(240)) + VB6.TwipsToPixelsY(195) ' RAW 20/10/2004 : CQ1814 : added use of CalculateLinesInCaption
                lblComboLabel(m_lIndex).Text = sNewCaption
                lblComboLabel(m_lIndex).Left = ((cboCombo(m_lIndex).Left) - (lblComboLabel(m_lIndex).Width) - VB6.TwipsToPixelsX(PBRiskScreenCommon.cControlHorizontalOffset))

            Case "lblTextLabel"
                lblTextLabel(m_lIndex).Text = sVisibleCaption
                txtText(m_lIndex).Text = sCaptionExtension ' RAW 20/10/2004 : CQ1814 : added
                SetLabelAppearance(txtText(m_lIndex), lblTextLabel(m_lIndex)) ' RAW 20/10/2004 : CQ1814 : renamed function
                If CDbl(Convert.ToString(txtText(m_lIndex).Tag)) >= 0 Then lblTextLabel(m_lIndex).Left = ((txtText(m_lIndex).Left) - (lblTextLabel(m_lIndex).Width) - VB6.TwipsToPixelsX(PBRiskScreenCommon.cControlHorizontalOffset))

        End Select

    End Sub

    Public Sub mnuControlDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlDelete.Click

        Dim lTag As Integer

        SetSizeAndPosition()

        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                chkYesNo(m_lIndex).Visible = False
                lblCheckLabel(m_lIndex).Visible = False
                lTag = CInt(Convert.ToString(chkYesNo(m_lIndex).Tag))
                m_vCheckArray(PBRiskScreenCommon.ACCIsDeleted, m_lIndex) = gPMConstants.PMEReturnCode.PMTrue
            Case "cmdCommand"
                cmdCommand(m_lIndex).Visible = False
                pnlPanel(m_lIndex).Visible = False
                lTag = CInt(Convert.ToString(cmdCommand(m_lIndex).Tag))
                m_vCommandArray(PBRiskScreenCommon.ACCIsDeleted, m_lIndex) = gPMConstants.PMEReturnCode.PMTrue
            Case "lblComboLabel"
                cboCombo(m_lIndex).Visible = False
                lblComboLabel(m_lIndex).Visible = False
                lTag = CInt(Convert.ToString(cboCombo(m_lIndex).Tag))
                m_vComboArray(PBRiskScreenCommon.ACCIsDeleted, m_lIndex) = gPMConstants.PMEReturnCode.PMTrue
            Case "lblTextLabel"
                txtText(m_lIndex).Visible = False
                lblTextLabel(m_lIndex).Visible = False
                lTag = CInt(Convert.ToString(txtText(m_lIndex).Tag))
                m_vTextArray(PBRiskScreenCommon.ACCIsDeleted, m_lIndex) = gPMConstants.PMEReturnCode.PMTrue
        End Select

        If lTag >= 0 Then 'check if attribute bound control

            m_vInUse(GISDataModelType.GISDMTypeRisk)(lTag Mod 10000) = gPMConstants.PMEReturnCode.PMFalse
        End If

    End Sub


    Public Sub mnuControlEntry_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlEntry.Click
        SetSizeAndPosition()
        m_lReturn = EntryRequirements()
    End Sub



    Public Sub mnuControlHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlHelp.Click

        Dim sCaption As String = ""

        SetSizeAndPosition()

        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                sCaption = CStr(m_vCheckArray(PBRiskScreenCommon.ACCHelpText, m_lIndex))
            Case "cmdCommand"
                sCaption = CStr(m_vCommandArray(PBRiskScreenCommon.ACCHelpText, m_lIndex))
            Case "lblComboLabel"
                sCaption = CStr(m_vComboArray(PBRiskScreenCommon.ACCHelpText, m_lIndex))
            Case "lblTextLabel"
                sCaption = CStr(m_vTextArray(PBRiskScreenCommon.ACCHelpText, m_lIndex))
        End Select

        Dim sNewCaption As String = Interaction.InputBox("Enter new help text", "Screen Designer", sCaption)

        If sNewCaption <> "" Then
            Select Case PBRiskScreenCommon.g_sControlName
                Case "chkYesNo"
                    m_vCheckArray(PBRiskScreenCommon.ACCHelpText, m_lIndex) = sNewCaption
                    ToolTip1.SetToolTip(chkYesNo(m_lIndex), sNewCaption)
                Case "cmdCommand"
                    m_vCommandArray(PBRiskScreenCommon.ACCHelpText, m_lIndex) = sNewCaption
                    ToolTip1.SetToolTip(cmdCommand(m_lIndex), sNewCaption)
                Case "lblComboLabel"
                    m_vComboArray(PBRiskScreenCommon.ACCHelpText, m_lIndex) = sNewCaption
                    ToolTip1.SetToolTip(cboCombo(m_lIndex), sNewCaption)
                Case "lblTextLabel"
                    m_vTextArray(PBRiskScreenCommon.ACCHelpText, m_lIndex) = sNewCaption
                    ToolTip1.SetToolTip(txtText(m_lIndex), sNewCaption)
            End Select
        End If

    End Sub

    Public Sub mnuControlIncludeInList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlIncludeInList.Click

        mnuControlIncludeInList.Checked = Not mnuControlIncludeInList.Checked

        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                m_vCheckArray(PBRiskScreenCommon.ACCIncludeInList, m_lIndex) = mnuControlIncludeInList.Checked
            Case "cmdCommand"
                m_vCommandArray(PBRiskScreenCommon.ACCIncludeInList, m_lIndex) = mnuControlIncludeInList.Checked
            Case "lblComboLabel"
                m_vComboArray(PBRiskScreenCommon.ACCIncludeInList, m_lIndex) = mnuControlIncludeInList.Checked
            Case "lblTextLabel"
                m_vTextArray(PBRiskScreenCommon.ACCIncludeInList, m_lIndex) = mnuControlIncludeInList.Checked
        End Select

    End Sub

    Public Sub mnuControlNameLabel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlNameLabel.Click


        SetSizeAndPosition()

        Dim sChar As String = Strings.Chr(13) & Strings.Chr(10)

        Dim sCaption As String = ""

        'sCaption = ToolTip1.GetToolTip(lblTextLabel(m_lIndex))
        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                sCaption = ToolTip1.GetToolTip(lblCheckLabel(m_lIndex))
            Case "cmdCommand"
                sCaption = ToolTip1.GetToolTip(cmdCommand(m_lIndex))
            Case "lblComboLabel"
                sCaption = ToolTip1.GetToolTip(lblComboLabel(m_lIndex))
            Case "lblTextLabel"
                sCaption = ToolTip1.GetToolTip(lblTextLabel(m_lIndex))
        End Select

        Do While (sCaption.IndexOf("."c) + 1)
            sCaption = sCaption.Substring(sCaption.Length - (sCaption.Length - (sCaption.IndexOf("."c) + 1)))
        Loop

        Dim sNewCaption As String = InputBoxEx("Enter label name", "Screen Designer", sCaption) ' RAW 20/10/2004 : CQ1818 : added

        If sNewCaption = sCaption Then
            ' we must assume that the user pressed cancel
            Exit Sub
        End If

        Do While (sNewCaption.IndexOf("."c) + 1)
            sNewCaption = sNewCaption.Substring(sNewCaption.Length - (sNewCaption.Length - (sNewCaption.IndexOf("."c) + 1)))
        Loop

        sNewCaption = cLabelNamePrefix & sNewCaption

        'ToolTip1.SetToolTip(lblTextLabel(m_lIndex), sNewCaption)
        Select Case PBRiskScreenCommon.g_sControlName
            Case "chkYesNo"
                ToolTip1.SetToolTip(lblCheckLabel(m_lIndex), sNewCaption)
            Case "cmdCommand"
                ToolTip1.SetToolTip(cmdCommand(m_lIndex), sNewCaption)
            Case "lblComboLabel"
                ToolTip1.SetToolTip(lblComboLabel(m_lIndex), sNewCaption)
            Case "lblTextLabel"
                ToolTip1.SetToolTip(lblTextLabel(m_lIndex), sNewCaption)
        End Select

        If IsArray(m_vTextArray) Then
            m_vTextArray(PBRiskScreenCommon.ACCHelpText, m_lIndex) = sNewCaption
        End If

    End Sub

    Public Sub mnuControlSetListColumnOrder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuControlSetListColumnOrder.Click

        Dim oListItem As ListViewItem
        Dim vArray(,) As Object

        m_frmSetListColumnOrder = New frmSetListColumnOrder()

        'Akash: commented
        'Load(m_frmSetListColumnOrder)
        'signal frmSetListColumnOrder what mode we are in
        m_frmSetListColumnOrder.Text = "Set List Column Order"

        'add the screen details that are included in the parent screen listview into vArray
        Dim iEntries As Integer = 0
        AddToListColumnPositionArray(iEntries, vArray, m_vCheckArray, lblCheckLabel, 1000)
        AddToListColumnPositionArray(iEntries, vArray, m_vTextArray, lblTextLabel, 2000)

        AddToListColumnPositionArray(iEntries, vArray, m_vComboArray, lblComboLabel, 3000)

        AddToListColumnPositionArray(iEntries, vArray, m_vCommandArray, cmdCommand, 4000)
        If iEntries Then
            'sort array into current list column order

            PBRiskScreenCommon.SortThreeElementArray(vArray)

            With m_frmSetListColumnOrder
                'set list view defaults
                .ListViewSetListColumnOrder.Items.Clear()
                .ListViewSetListColumnOrder.Columns.Add("D1", "Order", CInt(Math.Round(94 / 15)))
                .ListViewSetListColumnOrder.Columns.Item("D1").Width = CInt(0)
                .ListViewSetListColumnOrder.Columns.Add("D2", "Order", CInt(Math.Round(94 / 15)))
                .ListViewSetListColumnOrder.Columns.Item("D2").Width = CInt(VB6.TwipsToPixelsX(599))
                .ListViewSetListColumnOrder.Columns.Item("D2").TextAlign = HorizontalAlignment.Center
                .ListViewSetListColumnOrder.Columns.Add("D3", "Caption", CInt(Math.Round(94 / 15)))
                .ListViewSetListColumnOrder.Columns.Item("D3").Width = CInt(VB6.TwipsToPixelsX(4081)) '4110
                ListViewHelper.SetSortedProperty(.ListViewSetListColumnOrder, False)

                SetExtraListViewProperties(.ListViewSetListColumnOrder.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=False)
            End With


            'add to list view
            iEntries = 0

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                iEntries += 1
                oListItem = m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Add("Wibble")
                oListItem.Text = CStr(iEntries)


                oListItem.Tag = CStr(vArray(1, lTemp)) 'tag
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(iEntries)

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(vArray(2, lTemp)) ' caption
            Next

            m_frmSetListColumnOrder.ShowDialog()
        Else
            Interaction.MsgBox("There are no controls included in a list view", MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Information, "Set List Column Order")
        End If

        m_frmSetListColumnOrder = Nothing


    End Sub

    Public Sub mnuDefaultScreenSize_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDefaultScreenSize.Click
        picScreen.Height = VB6.TwipsToPixelsY(6000)
        picScreen.Width = VB6.TwipsToPixelsX(9015)
    End Sub

    Public Sub mnuTabMove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTabMove.Click

        Dim oListItem As ListViewItem

        m_frmSetListColumnOrder = New frmSetListColumnOrder()

        'Akash: Commented
        'Load(m_frmSetListColumnOrder)

        With m_frmSetListColumnOrder

            'signal frmSetListColumnOrder what mode we are in
            .Text = "Move Tab"

            'set list view defaults
            .ListViewSetListColumnOrder.Items.Clear()
            .ListViewSetListColumnOrder.Columns.Add("D1", "Order", CInt(Math.Round(94 / 15)))
            .ListViewSetListColumnOrder.Columns.Item("D1").Width = CInt(0)
            .ListViewSetListColumnOrder.Columns.Add("D2", "Order", 94)
            .ListViewSetListColumnOrder.Columns.Item("D2").Width = CInt(VB6.TwipsToPixelsX(599))
            .ListViewSetListColumnOrder.Columns.Item("D2").TextAlign = HorizontalAlignment.Center
            .ListViewSetListColumnOrder.Columns.Add("D3", "Tab Caption", CInt(Math.Round(94 / 15)))
            .ListViewSetListColumnOrder.Columns.Item("D3").Width = CInt(VB6.TwipsToPixelsX(4081)) '4110
            ListViewHelper.SetSortedProperty(.ListViewSetListColumnOrder, False)

            SetExtraListViewProperties(.ListViewSetListColumnOrder.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=False)
        End With

        'add to list view
        Dim iEntries As Integer = 0
        For lTemp As Integer = 0 To m_lTabIndex
            If PBTabStripCommon.IsTabVisible(TabStrip1, lTemp) Then
                iEntries += 1
                oListItem = m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Add("Wibble")
                oListItem.Text = CStr(iEntries)
                oListItem.Tag = "_" & lTemp
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(iEntries)
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = PBTabStripCommon.TabGetCaption(TabStrip1, lTemp)
            End If
        Next
        m_frmSetListColumnOrder.ShowDialog()

        m_frmSetListColumnOrder = Nothing

    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetListColumnPositionFromScreen
    '
    ' Description:
    '
    ' History: 10/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub SetListColumnPositionFromScreen()

        Dim lTag As Integer
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SetListColumnPositionFromScreen")

        Try

            For lTemp As Integer = 1 To m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Count

                lTag = Convert.ToString(m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Item(lTemp - 1).Tag)
                Select Case lTag \ 1000
                    Case 1
                        m_vCheckArray(PBRiskScreenCommon.ACCColumnPosition, lTag Mod 1000) = lTemp
                    Case 2
                        m_vTextArray(PBRiskScreenCommon.ACCColumnPosition, lTag Mod 1000) = lTemp
                    Case 3
                        m_vComboArray(PBRiskScreenCommon.ACCColumnPosition, lTag Mod 1000) = lTemp
                    Case 4
                        m_vCommandArray(PBRiskScreenCommon.ACCColumnPosition, lTag Mod 1000) = lTemp
                    Case Else
                        Throw New System.Exception("1, SetListColumnPositionFromScreen, Unknow case value")
                End Select
            Next

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SetListColumnPositionFromScreen")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetListColumnPositionFromScreen")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetListColumnPositionFromScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListColumnPositionFromScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Public Sub mnuFormat_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuFormat_0.Click, _mnuFormat_1.Click, _mnuFormat_2.Click, _mnuFormat_3.Click, _mnuFormat_4.Click, _mnuFormat_5.Click, _mnuFormat_6.Click, _mnuFormat_7.Click, _mnuFormat_8.Click, _mnuFormat_9.Click, _mnuFormat_10.Click, _mnuFormat_11.Click, _mnuFormat_12.Click, _mnuFormat_13.Click, _mnuFormat_14.Click, _mnuFormat_15.Click, _mnuFormat_16.Click, _mnuFormat_17.Click, _mnuFormat_18.Click, _mnuFormat_19.Click, _mnuFormat_20.Click, _mnuFormat_21.Click, _mnuFormat_22.Click, _mnuFormat_23.Click, _mnuFormat_24.Click, _mnuFormat_25.Click, _mnuFormat_26.Click, _mnuFormat_27.Click, _mnuFormat_28.Click, _mnuFormat_29.Click
        Dim Index As Integer = GetIndex(eventSender, mnuFormat) 'Array.IndexOf(mnuFormat, eventSender)

        Dim vFormatValue As Object
        mnuFormat(Index).Checked = Not mnuFormat(Index).Checked

        'clear down lower 5 bits of format information
        If Index = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
            If IsDBNull(m_vCheckArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex)) Then
                m_vCheckArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) = Nothing
            End If
            vFormatValue = m_vCheckArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) And PBRiskScreenCommon.ACFormatStandardClear
        Else

            'vFormatValue = CBool(CInt(IIf(Convert.ToString(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex)) = "", 0, Convert.ToString(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex))))) And PBRiskScreenCommon.ACFormatStandardClear
            If IsDBNull(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex)) Then
                m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) = Nothing
            End If
            vFormatValue = m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) And PBRiskScreenCommon.ACFormatStandardClear
        End If

        If Convert.IsDBNull(vFormatValue) Or IsNothing(vFormatValue) Then

            vFormatValue = 0
        End If

        If mnuFormat(Index).Checked Then
            'm_vCheckArray(ACCPMFormat, m_lIndex) = Index


            vFormatValue = CInt(vFormatValue) + Index
            If Index = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                'set caption BEFORE setting width
                chkYesNo(m_lIndex).Width = VB6.TwipsToPixelsY(PBRiskScreenCommon.cCheckBoxTriStateCaptionWidth)

                PBRiskScreenCommon2.simulateTriStateCheckBox(chkYesNo(m_lIndex).CheckState, chkYesNo(m_lIndex))

            End If
        Else
            'm_vCheckArray(ACCPMFormat, m_lIndex) = Null
            If Index = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                'change from tr-state to a CheckBox
                chkYesNo(m_lIndex).Width = VB6.TwipsToPixelsY(PBRiskScreenCommon.cCheckBoxCaptionWidth)
                chkYesNo(m_lIndex).Text = ""
            Else
                If Index = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine Then
                    txtText(m_lIndex).Height = VB6.TwipsToPixelsY(textBoxMinimumHeight)
                End If
            End If
        End If
        If Index = gPMConstants.PMEFormatStyle.PMFormatBoolean Then

            m_vCheckArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) = vFormatValue
        Else

            m_vTextArray(PBRiskScreenCommon.ACCPMFormat, m_lIndex) = vFormatValue
        End If

    End Sub


    Public Sub mnuFrameCaption_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFrameCaption.Click


        SetSizeAndPosition()

        Dim sCaption As String = fraFrame(m_lIndex).Text

        Dim sNewCaption As String = Interaction.InputBox("Enter new caption", "Screen Designer", sCaption)

        'Always do this...
        '    If (sNewCaption <> "") Then
        fraFrame(m_lIndex).Text = sNewCaption
        '    End If

    End Sub

    Public Sub mnuFrameDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFrameDelete.Click

        SetSizeAndPosition()

        fraFrame(m_lIndex).Visible = False

        m_lReturn = DeleteFrameContents(lIndex:=m_lIndex)

        m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, m_lIndex) = gPMConstants.PMEReturnCode.PMTrue

    End Sub

    Public Sub mnuFrameShowRateAndPremium_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFrameShowRateAndPremium.Click


        SetSizeAndPosition()

        mnuFrameShowRateAndPremium.Checked = Not mnuFrameShowRateAndPremium.Checked

        uctSumInsured1(m_lSumInsuredInThisFrame).ShowRateAndPremium = mnuFrameShowRateAndPremium.Checked

        m_vSumInsuredArray(PBRiskScreenCommon.ACCIsRateAndPremium, m_lSumInsuredInThisFrame) = mnuFrameShowRateAndPremium.Checked

    End Sub

    Public Sub mnuFrameShowValuation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFrameShowValuation.Click

        SetSizeAndPosition()

        mnuFrameShowValuation.Checked = Not mnuFrameShowValuation.Checked

        uctSumInsured1(m_lSumInsuredInThisFrame).ShowValuation = mnuFrameShowValuation.Checked

        m_vSumInsuredArray(PBRiskScreenCommon.ACCIsValuation, m_lSumInsuredInThisFrame) = mnuFrameShowValuation.Checked

    End Sub


    Public Sub mnuListViewDrillDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuListViewDrillDown.Click

        SetSizeAndPosition()

        m_lReturn = CallMe(lIndex:=m_lIndex)

    End Sub

    Public Sub mnuSnapToGrid_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSnapToGrid.Click

        mnuSnapToGrid.Checked = Not mnuSnapToGrid.Checked
        If mnuSnapToGrid.Checked Then
            m_iSnapToGrid = 1
        Else
            m_iSnapToGrid = 0
        End If
        m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, TabGetCurrentIndex(TabStrip1)) = m_iSnapToGrid

    End Sub

    Public Sub mnuTabAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTabAdd.Click
        Dim sTempCaption As String = ""

        SetSizeAndPosition()

        'PBTabStripCommon.AddTab(TabStrip1, TabStrip1.TabPages.Count , "&" & TabStrip1.TabPages.Count + 1 & " - General")
        PBTabStripCommon.AddTab(TabStrip1, TabStrip1.TabPages.Count, TabStrip1.TabPages.Count + 1 & " - General")
        m_lTabIndex = TabStrip1.TabPages.Count - 1

        ReDim Preserve m_vTabArray(PBRiskScreenCommon.ACFLastArrayPosition, m_lTabIndex)
        m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, m_lTabIndex) = 0 'clear snap to grid
        TabStrip1.Controls(m_lTabIndex).Visible = True
        TabStrip1.Controls(m_lTabIndex).AllowDrop = True
        AddHandler TabStrip1.TabPages(m_lTabIndex).DragDrop, AddressOf TabStrip1_DragDrop
        AddHandler TabStrip1.Controls(m_lTabIndex).DragEnter, AddressOf TabStrip1_DragEnter
    End Sub



    Public Sub mnuTabDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTabDelete.Click


        SetSizeAndPosition()

        Dim lTab As Integer = TabGetCurrentIndex(TabStrip1)

        If Interaction.MsgBox("Delete tab " & """" & TabStrip1.SelectedTab.Text.Replace("&", "") & """", MsgBoxStyle.Question Or MsgBoxStyle.OkCancel, "Confirm Tab Delete") = System.Windows.Forms.DialogResult.OK Then
            'PBTabStripCommon.HideTab(TabStrip1, lTab + 1)
            'm_lReturn = DeleteTabContents(lTab:=lTab + 1)
            m_lReturn = DeleteTabContents(lTab:=lTab)
            PBTabStripCommon.HideTab(TabStrip1, lTab)
            'm_lReturn = DeleteTabContents(lTab:=lTab)
        End If
        PBTabStripCommon.SelectTab(TabStrip1, 0)

    End Sub


    Public Sub mnuTabRename_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTabRename.Click


        SetSizeAndPosition()

        Dim sCaption As String = PBTabStripCommon.TabGetCaption(TabStrip1, TabGetCurrentIndex(TabStrip1))

        sCaption = Interaction.InputBox("Enter new caption", "Screen Designer", sCaption)

        If sCaption = "" Then
            Exit Sub
        End If
        PBTabStripCommon.TabSetCaption(TabStrip1, TabGetCurrentIndex(TabStrip1), sCaption)
        SSTabHelper.SetTabEnabled(TabStrip1, TabGetCurrentIndex(TabStrip1), True)
        '    For lTemp = LBound(m_vScreenDetails, 2) To UBound(m_vScreenDetails, 2)
        '        If (m_vScreenDetails(ACDGISPropertyId, lTemp) Is Null) Then
        '            If (m_vScreenDetails(ACDIsFrame, lCount) = False) Then
        '                If (m_vScreenDetails(ACDTabNumber, lCount) = lTab) Then
        '                    m_vScreenDetails(ACDCaption, lCount) = sNewCaption
        '                    Exit For
        '                End If
        '            End If
        '        End If
        '    Next lTemp

    End Sub

    Public Sub mnuTabSetControlTabOrder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTabSetControlTabOrder.Click

        m_lSetTabOrderCount = 0
        Dim lTab As Integer = TabGetCurrentIndex(TabStrip1)
        Try


            ' RDC 23112001 move from below so that number of qualifying controls can be checked first
            'count the controls on this tab
            'PN No 71403
            If Information.IsArray(m_vFrameArray) Then 'PN No 71403
                For lFrame As Integer = m_vFrameArray.GetLowerBound(1) To m_vFrameArray.GetUpperBound(1)
                    If lTab = CDbl(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrame)) Then
                        'got frame, find control
                        Dim arrListControl As New ArrayList
                        ControlList(Me, arrListControl)

                        'For Each ctlControl As Control In ContainerHelper.Controls(Me)
                        For ctlCount As Integer = 0 To arrListControl.Count - 1
                            Dim ctlControl As Control = CType(arrListControl(ctlCount), Control)
                            If HasVisibleProperty(ctlControl) Then
                                If ctlControl.Parent.Name.Contains("fraFrame") Then
                                    Dim checkST As String = ""
                                    checkST = ctlControl.Name
                                    If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
                                        checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                                    ElseIf checkST.StartsWith("_") Then
                                        checkST = checkST.Substring(1, checkST.Length - 2)
                                    End If
                                    Select Case checkST
                                        Case "chkYesNo", "cmdCommand", "cboCombo", "txtText", "lvwListView", "uctAddress1", "uctStandardWording1", "uctSumInsured1", "uctClaimReserve1", "uctClaimPayment1", "uctCLMCaseClaim1", "uctCLMCaseHeader1"

                                            '                                If HasEnabledProperty(ctlControl) Then
                                            If ContainerHelper.GetControlIndex(ctlControl.Parent) = lFrame And ControlHelper.GetVisible(ctlControl) Then
                                                ChangeForeColor(ctlControl, Color.Blue)
                                                m_lSetTabOrderCount += 1
                                            End If
                                            '                               End If

                                        Case Else
                                            Debug.WriteLine(ctlControl.Name)
                                    End Select
                                End If
                            End If
                        Next
                    End If
                Next
            End If


            ' any qualifying controls?
            If m_lSetTabOrderCount = 0 Then
                ' no controls have tabindex on this tab
                MessageBox.Show("Unable to set tab order - no qualifying controls", "Set Control tab order", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Dim sTmpMsg As String = "Click on each control's caption to set tab order." & Strings.Chr(13) & Strings.Chr(10) & "Press escape to cancel."

            If Interaction.MsgBox(sTmpMsg, MsgBoxStyle.OkCancel Or MsgBoxStyle.Information, "Set Control's tab order") = System.Windows.Forms.DialogResult.OK Then
                picScreen.Cursor = Cursors.AppStarting
                m_lSetTabOrderIndex = (lTab + 1) * 100
                'disable anything we shouldn't click on

                tvwDataDictionary.Enabled = False
                cmdCopy.Enabled = False
                cmdDefaults.Enabled = False
                cmdHelp.Enabled = False
                cmdOK.Enabled = False
                cmdSave.Enabled = False
                cmdValidation.Enabled = False
                ' RDC 23112001 control count moved to top of sub so that qualifying.
                ' controls can be counted. If no qualifying controls, sub exits.
                Dim arrListControl As New ArrayList
                ControlList(Me, arrListControl)

                'now disable any hyperlinks and freeformat text on this tab
                For lFrame As Integer = m_vFrameArray.GetLowerBound(1) To m_vFrameArray.GetUpperBound(1)
                    If lTab = CDbl(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrame)) Then
                        'got frame, find control

                        ' For Each ctlControl As Control In ContainerHelper.Controls(Me)
                        For ctlCount As Integer = 0 To arrListControl.Count - 1
                            Dim ctlControl As Control = CType(arrListControl(ctlCount), Control)
                            If ctlControl.Name.Contains("lblTextLabel") Then
                                If ctlControl.Parent.Name.Contains("fraFrame") Then
                                    If ContainerHelper.GetControlIndex(ctlControl.Parent) = lFrame Then

                                        'Akash: commented
                                        If ToolTip1.GetToolTip(ctlControl) = "" Or Mid(ToolTip1.GetToolTip(ctlControl), 1, cLabelNamePrefix.Length) = cLabelNamePrefix Then
                                            ctlControl.Enabled = False
                                        Else
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Else
                m_lSetTabOrderCount = 0 'make sure tab set order mode is cancelled
            End If 'okay
        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetControlTabIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetControlTabIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub
    'Akash: Modified
    'Private Sub PBFindControl_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseDown) Handles _PBFindControl_0.MouseDown
    'Private Sub PBFindControl_MouseDown(ByVal eventSender As Object, ByVal eventArgs As uctPBFindDesign.PBFindDesign.MouseDownEventArgs) Handles _PBFindControl_0.MouseDown
    Private Sub PBFindControl_MouseDown(ByVal eventSender As Object, ByVal eventArgs As uctPBFindDesign.PBFindDesign.MouseDownEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = (eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(PBFindControl, eventSender)
        Dim lRetVal As DialogResult

        PBRiskScreenCommon.g_lx = CInt(x)
        PBRiskScreenCommon.g_ly = CInt(y)

        'SetSizeAndPosition(CInt((PBFindControl(Index).Left)), CInt(VB6.PixelsToTwipsY(PBFindControl(Index).Top)))
        SetSizeAndPosition(CInt((PBFindControl(Index).Left)), CInt((PBFindControl(Index).Top)))

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                'Developer Guide No. 64
            Case MouseButtons.Right
                lRetVal = MessageBox.Show("Delete this find control ?", Application.ProductName, MessageBoxButtons.YesNo)
                If lRetVal = System.Windows.Forms.DialogResult.Yes Then
                    'Delete find control
                    PBFindControl(Index).Visible = False
                    m_vFindControlArray(PBRiskScreenCommon.ACCIsDeleted, Index) = gPMConstants.PMEReturnCode.PMTrue
                    PBFindControl(Index).Delete()
                End If
        End Select

    End Sub

    'Akash : Modified
    'Private Sub PBFindControl_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _PBFindControl_0.MouseMove
    Private Sub PBFindControl_MouseMove(ByVal eventSender As Object, ByVal eventArgs As uctPBFindDesign.PBFindDesign.MouseMoveEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        '      Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(PBFindControl, eventSender)
        Dim lTop, lLeft, lRight As Integer

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                ' Developer Guide No. 74
                lTop = CInt((PBFindControl(Index).Top) - PBRiskScreenCommon.g_ly + y)
                lLeft = CInt((PBFindControl(Index).Left) - PBRiskScreenCommon.g_lx + x)
                lRight = CInt(lLeft + (PBFindControl(Index).Width))

                If lTop > Math.Round(120 / 15) Then
                    If lTop < ((PBFindControl(Index).Parent.Height) - VB6.TwipsToPixelsY(420)) Then
                        PBFindControl(Index).Top = (PBFindControl(Index).Top) - PBRiskScreenCommon.g_ly + y
                    End If
                End If

                If lLeft > Math.Round(120 / 15) Then
                    If lRight < ((PBFindControl(Index).Parent.Width) - Math.Round(120 / 15)) Then
                        PBFindControl(Index).Left = (PBFindControl(Index).Left) - PBRiskScreenCommon.g_lx + x
                        'pnlPanel(Index).Left = PBFindControl(Index).Left + PBFindControl(Index).Width
                    End If
                End If

                SetSizeAndPosition(CInt((PBFindControl(Index).Left)), CInt((PBFindControl(Index).Top)))

        End Select

    End Sub

    'Akash: Modified
    'Private Sub PBFindControl_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _PBFindControl_0.MouseUp
    Private Sub PBFindControl_MouseUp(ByVal eventSender As Object, ByVal eventArgs As uctPBFindDesign.PBFindDesign.MouseUpEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ' Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        SetSizeAndPosition()
    End Sub
    'Private Sub PBFindControl_btnFind(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _PBFindControl_0.btnFind_renamed
    Private Sub PBFindControl_btnFind(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(PBFindControl, eventSender)
        Dim vScreenControlArray(,) As Object
        Dim ArrayIndex As Integer = -1
        Try


            'when the find control button is pressed re-load the control arrays and pass them to the control.
            'this is because some comtrols may have been edited/deleted since last load

            AddControlToFindControl(ArrayIndex, ContainerHelper.GetControlIndex(PBFindControl(Index).Parent), vScreenControlArray, m_vTextArray, txtText, lblTextLabel, 1)
            ' TBD AddControlToFindControl ArrayIndex, iFrameIndex, vScreenControlArray, m_vDateArray, txtDate, lblDateLabel, 1

            AddControlToFindControl(ArrayIndex, ContainerHelper.GetControlIndex(PBFindControl(Index).Parent), vScreenControlArray, m_vComboArray, cboCombo, lblComboLabel, 2)

            'send to control


            PBFindControl(Index).ScreenControlArray = vScreenControlArray
        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Find Button Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetControlTabIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub



    Private Sub SetSizeAndPosition(Optional ByRef lLeft As Integer = -1, Optional ByRef lTop As Integer = -1, Optional ByRef lWidth As Integer = -1, Optional ByRef lHeight As Integer = -1)

        Dim sPos, sSize As String
        If Not (lTop = -1 And lLeft = -1) Then
            sPos = "Left: " & lLeft & ", Top: " & CStr(lTop)
        End If
        pnlPosition.Text = sPos

        If Not (lHeight = -1 And lWidth = -1) Then
            sSize = "Height: " & lHeight & ", Width: " & CStr(lWidth)
        End If
        pnlSize.Text = sSize

    End Sub

    Private Sub picScreen_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles picScreen.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        'If Button = MouseButtonConstants.LeftButton Then picScreen.Cursor = Cursors.Default
        If Button = MouseButtons.Left Then picScreen.Cursor = Cursors.Default
    End Sub
    Private Sub picScreen_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles picScreen.Resize
        Dim NewLargeChange As Integer
        TabStrip1.Height = picScreen.Height
        'Developer Guide No. 74
        TabStrip1.Width = picScreen.Width
        frmSizer.Top = TabStrip1.Top + VB6.TwipsToPixelsY(350) 'put frmSizer below tabstrip
        frmSizer.Left = picScreen.Left + VB6.TwipsToPixelsX(50) 'put frmSizer to right of left border
        frmSizer.Height = picScreen.Height - VB6.TwipsToPixelsY(400) 'put frmSizer above bottom border
        frmSizer.Width = picScreen.Width - VB6.TwipsToPixelsX(100) 'put frmSizer to left or right border

        'Developer Guide No. 74
        If ((picScreen.Width)) > (picScreenMain.Width) - VB6.TwipsToPixelsX(90) Then
            lHScrollMultiplier = (CInt((picScreen.Width) - (picScreenMain.Width - VB6.TwipsToPixelsX(90))))
            'Developer Guide No. 74
            If lHScrollMultiplier > VB6.TwipsToPixelsX(32767) Then
                HScroll1.Maximum = (VB6.TwipsToPixelsX(32767) + HScroll1.LargeChange - 1)
            Else
                HScroll1.Maximum = (lHScrollMultiplier + HScroll1.LargeChange - 1)
            End If

            If gPMFunctions.ToSafeInteger((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 20, 1) < 1 Then
                HScroll1.SmallChange = 1
            Else
                HScroll1.SmallChange = gPMFunctions.ToSafeInteger((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 20, 1)
            End If

            If gPMFunctions.ToSafeInteger((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 5, 1) < 1 Then
                NewLargeChange = 1
                HScroll1.Maximum = HScroll1.Maximum + NewLargeChange - HScroll1.LargeChange
                HScroll1.LargeChange = NewLargeChange
            Else
                NewLargeChange = gPMFunctions.ToSafeInteger((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 5, 1)
                HScroll1.Maximum = HScroll1.Maximum + NewLargeChange - HScroll1.LargeChange
                HScroll1.LargeChange = NewLargeChange
            End If
            HScroll1.Visible = True
        Else
            HScroll1.Value = 0
            HScroll1.Visible = False
            picScreen.Left = 0
        End If

        'Developer Guide No. 74

        If (picScreen.Height) > picScreenMain.Height - VB6.TwipsToPixelsY(90) Then
            lVScrollMultiplier = ((picScreen.Height) - (picScreenMain.Height - VB6.TwipsToPixelsY(90)))
            If lVScrollMultiplier > VB6.TwipsToPixelsY(32767) Then
                VScroll1.Maximum = VB6.TwipsToPixelsY(32767)
            Else
                VScroll1.Maximum = lVScrollMultiplier
            End If

            If ToSafeInteger(VScroll1.Maximum / 20, 1) < 1 Then
                VScroll1.SmallChange = 1
            Else
                VScroll1.SmallChange = ToSafeInteger(VScroll1.Maximum / 20, 1)
            End If

            If ToSafeInteger(VScroll1.Maximum / 5, 1) < 1 Then
                VScroll1.LargeChange = 1
            Else
                VScroll1.LargeChange = ToSafeInteger(VScroll1.Maximum / 5, 1)
            End If
            VScroll1.Visible = True
        Else
            VScroll1.Value = 0
            VScroll1.Visible = False
            picScreen.Top = 0
        End If

    End Sub

    Private Sub frmSizer_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles frmSizer.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        'If Button = MouseButtonConstants.LeftButton Then picScreen.Cursor = Cursors.Default
        If eventArgs.Button = MouseButtons.Left Then picScreen.Cursor = Cursors.Default
    End Sub

    Private Sub frmSizer_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles frmSizer.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        Dim iPointer As Cursor

        Static bSizing, bSouth, bEast As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        If Not bSizing Then
            bSouth = (y > ((frmSizer.Height) - Math.Round(100 / 15)))
            bEast = (x > ((frmSizer.Width) - Math.Round(100 / 15)))

            If bEast And bSouth Then
                iPointer = Cursors.SizeNWSE
            ElseIf bEast Then
                iPointer = Cursors.SizeWE
            ElseIf bSouth Then
                iPointer = Cursors.SizeNS
            End If

            picScreen.Cursor = iPointer
        End If

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                bSizing = True

                If bSouth Then
                    If (y > Math.Round(500 / 15)) And y + Math.Round(400 / 15) < (picScreenMain.Height) - Math.Round(300 / 15) Then
                        picScreen.Height = (y + VB6.TwipsToPixelsY(400) - (picScreen.Top))
                    End If
                End If

                If bEast Then
                    If x > Math.Round(1000 / 15) And x + Math.Round(100 / 15) < picScreenMain.Width - Math.Round(300 / 15) Then
                        picScreen.Width = (x + Math.Round(100 / 15) - ((picScreen.Left)))
                    End If
                End If

                With picScreen
                    SetSizeAndPosition(CInt((.Left)), CInt((.Top)), CInt((.Width)), CInt((.Height)))
                End With

            Case Else
                bSizing = False
        End Select

    End Sub

    Private Sub pnlPanel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _pnlPanel_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = (eventArgs.Y)

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        Dim Index As Integer = Array.IndexOf(pnlPanel, eventSender)

        Dim lTop, lLeft, lWidth, lHeight, lCommandOffSet As Integer
        Dim iPointer As Cursor
        Static bSizing, bEast, bNorth, bSouth, bWest As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        With pnlPanel(Index)
            lTop = CInt((.Top))
            lLeft = CInt(.Left)
            lWidth = CInt((.Width))
            lHeight = CInt((.Height))
            lCommandOffSet = CInt((.Left) - (cmdCommand(Index).Left))
        End With



        If Not bSizing Then
            iPointer = Cursors.Arrow

            bEast = (x > (lWidth - VB6.TwipsToPixelsX(100)))
            bWest = (x < 100)
            bNorth = False
            bSouth = False

            If IsArray(m_vTextArray) Then
                If UBound(m_vTextArray, 2) >= Index Then
                    If Val(m_vTextArray(ACCPMFormat, Index) & "") = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine Then
                        bNorth = (y < 100)
                        bSouth = (y > (lHeight - 100))
                    End If
                End If
            End If

            If (bEast And bNorth) Or (bWest And bSouth) Then
                iPointer = Cursors.SizeNESW
            ElseIf ((bEast And bSouth) Or (bWest And bNorth)) Then
                iPointer = Cursors.SizeNWSE
            ElseIf (bEast Or bWest) Then
                iPointer = Cursors.SizeWE
            ElseIf (bSouth Or bNorth) Then
                iPointer = Cursors.SizeNS
            End If

            pnlPanel(Index).Cursor = iPointer
        End If


        Select Case Button
            'Case MouseButtonConstants.LeftButton
            Case MouseButtons.Left
                bSizing = True

                If bNorth Then
                    'Developer Guide No. 74
                    If (lTop + y) > Math.Round(50 / 15) And (lHeight - y) >= VB6.TwipsToPixelsY(textBoxMinimumHeight) Then
                        lHeight = CInt(lHeight - y)
                        lTop = CInt(lTop + y)
                    End If
                End If

                If bSouth Then
                    If y >= VB6.TwipsToPixelsY(textBoxMinimumHeight) And (lTop + y) < ((pnlPanel(Index).Parent.Height) - VB6.TwipsToPixelsY(50)) Then
                        lHeight = CInt(y)
                    End If
                End If


                If bWest Then
                    If (lLeft + x) > Math.Round(50 / 15) And (lWidth - x) >= Math.Round(300 / 15) Then
                        lWidth = CInt(lWidth - x)
                        lLeft = CInt(lLeft + x)
                    End If
                End If

                If bEast Then
                    If x >= 300 And (lLeft + x) < ((pnlPanel(Index).Parent.Width) - Math.Round(50 / 15)) Then
                        lWidth = CInt(x)
                    End If
                End If


                If bNorth Or bSouth Or bEast Or bWest Then
                    SetSizeAndPosition(lLeft, lTop, lWidth, lHeight)
                    pnlPanel(Index).SetBounds((lLeft), (lTop), (lWidth), (lHeight))
                    cmdCommand(Index).SetBounds((lLeft - lCommandOffSet), (lTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
                End If

            Case Else
                bSizing = False

        End Select

    End Sub

    Private Sub pnlPanel_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _pnlPanel_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(pnlPanel, eventSender)
        'Developer Guide No. 64
        If Button = MouseButtons.Left Then pnlPanel(Index).Cursor = Cursors.Default
    End Sub

    'Developer Guide No.181  
    Private Sub TabStrip1_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles _0.DragEnter
        e.Effect = DragDropEffects.Move


    End Sub
    Private Sub TabStrip1_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles _0.DragDrop

        Dim pt As Point = New Point()
        pt = CType(sender, TabPage).PointToClient(New Point(e.X, e.Y))
        Dim x As Single = pt.X 'e.X
        Dim y As Single = pt.Y ' e.Y
        Dim lTag As Object ' = Convert.ToString(m_oNode.Tag)
        lTag = m_oNode.Tag

        m_oNode = Nothing




        m_lReturn = AddControl(-1, lTag, x, y)
    End Sub



    'Developer Guide No. 181
    Private Sub TabStrip1_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles TabStrip1.MouseDown 'Handles TabStrip1.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)


        If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

        If TabStrip1.TabPages.Count > 1 Then
            mnuTabDelete.Available = True
            mnuTabMove.Available = True
        Else
            mnuTabDelete.Available = False
            mnuTabMove.Available = False
        End If

        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Right

                For Each oCtrl As Control In ContainerHelper.Controls(Me)
                    If TypeOf oCtrl Is GroupBox Then oCtrl.Cursor = Cursors.Default
                Next oCtrl

                Ctx_mnuTab.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End Select

    End Sub
    Private Sub picScreen_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles picScreen.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        Dim pt As New Point
        pt = PointToClient(New Point(x, y))


        Dim iPointer As Cursor

        Static bSizing, bSouth, bEast As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        If Not bSizing Then
            bSouth = (y > ((picScreen.Height) - Math.Round(100 / 15)))
            bEast = (x > ((picScreen.Width) - Math.Round(100 / 15)))

            If bEast And bSouth Then
                iPointer = Cursors.SizeNWSE
            ElseIf bEast Then
                iPointer = Cursors.SizeWE
            ElseIf bSouth Then
                iPointer = Cursors.SizeNS
            End If

            picScreen.Cursor = iPointer
        End If



        Select Case Button
            'Developer Guide No. 64
            Case MouseButtons.Left
                bSizing = True

                If bSouth Then
                    If (y > Math.Round(500 / 15)) And y < (picScreenMain.Height) Then picScreen.Height = (y)
                End If

                If bEast Then
                    If x > Math.Round(500 / 15) And x < (picScreenMain.Width) Then picScreen.Width = (x)
                End If

                With picScreen
                    SetSizeAndPosition(CInt((.Left)), CInt((.Top)), CInt((.Width)), CInt((.Height)))
                End With

            Case Else
                bSizing = False
        End Select



    End Sub
    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick

        Timer1.Enabled = False


        m_lReturn = ClearScreen()

        m_lReturn = BuildScreen()


    End Sub

    Private Sub tvwDataDictionary_AfterCollapse(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwDataDictionary.AfterCollapse
        Dim Node As TreeNode = eventArgs.Node

        Node.ImageKey = ACClosedFolder

    End Sub

    Private Sub tvwDataDictionary_AfterExpand(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwDataDictionary.AfterExpand
        Dim Node As TreeNode = eventArgs.Node
        Node.ImageKey = ACOpenFolder
        'Added by Sumeet-To provide GUI same as VB
        'start
        Dim cNode As TreeNode = Node.FirstNode
        If Not IsNothing(cNode) Then
            Do
                If cNode.GetNodeCount(False) > 0 And cNode.IsExpanded Then
                    cNode.ImageKey = ACOpenFolder
                ElseIf cNode.GetNodeCount(False) > 0 Then
                    cNode.ImageKey = ACClosedFolder
                Else
                    cNode.ImageIndex = -1
                End If
                cNode = cNode.NextNode
            Loop While Not IsNothing(cNode)
        End If
        'end
    End Sub

    Private Sub tvwDataDictionary_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwDataDictionary.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim lTag As Integer
        Dim sCaption, sName, sPropertyName, sColumnName As String
        Dim iDataType As Integer
        Dim lDataModeltype As Integer

        tvwDataDictionary.SelectedNode = tvwDataDictionary.GetNodeAt(x, y)

        m_oNode = tvwDataDictionary.SelectedNode

        If Not (m_oNode Is Nothing) Then
            If m_oNode.GetNodeCount(False) <> 0 Then
                m_oNode = Nothing
            End If
        End If

        If m_oNode Is Nothing Then
            Exit Sub
        End If

        'KB PN 5544
        'if we have no objects which are selectable for the screen then
        'm_oNode.Tag will not be an array and we'll get a type mismatch
        'so check first
        'If Information.IsArray(Convert.ToString(m_oNode.Tag)) Then
        If Information.IsArray(m_oNode.Tag) Then
            'tag 0=data model type   1=dictionary id

            lTag = m_oNode.Tag(1) 'CInt(Convert.ToString(1)) 'Alkesh
        Else

            MessageBox.Show("The Data Model does not have any objects which are selectable for screen", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub

        End If

        If lTag >= 0 Then 'check if attribute bound control

            lDataModeltype = m_oNode.Tag(0) ' CInt(Convert.ToString(0)) 'Alkesh


            sName = CStr(g_vDataDictionary(lDataModeltype)(ACPPropertyName, lTag))

            m_lReturn = SpacifyName(sName, sPropertyName)


            sColumnName = CStr(g_vDataDictionary(lDataModeltype)(ACPColumnName, lTag))

            iDataType = 0

            If CStr(g_vDataDictionary(lDataModeltype)(ACPDataType, lTag)) <> "" Then

                iDataType = CInt(g_vDataDictionary(lDataModeltype)(ACPDataType, lTag))
            End If
            'GSD
            sCaption = ""

            If (Convert.ToString(g_vDataDictionary(lDataModeltype)(ACPSpecialsType, lTag)) = "") Then
                g_vDataDictionary(lDataModeltype)(ACPSpecialsType, lTag) = 0
            End If

            Select Case g_vDataDictionary(lDataModeltype)(ACPSpecialsType, lTag)
                Case GISSharedPropertyConstants.ACOGISListID

                    sCaption = "GIS List: " & CStr(g_vDataDictionary(lDataModeltype)(ACPSpecialsTypeReference, lTag))
                Case GISSharedPropertyConstants.ACOPMLookupTableName

                    sCaption = "Look up: " & CStr(g_vDataDictionary(lDataModeltype)(ACPSpecialsTypeReference, lTag))
                Case GISSharedPropertyConstants.ACOPartyTypeID
                    sCaption = "Party"
                Case GISSharedPropertyConstants.ACOSumInsuredTypeID
                    sCaption = "Sum insured"
                Case GISSharedPropertyConstants.ACOStdWordingType
                    sCaption = "Standard wording"
                Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                    sCaption = "User defined"
                Case GISSharedPropertyConstants.ACOProductID
                    sCaption = "Policy"
                Case GISSharedPropertyConstants.ACOReserveID
                    sCaption = "Claim Reserve"
                Case GISSharedPropertyConstants.ACOPaymentID
                    sCaption = "Claim Payment"

                Case Else
                    If sPropertyName = "" Then
                        sCaption = "List view"
                    End If


                    If sColumnName.ToUpper().StartsWith("ADDRESS_CNT") Then
                        sCaption = "Address"
                    End If

                    If sCaption = "" Then
                        'If we're here then it's a textbox or checkbox
                        Select Case iDataType
                            Case iGISSharedConstants.GISDataTypeOption
                                sCaption = "Yes/No"
                            Case iGISSharedConstants.GISDataTypeCurrency
                                sCaption = "Currency"
                            Case iGISSharedConstants.GISDataTypeText
                                sCaption = "String"
                            Case iGISSharedConstants.GISDataTypeComment
                                sCaption = "Comment"
                            Case iGISSharedConstants.GISDataTypePercentage
                                sCaption = "Percentage"
                            Case iGISSharedConstants.GISDataTypeNumeric
                                sCaption = "Number"
                            Case iGISSharedConstants.GISDataTypeDate
                                sCaption = "Date"
                            Case iGISSharedConstants.GISDataTypeInteger
                                sCaption = "Integer"
                        End Select
                    End If
            End Select
        Else
            'non database element

            Select Case lTag
                Case PBRiskScreenCommon.ndcFreeFormatText
                    sCaption = freeFormatText
                Case PBRiskScreenCommon.ndcHyperlink
                    sCaption = hyperlink
                Case PBRiskScreenCommon.ndcFindControl
                    sCaption = FindControl
            End Select

        End If
        Select Case eventArgs.Button
            'Developer Guide No. 64
            Case MouseButtons.Left
            Case MouseButtons.Right
                'Menu time
                mnuTreeViewDataType.Text = sCaption

                Ctx_mnuTreeView.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End Select

    End Sub

    Private Sub tvwDataDictionary_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwDataDictionary.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        If m_oNode Is Nothing Then
            Exit Sub
        End If

        'If Button = MouseButtonConstants.LeftButton Then
        If eventArgs.Button = MouseButtons.Left Then


            'Akash: Commented
            'tvwDataDictionary.DragIcon = tvwDataDictionary.SelectedNode.CreateDragImage

            'Akash:Commented because need to use DoDragDrop function here
            'tvwDataDictionary.Drag(vbBeginDrag)
            'tvwDataDictionary.DoDragDrop(tvwDataDictionary.Tag, DragDropEffects.None)
            tvwDataDictionary.DoDragDrop(tvwDataDictionary.SelectedNode.Tag, DragDropEffects.Move)
        End If

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        m_lReturn = m_oFormFields.GotFocus(txtDescription)

    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave

        m_lReturn = m_oFormFields.LostFocus(txtDescription)

    End Sub

    Private Sub txtScreenCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScreenCode.Enter

        m_lReturn = m_oFormFields.GotFocus(txtScreenCode)

    End Sub

    Private Sub txtScreenCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScreenCode.Leave

        m_lReturn = m_oFormFields.LostFocus(txtScreenCode)
        ScreenCode = txtScreenCode.Text

    End Sub
    'Developer Guide No. 181
    Private Sub txtText_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs)
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        KeyCode = 0

    End Sub

    Private Sub SetLabelAppearance(ByRef cControl As Control, ByRef cLabel As Control)
        Dim lTag As Integer = CInt(Convert.ToString(ControlHelper.GetTag(cControl)))

        ' RAW 20/10/2004 : CQ1814 : reworked this function and removed code to set height etc

        If lTag = PBRiskScreenCommon.ndcHyperlink Then
            cLabel.Font = VB6.FontChangeUnderline(cLabel.Font, True)
            cLabel.ForeColor = Color.FromArgb(0, 0, 192)
        Else
            cLabel.Font = VB6.FontChangeUnderline(cLabel.Font, False)
        End If
    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetControlTabIndex
    '
    ' Description:
    '
    ' iIncrement, used where there is a bloco of controls such as list views
    '
    ' History: 02/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub SetControlTabIndex(ByRef TabSetIndex As Object, ByRef cControl As Control, ByVal iIncrement As Integer, Optional ByRef cLabel As Control = Nothing)


        Try

            If m_lSetTabOrderCount > 0 Then
                If HasEnabledProperty(cControl) Then

                    '   cControl.Enabled = IsInputControl(cControl)

                    m_lSetTabOrderCount -= 1

                    TabSetIndex = m_lSetTabOrderIndex

                    m_lSetTabOrderIndex += iIncrement


                    If Not (cLabel Is Nothing) Then
                        ControlHelper.SetEnabled(cLabel, False)
                        ChangeForeColor(cLabel, IIf(cLabel.Text = PBRiskScreenCommon2.ACBlankCaption, SystemColors.GrayText, (SystemColors.ControlText)))
                        'Developer Guide No. 74
                    ElseIf cControl.ForeColor = Color.Blue Then
                        ChangeForeColor(cControl, (SystemColors.ControlText))
                    End If

                    If m_lSetTabOrderCount = 0 Then
                        MessageBox.Show("Tab order has been set", "Set Control's tab order", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        EnableControlsAfterTabSet(False)
                    End If
                End If
            End If

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetControlTabIndex")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetControlTabIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetControlTabIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: EnableControlsAfterTabSet
    '
    ' Description:
    '
    ' History: 02/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub EnableControlsAfterTabSet(ByRef bClearTabStop As Boolean)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".EnableControlsAfterTabSet")

        Try

            Dim lTabs As Integer

            Dim arrListControl As New ArrayList
            ControlList(Me, arrListControl)

            'For Each ctlControl As Control In ContainerHelper.Controls(Me)
            For ctlCount As Integer = 0 To arrListControl.Count - 1
                'For Each ctlControl As Control In ContainerHelper.Controls(Me)
                Dim ctlControl As Control = CType(arrListControl(ctlCount), Control)
                Try


                    If HasVisibleProperty(ctlControl) Then
                        If ControlHelper.GetVisible(ctlControl) Then

                            If HasEnabledProperty(ctlControl) Then
                                If Not ControlHelper.GetEnabled(ctlControl) Then
                                    ControlHelper.SetEnabled(ctlControl, PBRiskScreenCommon2.IsInputControl(ctlControl, g_vDataDictionary))
                                End If
                            End If

                            If TypeOf ctlControl Is Label Then
                                ControlHelper.SetEnabled(ctlControl, True)
                                ChangeForeColor(ctlControl, IIf(ctlControl.Text = PBRiskScreenCommon2.ACBlankCaption, SystemColors.GrayText, (SystemColors.ControlText)))
                            Else
                                ChangeForeColor(ctlControl, SystemColors.ControlText)
                            End If
                        End If

                        If bClearTabStop Then 'And CBool((ctlControl.TabIndex > 9)) Then
                            If ctlControl.TabIndex > 9 Then ctlControl.TabIndex = 0
                        End If
                    End If
                Catch ex As Exception

                End Try
            Next

            picScreen.Cursor = Cursors.Default

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".EnableControlsAfterTabSet")

        Catch ex As Exception

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".EnableControlsAfterTabSet")
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableControlsAfterTabSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableControlsAfterTabSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddToListColumnPositionArray
    '
    ' Description:
    '
    ' History: 10/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub AddToListColumnPositionArray(ByRef iEntries As Integer, ByRef vArray(,) As Object, ByRef vControlArray(,) As Object, ByRef cControl As Object, ByRef lTag As Integer)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddToListColumnPositionArray")

        Try

            If Information.IsArray(vControlArray) Then
                For lTemp As Integer = vControlArray.GetLowerBound(1) To vControlArray.GetUpperBound(1)

                    If vControlArray(PBRiskScreenCommon.ACCIsDeleted, CInt(lTemp)) = gPMConstants.PMEReturnCode.PMFalse Then


                        If CDbl(vControlArray(PBRiskScreenCommon.ACCIncludeInList, CInt(lTemp))) <> 0 Then
                            If iEntries = 0 Then
                                ReDim vArray(2, iEntries)
                            Else
                                ReDim Preserve vArray(2, iEntries)
                            End If


                            If Convert.IsDBNull(vControlArray(PBRiskScreenCommon.ACCColumnPosition, CInt(lTemp))) Or IsNothing(vControlArray(PBRiskScreenCommon.ACCColumnPosition, CInt(lTemp))) Then


                                vControlArray(PBRiskScreenCommon.ACCColumnPosition, CInt(lTemp)) = 0
                            End If



                            vArray(0, iEntries) = CStr(CInt(vControlArray(PBRiskScreenCommon.ACCColumnPosition, CInt(lTemp))))

                            vArray(1, iEntries) = CStr(lTag + CDbl(lTemp))

                            vArray(2, iEntries) = PBRiskScreenCommon.StripColonFromCaption(cControl(lTemp).text)
                            iEntries += 1
                        End If
                    End If
                Next
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddToListColumnPositionArray")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddToListColumnPositionArray")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToListColumnPositionArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToListColumnPositionArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    Private Sub txtText_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) ' Handles _txtText_0.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)
        Dim lTop, lLeft, lWidth, lHeight, lLabelOffSet As Integer
        Dim iPointer As Cursor
        Static bSizing, bEast, bNorth, bSouth, bWest As Boolean

        If m_lSetTabOrderCount > 0 Then Exit Sub 'don't move controls whilst setting tabs

        With txtText(Index)
            'Developer Guide No. 74
            lTop = CInt((.Top))
            lLeft = CInt((.Left))
            lWidth = CInt((.Width))
            lHeight = CInt((.Height))
            lLabelOffSet = CInt((.Left) - (lblTextLabel(Index).Left))
        End With



        If Not bSizing Then
            iPointer = Cursors.Arrow
            'Developer Guide No. 74
            bEast = (x > (lWidth - CInt(VB6.TwipsToPixelsX(100))))
            bWest = (x < CInt(VB6.TwipsToPixelsX(100)))

            If Conversion.Val(NullToString(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, Index)) & "") = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine Then
                'Developer Guide No. 74
                bNorth = (y < CInt(VB6.TwipsToPixelsY(100)))
                bSouth = (y > (lHeight - CInt(VB6.TwipsToPixelsY(100))))
            Else
                bNorth = False
                bSouth = False
            End If

            If (bEast And bNorth) Or (bWest And bSouth) Then
                iPointer = Cursors.SizeNESW
            ElseIf ((bEast And bSouth) Or (bWest And bNorth)) Then
                iPointer = Cursors.SizeNWSE
            ElseIf (bEast Or bWest) Then
                iPointer = Cursors.SizeWE
            ElseIf (bSouth Or bNorth) Then
                iPointer = Cursors.SizeNS
            End If

            txtText(Index).Cursor = iPointer
        End If



        Select Case Button
            'Developer Guide No. 64
            Case Windows.Forms.MouseButtons.Left
                bSizing = True

                If bNorth Then
                    'Developer Guide No. 74
                    If (lTop + y) > 50 And (lHeight - y) >= VB6.TwipsToPixelsY((textBoxMinimumHeight)) Then
                        lHeight = CInt(lHeight - y)
                        lTop = CInt(lTop + y)
                    End If
                End If

                If bSouth Then
                    'Developer Guide No.74
                    If y >= VB6.TwipsToPixelsY((textBoxMinimumHeight)) And (lTop + y) < (txtText(Index).Parent.Height) - CInt((50)) Then
                        lHeight = CInt(y)
                    End If
                End If


                If bWest Then
                    If (lLeft + x) > 50 And (lWidth - x) >= VB6.TwipsToPixelsX(300) Then
                        lWidth = CInt(lWidth - x)
                        lLeft = CInt(lLeft + x)
                    End If
                End If

                If bEast Then
                    'If x >= 300 And (lLeft + x) < (VB6.PixelsToTwipsX(txtText(Index).Parent.Width) - 50) Then
                    If x >= VB6.TwipsToPixelsX(300) And (lLeft + x) < txtText(Index).Parent.Width - CInt(VB6.TwipsToPixelsX(50)) Then
                        lWidth = CInt(x)
                    End If
                End If


                If bNorth Or bSouth Or bEast Or bWest Then
                    SetSizeAndPosition(lLeft, lTop, lWidth, lHeight)
                    'Developer Guide No. 74
                    txtText(Index).SetBounds((lLeft), (lTop), (lWidth), (lHeight))
                    lblTextLabel(Index).SetBounds((lLeft - lLabelOffSet), (lTop + VB6.TwipsToPixelsY(45)), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

                End If

            Case Else
                bSizing = False

        End Select

    End Sub
    'Developer Guide No. 181
    Private Sub txtText_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) 'Handles _txtText_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)
        'Developer Guide No. 64
        If Button = Windows.Forms.MouseButtons.Left Then txtText(Index).Cursor = Cursors.IBeam
    End Sub
    ' ***************************************************************** '
    '
    ' Name: SetToolTip
    '
    ' Description:
    '
    ' History: 10/10/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub SetToolTip(ByRef r_oControl As Object, ByRef r_oLabel As Object, ByRef v_sObjectName As String, ByVal v_sPropertyName As String)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SetToolTip")

        Try

            SpacifyName(v_sObjectName & "." & v_sPropertyName, v_sObjectName)

            '         r_oControl.ToolTipText = v_sObjectName

            'r_oLabel.ToolTipText = v_sObjectName

            ToolTip1.SetToolTip(r_oControl, v_sObjectName)
            ToolTip1.SetToolTip(r_oLabel, v_sObjectName)

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SetToolTip")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetToolTip")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetToolTip Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetToolTip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: RejigControlArray
    '
    ' Description: if control, in an array, is out of position, rejig it
    '
    ' History: 26/07/2001 CLG - Created, from Tom O'Toole code
    '
    ' ***************************************************************** '
    Public Sub RejigControlArray(ByRef cControls As Object)


        Try


            For Each cControl As Object In cControls

                If cControl.Left < 0 Then
                    cControl.Left += VB6.TwipsToPixelsX(75000)
                End If

                If cControl.Left > 75000 Then
                    cControl.Left -= VB6.TwipsToPixelsX(75000)
                End If
            Next cControl

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RejigControl")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RejigControlArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RejigControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub


    ' ***************************************************************** '
    '
    ' Name: RuleEditor
    '
    ' Description:
    '
    ' History: 15/02/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function RuleEditor(ByVal iRuleEditMode As Integer) As Integer
        Dim result As Integer = 0

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RuleEditor")



        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'This seems to cause the VB multiple instance error - dunno why
            Dim temp_oRuleEditor As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="iPMURuleEditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRuleEditor = temp_oRuleEditor

            If oRuleEditor Is Nothing Then
                Return result
            End If

            'set the default editor values

            oRuleEditor.RulePath = m_sRulePath

            oRuleEditor.FixedFile = True

            oRuleEditor.DataModelId = GISDataModelId

            oRuleEditor.DataModelCode = GISDataModelCode

            Select Case iRuleEditMode
                Case RuleEditorDefaults

                    oRuleEditor.RuleFileName = txtScreenCode.Text.Trim() & "Def.rul"

                Case RuleEditorDynamicValidation

                    oRuleEditor.RulePath = m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0)

                    oRuleEditor.RuleFileName = "Database"

                Case RuleEditorPostSaveValidation

                    oRuleEditor.RuleFileName = txtScreenCode.Text.Trim() & "Val.rul"

                Case RuleEditorRisk
                    oRuleEditor = Nothing

                Case RuleEditorUnderwritingAuithorityLimits
                    oRuleEditor = Nothing
            End Select

            If Not (oRuleEditor Is Nothing) Then

                oRuleEditor.Start()


                If oRuleEditor.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    Select Case iRuleEditMode
                        Case RuleEditorDynamicValidation

                            m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0) = oRuleEditor.RulePath
                    End Select
                End If


                oRuleEditor.Dispose()


                oRuleEditor = Nothing
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RuleEditor")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RuleEditor")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RuleEditor Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RuleEditor", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GenericMenuSetup
    '
    ' Description:
    '
    ' History: 06/03/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub GenericClickEvent(ByRef sControlTypeName As String, ByRef vControlArray(,) As Object, ByRef cControlArray() As Object, ByRef cLabelArray() As Object, ByRef Index As Integer, ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GenericClickEvent")

        Try


            PBRiskScreenCommon.g_lx = CInt(x)
            PBRiskScreenCommon.g_ly = CInt(y)



            SetSizeAndPosition(cControlArray(Index).Left, cControlArray(Index).Top)

            Select Case Button
                'Developer Guide No. 64
                Case Windows.Forms.MouseButtons.Left


                    SetControlTabIndex(vControlArray(PBRiskScreenCommon.ACCTabSetIndex, Index), cControlArray(Index), 1, cLabelArray(Index))
                    'Developer Guide No. 64
                Case Windows.Forms.MouseButtons.Right

                    If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

                    'Menu time
                    m_lIndex = Index
                    PBRiskScreenCommon.g_sControlName = sControlTypeName
                    'RVH 26/9/2003 CQ1946 - Set array at time of click event - this allows
                    '                       for the menu items selected later to determine
                    '                       information about the control that has been
                    '                       clicked.
                    m_vGenericClickArray = cControlArray

                    mnuControlFormat.Available = True
                    mnuControlHelp.Available = True
                    mnuControlEntry.Available = True
                    mnuControlNameLabel.Available = False
                    mnuControlFormat.Available = False

                    'are we on a child screen? Add include in list/set column order options

                    If m_lGISObjectId = 0 Or Not cControlArray(Index).Visible Then
                        mnuControlIncludeInList.Available = False
                    Else
                        mnuControlIncludeInList.Available = True

                        mnuControlIncludeInList.Checked = CBool(vControlArray(PBRiskScreenCommon.ACCIncludeInList, Index))
                    End If
                    mnuControlSetListColumnOrder.Available = mnuControlIncludeInList.Available

                    mnuFormat(formatLastValue + 1).Available = True 'show dummy entry to ensure something is displayed
                    For iTemp As Integer = gPMConstants.PMEFormatStyle.PMFormatString To formatLastValue
                        mnuFormat(iTemp).Available = False
                        mnuFormat(iTemp).Checked = False
                    Next iTemp

                    'special handling
                    Select Case PBRiskScreenCommon.g_sControlName
                        Case "lblTextLabel"
                            mnuControlFormat.Available = True
                            'First split them by type...

                            'Developer Guide No. 65
                            If cControlArray(Index).Tag >= 0 Then 'check if attribute bound control

                                'Developer Guide No. 65
                                Select Case g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPDataType, cControlArray(Index).Tag Mod 10000)
                                    Case iGISSharedConstants.GISDataTypeText
                                        mnuControlFormat.Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatStringCase).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatStringUpper).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatStringMultiLine).Available = True
                                        mnuControlEntry.Available = True
                                    Case iGISSharedConstants.GISDataTypeComment
                                        mnuControlFormat.Available = False
                                        mnuControlIncludeInList.Available = False
                                        mnuControlEntry.Available = True
                                    Case iGISSharedConstants.GISDataTypeDate
                                        mnuControlFormat.Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateShort).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateMedium).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateLong).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatDateTimeLong).Available = True
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatString).Available = False
                                        mnuFormat(gPMConstants.PMEFormatStyle.PMFormatStringMultiLine).Available = False
                                    Case Else
                                        mnuControlFormat.Available = False
                                End Select

                            Else
                                'free format text
                                'g_sControlName = "lblFreeFormatText"
                                mnuControlFormat.Available = False
                                mnuControlHelp.Available = False
                                mnuControlEntry.Available = False
                                mnuControlFormat.Available = False
                                mnuControlNameLabel.Available = True
                            End If

                            If Not (Convert.IsDBNull(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, Index)) Or IsNothing(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, Index))) Then
                                'format stored in the lower 6 bits
                                If CBool(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, Index)) And PBRiskScreenCommon.ACFormatStandardMask Then
                                    'Developer Guide No. 101
                                    mnuFormat(CInt(m_vTextArray(PBRiskScreenCommon.ACCPMFormat, Index)) And PBRiskScreenCommon.ACFormatStandardMask).Checked = True
                                End If
                            End If
                        Case "chkYesNo"
                            'has to be done like this otherwise you can't hide the last entry
                            mnuFormat(gPMConstants.PMEFormatStyle.PMFormatBoolean).Available = True
                            For iTemp As Integer = gPMConstants.PMEFormatStyle.PMFormatString To formatLastValue
                                If iTemp <> gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                                    mnuFormat(iTemp).Available = False
                                    mnuFormat(iTemp).Checked = False
                                End If
                            Next iTemp
                            mnuControlFormat.Available = True
                            mnuFormat(gPMConstants.PMEFormatStyle.PMFormatBoolean).Checked = False
                            If m_vCheckArray(PBRiskScreenCommon.ACCPMFormat, Index) = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                                mnuFormat(gPMConstants.PMEFormatStyle.PMFormatBoolean).Checked = True
                            End If

                    End Select

                    ' SET 26072002 - following 2 lines cause an error when formating
                    ' an integer text box. Commented out because the line 'mnuControlFormat.Visible = False'
                    ' above seems to do the required thing
                    '        mnuFormat(formatLastValue + 1).Visible = False
                    '        mnuFormat(formatLastValue + 1).Visible = False
                    ' SET 26072002 - End

                    If mnuControlFormat.Available Then
                        mnuFormat(formatLastValue + 1).Available = False 'hide dummy entry tnow we know something is displayed
                    End If
                    Ctx_mnuControl.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    SetSizeAndPosition()
            End Select



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GenericClickEvent")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GenericClickEvent")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenericClickEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenericClickEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: SnapToGrid
    '
    ' Description:
    '
    ' History: 15/04/2002 CLG - Created.
    '
    ' ***************************************************************** '

    'Private Sub SnapToGrid(ByRef cControl As Object)
    '
    'Dim dValue As Double
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SnapToGrid")
    '
    'Try 
    '

    'dValue = cControl.Left / 10#

    'cControl.Left = Math.Round(dValue, 0) * 10
    '

    'dValue = cControl.Top / 10#

    'cControl.Top = Math.Round(dValue, 0) * 10
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SnapToGrid")
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SnapToGrid")
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SnapToGrid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SnapToGrid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub
    ' ***************************************************************** '
    '
    ' Name: MoveTab
    '
    ' Description:
    '
    ' History: 17/07/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub MoveTab()

        Dim lTag As Integer
        Dim vArray() As Object
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".MoveTab")

        Try

            ReDim vArray(m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Count - 1)

            'for each entry, get tab caption for each tab
            For lTemp As Integer = 1 To m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Count
                lTag = CInt(Mid(Convert.ToString(m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Item(lTemp - 1).Tag), 2))

                vArray(lTemp - 1) = PBTabStripCommon.TabGetCaption(TabStrip1, lTag)
            Next
            'no fixup the captions in the new positions
            For lTemp As Integer = 1 To m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Count

                PBTabStripCommon.TabSetCaption(TabStrip1, lTemp - 1, CStr(vArray(lTemp - 1)))
            Next

            'for each entry, set the new position of the frame to the -ve value so as not to trash the other values
            For lTemp As Integer = 1 To m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Count
                For lFrameIndex As Integer = 0 To m_lFrameIndex
                    If Convert.ToString(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex)) = Convert.ToString(Mid(Convert.ToString(m_frmSetListColumnOrder.ListViewSetListColumnOrder.Items.Item(lTemp - 1).Tag), 2)) Then
                        m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex) = "_" & lTemp - 1
                    End If
                Next
            Next

            For lFrameIndex As Integer = 0 To m_lFrameIndex
                If CStr(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex)).Substring(0, 1) = "_" Then
                    m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex) = CInt(Mid(CStr(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex)), 2))
                    fraFrame(lFrameIndex).Parent = TabStrip1.TabPages(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lFrameIndex))
                End If
            Next

            'TabStrip1.SelectTab(0)

            TabStrip1_SelectedIndexChanged(Nothing, Nothing)
            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".MoveTab")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".MoveTab")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MoveTab Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveTab", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    '
    ' Name: TabGetCurrentIndex
    '
    ' Description:
    '
    ' History: 10/07/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function TabGetCurrentIndex(ByRef r_TabStrip As TabControl) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".TabGetCurrentIndex")

        Try
            'Developer Guide No. 159
            result = CInt(r_TabStrip.SelectedTab.Name.Substring(1, r_TabStrip.SelectedTab.Name.Length - 1))

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".TabGetCurrentIndex")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".TabGetCurrentIndex")

            result = 0

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TabGetCurrentIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TabGetCurrentIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: addItemsToTreeView
    '
    ' Description:
    '
    ' History: 19/12/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function addItemsToTreeView(ByRef tvwDataDictionary As TreeView, ByRef r_vDataDictionary(,) As Object, ByVal v_sParent As String, ByVal v_lGISDMType As Integer) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addItemsToTreeView")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim lObjectId, lTheParentId As Integer
            Dim lParentId As Integer
            Dim bIsSpecial As Boolean
            Dim sName As String = ""
            Dim nodTemp As TreeNode
            Dim lPropertyId As Integer

            lObjectId = -100


            lTheParentId = CInt(r_vDataDictionary(ACOParentObjectId, 0))

            For iTemp As Integer = r_vDataDictionary.GetLowerBound(1) To r_vDataDictionary.GetUpperBound(1)

                'Change of object?


                If CInt(r_vDataDictionary(ACOGISObjectId, iTemp)) <> lObjectId Then


                    lObjectId = CInt(r_vDataDictionary(ACOGISObjectId, iTemp))

                    lParentId = 0

                    If CStr(r_vDataDictionary(ACOParentObjectId, iTemp)) <> "" Then

                        lParentId = CInt(r_vDataDictionary(ACOParentObjectId, iTemp))
                    End If

                    bIsSpecial = False

                    If CStr(r_vDataDictionary(ACOIsNonGIS, iTemp)) <> "" Then

                        If CStr(r_vDataDictionary(ACOIsNonGIS, iTemp)) = "1" Then
                            bIsSpecial = True
                        End If
                    End If


                    m_lReturn = SpacifyName(sOriginal:=CStr(r_vDataDictionary(ACOObjectName, iTemp)), sNew:=sName)




                    If (CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTRisk Or CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTClaim) And ((lParentId <> lTheParentId) And (m_lGISObjectId <> lObjectId)) Then
                        nodTemp = tvwDataDictionary.Nodes.Find(v_sParent, True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000"), sName)

                        'Akash : Modified the code
                        'nodTemp.Tag = CStr(New Object() {v_lGISDMType, iTemp})
                        nodTemp.Tag = New Object() {v_lGISDMType, iTemp}

                    Else



                        If (m_lScreenType = GISDataModelType.GISOTRisk And CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTDisclosure) Or 0 = 1 Then
                            'don't display GISOTDisclosure as a child in a risk screen
                        Else
                            nodTemp = tvwDataDictionary.Nodes.Find(v_sParent, True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000"), sName, ACOpenFolder)
                            nodTemp.Expand()

                            'Akash : Modified the code
                            'nodTemp.Tag = CStr(New Object() {v_lGISDMType, iTemp})
                            nodTemp.Tag = New Object() {v_lGISDMType, iTemp}
                        End If
                    End If
                End If

                If (CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTAssociatedClient Or CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTDisclosure Or CDbl(r_vDataDictionary(ACOIsNonGIS, iTemp)) = GISDataModelType.GISOTPeril) And m_lGISObjectId <> lObjectId Then
                    'for non risk objects clear the properties

                    g_vDataDictionary(v_lGISDMType)(ACPPropertyName, iTemp) = ""

                    g_vDataDictionary(v_lGISDMType)(ACPColumnName, iTemp) = ""

                    g_vDataDictionary(v_lGISDMType)(ACPDataType, iTemp) = ""
                Else


                    If CStr(r_vDataDictionary(ACPGISPropertyId, iTemp)) <> "" Then

                        lPropertyId = CInt(r_vDataDictionary(ACPGISPropertyId, iTemp))


                        m_lReturn = SpacifyName(sOriginal:=CStr(r_vDataDictionary(ACPPropertyName, iTemp)), sNew:=sName)

                        nodTemp = tvwDataDictionary.Nodes.Find("c" & StringsHelper.Format(lObjectId, "000"), True)(0).Nodes.Add("c" & StringsHelper.Format(lObjectId, "000") & StringsHelper.Format(lPropertyId, "0000"), sName)
                        nodTemp.ImageIndex = -1
                        nodTemp.Tag = New Object() {v_lGISDMType, iTemp}
                    End If
                End If
            Next iTemp



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addItemsToTreeView")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".addItemsToTreeView")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="addItemsToTreeView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="addItemsToTreeView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RedimSecondElement
    '
    ' Description:
    '
    ' History: 02/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub RedimSecondElement(ByRef r_vMyarray() As Object, ByVal v_iSize As Integer)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RedimSecondElement")

        Try

            ReDim r_vMyarray(v_iSize)

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RedimSecondElement")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RedimSecondElement")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RedimSecondElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RedimSecondElement", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: SelectChildScreen
    '
    ' Description:
    '
    ' History: 21/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function SelectChildScreen(ByVal v_lScreenType As Integer, ByRef r_lScreenId As Integer) As Integer

        Dim result As Integer = 0
        Dim ofrmSelectChildScreen As frmSelectChildScreen

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectChildScreen")

        Try

            Dim vArray(,) As Object
            Dim oListItem As ListViewItem

            result = gPMConstants.PMEReturnCode.PMTrue

            ofrmSelectChildScreen = New frmSelectChildScreen()

            Select Case v_lScreenType
                Case GISDataModelType.GISOTAssociatedClient
                    ofrmSelectChildScreen.Text = "Select Associated Client Screen"
                Case GISDataModelType.GISOTDisclosure
                    ofrmSelectChildScreen.Text = "Select Disclosure Screen"
                    '        Case GISOTClaim
                    '            ofrmSelectChildScreen.Caption = "Select Claim Screen"
                Case GISDataModelType.GISOTPeril
                    ofrmSelectChildScreen.Text = "Select Claim Peril Screen"

            End Select

            With ofrmSelectChildScreen
                'set list view defaults
                .ListViewSelectScreen.Items.Clear()
                .ListViewSelectScreen.Columns.Add("D1", "Code", 94)
                .ListViewSelectScreen.Columns.Item("D1").Width = CInt(VB6.TwipsToPixelsX(1100))
                .ListViewSelectScreen.Columns.Add("D2", "Description", 94)
                .ListViewSelectScreen.Columns.Item("D2").Width = CInt(VB6.TwipsToPixelsX(4350))
                '    .ListViewSelectScreen.ColumnHeaders.Item("D2").Alignment = lvwColumnCenter
                ListViewHelper.SetSortedProperty(.ListViewSelectScreen, False)

                SharedFiles.ListViewFunc.SetExtraListViewProperties(.ListViewSelectScreen.Handle.ToInt32(), True, False)
            End With

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRMaintainScreenData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness


            m_oBusiness.GetScreensByObjectType(v_lScreenType, m_lGISDataModelId(m_lGISDMType), vArray)

            ' Terminate the business object

            m_oBusiness.Dispose()
            m_oBusiness = Nothing

            'add to list view
            If Information.IsArray(vArray) Then

                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                    oListItem = ofrmSelectChildScreen.ListViewSelectScreen.Items.Add("Wibble")

                    oListItem.Text = CStr(vArray(2, lTemp))


                    oListItem.Tag = CStr(vArray(0, lTemp)) 'tag

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vArray(3, lTemp)) ' caption
                    'oListItem.SubItems(2) = vArray(3, lTemp) ' caption
                Next
            End If

            ofrmSelectChildScreen.ShowDialog()

            r_lScreenId = ofrmSelectChildScreen.lScreenId

            ofrmSelectChildScreen.Close()
            ofrmSelectChildScreen = Nothing

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectChildScreen")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SelectChildScreen")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectChildScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectChildScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    'Akash: Modified
    'Private Sub uctCLMPerilDT1_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles uctCLMPerilDT1.MouseDown
    Private Sub uctCLMPerilDT1_MouseDown(ByVal eventSender As Object, ByVal eventArgs As uctCLMPerilDTControl.uctCLMPerilDT.MouseDownEventArgs) Handles uctCLMPerilDT1.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        SetSizeAndPosition()

        Select Case Button
            'Alkeh
            'Case MouseButtonConstants.LeftButton
            'Case MouseButtonConstants.RightButton
            Case MouseButtons.Left
            Case MouseButtons.Right

                If m_lSetTabOrderCount > 0 Then Exit Sub 'in set tab order mode

                'Menu time
                PBRiskScreenCommon.g_sControlName = "uctCLMPerilDT1"
                m_lIndex = 0

                If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                    Ctx_mnuListView.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                Else
                    MessageBox.Show("Please save this screen before editing the Drill Down", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

        End Select


    End Sub


    Private Sub GenericLabelChangeEvent(ByRef r_oLabelCtrl As Object)


        'Developer Guide No. 60
        r_oLabelCtrl.ForeColor = IIf(r_oLabelCtrl.Name = PBRiskScreenCommon2.ACBlankCaption, SystemColors.GrayText, SystemColors.ControlText)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: HasEnabledProperty
    '
    ' Description: Does the passed in control support 'Enabled'.

    '
    ' History: 20/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Private Function HasEnabledProperty(ByRef r_oCtrl As Object) As Boolean
        Return TypeOf r_oCtrl Is Button Or TypeOf r_oCtrl Is Label Or TypeOf r_oCtrl Is TextBox Or TypeOf r_oCtrl Is CheckBox Or TypeOf r_oCtrl Is ListView Or TypeOf r_oCtrl Is TreeView Or TypeOf r_oCtrl Is GroupBox Or TypeOf r_oCtrl Is uctSimulateCombo


    End Function


    ' ***************************************************************** '
    '
    ' Name: HasVisibleProperty
    '
    ' Description: Does the passed in control support 'Visible'.
    '
    ' History: 20/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    'Akash: Change the type of parameter
    'Private Function HasVisibleProperty(ByRef r_oCtrl As Control) As Boolean
    Private Function HasVisibleProperty(ByRef r_oCtrl As Object) As Boolean
        Return Not (TypeOf r_oCtrl Is Timer) And Not (TypeOf r_oCtrl Is ImageList) And Not (TypeOf r_oCtrl Is ToolStripMenuItem)


    End Function

    Private Sub textLabelMouseMove(ByVal v_sControlName As String, ByRef Button As Integer, ByVal x As Single, ByVal y As Single, ByRef cControl As Control, ByRef cLabel As Control, ByRef cCaption As Label, ByVal v_vSnapToGrid As Object)

        If m_lSetTabOrderCount = 0 Then
            PBRiskScreenCommon2.textLabel_MouseMove(v_sControlName, Button, x, y, cControl, cLabel, cCaption, v_vSnapToGrid)
        End If

    End Sub

    Private Sub ChangeForeColor(ByRef r_oCtrl As Object, ByRef iColor As Color)
        Dim checkST As String = ""
        checkST = r_oCtrl.Name
        If checkST.StartsWith("_") And checkST.LastIndexOf("_") <> 0 Then
            checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
        ElseIf checkST.StartsWith("_") Then
            checkST = checkST.Substring(1, checkST.Length - 2)
        End If

        Select Case checkST
            Case "uctAddress1", "uctStandardWording1", "uctSumInsured1", "uctClaimReserve1", "uctClaimPayment1", "uctCLMCaseClaim1", "uctCLMCaseHeader1"

                r_oCtrl.Parent.ForeColor = iColor

            Case "cmdCommand"

                pnlPanel(GetIndex(r_oCtrl, cmdCommand)).ForeColor = iColor

            Case "lvwListView"

                r_oCtrl.Parent.ForeColor = iColor
            Case "cboCombo"

                lblComboLabel(GetIndex(r_oCtrl, cboCombo)).ForeColor = iColor
            Case "txtText"

                lblTextLabel(GetIndex(r_oCtrl, txtText)).ForeColor = iColor
            Case "chkYesNo"

                lblCheckLabel(GetIndex(r_oCtrl, chkYesNo)).ForeColor = iColor

            Case "lblCheckLabel", "lblComboLabel", "lblTextLabel", "pnlPanel", "fraFrame"

                r_oCtrl.ForeColor = iColor
            Case Else
                'not a developed ctrl.
        End Select


    End Sub
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


    Private Function CheckMandatoryProperties() As Integer

        Dim result As Integer = 0
        Dim vDictionary(,) As Object
        Dim bFound As Boolean
        Dim dReferencedObjects As Collection
        Dim dReferencedObjectsItem As Object

        Try


            result = gPMConstants.PMEReturnCode.PMTrue



            vDictionary = g_vDataDictionary(m_lGISDMType)

            dReferencedObjects = New Collection()

            For iDict As Integer = 0 To g_vDataDictionary(GISDataModelType.GISDMTypeRisk).GetUpperBound(1)
                If m_vInUse(GISDataModelType.GISDMTypeRisk)(iDict) = gPMConstants.PMEReturnCode.PMTrue Then
                    Try
                        dReferencedObjectsItem = Nothing

                        dReferencedObjectsItem = dReferencedObjects("_" & CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, iDict)))

                        If Object.Equals(dReferencedObjectsItem, Nothing) Then

                            dReferencedObjects.Add(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, iDict), "_" & CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, iDict)))
                        End If

                    Catch ex As Exception

                    End Try

                End If
            Next iDict


            'we have a list of object which we have referenced properties
            'now check for the absence of any of the mandatory properties from these objects

            For dReferencedObjectsCount As Integer = 1 To dReferencedObjects.Count

                For iDict As Integer = vDictionary.GetLowerBound(1) To vDictionary.GetUpperBound(1)



                    If vDictionary(ACOGISObjectId, iDict).Equals(dReferencedObjects(dReferencedObjectsCount)) And Conversion.Val(CStr(vDictionary(ACPEditFlags, iDict))) And GISSharedPropertyConstants.GISDSEditMandatory Then
                        bFound = False
                        For iCtrls As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)


                            If vDictionary(ACPGISPropertyId, iDict).Equals(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, iCtrls)) And vDictionary(ACPGISObjectId, iDict).Equals(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, iCtrls)) Then
                                bFound = True
                                Exit For
                            End If
                        Next iCtrls


                        If Not bFound And CStr(vDictionary(ACPPropertyName, iDict)) <> "" Then

                            'Developer Guide No.248
                            MessageBox.Show("'" & CStr(vDictionary(ACOObjectName, iDict)) & "." + vDictionary(ACPPropertyName, iDict) & "' is a mandatory property." & _
                                            Strings.Chr(13) & Strings.Chr(10) & "Add this property to the screen.", "Screen Designer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                Next iDict
            Next

            dReferencedObjects = Nothing

        Catch ex As Exception
            dReferencedObjects = Nothing
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatoryProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End Try

        Return result
    End Function


    Private Function ResetArrays() As Integer
        m_lCheckIndex = -1
        m_lComboIndex = -1
        m_lCommandIndex = -1
        m_lTextIndex = -1
        m_lListViewIndex = -1
        m_lStandardWordingIndex = -1
        m_lSumInsuredIndex = -1
        m_lAddressIndex = -1
        m_lFrameIndex = -1
        m_lDateIndex = -1


    End Function

    Private Function HideControls() As Object
        chkYesNo(0).Visible = False
        lblCheckLabel(0).Visible = False
        'Akash : 
        'cboCombo(0).Visible = False
        lblComboLabel(0).Visible = False
        cmdCommand(0).Visible = False
        pnlPanel(0).Visible = False
        txtText(0).Visible = False
        ' TBD txtDate(0).Visible = False
        lblTextLabel(0).Visible = False
        lvwListView(0).Visible = False
        cmdListViewAdd(0).Visible = False
        cmdListViewDelete(0).Visible = False
        cmdListViewEdit(0).Visible = False
        uctSumInsured1(0).Visible = False
        uctStandardWording1(0).Visible = False
        uctAddress1(0).Visible = False
        fraFrame(0).Visible = False
        PBFindControl(0).Visible = False
        uctClaimReserve1.Visible = False
        uctClaimPayment1.Visible = False
        uctCLMCaseClaim1.Visible = False
        uctCLMCaseHeader1.Visible = False
    End Function


    Private Function BuildDetail(ByRef r_lCount As Integer, ByRef r_vFrameArray() As Object, ByRef r_vControlArray(,) As Object, ByRef r_Controls As Object, ByRef r_lablels As Object, Optional ByRef v_lTag As Integer = 0) As Object
        Dim lTag, lColPosition As Integer

        If Information.IsArray(r_vControlArray) Then
            For lTemp As Integer = r_vControlArray.GetLowerBound(1) To r_vControlArray.GetUpperBound(1)
                If r_vControlArray(PBRiskScreenCommon.ACCIsDeleted, lTemp) = gPMConstants.PMEReturnCode.PMFalse Then
                    r_lCount += 1
                    If r_lCount = 0 Then
                        ReDim m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, r_lCount)
                    Else
                        ReDim Preserve m_vScreenDetails(PBDatabaseConsts.ACDLastArrayPosition, r_lCount)
                    End If

                    lTag = r_Controls(lTemp).Tag
                    'clear array
                    For lLoopCount As Integer = PBDatabaseConsts.ACDGISScreenId To PBDatabaseConsts.ACDLastArrayPosition

                        m_vScreenDetails(lLoopCount, r_lCount) = DBNull.Value
                    Next
                    'set other values
                    m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, r_lCount) = m_lScreenId
                    m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, r_lCount) = r_lCount

                    If lTag >= 0 Then 'check if attribute bound control

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, r_lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISObjectId, lTag Mod 10000)

                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, r_lCount) = g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, lTag Mod 10000)
                    Else

                        m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, r_lCount) = DBNull.Value
                        m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, r_lCount) = lTag
                    End If

                    'multi line hyperlink processing
                    If lTag = PBRiskScreenCommon.ndcHyperlink Then
                        ' note that txtText holds the target address - but only when different to the caption
                        If txtText(lTemp).Text <> "" Then

                            'Developer Guide No. 65
                            m_vScreenDetails(PBDatabaseConsts.ACDCaption, r_lCount) = r_lablels(lTemp).Text & "|" & txtText(lTemp).Text
                        Else

                            'Developer Guide No. 65
                            m_vScreenDetails(PBDatabaseConsts.ACDCaption, r_lCount) = r_lablels(lTemp).Text
                        End If
                    Else
                        m_vScreenDetails(PBDatabaseConsts.ACDCaption, r_lCount) = r_lablels(lTemp).Text
                    End If

                    m_vScreenDetails(PBDatabaseConsts.ACDTop, r_lCount) = (r_lablels(lTemp).Top)
                    m_vScreenDetails(PBDatabaseConsts.ACDLeft, r_lCount) = (r_lablels(lTemp).Left)
                    m_vScreenDetails(PBDatabaseConsts.ACDHeight, r_lCount) = (r_Controls(lTemp).Height)
                    If r_Controls(0).GetType().Name = "Button" Then
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, r_lCount) = ((pnlPanel(lTemp).Width))
                    Else
                        m_vScreenDetails(PBDatabaseConsts.ACDWidth, r_lCount) = (r_Controls(lTemp).Width)
                    End If

                    If CDbl(r_vControlArray(PBRiskScreenCommon.ACCIncludeInList, lTemp)) <> 0 Then
                        m_vScreenDetails(PBDatabaseConsts.ACDColumnWidth, r_lCount) = 1440
                    End If
                    lColPosition += 1

                    m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCPreQuote, lTemp)

                    m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCPostQuote, lTemp)

                    m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCPurchase, lTemp)


                    m_vScreenDetails(PBDatabaseConsts.ACDParentId, r_lCount) = r_vFrameArray(CInt(r_vControlArray(PBRiskScreenCommon.ACCFrameNumber, lTemp)))

                    m_vScreenDetails(PBDatabaseConsts.ACDHelpText, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCHelpText, lTemp)

                    m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCPMFormat, lTemp)
                    m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, r_lCount) = r_vControlArray(ACCColumnPosition, lTemp)
                    m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, r_lCount) = r_vControlArray(PBRiskScreenCommon.ACCTabSetIndex, lTemp)
                    m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, r_lCount) = lTag \ 10000
                End If
            Next lTemp
        End If
    End Function
    Private Function AddControlToFindControl(ByRef r_iArrayIndex As Integer, ByVal v_iFrameIndex As Integer, ByRef r_vScreenControlArray(,) As Object, ByRef r_vControlArray(,) As Object, ByRef r_cntControl As Object, ByRef r_cntLablels As Object, ByVal v_iType As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim bProcess As Boolean

        Try

            If Information.IsArray(r_vControlArray) Then
                If Not Information.IsArray(r_vScreenControlArray) Then
                    ReDim r_vScreenControlArray(7, 0)
                End If
                For i As Integer = r_vControlArray.GetLowerBound(1) To r_vControlArray.GetUpperBound(1)
                    bProcess = False



                    If CDbl(r_vControlArray(PBRiskScreenCommon.ACCFrameNumber, i)) = v_iFrameIndex And CDbl(r_vControlArray(PBRiskScreenCommon.ACCIsDeleted, i)) = 0 And r_cntControl(i).Tag >= 0 Then


                        If v_iType = 1 Then
                            bProcess = True
                            'Developer Guide No. 65
                        ElseIf v_iType = 2 And CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPSpecialsTypeReference, r_cntControl(i).Tag Mod 10000)) <> "" Then
                            bProcess = True
                        End If
                        If bProcess Then
                            r_iArrayIndex += 1
                            'resize array
                            ReDim Preserve r_vScreenControlArray(7, r_iArrayIndex)

                            'add item
                            'control index

                            r_vScreenControlArray(0, r_iArrayIndex) = i
                            'label

                            r_vScreenControlArray(1, r_iArrayIndex) = r_cntLablels(i).Text
                            r_vScreenControlArray(3, r_iArrayIndex) = v_iType
                            r_vScreenControlArray(4, r_iArrayIndex) = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOGISObjectId, r_cntControl(i).Tag Mod 10000))
                            r_vScreenControlArray(5, r_iArrayIndex) = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPGISPropertyId, r_cntControl(i).Tag Mod 10000))
                            r_vScreenControlArray(6, r_iArrayIndex) = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACOObjectName, r_cntControl(i).Tag Mod 10000))

                            r_vScreenControlArray(7, r_iArrayIndex) = CStr(g_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPPropertyName, r_cntControl(i).Tag Mod 10000))
                        End If
                    End If
                Next i
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddControlToFindControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddControlToFindControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    Private Sub VScroll1_Change(ByVal newScrollValue As Integer)
        'picScreen.Top = ((-1) * (newScrollValue / (VScroll1.Maximum - (VScroll1.LargeChange + 1))) * lVScrollMultiplier)
        picScreen.Top = (-1) * (newScrollValue / VScroll1.Maximum) * lVScrollMultiplier
    End Sub

    ' ***************************************************************** '
    ' Name: AddClaimCaseHeader
    '
    ' Description: Add claim case header control
    '
    ' History:
    ' ***************************************************************** '
    Private Function AddClaimCaseHeader(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            uctCLMCaseHeader1.Parent = fraFrame(iFrameIndex)

            uctCLMCaseHeader1.Top = VB6.TwipsToPixelsY(200)
            uctCLMCaseHeader1.Height = VB6.TwipsToPixelsY(1320)
            fraFrame(iFrameIndex).Height = uctCLMCaseHeader1.Height + VB6.TwipsToPixelsY(240)
            uctCLMCaseHeader1.Left = VB6.TwipsToPixelsX(100)
            uctCLMCaseHeader1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctCLMCaseHeader1.Visible = True


            uctCLMCaseHeader1.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctCLMCaseHeader1, fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimCaseHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimCaseHeader", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddClaimCaseHeader
    '
    ' Description: Add claim case header control
    '
    ' History:
    ' ***************************************************************** '
    Private Function AddCaseClaimList(ByRef iFrameIndex As Integer, ByRef vTag() As Object, ByRef lX As Single, ByRef lY As Single, ByRef sCaption As String, ByRef bAddFrame As Boolean, ByRef sObjectName As String, ByRef sPropertyName As String, Optional ByRef lScreenDetailIndex As Integer = -1, Optional ByRef vTabSetIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex:=iFrameIndex, vTag:=vTag, lX:=lX, lY:=lY, lAddIfLessThan:=10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            uctCLMCaseClaim1.Parent = fraFrame(iFrameIndex)

            uctCLMCaseClaim1.Top = VB6.TwipsToPixelsY(200)
            uctCLMCaseClaim1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(300)
            uctCLMCaseClaim1.Left = VB6.TwipsToPixelsX(100)
            uctCLMCaseClaim1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(180)
            uctCLMCaseClaim1.Visible = True


            uctCLMCaseClaim1.Tag = CStr(CDbl(vTag(0)) * 10000 + CDbl(vTag(1)))


            m_vInUse(GISDataModelType.GISDMTypeRisk)(vTag(1)) = gPMConstants.PMEReturnCode.PMTrue

            'add object and property name as tooltip
            SetToolTip(uctCLMCaseClaim1, fraFrame(iFrameIndex), sObjectName, sPropertyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCaseClaimList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCaseClaimList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
    Private Sub HScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles HScroll1.Scroll
        'Select Case eventArgs.Type
        ' Case ScrollEventType.EndScroll
        HScroll1_Change(eventArgs.NewValue)
        'End Select
    End Sub
    Private Sub VScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles VScroll1.Scroll
        ' Select Case eventArgs.Type
        ' Case ScrollEventType.EndScroll
        VScroll1_Change(eventArgs.NewValue)
        'End Select
    End Sub
    Private Sub frmSizer_DragDrop(ByVal eventSender As System.Object, ByVal e As DragEventArgs) Handles frmSizer.DragDrop
        Dim Source As New Control

        'tabstrip1_DragDrop(Source, x, y)
        TabStrip1_DragEnter(eventSender, e)
    End Sub



    Private Sub frmSizer_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles frmSizer.MouseHover

    End Sub

    Private Sub frmSizer_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles frmSizer.DragEnter
        TabStrip1_DragEnter(sender, e)
    End Sub

    Private Sub tvwDataDictionary_BeforeExpand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvwDataDictionary.BeforeExpand

        For Each tr As TreeNode In tvwDataDictionary.Nodes
            If tr.Nodes.Count > 0 Then
                For Each trTemp As TreeNode In tr.Nodes
                    'If trTemp Then

                    'End If
                Next

            End If
        Next
    End Sub
    'Alkesh
    Private Sub TabStrip1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStrip1.SelectedIndexChanged
        Static lCurrentTab As Integer
        If m_lSetTabOrderCount > 0 Then
            'If lCurrentTab <> ContainerHelper.GetControlIndex(TabStrip1.SelectedTab) Then
            If lCurrentTab <> TabStrip1.SelectedIndex Then
                TabStrip1.SelectTab(lCurrentTab)
            End If
            Exit Sub
        End If
        ' Alkesh changes
        If TabStrip1.SelectedTab Is Nothing Then
            Exit Sub
        End If
        lCurrentTab = TabGetCurrentIndex(TabStrip1) 'TabStrip1.SelectedIndex
        'lCurrentTab = ContainerHelper.GetControlIndex(TabStrip1.SelectedTab)

        'This is a workaround - putting a usercontrol in a frame on a    tab other than the
        'first sets the left property incorrectly - it thinks that the control is still on
        'the other tab and adds 75000 or something like that
        m_lReturn = True 'RejigControls

        'set the snap to grid menu option for this tab
        m_iSnapToGrid = 0
        If m_vTabArray.GetUpperBound(1) >= TabGetCurrentIndex(TabStrip1) Then

            m_iSnapToGrid = CInt(m_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, TabGetCurrentIndex(TabStrip1)))
            mnuSnapToGrid.Checked = m_iSnapToGrid
        End If
        mnuSnapToGrid.Checked = m_iSnapToGrid
        For lTemp As Integer = 0 To m_lFrameIndex
            fraFrame(lTemp).Visible = (CDbl(m_vFrameArray(PBRiskScreenCommon.ACFTabNumber, lTemp)) = TabGetCurrentIndex(TabStrip1)) And m_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, lTemp) <> gPMConstants.PMEReturnCode.PMTrue
        Next lTemp
    End Sub
    'Added by Sumeet-To provide GUI same as VB
    'start
    Private Sub tvwDataDictionary_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwDataDictionary.NodeMouseClick
        Dim Node As TreeNode = e.Node
        If Not IsNothing(Node) Then
            If Node.GetNodeCount(False) > 0 And Node.IsExpanded Then
                Node.SelectedImageKey = ACOpenFolder
            ElseIf Node.GetNodeCount(False) > 0 Then
                Node.SelectedImageKey = ACClosedFolder
            Else
                Node.SelectedImageIndex = -1
            End If
        End If
    End Sub
    'end

    Private Function ValidateCompiledDefaults()

        Dim Rules As Object = Nothing

        Try

            If Trim(CompiledDefaultRuleClassName) = "" Then
                MsgBox("Unable to resolve the assembly and class name for compiled default rules.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
                Exit Function
            End If
            CompiledRulesObject = GISDataModelCode + "." + Trim(ScreenCode) + Trim(CompiledDefaultRuleClassName)
            Try
                Rules = CreateLateBoundObject(CompiledRulesObject)
            Catch ex As Exception
                Rules = Nothing
            End Try

            If IsNothing(Rules) Then
                MsgBox("Unable to create rating class " + CompiledRulesObject + ". Please ensure the " + GISDataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                       "The format should be DATAMODELCODE." + CompiledRulesObject, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
            Else
                MsgBox("Default rules class " + CompiledRulesObject + " validated successfully.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
            End If
        Catch
            MsgBox("Unable to create rating class " + CompiledRulesObject + ". Please ensure the " + GISDataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                      "The format should be DATAMODELCODE." + CompiledRulesObject, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")

        Finally
            Rules = Nothing
        End Try


    End Function


    Private Function ValidateCompiledValidation()

        Dim Rules As Object = Nothing

        Try

            If Trim(CompiledValidationRuleClassName) = "" Then
                MsgBox("Unable to resolve the assembly and class name for compiled default rules.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
                Exit Function
            End If
            CompiledRulesObject = GISDataModelCode + "." + Trim(ScreenCode) + Trim(CompiledValidationRuleClassName)
            Try
                Rules = CreateLateBoundObject(CompiledRulesObject)
            Catch ex As Exception
                Rules = Nothing
            End Try

            If IsNothing(Rules) Then
                MsgBox("Unable to create rating class " + CompiledRulesObject + ". Please ensure the " + GISDataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                       "The format should be DATAMODELCODE." + CompiledValidationRuleClassName, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
            Else
                MsgBox("Validation rules class " + CompiledRulesObject + " validated successfully.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")
            End If
        Catch
            MsgBox("Unable to create rating class " + CompiledRulesObject + ". Please ensure the " + GISDataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                      "The format should be DATAMODELCODE." + CompiledValidationRuleClassName, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Compiled Rules")

        Finally
            Rules = Nothing
        End Try
    End Function

    Private Sub chkEnableCompileRule_CheckedChanged(sender As Object, e As EventArgs) Handles chkEnableCompileRule.CheckedChanged

        OnCompileRuleCheckChange()
    End Sub

    Private Sub OnCompileRuleCheckChange()
        lblScreenLayout.Visible = False
        If chkEnableCompileRule.Checked Then
            grpCompiledRuleAssembly.Visible = True
            UctCompiledRuleDefaults.Enabled = True
            UctCompiledRuleValidation.Enabled = True
            If UctCompiledRuleDefaults.Text.Trim().ToUpper().Contains(".RUL") Then
                UctCompiledRuleDefaults.Text = ""
                UctCompiledRuleValidation.Text = ""
            End If
            cmdDefaults.Enabled = False
            cmdValidation.Enabled = False
            UctCompiledRuleDefaults.Load()
            UctCompiledRuleDefaults.bEnterOnlyAssemblyName = False
            UctCompiledRuleValidation.Load()
            UctCompiledRuleValidation.bEnterOnlyAssemblyName = False
            IsCompileRuleEnabled = True
        Else
            grpCompiledRuleAssembly.Visible = False
            UctCompiledRuleDefaults.Enabled = False
            UctCompiledRuleValidation.Enabled = False
            UctCompiledRuleDefaults.Text = ""
            UctCompiledRuleValidation.Text = ""
            cmdDefaults.Enabled = True
            cmdValidation.Enabled = True
            IsCompileRuleEnabled = False
        End If
    End Sub

    ''' <summary>
    ''' Validate on Compiled rule assembly name on SAVE and OK
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckCompiledRuleValues() As Integer
        Dim sObjectName As String = ""
        Dim oRules As Object = Nothing
        Dim sRulePath As String = ""
        Dim sAssemblyName As String = ""
        Dim sSubKey As String = "GIS"
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If chkEnableCompileRule.Checked Then
            Try
                If Not UctCompiledRuleDefaults.bEnterOnlyAssemblyName Then
                    m_oCompiledRuleClassNameDefaults = UctCompiledRuleDefaults.Text
                    If Trim(m_oCompiledRuleClassNameDefaults) = "" Then
                        MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sObjectName = Trim(m_oCompiledRuleClassNameDefaults)

                    result = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                    If sRulePath <> "" Then
                        If Not sRulePath.EndsWith("\") Then
                            sRulePath = sRulePath & "\" & sObjectName
                        End If
                    End If

                    If sRulePath.Length > 255 Then
                        MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    Try
                        oRules = CreateLateBoundObject_CompiledRules(sObjectName)
                        If Not IsNothing(oRules) Then
                            MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK)
                        End If

                    Catch ex As Exception
                        oRules = Nothing
                    End Try

                    If Not String.IsNullOrEmpty(sObjectName) Then
                        sAssemblyName = sObjectName.Split(".")(0).Trim()
                    End If

                    If IsNothing(oRules) Then
                        MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                       "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                If Not UctCompiledRuleValidation.bEnterOnlyAssemblyName Then
                    m_oCompiledRuleClassNameValidation = UctCompiledRuleValidation.Text
                    If Trim(m_oCompiledRuleClassNameValidation) = "" Then
                        MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sObjectName = Trim(m_oCompiledRuleClassNameValidation)

                    result = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                    If sRulePath <> "" Then
                        If Not sRulePath.EndsWith("\") Then
                            sRulePath = sRulePath & "\" & sObjectName
                        End If
                    End If

                    If sRulePath.Length > 255 Then
                        MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    Try
                        oRules = CreateLateBoundObject_CompiledRules(sObjectName)
                        If Not IsNothing(oRules) Then
                            MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK)
                        End If

                    Catch ex As Exception
                        oRules = Nothing
                    End Try

                    If Not String.IsNullOrEmpty(sObjectName) Then
                        sAssemblyName = sObjectName.Split(".")(0).Trim()
                    End If

                    If IsNothing(oRules) Then
                        MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                       "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Catch ex As Exception
                If Not String.IsNullOrEmpty(sObjectName) Then
                    sAssemblyName = sObjectName.Split(".")(0).Trim()
                End If

                MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                  "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
            Finally
                If Not IsNothing(oRules) Then
                    oRules = Nothing
                End If
            End Try
        End If

        Return result
    End Function
    Private Sub frmInterface_KeyPress(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        For Each tp As TabPage In TabStrip1.TabPages
            If tp.Text Like "*&" & Chr(e.KeyCode) & "*" And e.Alt Then
                TabStrip1.SelectedTab = tp
                Exit For
            End If
        Next
    End Sub
End Class
