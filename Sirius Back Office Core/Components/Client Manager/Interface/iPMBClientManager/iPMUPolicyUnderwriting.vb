Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmPolicyUnderwriting
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmPolicyUnderwriting
	'
	' Date: 08/06/1998
	'
	' Description: Party Corporate Interface.
	'
	' Edit History:
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_iIndex As Integer
    Private m_sShortName As String = ""
    Private m_sInsReference As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsFileCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_sFooter As String = ""
    Private m_sResolvedName As String = ""
    Private m_sPartyType As String = ""

    Private m_bEvent As Boolean

    Private m_bEventRaised As Boolean

    'TN200809
    Private m_bPMRaiseEvent As Boolean 'set to true to create event

    'eck 180500
    Private m_iSourceID As Integer
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    'AK 270401 - variable to store Renewal Events
    Private m_bRenEvent As Boolean


    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property

    ' RAG 26-10-01 - This code copied from frmPolicy,
    ' Otherwise you get an error when you try to load this form.
    ' (CTAF 100701)
    ' CopiedPolicy
    Private m_bCopiedPolicy As Boolean

    Public Property CopiedPolicy() As Boolean
        Get
            Return m_bCopiedPolicy
        End Get
        Set(ByVal Value As Boolean)
            m_bCopiedPolicy = Value
        End Set
    End Property
    ' RAG End


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

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

        End Set
    End Property


    'AK 270401 - set property procedures for Renewal Event - Begin
    Public Property RenewalEvent() As Boolean
        Get

            Return m_bRenEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bRenEvent = Value

        End Set
    End Property
    'AK 270401 - set property procedures for Renewal Event - End

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

    Public Property Index() As Integer
        Get

            ' Return the objects task.
            Return m_iIndex

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iIndex = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public Property InsReference() As String
        Get

            Return m_sInsReference

        End Get
        Set(ByVal Value As String)

            m_sInsReference = Value

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

    Public Property ResolvedName() As String
        Get

            Return m_sResolvedName

        End Get
        Set(ByVal Value As String)

            m_sResolvedName = Value

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property

    Public Property InsFileCnt() As Integer
        Get

            Return m_lInsFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsFileCnt = Value

        End Set
    End Property
    'eck180500
    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property PartyType() As String
        Get

            Return m_sPartyType

        End Get
        Set(ByVal Value As String)

            m_sPartyType = Value

        End Set
    End Property

    Public Property Footer() As String
        Get

            Return m_sFooter

        End Get
        Set(ByVal Value As String)

            m_sFooter = Value

        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    Public Property EventRaised() As Boolean
        Get

            Return m_bEventRaised

        End Get
        Set(ByVal Value As Boolean)

            m_bEventRaised = Value

        End Set
    End Property

    Public Property PMRaiseEvent() As Boolean
        Get
            Return m_bPMRaiseEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bPMRaiseEvent = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: SetRaiseEvent
    '
    ' Description:
    '
    ' History: 09/08/2000 Tinny - Created
    '
    ' ***************************************************************** '
    Public Function SetRaiseEvent() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            uctPMUPolicyControl1.PMRaiseEvent = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRaiseEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRaiseEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetEventRaised
    '
    ' Description:
    '
    ' History: 06/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function SetEventRaised() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            uctPMUPolicyControl1.EventRaised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetEventRaised Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetEventRaised", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim bThere As Boolean

        ' Click event of the Cancel button.
        Try

            'Check that the risk screen isn't open
            m_lReturn = objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsFileCnt, v_lRiskGroupID:=uctPMUPolicyControl1.RiskGroupId, v_bFromEvent:=m_bEvent, r_bThere:=bThere)

            If bThere Then
                MessageBox.Show("You must close risk screen before exiting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPMUPolicyControl1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
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
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            uctPMUPolicyControl1.ShowHelpScreen(cmdHelp, objCM.ScreenHelpID)

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdInstalment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInstalment.Click
        ShowInstalments()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bThere As Boolean

        ' Click event of the OK button.

        Try

            'Check that the risk screen isn't open
            m_lReturn = objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsFileCnt, v_lRiskGroupID:=uctPMUPolicyControl1.RiskGroupId, v_bFromEvent:=m_bEvent, r_bThere:=bThere)

            If bThere Then
                MessageBox.Show("You must close risk screen before exiting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = uctPMUPolicyControl1.OKClick()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Update the policy window
            m_lReturn = objCM.m_ofrmMDI.RefreshPolicies()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCommission.Click
        ShowPolicyCommission()
    End Sub

    Private Sub cmdFee_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFee.Click
        ShowPolicyFees()
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdReInsurance_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdReInsurance_Click()
    'Try 
    '
    'm_lReturn = uctPMUPolicyControl1.GetRiskReinsurance()
    'If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get RiskReinsurance.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Exit Sub
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get RiskReinsurance.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'End Try
    'End Sub
    Private Sub cmdPolicyTax_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicyTax.Click
        ShowPolicyTaxes()
    End Sub



    Private Sub frmPolicyUnderwriting_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = Me.InsReference
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            '    objcm.m_ofrmMDI.Caption = "[" & Trim$(Me.ResolvedName) & "] Policy Master Client Manager"
            '    Me.Caption = "Policy : [" & Trim$(Me.ResolvedName) & "]"
            '    objcm.m_ofrmMDI.Caption = "[" & Trim$(Me.ShortName) & "] Policy Master Client Manager"
            '    Me.Caption = "Policy : [" & Trim$(Me.ResolvedName) & "]"
            objCM.m_ofrmMDI.Text = "[" & Me.ShortName.Trim() & " " & Me.InsReference.Trim() & "] Sirius Client Manager"
            Me.Text = "Policy : [" & Me.ShortName.Trim() & " " & Me.InsReference.Trim() & "]"

            m_lReturn = objCM.SetToolbar(v_sFormName:=Name, v_bFromEvent:=FromEvent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

            objCM.m_ofrmMDI.InsuranceFolderCnt = Me.InsuranceFolderCnt
            objCM.m_ofrmMDI.InsFileCnt = Me.InsFileCnt
            objCM.m_ofrmMDI.InsReference = Me.InsReference
            objCM.m_ofrmMDI.PolicyTypeId = PMBConst.PMBPolicyTypeUnderwriting
            objCM.m_ofrmMDI.GeminiPolicyStatus = 0

        End If
    End Sub

    ' ***************************************************************** '
    ' Name: LoadInterface
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function LoadInterface() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SwitchTo
    '
    ' Description: Switches focus to this form.
    '
    ' ***************************************************************** '
    Public Function SwitchTo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the focus
            Me.Activate()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section
        'm_lErrorNumber = gPMConstants.PMEReturnCode.PMError
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub


    Private Sub frmPolicyUnderwriting_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim i As Integer
        Dim lReturn, lStatus As Integer
        Dim sOption As String
        ' Forms load event.
        Try

            'MSS280901 - Added for merge
            mnuPolicy.Available = False
            mnuGotoTransaction.Available = False
            mnuGoToSwift.Available = False

            'MSS280901 - Merge end

            If FromEvent Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If


            Me.Height = VB6.TwipsToPixelsY(8490)
            Me.Width = VB6.TwipsToPixelsX(9300)


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            With uctPMUPolicyControl1
                '.set_Task(m_iTask)
                .Task = m_iTask
                '.set_Status(gPMConstants.PMEReturnCode.PMTrue)
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .InsuranceFileCnt = m_lInsFileCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .PartyCnt = m_lPartyCnt
                .FromEvent = FromEvent
                .EventRaised = EventRaised
                'eck180500

                '.set_SourceID(SourceID)
                .SourceId = SourceID
            End With
            'Developer Guide No.9
            m_lReturn = uctPMUPolicyControl1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPMUPolicyControl1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPMUPolicyControl1.GetPolicy()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            lStatus = uctPMUPolicyControl1.Status

            'AK 270401 - Added Renewal Events case
            If FromEvent Or m_bRenEvent Then
                mnuPolicy.Text = "Event"
                mnuPolicyCopy.Available = False
                mnuPolicyDelete.Available = False
                mnuPolicyMove.Available = False
                mnuGotoAccounts.Available = False
                mnuGoToClaim.Available = False
                mnuGoToDocumaster.Available = False
                mnuGoToEvents.Available = False
                mnuGoToNotes.Available = False
                mnuGoToSwift.Available = False
                mnuGoToTextFiles.Available = False
                mnuGotoTransaction.Available = False
                mnuDocumentation.Available = False
                mnuReports.Available = False
                mnuTasks.Available = False
            End If

            'MSS280901 - Added for merge
            mnuTasks.Available = False
            mnuGotoTransaction.Available = False
            mnuReportsClientSummary.Available = False
            'MSS280901 - Merge end

            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGoToNotes.Available = False
            End If
            'sj 04/10/2002 - end

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'Sharepoint
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            If sOption = "1" Then
                mnuGoToDocumaster.Visible = True
                mnuGoToSharePoint.Visible = False
            ElseIf sOption = "2" Then
                mnuGoToDocumaster.Visible = False
                mnuGoToSharePoint.Visible = True
            End If
            'Sharepoint
        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmPolicyUnderwriting_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim iCount As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                cmdCancel.Focus() 'ct 31/08/00 This stops VB error 91 when closing not closing form via cancel button

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPMUPolicyControl1.CancelClick()

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
            uctPMUPolicyControl1.Dispose()

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
            objCM.m_ofrmMDI.Text = "Sirius Client Manager"

            iCount = 0

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objCM.m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                m_lReturn = objCM.SetToolbar(v_sFormName:=objCM.m_ofrmMDI.Name)
            End If

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

    Private isInitializingComponent As Boolean
    Private Sub frmPolicyUnderwriting_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then

            Me.Height = 620 ' VB6.TwipsToPixelsY(8490)
            Me.Width = 636 'VB6.TwipsToPixelsX(9300)

        End If

    End Sub

    Private Sub frmPolicyUnderwriting_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        '         Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        '         Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            '             Call the recent file list procedure
            '        GetRecentFiles
        End If

    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        'CT 31/08/00 bugfix 379
        cmdCancel.Focus()
        cmdCancel_Click(cmdCancel, New EventArgs())
    End Sub

    Public Sub mnuDiaryFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryFind.Click

        m_lReturn = objCM.ShowTaskList(v_lPartyCnt:=PartyCnt, v_lInsuranceFileCnt:=InsFileCnt)

    End Sub

    Public Sub mnuDiaryNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryNew.Click

        m_lReturn = objCM.ProcessTasks(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference)

    End Sub

    ' PRIVATE Property Procedures (End)

    Public Sub mnuDocumentationLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationLetterWriting.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGotoAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoAccounts.Click

        Try

            ' Call OrionLinkFunc function
            'eck220800
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=m_sShortName, v_sInsuranceRef:=m_sInsReference)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoAccounts menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoAccounts_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuGoToClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToClaim.Click

        Try

            ' CTAF 030300
            MessageBox.Show("This functionality is yet to be implemented.", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub

            'DC071100 process 'goto' claim

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowClaimList(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sInsReference:=m_sInsReference)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch


            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGoToDocumaster_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToDocumaster.Click

        ' CTAF 030300
        'MsgBox "This functionality is yet to be implemented.", vbInformation, "Documaster"

        ' ND 181000
        ' Call Documaster link to open Documaster at policy level (2) for this client
        m_lReturn = objCM.ShowDocumaster(v_sLinkCode:=m_sInsReference & objCM.DME_POLICY)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If


    End Sub

    Public Sub mnuGoToEvents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToEvents.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.17)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"
        'Sankar - PN 55197 - Added m_sInsReference
        m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sInsuranceRef:=m_sInsReference, v_sShortName:=m_sShortName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.17)

    Public Sub mnuGoToNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToNotes.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNamePolicy, v_lEntityCnt:=m_lInsFileCnt, v_sTextType:="Public", v_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGoToRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToRisk.Click

        Dim vRiskCodeId As Object
        Dim lRiskCodeId As Integer
        Dim vRiskGroupId As Object
        Dim lRiskGroupId As Integer

        'Show ListRiskForm as underwriting policies may have multiple risks against them
        'Call Toolbar Control function
        m_lReturn = objCM.ShowListofRisks(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId, v_sInsuranceRef:=m_sInsReference, v_bFromEvent:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If
        'CT 13/09/00 Show RSA underwriting ListRisk screen - end
        Exit Sub



        'Continue as not serious
        Exit Sub

    End Sub

    Public Sub mnuGoToTextFiles_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToTextFiles.Click

        Dim vRiskCodeId, vRiskGroupId As Object
        Dim lRiskCodeId, lRiskGroupId As Integer

        Try



            vRiskCodeId = uctPMUPolicyControl1.RiskCodeId


            vRiskGroupId = uctPMUPolicyControl1.RiskGroupId


            If Convert.IsDBNull(vRiskCodeId) Or IsNothing(vRiskCodeId) Then
                lRiskCodeId = 0
            Else

                lRiskCodeId = CInt(vRiskCodeId)
            End If


            If Convert.IsDBNull(vRiskGroupId) Or IsNothing(vRiskGroupId) Then
                lRiskGroupId = 0
            Else

                lRiskGroupId = CInt(vRiskGroupId)
            End If

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGotoTransactionCash_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionCash.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGotoTransactionCash, v_sShortName:=m_sShortName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCash menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCash_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'EK 22/9/99
    Public Sub mnuGotoTransactionDebit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionDebit.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionDebit, v_lInsuranceFileCnt:=m_lInsFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionDebit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionDebit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Public Sub mnuGotoTransactionCredit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionCredit.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionCredit, v_lInsuranceFileCnt:=m_lInsFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCredit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCredit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'eck080800
    Public Sub mnuGotoTransactionAJ_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionAJ.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGoToTransactionAJ, v_lInsuranceFileCnt:=m_lInsFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionAJ menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionAJ_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Public Sub mnuGotoTransactionAJReversal_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionAJReversal.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGoToTransactionAJReversal, v_lInsuranceFileCnt:=m_lInsFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionAJReversal menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionAJReversal_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = objCM.ShowSBOAbout()

    End Sub

    Public Sub mnuPolicyDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPolicyDelete.Click

        Dim bThere As Boolean

        Try

            'It may turn out that we don't care about this, and we just close them afterward
            'It's a business flow decision left until this is system tested

            'Check that the risk screen isn't open
            m_lReturn = objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsFileCnt, v_lRiskGroupID:=uctPMUPolicyControl1.RiskGroupId, v_bFromEvent:=False, r_bThere:=bThere)

            If bThere Then
                MessageBox.Show("You must close risk screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Check that the text screen isn't open
            m_lReturn = objCM.CheckText(v_lInsuranceFileCnt:=m_lInsFileCnt, v_bFromEvent:=False, r_bThere:=bThere)

            If bThere Then
                MessageBox.Show("You must close text screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Check that the event screen isn't open
            m_lReturn = objCM.CheckEvent(v_lInsuranceFileCnt:=m_lInsFileCnt, r_bThere:=bThere)

            If bThere Then
                MessageBox.Show("You must close event screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Call DeleteClick function
            m_lReturn = uctPMUPolicyControl1.DeleteClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Update the policy window
            m_lReturn = objCM.m_ofrmMDI.RefreshPolicies()

            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuPolicyDelete menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPolicyDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
	' ***************************************************************** '
	' Name: GetPolicyTax
	'
	' Desc: get policy level tax
	'
	' Hist: 12/01/2004 Tinny - Created
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetPolicyTax) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetPolicyTax() As Integer
		'Dim result As Integer = 0
		'Dim iPMURITax As Object
		'

		'Dim oObject As iPMURITax.Interface_Renamed
		'
		'On Error GoTo Err_GetPolicyTax
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'
		'Dim temp_oObject As Object
		'm_lReturn = objCM.g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURITax.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		'oObject = temp_oObject
		'
		'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'result = gPMConstants.PMEReturnCode.PMFalse
			'GoTo End_GetPolicyTax
		'End If
		'

		'm_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
		'

		'oObject.InsuranceFileCnt = m_lInsFileCnt
		'

		'm_lReturn = oObject.Start()
		'
		'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'result = gPMConstants.PMEReturnCode.PMFalse
			'GoTo End_GetPolicyTax
		'End If
		'
		'GoTo End_GetPolicyTax
		'
		'Return result
		'
'Err_GetPolicyTax: '
		'
		'result = gPMConstants.PMEReturnCode.PMError
		'
		' Log Error Message
		'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTax", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		'
		'GoTo End_GetPolicyTax
		'
'End_GetPolicyTax: '
		'
		'If Not (oObject Is Nothing) Then

			'm_lReturn = oObject.Terminate()
			'oObject = Nothing
		'End If
		'
		'Return result
		'
	'End Function
	
	' ***************************************************************** '
	' Name: ShowPolicyTaxes
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ShowPolicyTaxes() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ShowPolicyTaxes"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lReturn = uctPMUPolicyControl1.GetPolicyTaxDetail()
		If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			gPMFunctions.RaiseError(kMethodName, "GetPolicyTaxDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: ShowPolicyFees
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ShowPolicyFees() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ShowPolicyFees"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lReturn = uctPMUPolicyControl1.GetFeeDetail()
		If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			gPMFunctions.RaiseError(kMethodName, "GetFeeDetail Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: ShowInstalments
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ShowInstalments() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ShowInstalments"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lReturn = uctPMUPolicyControl1.HasInstalment(m_lInsFileCnt)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				MessageBox.Show("There are no instalments for this policy version.", "Instalment Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Else
				gPMFunctions.RaiseError(kMethodName, "HasInstalment Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
		Else
			lReturn = uctPMUPolicyControl1.GetInstalmentDetail()
			If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				gPMFunctions.RaiseError(kMethodName, "GetInstalmentDetail Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: ShowPolicyCommission
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ShowPolicyCommission() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ShowPolicyCommission"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lReturn = uctPMUPolicyControl1.GetCommissionDetail()
		If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			gPMFunctions.RaiseError(kMethodName, "GetCommissionDetail Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function

    Private Sub uctPMUPolicyControl1_BusinessTypeChange(ByVal Sender As Object, ByVal e As PMUPolicyControl.uctPMUPolicyControl.BusinessTypeChangeEventArgs) Handles uctPMUPolicyControl1.BusinessTypeChange
        'Commission button disabled when Business is Direct and no subagent associated with policy
        cmdCommission.Enabled = Not (uctPMUPolicyControl1.BusinessTypeId = 1 And Not uctPMUPolicyControl1.IsSubAgentAdded)
    End Sub

    Private Sub mnuGoToSharePoint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuGoToSharePoint.Click
        Dim sOption, sSPUrl, sDocLIB As String
        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)
        Dim sDocumentLibrary As String = ""

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return
        End If

        If sOption = "2" Then
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5085, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            m_lReturn = objCM.GetDocumentLibrary(v_lPartyCnt:=m_lPartyCnt, r_lDocumentLibrary:=sDocumentLibrary)
            sDocLIB = sDocumentLibrary
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            If String.IsNullOrEmpty(sDocLIB) Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return
                End If
            End If
        End If
        System.Diagnostics.Process.Start(sSPUrl & "\" & sDocLIB & "\" & m_sShortName.Trim() & "\Policy\" & m_sInsReference)
    End Sub
End Class
