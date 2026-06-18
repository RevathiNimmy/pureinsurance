Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
'Developer Guide No. 186
Imports uctListPolicyControl
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form


    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' Private variables
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lStatus As Integer

    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lReturn As Integer
    Private m_lErrorNumber As Integer

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    'TN20001117
    Private m_lFindType As Integer
    'ED 18072002
    Private m_bUnderwriting As Boolean
    'TF040902
    Private m_sInsFileType As String = ""
    Private m_bDisableInsFileType As Boolean

    'Kevin Renshaw
    Private m_lDummyFindType As Integer
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private Const sLvwDoubleClick As String = "uctListPolicyControl_lvwSearchDetailsDblClick"
    Public Property BackDatedMTAsAllowed() As Boolean
        Get
            Return m_bBackDatedMTAsAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_bBackDatedMTAsAllowed = Value
        End Set
    End Property
    Public Property SelectedPolicyStatus() As String
        Get
            Return m_sSelectedPolicyStatus
        End Get
        Set(ByVal Value As String)
            m_sSelectedPolicyStatus = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)


    'TN20001117 (Start)
    Public WriteOnly Property FindType() As Integer
        Set(ByVal Value As Integer)
            m_lFindType = Value
        End Set
    End Property
    'TN20001117 (End)

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property


    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    'ED 18072002

    Public Property IsUnderwriting() As Boolean
        Get
            Return m_bUnderwriting
        End Get
        Set(ByVal Value As Boolean)
            m_bUnderwriting = Value
        End Set
    End Property
    'ED 18072002 (End)

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public ReadOnly Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
    End Property

    Public ReadOnly Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
    End Property

    Public ReadOnly Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
    End Property

    'TF0409002
    Public WriteOnly Property InsFileType() As String
        Set(ByVal Value As String)
            m_sInsFileType = Value
        End Set
    End Property
    Public WriteOnly Property DisableInsFileType() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableInsFileType = Value
        End Set
    End Property

    'Kevin Renshaw (CMG) 25/2/2003 (Start)
    Public WriteOnly Property DummyFindType() As Integer
        Set(ByVal Value As Integer)
            m_lDummyFindType = Value
        End Set
    End Property
    'Kevin Renshaw (CMG) (End)


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMTADate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        ' Click event of the Cancel button.
        Try

            If tabMTAtab.Visible Then
                tabMTAtab.Visible = False
                uctListPolicyControl.Visible = True
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListPolicyControl.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Developer Guide No. 231
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184 (Latest guide) and replace PMHelpFunc.ShowHelp(dlgHelp, MainModule.ScreenHelpID) by SharedFiles.PMHelpFunc.ShowHelp()
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = SharedFiles.PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = 0
            m_lInsuranceFolderCnt = 0
            m_sInsuranceRef = ""

            ' hide the interface.
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private sSender As String = ""
    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sDisableTempMTA As String

        ' Click event of the OK button.

        Try
            uctListPolicyControl_lvwSearchDetailsMouseDown(Nothing, Nothing)
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            If uctListPolicyControl.SelectedPolicyStatus = "Cancelled" And m_sTransactionType = SharedFiles.gSIRLibrary.SIRProcessCodeMTA Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5116, r_sOptionValue:=sDisableTempMTA)
                If ToSafeLong(sDisableTempMTA, "0") = 1 Then
                    'Temp MTAs are disabled by System option - therefore you cannot do any MTAs on cancelled policies
                    MsgBox("Not allowed to do MTA on Cancelled Policy", , "MTA on Cancelled Policy")
                    Exit Sub
                End If
            End If

            ' Process the OK in the control
            m_lReturn = uctListPolicyControl.OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = uctListPolicyControl.InsFileCnt
            m_lInsuranceFolderCnt = uctListPolicyControl.InsuranceFolderCnt
            m_sInsuranceRef = uctListPolicyControl.InsReference
            m_sShortName = uctListPolicyControl.ShortName
            sSender = eventSender.Name
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                uctListPolicyControl.lvwSearchDetailsSetFocus()
                'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                'To enable the ok button only if a vaild policy is selected
                cmdOK.Enabled = (uctListPolicyControl.Selected = gPMConstants.PMEReturnCode.PMTrue)
                'cmdOK.Enabled = False
                'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                'Kevin Renshaw (CMG) don't reset it to visible for MTA / EDIT find type
                If m_lDummyFindType <> 5 And m_lDummyFindType <> 6 Then
                    cmdNew.Visible = Me.IsUnderwriting
                End If
                'End CMG

                ' Hide the "new" button if we are cancelling or reinstating a policy
                If m_sTransactionType.Trim().ToUpper() = "MTC" Or m_sTransactionType.Trim().ToUpper() = "MTR" Then
                    cmdNew.Visible = False
                End If
                ' /Alix

                Exit Sub

            Catch excep As System.Exception



                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Activate failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub


    Public Sub frmInterface_Load()

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_oFormFields = New iPMFormControl.FormFields()

            'TN20010420 Start
            cmdOK.Enabled = False
            'TN20010420 End

            tabMTAtab.Visible = False
            tabMTAtab.Top = VB6.TwipsToPixelsY(120)

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            With uctListPolicyControl
                ' Task
                m_lReturn = .SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

                'TN20001117 (Start)
                '.FindType = 0

                .FindType = m_lFindType
                'TN20001117 (End)
                'TF040902
                .InsFileType = m_sInsFileType
                .DisableInsFileType = m_bDisableInsFileType

                .InsHolderCnt = m_lPartyCnt
                ' Short Name
                .ShortName = m_sShortName
            End With
            'Developer Guide No 9. 
            m_lReturn = uctListPolicyControl.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctListPolicyControl.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            '    m_lReturn& = uctListPolicyControl.GetBusiness()
            '    If (m_lReturn& = PMFalse) Then
            '        SetMousePointer PMMouseNormal
            ' Log Error.
            '        LogMessage _
            'iType:=PMLogOnError, _
            'sMsg:="Failed to get the business.", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="Form_Load", _
            'vErrNo:=Err.Number, _
            'vErrDesc:=Err.Description
            '        Exit Sub
            '    End If

            m_lReturn = uctListPolicyControl.GetPolicies()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the policies.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If (m_lDummyFindType = 5) Or (m_lDummyFindType = 6) Then
                cmdNew.Visible = False
            End If

            ' Alix - 05/01/2003 - PN9243
            ' Hide the "new" button if we are cancelling a policy
            If m_sTransactionType.Trim().ToUpper() = "MTC" Then
                cmdNew.Visible = False
            End If
            ' /Alix


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        If sSender = cmdOK.Name Or sSender = sLvwDoubleClick Then
            Exit Sub
        End If
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctListPolicyControl.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctListPolicyControl.Dispose()
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub txtMTADate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMTADate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMTADate)

    End Sub

    Private Sub txtMTADate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMTADate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMTADate)

    End Sub

    Private Sub uctListPolicyControl_cboStatusChange(ByVal Sender As Object, ByVal e As EventArgs) Handles uctListPolicyControl.cboStatusChange
        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
        'To enable the ok button only if a vaild policy is selected
        cmdOK.Enabled = False
        'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    End Sub

    Private Sub uctListPolicyControl_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctListPolicyControl.Click
        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
        'To enable the ok button only if a vaild policy is selected
        'cmdOK.Enabled = True
        'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    End Sub

    Private Sub uctListPolicyControl_lvwSearchDetailsMouseDown(ByVal Sender As Object, ByVal e As uctListPolicy.lvwSearchDetailsMouseDownEventArgs) Handles uctListPolicyControl.lvwSearchDetailsMouseDown
        'cmdOK.Enabled = (v_lSelected = PMTrue)
        Const kMethodName As String = "uctListPolicyControl_PolicyListRefreshed"
        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.1.1.1)
        'Dim blnBackDatedMTAsAllowed As Boolean
        Dim oBackDated As Object
        Dim vValue As Object

        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.1.1.1)
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.1.1.1)
            'As said by Gaurav
            Dim temp_oBackDated As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBackDated, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBackDated = temp_oBackDated

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create the policy business object", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            If e Is Nothing Then
                m_lReturn = oBackDated.BackDatedMTAsAllowed(v_lInsuranceFileCnt:=uctListPolicyControl.m_vSearchData(2, 0), r_lRecordsAffected:=vValue)
            Else
                m_lReturn = oBackDated.BackDatedMTAsAllowed(v_lInsuranceFileCnt:=uctListPolicyControl.m_vSearchData(2, e.m_lSelected - 1), r_lRecordsAffected:=vValue)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oBackDated.Dispose()
                oBackDated = Nothing
                gPMFunctions.RaiseError(kMethodName, "oBackDated.BackDatedMTAsAllowed Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If


            ' blnBackDatedMTAsAllowed = vValue
            If Information.IsArray(vValue) Then
                m_bBackDatedMTAsAllowed = gPMFunctions.ToSafeInteger(vValue(0, 0), 0) = 1
            End If

            If uctListPolicyControl.SelectedPolicyStatus = "Cancelled" Or uctListPolicyControl.SelectedPolicyStatus = "Lapsed" Then
                ' Proceed to next screen; if backdated MTA is not allowed it will disable the Perm. MTA check box
                cmdOK.Enabled = True
            End If

            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'To enable the ok button only if a vaild policy is selected
            If m_sTransactionType = "MTA" And uctListPolicyControl.SelectedPolicyStatus = "Quote" Then
                cmdOK.Enabled = False
            End If
            If m_sTransactionType = "MTA" And uctListPolicyControl.SelectedPolicyStatus = "Active" Then
                cmdOK.Enabled = True
            End If
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            If m_sTransactionType <> "MTA" Then
                cmdOK.Enabled = True
            End If

            If Not uctListPolicyControl.ItemSelected Then
                cmdOK.Enabled = False
            End If

            m_sSelectedPolicyStatus = uctListPolicyControl.SelectedPolicyStatus
            If Not (oBackDated Is Nothing) Then


                oBackDated.Dispose()
            End If

        Catch excep As System.Exception
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.1.1.1)

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process uctListPolicyControl_PolicyListRefreshed", vApp:=ACApp, vClass:=ACClass, vMethod:="uctListPolicyControl_PolicyListRefreshed", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub
    ''' <summary>
    ''' uctListPolicyControl_lvwSearchDetailsDblClick
    ''' </summary>
    ''' <param name="Sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Allowing to select the policy by double clicking</remarks>
    Private Sub uctListPolicyControl_lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As uctListPolicy.lvwSearchDetailsDblClickEventArgs) Handles uctListPolicyControl.lvwSearchDetailsDblClick
        Dim oResult(,) As Object
        Dim obPMUPolicy As Object
        Dim bIsPendingPortfolioTransfer As Boolean
        Dim bIsPendingCloneTransfer As Boolean
        Dim sDisableTempMTA As String
        Const kMethodName = "uctListPolicyControl_lvwSearchDetailsDblClick"

        'Double Click event of the Control.
        Try
            'PN 49329
            If Not cmdOK.Enabled Then
                Exit Sub
            End If
            ' Set the interface status.
            m_lStatus = PMEReturnCode.PMOK

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = e.m_lInsFileCnt
            m_lInsuranceFolderCnt = e.m_lInsuranceFolderCnt 'PN35753 --RC
            m_sInsuranceRef = e.m_sInsReference
            m_sShortName = e.m_sShortName
            sSender = sLvwDoubleClick
            If uctListPolicyControl.SelectedPolicyStatus = "Cancelled" AndAlso m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5116, r_sOptionValue:=sDisableTempMTA)
                If ToSafeLong(sDisableTempMTA, "0") = 1 Then
                    'Temp MTAs are disabled by System option - therefore you cannot do any MTAs on cancelled policies
                    MsgBox("Not allowed to do MTA on Cancelled Policy", , "MTA on Cancelled Policy")
                    Exit Sub
                End If
            End If

            Dim otemp_bPMUPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=otemp_bPMUPolicy, sClassName:="bPMUPolicy.Business", vInstanceManager:=PMGetViaClientManager)
            obPMUPolicy = otemp_bPMUPolicy

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to create the policy business object", PMELogLevel.PMLogError)
                Exit Sub
            End If

            m_lReturn = obPMUPolicy.IsPendingPortfolioTransfer(sInsuranceFileRef:=m_sInsuranceRef, r_oResult:=oResult, r_bIsPendingPortfolioTransfer:=bIsPendingPortfolioTransfer, r_bIsPendingCloneTransfer:=bIsPendingCloneTransfer)

            obPMUPolicy.Dispose()

            If IsArray(oResult) Or bIsPendingPortfolioTransfer Then
                MessageBox.Show("Pending Portfolio Transfer.", "Pending portfolio transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = PMEReturnCode.PMCancel
                Exit Sub
            ElseIf bIsPendingCloneTransfer Then
                MessageBox.Show("Pending Cloned Transfer.", "Pending Cloned transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lStatus = PMEReturnCode.PMCancel
                Exit Sub
            End If

            Me.Hide()

        Catch excep As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process the Double Click", vApp:=ACApp, vClass:=ACClass, vMethod:="uctListPolicyControl_lvwSearchDetailsDblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub uctListPolicyControl_PolicyListRefreshed(ByVal Sender As Object, ByVal e As uctListPolicy.PolicyListRefreshedEventArgs) Handles uctListPolicyControl.PolicyListRefreshed


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            If e.ItemsFound > 0 Then
                'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                'To enable the ok button only if a vaild policy is selected
                ' cmdOK.Enabled = True
                'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                If (m_sTransactionType = "MTC" Or m_sTransactionType = "EDIT") And uctListPolicyControl.SelectedPolicyStatus <> "Active" Then
                    cmdOK.Enabled = False
                End If

            Else
                cmdOK.Enabled = False
            End If

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process uctListPolicyControl_PolicyListRefreshed", vApp:=ACApp, vClass:=ACClass, vMethod:="uctListPolicyControl_PolicyListRefreshed", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctListPolicyControl.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub


End Class