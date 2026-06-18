Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmRuleSet
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""

    ' Declare an instance of the general interface object.
    'Private m_oGeneral As Object    'iPMURiskTypeRuleSet.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' {* USER DEFINED CODE (Begin) *}
    'Private m_lRiskTypeRuleSetID As Long
    Private m_lRuleSetId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_dtRuleEffectiveDate As Date
    'Private m_vRiskTypeID As Variant
    Private m_sFileName As String = ""
    Private m_iLive As Integer
    ' BasisForNewFile
    Private m_sBasisForNewFile As String = ""
    Private VBRuleFileName As String
    Private CompiledRuleClassName As Object
    Private Const VBScriptRules As Integer = 1
    Private Const CompiledRules As Integer = 3
    Private m_lRuleTypeID As Integer
    Private m_lSelectedRuleType As Integer
    Private DataModelCode As String
    Private m_lRuleType As Integer

    Public Property BasisForNewFile() As String
        Get
            Return m_sBasisForNewFile
        End Get
        Set(ByVal Value As String)
            m_sBasisForNewFile = Value
        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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

    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    'User Defined Property (Start)
    Public Property RuleSetID() As Integer
        Get
            Return m_lRuleSetId
        End Get
        Set(ByVal Value As Integer)
            m_lRuleSetId = Value
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
            Return m_sDescription
        End Get
        Set(ByVal Value As Object)

            m_sDescription = CStr(Value)
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


    Public Property FileName() As String
        Get
            Return m_sFileName
        End Get
        Set(ByVal Value As String)
            m_sFileName = Value
        End Set
    End Property

    Public Property Live() As Integer
        Get
            Return m_iLive
        End Get
        Set(ByVal Value As Integer)
            m_iLive = Value
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


    'User Defined Property (End)

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

        Dim nResult As Integer = 0
        Dim lCount As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If BusinessToData() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lRuleSetId = 0 Then
                Return nResult
            End If

            '    If BusinessToData() <> PMTrue Then
            '        BusinessToInterface = PMFalse
            '        Exit Function
            '    End If

            ' Update the interface details.

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Developer Guide No. 26
            'lblRuleFile.Text = m_sFileName

            chkLive.CheckState = m_iLive

            ' {* USER DEFINED CODE (End) *}

            m_lSelectedRuleType = m_lRuleType

            Select Case m_lSelectedRuleType
                Case VBScriptRules
                    VBRuleFileName = lblRuleFile.Text
                Case CompiledRules
                    CompiledRuleClassName = UctCompiledRule1.Text
            End Select

            Select Case m_lSelectedRuleType
                Case VBScriptRules
                    cboRuleType.SelectedIndex = 0
                    lblRuleFile.Text = m_sFileName
                    UctCompiledRule1.Visible = False
                    lblRuleFile.Visible = True
                    pnlRuleFile.Visible = True
                Case CompiledRules
                    cboRuleType.SelectedIndex = 1
                    UctCompiledRule1.Text = m_sFileName
                    lblRuleFile.Visible = False
                    pnlRuleFile.Visible = False
                    UctCompiledRule1.Visible = True
            End Select

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.

            m_lReturn = g_oBusiness.UpdateRuleSet(v_iTask:=m_iTask, r_lRuleSetID:=m_lRuleSetId, v_sCode:=m_sCode, v_sDescription:=m_sDescription, v_dtEffectiveDate:=m_dtEffectiveDate, v_sFileName:=m_sFileName, v_iLive:=m_iLive, v_lRiskTypeRuleSetTypeID:=m_lRuleTypeID)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Resume

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



            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))

            m_sDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))

            m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            'Developer Guide No. 26
            Select Case m_lSelectedRuleType
                'Case PRERules, CompiledRules -- Parth
                Case VBScriptRules
                    m_sFileName = lblRuleFile.Text
                Case CompiledRules
                    m_sFileName = UctCompiledRule1.Text
            End Select
            m_iLive = chkLive.CheckState
            m_lRuleTypeID = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)
            ' {* USER DEFINED CODE (End) *}

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
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            result = PopulateRuleTypes()

            'default values when adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lRuleSetId = 0
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=DateTime.Today)

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


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRuleFile.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblRuleFile, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkLive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblLive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRMaintainAURule.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

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


    Private Sub frmRuleSet_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        UctCompiledRule1.Load()
        UctCompiledRule1.bEnterOnlyAssemblyName = False
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



            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

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
            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

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




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmRuleSet_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                ' m_lReturn = m_oGeneral.ProcessCommand()

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    'Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            '    ' Terminate the general object.
            '    m_lReturn& = m_oGeneral.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '    End If
            '
            '    ' Destroy the instance of the general object
            '    ' from memory.
            '    Set m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
		m_oFormFields.Dispose()

            ' Check for errors.

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

    Private Sub frmRuleSet_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch

            Exit Sub
        End Try


    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click event of the OK button.
        Dim sObjectName As String = ""
        Dim sAssemblyName As String = ""
        Dim sRulePath As String = ""
        Dim sSubKey As String = "GIS"

        Select Case m_lSelectedRuleType

            Case CompiledRules
                Dim oRules As Object = Nothing
                Try
                    If Not UctCompiledRule1.bEnterOnlyAssemblyName Then
                        CompiledRuleClassName = UctCompiledRule1.Text
                        If Trim(CompiledRuleClassName) = "" Then
                            MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK)
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

                        If Not String.IsNullOrEmpty(sObjectName) Then
                            sAssemblyName = sObjectName.Split(".")(0).Trim()
                        End If

                        If IsNothing(oRules) Then
                            MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                          "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
                            Exit Sub
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

            m_lReturn = CType(InterfaceToBusiness(), gPMConstants.PMEReturnCode)

            '    ' Process the next set of actions depending
            '    ' upon the interface task etc.
            '    m_lReturn& = m_oGeneral.ProcessCommand()
            '
            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        Try
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Log Error.
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

    ' ***************************************************************** '
    '
    ' Name: GenerateFileName
    '
    ' Description: Generates unique filename for new rule.
    '
    ' History: 08/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateFileName(ByRef r_sGeneratedFileName As String) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFileName = "UA" & DateTime.Now.ToString("yyMMddHHMMss") & ".rul"

            r_sGeneratedFileName = sFileName

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateFileName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_sRulePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sSubKey As String = ""

        sSubKey = "GIS\" & g_sDataModelCode

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        If r_sRulePath = "" Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        Else
            If Not r_sRulePath.EndsWith("\") Then
                r_sRulePath = r_sRulePath & "\"
            End If
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' binds rule type ddl
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateRuleTypes() As Integer

        Dim oResultArray(,) As Object = Nothing
        Dim nCount As Integer = 0
        Dim nResult As Integer = PMEReturnCode.PMTrue

        nResult = m_oBusiness.GetRuleTypes(r_oResultArray:=oResultArray)

        Dim nNewIndex As Integer
        If IsArray(oResultArray) Then
            For nCount = 0 To UBound(oResultArray, 2)
                nNewIndex = cboRuleType.Items.Add(oResultArray(0, nCount))
                VB6.SetItemData(cboRuleType, nNewIndex, CInt(oResultArray(1, nCount)))
            Next
            cboRuleType.SelectedIndex = 0
            m_lSelectedRuleType = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)
        End If

        Return nResult

    End Function

    Private Sub cboRuleType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRuleType.SelectedIndexChanged

        m_lSelectedRuleType = VB6.GetItemData(cboRuleType, cboRuleType.SelectedIndex)

        Select Case m_lSelectedRuleType
            Case VBScriptRules
                UctCompiledRule1.Visible = False
                pnlRuleFile.Visible = True
                lblRuleFile.Visible = True
                lblFile.Visible = True
                lblFile.Text = "Rule File:"
                cmdRuleFile.Visible = True
            Case CompiledRules
                lblRuleFile.Visible = False
                pnlRuleFile.Visible = False
                cmdRuleFile.Visible = False
                UctCompiledRule1.Visible = True
                lblFile.Visible = True
                UctCompiledRule1.BringToFront()
                UctCompiledRule1.Load()
                UctCompiledRule1.bEnterOnlyAssemblyName = False
                lblFile.Text = "Compiled Rule Assembly:"
        End Select

    End Sub

    Private Sub cmdRuleFile_Click(sender As Object, e As EventArgs) Handles cmdRuleFile.Click
        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed
        Dim sRuleFileName, sTitle, sMessage, sRulePath As String
        Dim sObjectName As String = ""
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
                    '    Set oRuleEditor = CreateObject("iPMURuleEditor.Interface")

                    ' Get an instance of the general interface object object via
                    ' the public object manager.
                    Dim temp_oRuleEditor As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="iPMURuleEditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oRuleEditor = temp_oRuleEditor

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get an instance of the interface object.
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                        ' Display error stating the problem.

                        ' Get description from the resource file.

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Exit Sub
                    End If


                    'Developer Guide No. 26
                    If m_lSelectedRuleType = 1 Then
                        sRuleFileName = lblRuleFile.Text.Trim()
                    End If


                    CType(oRuleEditor, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                    'If we don't already have the file name then generate a new one.
                    If sRuleFileName = "" Then
                        m_lReturn = CType(GenerateFileName(sRuleFileName), gPMConstants.PMEReturnCode)

                        'If the user has elected to use an existing file as a basis for the
                        'new one, then copy the existing to New file name.
                        If m_sBasisForNewFile <> "" Then
                            'default data model
                            g_sDataModelCode = "RSA"
                            m_lReturn = CType(GetRulePath(sRulePath), gPMConstants.PMEReturnCode)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                File.Copy(sRulePath & m_sBasisForNewFile, sRulePath & sRuleFileName)


                                m_lReturn = oRuleEditor.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                            End If
                        Else
                            'RWH(08/01/2001) We must be adding new file so set Task.

                            m_lReturn = oRuleEditor.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
                        End If


                    End If


                    oRuleEditor.RuleFileName = sRuleFileName
                    oRuleEditor.Start()

                    lblRuleFile.Text = oRuleEditor.RuleFileName


                    oRuleEditor.Dispose()
                    oRuleEditor = Nothing

                Catch excep As System.Exception



                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load rule editor object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRuleFile_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Exit Sub
                End Try
        End Select

    End Sub

    ''' <summary>
    ''' Updates the data storage from business
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BusinessToData() As Integer

        Dim nResult As Integer = 0
        Dim oResultArray(,) As Object = Nothing

        nResult = m_oBusiness.GetRuleType(v_nRuleType:=m_lRuleSetId, r_oResultArray:=oResultArray)

        If Information.IsArray(oResultArray) Then
            m_sFileName = oResultArray(7, 0)
            m_lRuleType = oResultArray(10, 0)
        End If

        Return nResult
    End Function
End Class
