Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURuleEditor.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sRuleFileName As String = ""
    Private m_sRulePath As String = ""
    Private m_bFixedFile As Boolean
    Private m_bRuleSaved As Boolean
    Private m_sDataModelCode As String = ""
    Private m_lDataModelId As Integer
    Private m_lSchemeID As Integer

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


    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
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
    Public Property RuleFileName() As String
        Get
            Return m_sRuleFileName
        End Get
        Set(ByVal Value As String)
            m_sRuleFileName = Value
        End Set
    End Property
    Public Property RulePath() As String
        Get
            Return m_sRulePath
        End Get
        Set(ByVal Value As String)
            m_sRulePath = Value
        End Set
    End Property
    Public Property FixedFile() As Boolean
        Get
            Return m_bFixedFile
        End Get
        Set(ByVal Value As Boolean)
            m_bFixedFile = Value
        End Set
    End Property
    Public Property DataModelCode() As String
        Get
            Return m_sDataModelCode
        End Get
        Set(ByVal Value As String)
            m_sDataModelCode = Value
        End Set
    End Property
    Public Property DataModelId() As Integer
        Get
            Return m_lDataModelId
        End Get
        Set(ByVal Value As Integer)
            m_lDataModelId = Value
        End Set
    End Property
    Public WriteOnly Property SchemeID() As Integer
        Set(ByVal Value As Integer)
            m_lSchemeID = Value
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

            result = gPMConstants.PMEReturnCode.PMFalse


            Return m_oFormFields.AddNewFormField(ctlControl:=txtRule, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

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
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

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
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

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
        result = gPMConstants.PMEReturnCode.PMFalse

        ' Assign the details from the interface to the data storage.

        Return InterfaceToData()



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' PUBLIC Methods (End)

    'Private Methords (Begin)
    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try


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


            Return gPMConstants.PMEReturnCode.PMTrue

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
            '    CenterForm Me

            'default to first tab
            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)

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
            Me.cmdSearch.Enabled = False
            Me.cmdTestRule.Enabled = False
            Me.txtRule.Enabled = False

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = CType(CreateRule(m_sRulePath & m_sRuleFileName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_sRuleFileName.Trim() <> "" Then
                result = OpenRule(m_sRulePath, m_sRuleFileName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'we probably want to unload here.

            End If

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtRule
            m_ctlTabFirstLast(ACControlEnd, 0) = txtRule

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


            cmdSave.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdClear.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdTestRule.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTestRuleButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

    'Private Function ValidateForm() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click
        txtRule.SelectionStart = 0
        txtRule.SelectionLength = Strings.Len(txtRule.Text)

        txtRule.SelectionFont = VB6.FontChangeBold(txtRule.SelectionFont, False)
        txtRule.SelectionColor = SystemColors.WindowText
        txtRule.SelectionLength = 0

    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSave.Click

        m_lReturn = CType(SaveRule(m_sRuleFileName), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSearch.Click
        m_lReturn = Search()
    End Sub

    Private Sub cmdTestRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTestRule.Click

        Dim oTestHarness As Object

        Try
            'TODO: to be handled later
            'oTestHarness = New iPMUTestHarness.Interface()
            oTestHarness = New Object

            If oTestHarness Is Nothing Then

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create TestHarness object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTestRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If


            CType(oTestHarness, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            oTestHarness.RuleFileName = m_sRulePath & m_sRuleFileName

            oTestHarness.DataModelCode = DataModelCode

            oTestHarness.DataModelId = DataModelId

            oTestHarness.Start()

            oTestHarness.Dispose()

            oTestHarness = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to test rule", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTestRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'default to rule hasn't been saved
            m_bRuleSaved = gPMConstants.PMEReturnCode.PMFalse

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

        Try

            'RWH(08/01/2001) Disable menu for now. We will pass in required file and
            'automatically open or create it. Save will be done when OK form.
            mnuRule.Available = False

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'get default rule path from registry
            If (m_sRulePath = "") And m_sRuleFileName <> "Database" Then
                If GetRulePath(m_sRulePath) = gPMConstants.PMEReturnCode.PMFalse Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub

                End If
            End If

            If Not m_bFixedFile Then
                mnuRuleNew.Available = True
                mnuRuleOpen.Available = True
            End If

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

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

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

            'Developer Guide No.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'has rule been saved
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            If m_bRuleSaved = gPMConstants.PMEReturnCode.PMFalse Then
                If MessageBox.Show("Save Rule File?", "Save File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    m_lReturn = CType(SaveRule(RuleFileName), gPMConstants.PMEReturnCode)
                End If
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If VB6.PixelsToTwipsY(Me.Height) < 6585 Then
            Me.Height = VB6.TwipsToPixelsY(6585)
        End If

        If VB6.PixelsToTwipsX(Me.Width) < 9345 Then
            Me.Width = VB6.TwipsToPixelsX(9345)
        End If

        tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(9345 - 9060)
        tabMainTab.Height = Me.Height - VB6.TwipsToPixelsY(6585 - 4965)

        txtRule.Height = tabMainTab.Height - VB6.TwipsToPixelsY(4965 - 4065)
        txtRule.Width = tabMainTab.Width - VB6.TwipsToPixelsX(9060 - 8880)

        cmdClear.Left = tabMainTab.Width - VB6.TwipsToPixelsX(9060 - 7560)
        cmdSearch.Left = cmdClear.Left - VB6.TwipsToPixelsX(1440)
        cmdSave.Left = cmdSearch.Left - VB6.TwipsToPixelsX(1440)

        cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(9345 - 8070)
        cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(1200)
        cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(1200)

    End Sub

    Public Sub mnuRuleNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRuleNew.Click

        Dim vRuleFileName, vRulePath As String

        Try

            vRuleFileName = ""
            vRulePath = ""


            With dlgRuleFileSave
                .FileName = ""
                .Title = "Provide File Name to Save"
                .InitialDirectory = m_sRulePath
                .DefaultExt = "*.Rul"

                .Filter = "Rule Files (*.Rul)|*.Rul"
                .ShowDialog()

                If .FileName <> "" Then

                    vRulePath = .FileName.Substring(0, Math.Min(.FileName.Length, .FileName.IndexOf(Path.GetFileName(.FileName))))

                    vRuleFileName = Path.GetFileName(.FileName)
                End If
            End With

            If vRuleFileName.Trim() <> "" Then
                If CreateRule(vRulePath & vRuleFileName) = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(OpenRule(vRulePath, vRuleFileName), gPMConstants.PMEReturnCode)
                End If
            End If

        Catch excep As System.Exception




            If Information.Err().Number <> DialogResult.Cancel Then '(32755)

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new rule", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRuleNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

        End Try

    End Sub

    Public Sub mnuRuleOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRuleOpen.Click
        m_lReturn = CType(OpenRule(), gPMConstants.PMEReturnCode)
    End Sub


    Private Function OpenRule(Optional ByVal v_sRulePath As String = "", Optional ByVal v_sRuleFile As String = "") As Integer

        Dim result As Integer = 0
        On Error GoTo Err_OpenRule

        result = gPMConstants.PMEReturnCode.PMTrue

        m_sRuleFileName = v_sRuleFile.Trim()

        If Not False Then
            m_sRulePath = v_sRulePath
        End If

        If m_sRuleFileName <> "" Then

            Me.txtRule.Enabled = True

            If m_sRuleFileName = "Database" Then
                Me.txtRule.Text = m_sRulePath
            Else
                'load file into textbox
                On Error Resume Next
                'open file, might error if it does not exist
                Me.txtRule.LoadFile(m_sRulePath & m_sRuleFileName, Windows.Forms.RichTextBoxStreamType.PlainText)
                If Information.Err().Number = 0 Then 'loaded okay
                    If FileSystem.GetAttr(m_sRulePath & m_sRuleFileName) And FileAttribute.ReadOnly Then
                        MessageBox.Show("File is read only", "Open File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cmdSave.Enabled = False
                        cmdOK.Enabled = False
                    End If
                Else
                    If Information.Err().Number = 75 Or Information.Err().Number = 53 Then
                        Information.Err().Clear()

                        Me.txtRule.SaveFile(m_sRulePath & m_sRuleFileName, Windows.Forms.RichTextBoxStreamType.PlainText)

                        If Information.Err().Number <> 0 Then
                            result = gPMConstants.PMEReturnCode.PMError

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRule Failed (" & m_sRulePath & m_sRuleFileName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                            Return result

                        End If

                        Me.txtRule.LoadFile(m_sRulePath & m_sRuleFileName, Windows.Forms.RichTextBoxStreamType.PlainText)
                    Else
                        result = gPMConstants.PMEReturnCode.PMError

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRule Failed (" & m_sRulePath & m_sRuleFileName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                End If
                'Me.cmdTestRule.Enabled = True
            End If


            On Error GoTo Err_OpenRule

            Me.txtRule.SelectionStart = 0

            Me.cmdSearch.Enabled = True

            UpdateStatusBar("FileName")
            UpdateStatusBar("LineNo")
            'TR 12/03/2004 - Add new message if a value for Scheme ID is passed in
            If m_lSchemeID <> 0 Then
                UpdateStatusBar("Message", "Gis Scheme ID: " & m_lSchemeID)
            End If
        Else
            MessageBox.Show("File name is empty", "Open Rule", MessageBoxButtons.OK)
        End If

        Return result

Err_OpenRule:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenRule Failed (" & m_sRulePath & m_sRuleFileName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenRule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Private Function CreateRule(ByVal v_sRuleFileName As String) As Integer

        Dim result As Integer = 0
        Dim lFileNo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lFileNo = FileSystem.FreeFile()
            FileSystem.FileOpen(lFileNo, v_sRuleFileName, OpenMode.Output)

            FileSystem.PrintLine(lFileNo, "'START OF RULES *****************************************************************************************")

            FileSystem.PrintLine(lFileNo, "")
            ' We have to update the rule files in midst

            FileSystem.PrintLine(lFileNo, "'END OF RULES *******************************************************************************************")

            FileSystem.FileClose(lFileNo)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function Search() As Integer


        Dim sSought As String = Interaction.InputBox("Enter the text to search", "Searching....")

        If sSought = "" Then
            Exit Function
        End If

        m_lReturn = CType(FindIt(Me.txtRule, sSought, 1, ColorTranslator.ToOle(Color.Red)), gPMConstants.PMEReturnCode)

    End Function


    Private Function UpdateStatusBar(ByVal v_sKey As String, Optional ByVal v_sMessage As String = "") As Object

        Select Case v_sKey.ToUpper()
            Case "FILENAME"
                Me.stbMain.Items.Item(v_sKey).Text = "FileName: " & m_sRuleFileName
            Case "LINENO"
                Me.stbMain.Items.Item(v_sKey).Text = "Line No:" & Me.txtRule.GetLineFromCharIndex(Me.txtRule.SelectionStart) + 1
            Case "MESSAGE"
                Me.stbMain.Items.Item(v_sKey).Text = v_sMessage
        End Select
    End Function

    Private Function SaveRule(ByVal v_sRuleFileName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sRuleFileName = "Database" Then
                m_sRulePath = Me.txtRule.Text
            Else
                txtRule.SaveFile(m_sRulePath & v_sRuleFileName, Windows.Forms.RichTextBoxStreamType.PlainText)
            End If

            m_bRuleSaved = gPMConstants.PMEReturnCode.PMTrue

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRule Failed (" & m_sRulePath & v_sRuleFileName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer

        Dim result As Integer = 0
        Dim sSubKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   sSubKey = "GIS\" & g_sDataModelCode
            sSubKey = "GIS\" & m_sDataModelCode

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If r_sRulePath = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read RulePath registry setting for " & sSubKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRulePath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Else
                If Not r_sRulePath.EndsWith("\") Then
                    r_sRulePath = r_sRulePath & "\"
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get rule's path from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRulePath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub mnuRuleSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRuleSave.Click
        m_lReturn = CType(SaveRule(RuleFileName), gPMConstants.PMEReturnCode)
    End Sub


    Private Sub txtRule_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRule.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Try

            Dim lStart, lEnd As Integer
            Dim Source As String = ""

            'RWH(08/01/2001) Reset saved flag if more editing done.
            m_bRuleSaved = False

            If Strings.Chr(KeyAscii).ToString() = "'" Then
                Me.txtRule.SelectionColor = Color.Lime

                Source = Me.txtRule.Text

                lStart = Me.txtRule.SelectionStart

                lEnd = Strings.InStr(lStart, Source, Strings.Chr(13) & Strings.Chr(10))

                ' Check for the end of line using (vbcrlf)
                'starting at the beginning of the the comment Mark

                If (lEnd - lStart) > 0 Then 'there is at least one more occurrence of
                    'the string
                    With Me.txtRule
                        .SelectionStart = lStart
                        .SelectionLength = lEnd - lStart

                        .SelectionColor = Color.Green
                        .SelectionLength = 0 'this line removes the selection highlight
                    End With
                End If

            End If

            If KeyAscii = 13 Then
                Me.txtRule.SelectionColor = Color.Black
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            eventArgs.KeyChar = Convert.ToChar(KeyAscii)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Private Sub txtRule_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtRule.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Try

            If Shift = ShiftConstants.CtrlMask Then
                Select Case KeyCode
                    ' If Ctrl+A:
                    Case Keys.A
                        ' Select all...
                        txtRule.SelectionStart = 0
                        txtRule.SelectionLength = Strings.Len(txtRule.Text)
                        ' If Ctrl+C:
                        'Case Keys.C

                        'My.Computer.Clipboard.SetText(txtRule.SelectedRtf, TextDataFormat.Rtf)
                        ' If Ctrl+S:
                    Case Keys.S
                        ' Select to the end of the sentence.

                        txtRule.Find(".?!:", RichTextBoxFinds.None)
                        ' Extend selection to include punctuation.
                        txtRule.SelectionLength += 1
                        ' If Ctrl+T:
                    Case Keys.T
                        txtRule.SelectedText = Strings.Chr(9)
                        ' If Ctrl+V:
                    Case Keys.V
                        If My.Computer.Clipboard.ContainsData(DataFormats.Rtf) Then
                            My.Computer.Clipboard.GetText(TextDataFormat.Rtf)
                        End If
                        ' If Ctrl+W:
                    Case Keys.W
                        ' Select to the end of the word.

                        txtRule.Find(" ,;:.?!", RichTextBoxFinds.None)
                End Select
            End If

            Select Case KeyCode
                Case Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown, Keys.Back, Keys.Delete

                    UpdateStatusBar("LineNo")

            End Select

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub txtRule_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles txtRule.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y
        UpdateStatusBar("LineNo")
    End Sub

End Class