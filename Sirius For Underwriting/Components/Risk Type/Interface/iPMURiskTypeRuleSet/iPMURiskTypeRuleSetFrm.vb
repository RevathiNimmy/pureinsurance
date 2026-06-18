Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports iPMURuleEditor

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'consts to match table risk_type_rule_set_type
    Private Const VBScriptRules As Integer = 1
    Private Const PRERules As Integer = 2
    Private Const CompiledRules As Integer = 3

    Private Const ksPreVersion1 As String = "DREORPRE1"
    Private Const ksPreVersion2 As String = "PRE2"
    Private Const ksPRECoverEffectiveDate As String = "CoverDate"
    Private Const ksPRETransactionEffectiveDate As String = "TransactionDate"
    Private Const ksPREInceptionEffectiveDate As String = "InceptionTPI"
    Private Const ksPRETMPInceptionEffectiveDate As String = "InceptionTPI(Monthly)"

    Public Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskTypeRuleSet.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lRiskTypeRuleSetID As Integer
    Private m_sCode As String = ""

    Private m_vDescription As Object
    Private m_dtRuleEffectiveDate As Date

    Private m_vRiskTypeID As Object

    Private m_vFileName As Object
    Private VBRuleFileName As String
    Private CompiledRuleClassName As Object
    Private PRECompiledRuleClassName As Object
    Private m_vLive As Object
    Private m_lRuleTypeID As Integer
    Private m_sRuleTypeDesc As String
    Private m_sDREExecutorURL As String
    Private m_sDREDefaultToken As String
    Private m_lSelectedRuleType As Integer


    Private m_sRuleType As String = ""
    Private m_bDREDefault As Boolean
    Private m_bDREQuote As Boolean
    Private m_bDREValidate As Boolean
    Private m_bPostPRE As Boolean
    Private m_bPrePRE As Boolean
    Private DataModelCode As String
    Private m_nRuleSetTypeID As Integer
    Private m_sPREVersion As String
    Private m_sPRERulesetEffDate As String
    Private m_bUseChildRuleSetEffDate As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

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

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

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

    'User Defined Property (Start)
    Public Property RiskTypeRuleSetID() As Integer
        Get
            Return m_lRiskTypeRuleSetID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeRuleSetID = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property

    Public Property Description() As Object
        Get
            Return m_vDescription
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            m_vDescription = Value
        End Set
    End Property

    Public Property RuleEffectiveDate() As Date
        Get
            Return m_dtRuleEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtRuleEffectiveDate = Value
        End Set
    End Property

    Public WriteOnly Property RiskTypeID() As Object
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)
            m_vRiskTypeID = Value
        End Set
    End Property

    Public Property FileName() As Object
        Get
            Return m_vFileName
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'm_vFileName = CStr(Value)
            m_vFileName = Value
        End Set
    End Property

    Public Property Live() As Object
        Get
            Return m_vLive
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'm_vLive = CStr(Value)
            m_vLive = Value
        End Set
    End Property

    Public Property RuleType() As String
        Get
            Return m_sRuleType
        End Get
        Set(ByVal Value As String)
            m_sRuleType = Value
        End Set
    End Property

    Public Property RuleTypeID() As Integer
        Get
            Return m_lRuleTypeID
        End Get
        Set(ByVal value As Integer)
            m_lRuleTypeID = value
        End Set
    End Property

    Public Property RuleTypeDescription() As String
        Get
            Return m_sRuleTypeDesc
        End Get
        Set(ByVal value As String)
            m_sRuleTypeDesc = value
        End Set
    End Property

    Public Property DREExecutorURL() As String
        Get
            Return m_sDREExecutorURL
        End Get
        Set(ByVal value As String)
            m_sDREExecutorURL = value
        End Set
    End Property

    Public Property DREDefaultToken() As String
        Get
            Return m_sDREDefaultToken
        End Get
        Set(ByVal value As String)
            m_sDREDefaultToken = value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal value As String)
            m_sUniqueId = value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal value As String)
            m_sScreenHierarchy = value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

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
            '        If (m_lReturn <> PMTrue) Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BusinessToData() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the interface details.
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_vDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtRuleEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            txtRuleFileText.Text = m_vFileName

            chkLive.CheckState = CInt(m_vLive)

            For lCount = 0 To cboRuleType.Items.Count - 1
                If RuleTypeID = VB6.GetItemData(cboRuleType, lCount) Then
                    cboRuleType.SelectedIndex = lCount
                    m_lSelectedRuleType = RuleTypeID
                    Exit For
                End If
            Next

            Select Case m_lSelectedRuleType
                Case VBScriptRules
                    VBRuleFileName = txtRuleFileText.Text
                Case CompiledRules
                    CompiledRuleClassName = UctCompiledRule1.Text
                    chkDREDefault.CheckState = CheckState.Unchecked
                    chkDREQuote.CheckState = CheckState.Unchecked
                    chkDREValidate.CheckState = CheckState.Unchecked
                Case PRERules
                    txtDREExecutorURL.Text = m_sDREExecutorURL
                    txtDREDefaultToken.Text = m_sDREDefaultToken
                    chkDREDefault.CheckState = IIf(m_bDREDefault, 1, 0)
                    chkDREQuote.CheckState = IIf(m_bDREQuote, 1, 0)
                    chkDREValidate.CheckState = IIf(m_bDREValidate, 1, 0)
                    chkPostPRE.CheckState = IIf(m_bPostPRE, 1, 0)
                    chkPrePRE.CheckState = IIf(m_bPrePRE, 1, 0)
                    UctCompiledRulePRE.Text = m_vFileName
                    PRECompiledRuleClassName = m_vFileName

                    If m_sPREVersion = ksPreVersion1 Then
                        cboPREVersion.SelectedItem = "DRE, PRE"
                    ElseIf m_sPREVersion = ksPreVersion2 Then
                        cboPREVersion.SelectedItem = "PRE2"
                    End If

                    If m_sPRERulesetEffDate = ksPRETransactionEffectiveDate Then
                        cboRuleEffectiveDate.SelectedItem = "Transaction Date"
                    ElseIf m_sPRERulesetEffDate = ksPRECoverEffectiveDate Then
                        cboRuleEffectiveDate.SelectedItem = "Cover Effective Date"
                    ElseIf m_sPRERulesetEffDate = ksPREInceptionEffectiveDate Then
                        cboRuleEffectiveDate.SelectedItem = "Inception Date TPI"
                    ElseIf m_sPRERulesetEffDate = ksPRETMPInceptionEffectiveDate Then
                        cboRuleEffectiveDate.SelectedItem = "Inception Date TPI(Monthly)"
                    End If
                    chkChildRuleEffectiveDate.CheckState = IIf(m_bUseChildRuleSetEffDate = True, 1, 0)
            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = m_sScreenHierarchy & $"/Code({m_sCode.Trim()})"
            ' Update the business object.
            m_lReturn = m_oBusiness.UpdateRiskTypeRuleSet(v_iTask:=m_iTask, r_lRiskTypeRuleSetID:=m_lRiskTypeRuleSetID, v_sCode:=m_sCode,
                                                          v_vDescription:=m_vDescription, v_dtEffectiveDate:=m_dtRuleEffectiveDate, v_vRiskTypeID:=m_vRiskTypeID,
                                                          v_vFileName:=m_vFileName, v_vLive:=m_vLive, v_vType:=m_sRuleType,
                                                          v_lRiskTypeRuleSetTypeID:=m_lRuleTypeID, v_sDREExecutorURL:=m_sDREExecutorURL,
                                                          v_sDREDefaultToken:=m_sDREDefaultToken, v_bDREDefault:=m_bDREDefault, v_bDREQuote:=m_bDREQuote,
                                                          v_bDREValidate:=m_bDREValidate, v_bPostDREVB:=m_bPostPRE, v_bPrePRE:=m_bPrePRE,
                                                         v_lPREVersion:=m_sPREVersion, v_lPRERulesetEffectiveDate:=m_sPRERulesetEffDate, v_bUseChildRuleSetEffDate:=m_bUseChildRuleSetEffDate,
                                                         v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from business
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = m_oBusiness.GetRiskTypeRuleSet(v_lRiskTypeRuleSetID:=m_lRiskTypeRuleSetID, v_vRiskTypeID:=m_vRiskTypeID, r_vResultArray:=vResultArray)

            If Information.IsArray(vResultArray) Then

                m_lRiskTypeRuleSetID = CInt(vResultArray(ACFieldPosRiskTypeRuleSetID, 0))
                m_sCode = CStr(vResultArray(ACFieldPosCode, 0))
                m_vDescription = CStr(vResultArray(ACFieldPosDescription, 0))
                m_dtRuleEffectiveDate = CDate(vResultArray(ACFieldPosEffectiveDate, 0))
                m_vRiskTypeID = CStr(vResultArray(ACFieldPosRiskTypeID, 0))
                m_vFileName = CStr(vResultArray(ACFieldPosFileName, 0))
                m_vLive = CStr(vResultArray(ACFieldPosLive, 0))
                m_sDREExecutorURL = CStr(vResultArray(ACFieldPosDREExecutorURL, 0))
                m_sDREDefaultToken = CStr(vResultArray(ACFieldPosDREDefaultToken, 0))
                m_bDREDefault = IIf(CInt(vResultArray(ACFieldPosDREDefault, 0)) = 1, True, False)
                m_bDREQuote = IIf(CInt(vResultArray(ACFieldPosDREQuote, 0)) = 1, True, False)
                m_bDREValidate = IIf(CInt(vResultArray(ACFieldPosDREValidate, 0)) = 1, True, False)
                m_bPostPRE = IIf(CInt(vResultArray(ACFieldPosPostPRE, 0)) = 1, True, False)
                m_bPrePRE = IIf(CInt(vResultArray(ACFieldPosPrePRE, 0)) = 1, True, False)
                DataModelCode = ToSafeString(vResultArray(ACFieldPosDataModelCode, 0))
                m_nRuleSetTypeID = CInt(vResultArray(ACFieldPosRuleSetTypeID, 0))
                m_sPREVersion = CStr(vResultArray(ACFieldPREVersion, 0))
                m_sPRERulesetEffDate = CStr(vResultArray(ACFieldPRERulesetEffDate, 0))
                m_bUseChildRuleSetEffDate = IIf(CInt(vResultArray(ACFieldPREChildRulesetEffDate, 0)) = 1, True, False)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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

            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))
            m_vDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))
            m_dtRuleEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            Select Case m_lSelectedRuleType
                Case PRERules
                    m_vFileName = UctCompiledRulePRE.Text
                Case CompiledRules
                    m_vFileName = UctCompiledRule1.Text
                Case Else
                    m_vFileName = txtRuleFileText.Text
            End Select

            m_vLive = CStr(chkLive.CheckState)
            m_lRuleTypeID = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)
            m_sRuleTypeDesc = cboRuleType.Text

            Select Case m_lSelectedRuleType
                Case PRERules
                    m_bDREDefault = chkDREDefault.CheckState
                    m_bDREQuote = chkDREQuote.CheckState
                    m_bDREValidate = chkDREValidate.CheckState
                    m_bPostPRE = chkPostPRE.CheckState
                    m_bPrePRE = chkPrePRE.CheckState
                    m_sDREExecutorURL = txtDREExecutorURL.Text
                    m_sDREDefaultToken = txtDREDefaultToken.Text

                    If cboPREVersion.SelectedItem.ToUpper() = "DRE, PRE" Then
                        m_sPREVersion = ksPreVersion1
                    ElseIf cboPREVersion.SelectedItem.ToUpper() = "PRE2" Then
                        m_sPREVersion = ksPreVersion2
                    End If

                    If cboRuleEffectiveDate.SelectedItem.ToUpper() = "TRANSACTION DATE" Then
                        m_sPRERulesetEffDate = ksPRETransactionEffectiveDate
                    ElseIf cboRuleEffectiveDate.SelectedItem.ToUpper() = "COVER EFFECTIVE DATE" Then
                        m_sPRERulesetEffDate = ksPRECoverEffectiveDate
                    ElseIf cboRuleEffectiveDate.SelectedItem.ToUpper() = "INCEPTION DATE TPI" Then
                        m_sPRERulesetEffDate = ksPREInceptionEffectiveDate
                    ElseIf cboRuleEffectiveDate.SelectedItem.ToUpper() = "INCEPTION DATE TPI(MONTHLY)" Then
                        m_sPRERulesetEffDate = ksPRETMPInceptionEffectiveDate
                    End If
                    m_bUseChildRuleSetEffDate = chkChildRuleEffectiveDate.CheckState
                Case VBScriptRules, CompiledRules
                    m_bDREDefault = chkDREDefault.CheckState
                    m_bDREQuote = chkDREQuote.CheckState
                    m_bDREValidate = chkDREValidate.CheckState
                    m_bPostPRE = chkPostPRE.CheckState
                    m_bPrePRE = chkPrePRE.CheckState

            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = PopulateRuleTypes()

            'default values when adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lRiskTypeRuleSetID = 0
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=DateTime.Today)
                cboRuleType_Click(Nothing, Nothing)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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

            m_ctlTabFirstLast(ACControlStart, 0) = txtCode
            m_ctlTabFirstLast(ACControlEnd, 0) = txtEffectiveDate

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


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRuleFile.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRuleFile, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRuleFile.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRuleFileText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkLive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0

        Try

            Select Case m_lSelectedRuleType
                Case VBScriptRules
                    If Trim(txtRuleFileText.Text) = "" Then
                        MsgBox("This is a mandatory field. You must enter data in this field", vbCritical + vbOKOnly, "Mandatory Field - Rule File")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case PRERules
                    If (cboPREVersion.SelectedItem = "") Then
                        MsgBox("This is a mandatory field. Please select valid value Version from List", vbCritical + vbOKOnly, "Mandatory Field - PRE Version")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Trim(txtDREExecutorURL.Text) = "" Then
                        MsgBox("This is a mandatory field. You must enter data in this field", vbCritical + vbOKOnly, "Mandatory Field - PRE Executor URL")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    ElseIf Trim(txtDREDefaultToken.Text) = "" Then
                        MsgBox("This is a mandatory field. You must enter data in this field", vbCritical + vbOKOnly, "Mandatory Field - PRE Profile Token")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If (cboRuleEffectiveDate.SelectedItem = "") Then
                        MsgBox("This is a mandatory field. Please select valid Date type from List", vbCritical + vbOKOnly, "Mandatory Field - PRE Ruleset Effective Date")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If UctCompiledRulePRE.Text = "" AndAlso (chkPrePRE.Checked Or chkPostPRE.Checked) Then
                        MsgBox("This is a mandatory field. You must enter data in this field", vbCritical + vbOKOnly, "Mandatory Field - PRE assembly name")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case CompiledRules
                    If Trim(UctCompiledRule1.Text) = "" Then
                        MsgBox("This is a mandatory field. You must enter data in this field", vbCritical + vbOKOnly, "Mandatory Field - Rating Class Name")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
            End Select

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdRuleFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRuleFile.Click

        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed
        Dim sRuleFileName As String = ""
        Dim sObjectName As String = ""
        Dim sRulePath As String = ""
        Dim sSubKey As String = "GIS"

        Select Case m_lSelectedRuleType

            Case CompiledRules
                Dim oRules As Object = Nothing
                Try
                    If Not UctCompiledRule1.bEnterOnlyAssemblyName Then
                        CompiledRuleClassName = UctCompiledRule1.Text
                        If Trim(CompiledRuleClassName) = "" Then
                            MessageBox.Show("Please enter the Rating class name.", "Compiled Rules", MessageBoxButtons.OK)
                            Exit Sub
                        End If
                        sObjectName = Trim(CompiledRuleClassName)

                        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                        If sRulePath <> "" Then
                            If Not sRulePath.EndsWith("\") Then
                                sRulePath = sRulePath & "\" & sObjectName
                            End If
                        End If

                        If sRulePath.Length > 255 Then
                            MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK)
                            Exit Sub
                        End If

                        Try
                            oRules = CreateLateBoundObject_CompiledRules(sObjectName)

                            If Not IsNothing(oRules) Then
                                MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK)
                            End If

                        Catch ex As Exception
                            oRules = Nothing
                        End Try

                        If IsNothing(oRules) Then
                            MessageBox.Show("Unable to create rating class " + sObjectName + ". Please ensure the " + DataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                                   "The format should be DATAMODELCODE." + CompiledRuleClassName, "Compiled Rules", MessageBoxButtons.OK)
                        End If
                    End If

                Catch
                    MessageBox.Show("Unable to create rating class " + sObjectName + ". Please ensure the " + DataModelCode + " assembly is in the Pure application and Web Services BIN folders and that filenames are correct. " +
                              "The format should be DATAMODELCODE." + CompiledRuleClassName, "Compiled Rules", MessageBoxButtons.OK)

                Finally
                    If Not IsNothing(oRules) Then
                        oRules = Nothing
                    End If
                End Try

            Case VBScriptRules

                Try
                    If RuleType.Trim() = "" Then
                        m_sRuleType = "RT"
                    End If

                    oRuleEditor = New iPMURuleEditor.Interface_Renamed()
                    sRuleFileName = txtRuleFileText.Text.Trim()
                    CType(oRuleEditor, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                    'pass through filename if exists
                    If sRuleFileName <> "" Then
                        m_lReturn = oRuleEditor.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                        oRuleEditor.RuleFileName = sRuleFileName
                    Else
                        m_lReturn = oRuleEditor.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
                        oRuleEditor.RuleFileName = RuleType & DateTime.Now.ToString("yyyyMMddHHMMss") & ".Rul"
                    End If

                    oRuleEditor.Start()
                    txtRuleFileText.Text = oRuleEditor.RuleFileName
                    VBRuleFileName = txtRuleFileText.Text
                    oRuleEditor.Dispose()

                Catch ex As Exception
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the rule editor", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRuleFile_Click", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
                Finally
                    oRuleEditor = Nothing
                End Try

        End Select
        Exit Sub
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRRiskType.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

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

                Exit Sub
            End If


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURiskTypeRuleSet.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        UctCompiledRule1.Load()
        UctCompiledRule1.bEnterOnlyAssemblyName = False
        UctCompiledRulePRE.Load()
        UctCompiledRulePRE.bEnterOnlyAssemblyName = False

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

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
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.

                    eventArgs.Cancel = True
                    Cancel = 1

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

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()


            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
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
                End Select
            End With

        Catch




            Exit Sub
        End Try


    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim sObjectName As String = ""
        Dim sAssemblyName As String = ""
        Dim sFileExtension As String = ""
        Dim oRules As Object = Nothing
        Dim sRulePath As String = ""
        Dim sSubKey As String = "GIS"

        Select Case m_lSelectedRuleType

            Case CompiledRules
                Try
                    If Not UctCompiledRule1.bEnterOnlyAssemblyName Then
                        CompiledRuleClassName = UctCompiledRule1.Text
                        If Trim(CompiledRuleClassName) = "" Then
                            MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                        sObjectName = Trim(CompiledRuleClassName)

                        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                        If sRulePath <> "" Then
                            If Not sRulePath.EndsWith("\") Then
                                sRulePath = sRulePath & "\" & sObjectName
                            End If
                        End If

                        If sRulePath.Length > 255 Then
                            MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If

                        Try
                            oRules = CreateLateBoundObject_CompiledRules(sObjectName)

                            If Not IsNothing(oRules) Then
                                MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If

                        Catch ex As Exception
                            oRules = Nothing
                        End Try

                        If Not String.IsNullOrEmpty(sObjectName) Then
                            sAssemblyName = sObjectName.Split(".")(0).Trim()

                        End If

                        If IsNothing(oRules) Then
                            MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                           "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    End If
                Catch ex As Exception
                    If Not String.IsNullOrEmpty(sObjectName) Then
                        sAssemblyName = sObjectName.Split(".")(0).Trim()
                    End If

                    MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                       "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Finally
                    If Not IsNothing(oRules) Then
                        oRules = Nothing
                    End If
                End Try

            Case PRERules
                Try

                    If chkPrePRE.CheckState = CheckState.Checked Or chkPostPRE.CheckState = CheckState.Checked Then

                        If Not UctCompiledRulePRE.bEnterOnlyAssemblyName Then
                            PRECompiledRuleClassName = UctCompiledRulePRE.Text
                            If Trim(PRECompiledRuleClassName) = "" Then
                                MessageBox.Show("Please enter the PRE rule assembly and class name.", "PRE Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                            sObjectName = Trim(PRECompiledRuleClassName)

                            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                            If sRulePath <> "" Then
                                If Not sRulePath.EndsWith("\") Then
                                    sRulePath = sRulePath & "\" & sObjectName
                                End If
                            End If

                            If sRulePath.Length > 255 Then
                                MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "PRE Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If

                            Try
                                oRules = CreateLateBoundObject_CompiledRules(sObjectName)

                                If Not IsNothing(oRules) Then
                                    MessageBox.Show(sObjectName + " validated successfully.", "PRE Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If

                            Catch ex As Exception
                                oRules = Nothing
                            End Try

                            If Not String.IsNullOrEmpty(sObjectName) Then
                                sAssemblyName = sObjectName.Split(".")(0).Trim()
                                sFileExtension = sObjectName.Split(".")(1).Trim().ToUpper()
                            End If

                            If IsNothing(oRules) AndAlso sFileExtension <> "RUL" Then
                                MessageBox.Show("Unable to find PRE rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                               "The format should be assemblyname.classname.", "PRE Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                        End If
                    End If
                Catch ex As Exception
                    If Not String.IsNullOrEmpty(sObjectName) Then
                        sAssemblyName = sObjectName.Split(".")(0).Trim()
                    End If

                    MessageBox.Show("Unable to find PRE rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                       "The format should be assemblyname.classname.", "PRE Rules", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Finally
                    If Not IsNothing(oRules) Then
                        oRules = Nothing
                    End If
                End Try

        End Select

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

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

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub cboRuleType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRuleType.Click
        'If Trim(cboRuleType.Text) = ".Rul file script" Then
        '    lblRuleFile.Font = New Font(lblRuleFile.Font, FontStyle.Bold)
        '    cmdRuleFile.Enabled = True
        '    lblDREExecutorURL.Font = New Font(lblDREExecutorURL.Font, FontStyle.Regular)
        '    lblDREDefaultToken.Font = New Font(lblDREDefaultToken.Font, FontStyle.Regular)
        '    txtDREExecutorURL.Enabled = False
        '    txtDREDefaultToken.Enabled = False
        '    txtDREExecutorURL.Text = ""
        '    txtDREDefaultToken.Text = ""
        '    grpDRERules.Enabled = False
        '    'chkDREDefault.Value = vbUnchecked
        '    chkDREQuote.CheckState = CheckState.Unchecked
        '    'chkDREValidate.Value = vbUnchecked
        '    chkPostDREVB.CheckState = CheckState.Unchecked
        '    chkPostDREVB.Enabled = False
        '    txtRuleFileText.Enabled = False
        'ElseIf Trim(cboRuleType.Text) = "Precision Rating Engine" Then
        '    lblRuleFile.Font = New Font(lblRuleFile.Font, FontStyle.Regular)
        '    'lblRuleFileText.Text = ""
        '    cmdRuleFile.Enabled = False
        '    lblDREExecutorURL.Font = New Font(lblDREExecutorURL.Font, FontStyle.Bold)
        '    lblDREDefaultToken.Font = New Font(lblDREDefaultToken.Font, FontStyle.Bold)
        '    txtDREExecutorURL.Enabled = True
        '    txtDREDefaultToken.Enabled = True
        '    grpDRERules.Enabled = True
        '    chkPostDREVB.Enabled = True
        'End If
        'm_lSelectedRuleType = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)
    End Sub

    Private Sub cboRuleType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRuleType.SelectedIndexChanged

        m_lSelectedRuleType = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)

        Select Case m_lSelectedRuleType
            Case VBScriptRules
                UctCompiledRule1.Visible = False
                txtRuleFileText.Visible = True
                cmdRuleFile.Visible = True
                lblRuleFile.Font = New Font(lblRuleFile.Font, FontStyle.Bold)
                cmdRuleFile.Enabled = True
                lblDREExecutorURL.Font = New Font(lblDREExecutorURL.Font, FontStyle.Regular)
                lblDREDefaultToken.Font = New Font(lblDREDefaultToken.Font, FontStyle.Regular)
                txtDREExecutorURL.Enabled = False
                txtDREDefaultToken.Enabled = False
                txtDREExecutorURL.Text = ""
                txtDREDefaultToken.Text = ""
                lblRuleFile.Text = "Rule File:"
                cmdRuleFile.Text = "..."
                grpDRERules.Enabled = False
                chkDREQuote.CheckState = CheckState.Unchecked
                chkDREDefault.CheckState = CheckState.Unchecked
                chkDREValidate.CheckState = CheckState.Unchecked
                txtRuleFileText.Enabled = False
                txtRuleFileText.BackColor = Color.FromKnownColor(KnownColor.Control)
                If VBRuleFileName = "" And m_vFileName = "" Then
                    txtRuleFileText.Text = ""
                ElseIf m_vFileName <> "" Then
                    txtRuleFileText.Text = m_vFileName
                Else
                    txtRuleFileText.Text = VBRuleFileName
                End If
                txtRuleFileText.Text = ""
                txtDREExecutorURL.Text = ""
                txtDREDefaultToken.Text = ""
                chkPostPRE.Enabled = False
                chkPrePRE.Enabled = False
                chkPostPRE.CheckState = CheckState.Unchecked
                chkPrePRE.CheckState = CheckState.Unchecked
                cmdRuleFile.Image = Nothing
                UctCompiledRulePRE.Text = ""
                chkChildRuleEffectiveDate.Enabled = False
                cboPREVersion.Enabled = False
                lblPREVersion.Font = New Font(lblPREVersion.Font, FontStyle.Regular)
                cboRuleEffectiveDate.Enabled = False
                lblRuleEffectiveDate.Font = New Font(lblRuleEffectiveDate.Font, FontStyle.Regular)
                cboPREVersion.ResetText()
                cboRuleEffectiveDate.ResetText()
                chkChildRuleEffectiveDate.Checked = False
            Case PRERules
                grpDRERules.Text = "Use PRE Rules"
                UctCompiledRule1.Visible = False
                txtRuleFileText.Visible = True
                cmdRuleFile.Visible = True
                lblRuleFile.Font = New Font(lblRuleFile.Font, FontStyle.Regular)
                lblRuleFile.Text = "Rule File:"
                cmdRuleFile.Text = "..."
                cmdRuleFile.Enabled = False
                lblDREExecutorURL.Font = New Font(lblDREExecutorURL.Font, FontStyle.Bold)
                lblDREDefaultToken.Font = New Font(lblDREDefaultToken.Font, FontStyle.Bold)
                txtDREExecutorURL.Enabled = True
                txtDREDefaultToken.Enabled = True
                grpDRERules.Enabled = True
                txtRuleFileText.Enabled = False
                txtRuleFileText.BackColor = Color.FromKnownColor(KnownColor.Control)
                chkPostPRE.Enabled = True
                chkPrePRE.Enabled = True
                UctCompiledRulePRE.Enabled = IIf((chkPrePRE.Checked OrElse chkPostPRE.Checked), True, False)
                chkDREDefault.CheckState = CheckState.Unchecked
                chkDREDefault.Enabled = False
                chkDREValidate.CheckState = CheckState.Unchecked
                chkDREValidate.Enabled = False
                cmdRuleFile.Image = Nothing
                txtDREExecutorURL.Text = m_sDREExecutorURL
                txtDREDefaultToken.Text = m_sDREDefaultToken
                chkDREQuote.CheckState = IIf(m_bDREQuote, 1, 0)
                txtRuleFileText.Text = ""
                chkDREQuote.CheckState = CheckState.Checked
                chkDREQuote.Enabled = False
                cboPREVersion.Enabled = True
                lblPREVersion.Font = New Font(lblPREVersion.Font, FontStyle.Bold)
                cboRuleEffectiveDate.Enabled = True
                lblRuleEffectiveDate.Font = New Font(lblRuleEffectiveDate.Font, FontStyle.Bold)
            Case CompiledRules
                UctCompiledRulePRE.Text = ""
                UctCompiledRule1.Visible = True
                txtRuleFileText.Visible = False
                cmdRuleFile.Visible = False
                lblRuleFile.Font = New Font(lblRuleFile.Font, FontStyle.Bold)
                lblDREExecutorURL.Font = New Font(lblDREExecutorURL.Font, FontStyle.Regular)
                lblDREDefaultToken.Font = New Font(lblDREDefaultToken.Font, FontStyle.Regular)
                txtDREExecutorURL.Enabled = False
                txtDREDefaultToken.Enabled = False
                lblRuleFile.Text = "Compiled Rule Assembly:"
                grpDRERules.Text = "Use Compiled Rules"
                grpDRERules.Enabled = False
                cmdRuleFile.Enabled = True
                cmdRuleFile.Text = ""
                chkPostPRE.CheckState = CheckState.Unchecked
                chkPostPRE.Enabled = False
                chkPrePRE.CheckState = CheckState.Unchecked
                chkPrePRE.Enabled = False
                cmdRuleFile.Image = ImageList1.Images(0)
                chkDREDefault.Enabled = False
                chkDREValidate.Enabled = False
                chkDREQuote.Enabled = False
                chkDREQuote.CheckState = CheckState.Unchecked
                chkDREDefault.CheckState = CheckState.Unchecked
                chkDREValidate.CheckState = CheckState.Unchecked
                txtDREExecutorURL.Text = ""
                txtDREDefaultToken.Text = ""
                txtRuleFileText.Enabled = True
                txtRuleFileText.BackColor = Color.White
                If CompiledRuleClassName = "" And m_lSelectedRuleType = CompiledRules And m_vFileName = "" Then
                    txtRuleFileText.Text = ""
                    UctCompiledRule1.Load()
                    UctCompiledRule1.bEnterOnlyAssemblyName = False
                ElseIf m_vFileName <> "" Then
                    If m_lSelectedRuleType = CompiledRules Then
                        txtRuleFileText.Text = m_vFileName
                        UctCompiledRule1.Text = m_vFileName
                    ElseIf m_lSelectedRuleType = PRERules Then
                        UctCompiledRulePRE.Text = m_vFileName
                    End If
                ElseIf PRECompiledRuleClassName = "" And m_lSelectedRuleType = PRERules And m_vFileName = "" Then
                    UctCompiledRulePRE.Load()
                    UctCompiledRulePRE.bEnterOnlyAssemblyName = False
                    UctCompiledRulePRE.Text = m_vFileName
                    If m_nRuleSetTypeID = 3 Then
                        UctCompiledRule1.Text = m_vFileName
                    End If
                End If
                chkChildRuleEffectiveDate.Enabled = False
                cboPREVersion.Enabled = False
                lblPREVersion.Font = New Font(lblPREVersion.Font, FontStyle.Regular)
                cboRuleEffectiveDate.Enabled = False
                lblRuleEffectiveDate.Font = New Font(lblRuleEffectiveDate.Font, FontStyle.Regular)
                cboPREVersion.ResetText()
                cboRuleEffectiveDate.ResetText()
                chkChildRuleEffectiveDate.Checked = False
        End Select

    End Sub

    ''' <summary>
    ''' populate rule type ddl
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateRuleTypes() As Integer

        Dim vResultArray(,) As Object = Nothing
        Dim nCount As Integer = 0
        Dim nResult As Integer = PMEReturnCode.PMTrue

        nResult = m_oBusiness.GetRuleTypes(r_oResultArray:=vResultArray)
            Dim lNewIndex As Integer
            If IsArray(vResultArray) Then
                For nCount = 0 To UBound(vResultArray, 2)
                    lNewIndex = cboRuleType.Items.Add(vResultArray(0, nCount))
                    VB6.SetItemData(cboRuleType, lNewIndex, CInt(vResultArray(1, nCount)))
                Next
                cboRuleType.SelectedIndex = 0
                m_lSelectedRuleType = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)
            End If

        Return nResult
    End Function

    Private Sub chkPostPRE_CheckStateChanged(sender As Object, e As EventArgs) Handles chkPostPRE.CheckStateChanged

        If (Not chkPrePRE.Checked) AndAlso (Not chkPostPRE.Checked) Then
            UctCompiledRulePRE.Text = ""
            UctCompiledRulePRE.Enabled = False
        Else
            UctCompiledRulePRE.Text = PRECompiledRuleClassName
            UctCompiledRulePRE.Enabled = True
        End If

    End Sub

    Private Sub chkPrePRE_CheckStateChanged(sender As Object, e As EventArgs) Handles chkPrePRE.CheckStateChanged
        If (Not chkPrePRE.Checked) AndAlso (Not chkPostPRE.Checked) Then
            UctCompiledRulePRE.Text = ""
            UctCompiledRulePRE.Enabled = False
        Else
            UctCompiledRulePRE.Text = PRECompiledRuleClassName
            UctCompiledRulePRE.Enabled = True
        End If
    End Sub

    Private Sub cboPREVersion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPREVersion.SelectedIndexChanged
        If cboPREVersion.SelectedItem.ToUpper() = "DRE, PRE" Then
            chkChildRuleEffectiveDate.Enabled = False
            chkChildRuleEffectiveDate.Checked = False
        ElseIf cboPREVersion.SelectedItem.ToUpper() = "PRE2" Then
            chkChildRuleEffectiveDate.Enabled = True
        End If
    End Sub
End Class
