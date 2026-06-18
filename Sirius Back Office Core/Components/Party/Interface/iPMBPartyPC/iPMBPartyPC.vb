Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form



    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
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
    'EK?
    Private m_lPartyCnt As Integer
    Private m_sShortname As String = ""
    Private m_sSurname As String = ""
    Private m_sAddressLine1 As String = ""
    Private m_sMainPostCode As String = ""
    Private m_bFromEvent As Boolean
    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAddressCount As Integer
    'eck120500
    Private m_iSourceID As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    'Private m_oGeneral As iPMBPartyPC.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private Const vbScrollBars As String = "0x80000000"
    Private Const vbDesktop As String = "0x80000001"
    Private Const vbActiveTitleBar As String = "0x80000002"
    Private Const vbInactiveTitleBar As String = "0x80000003"
    Private Const vbMenuBar As String = "0x80000004"
    Private Const vbWindowBackground As String = "H00FFFFC0"
    Private Const vbWindowFrame As String = "0x80000006"
    Private Const vbMenuText As String = "0x80000007"
    Private Const vbWindowText As String = "0x80000008"
    Private Const vbTitleBarText As String = "0x80000009"
    Private Const vbActiveBorder As String = "0x8000000A"
    Private Const vbInactiveBorder As String = "0x8000000B"
    Private Const vbApplicationWorkspace As String = "0x8000000C"
    Private Const vbHighlight As String = "0x8000000D"
    Private Const vbHighlightText As String = "0x8000000E"
    Private Const vbButtonFace As String = "0x8000000F"
    Private Const vbButtonShadow As String = "0x80000010"
    Private Const vbGrayText As String = "0x80000011"
    Private Const vbButtonText As String = "0x80000012"
    Private Const vbInactiveCaptionText As String = "0x80000013"
    Private Const vb3DHighlight As String = "0x80000014"
    Private Const vb3DDKShadow As String = "0x80000015"
    Private Const vb3DLight As String = "0x80000016"
    Private Const vbInfoText As String = "0x80000017"
    Private Const vbInfoBackground As String = "0x80000018"

    ' SwiftPartyID
    Private m_lSwiftPartyID As Integer
    'sj 21/08/2002 - start
    'Developer Guide No. 187(Guide)
    Private uctControl As New PartyPCControl.uctPartyPCControl
    'DC260106 PN27052
    Private m_bApplyWasClicked As Boolean

    Public ReadOnly Property uctPartyPCControl1() As Object
        Get

            'Developer Guide No. 187(Guide)
            Return Me.uctControl
        End Get
    End Property
    'sj 21/08/2002 - end

    Public Property SwiftPartyID() As Integer
        Get
            Return m_lSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lSwiftPartyID = Value
        End Set
    End Property

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
    Public WriteOnly Property FromEvent() As Boolean
        Set(ByVal Value As Boolean)

            m_bFromEvent = Value

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
    'eck120500
    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property
    Public Property ShortName() As String
        Get

            Return m_sShortname

        End Get
        Set(ByVal Value As String)

            m_sShortname = Value

        End Set
    End Property
    Public Property Surname() As String
        Get

            Return m_sSurname

        End Get
        Set(ByVal Value As String)

            'For some reason this wont compile if use 'name' as the property
            'name.
            m_sSurname = Value

        End Set
    End Property
    Public Property MainPostCode() As String
        Get

            Return m_sMainPostCode

        End Get
        Set(ByVal Value As String)

            m_sMainPostCode = Value

        End Set
    End Property
    Public Property AddressLine1() As String
        Get

            Return m_sAddressLine1

        End Get
        Set(ByVal Value As String)

            m_sAddressLine1 = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    'JDW for CNIC pre filling names
    Public Function SetQASNamesData(ByVal sSurname As String) As Boolean


        uctPartyPCControl1.Surname = sSurname

    End Function

    ' PRIVATE Events (Begin)

    'eck011001 Apply Button

    'eck011001
    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the Apply button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            m_lReturn = uctPartyPCControl1.OKClick

            m_sShortname = uctPartyPCControl1.IDReference


            m_lPartyCnt = uctPartyPCControl1.PartyCnt

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK

                'DC260106 PN27052
                m_bApplyWasClicked = True

                cmdApply.Visible = False


                uctPartyPCControl1.Task = gPMConstants.PMEComponentAction.PMEdit


                m_lReturn = uctPartyPCControl1.ApplyParty()


                m_lReturn = uctPartyPCControl1.GetParty()

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = SSfunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try
            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'DC260106 PN27052
            m_bApplyWasClicked = False

        Catch excep As System.Exception



            ' Error Section
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim lStatus As Integer
        Dim sInitials As String = ""
        Dim bSuccess As Boolean
        'user type from uctpartyPCControl
        Dim oQASNData As MainModule.QASNamesData = MainModule.QASNamesData.CreateInstance()
        '
        '        ' Forms load event.
        '
        Try

            If Me.PartyCnt <> 0 Then
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
                cmdExtractClientData.Visible = False
            Else
                m_iTask = gPMConstants.PMEComponentAction.PMAdd
                cmdExtractClientData.Visible = False
            End If

            If m_bFromEvent Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            cmdCancel.Enabled = True

            If m_sCallingAppName = "iPMBPartyConvert" Then
                cmdCancel.Enabled = False
            End If

            'eck 2005 Roadmap
            Me.Height = VB6.TwipsToPixelsY(7575)
            Me.Width = VB6.TwipsToPixelsX(10960)


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'sj 21/08/2002 - start
            ' Add the PartyPC user control
            'm_lReturn = iPMFunc.CreateUserControl(v_sProgID:="PartyPCControl.uctPartyPCControl", v_sObjectName:="uctPartyPCControl1", v_oForm:=Me, r_oContainer:=picHolder, r_oControl:=uctControl)
            m_lReturn = iPMFunc.CreateUserControl("PartyPCControl.uctPartyPCControl", "uctPartyPCControl1", Me, picHolder, uctControl)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create user control - PartyPCControl.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            'sj 21/08/2002 - end

            With uctPartyPCControl1

                .Task = m_iTask

                .Status = gPMConstants.PMEReturnCode.PMTrue

                .TransactionType = ""

                .EffectiveDate = DateTime.Today

                .ProcessMode = 0

                .PartyCnt = m_lPartyCnt

                .FromEvent = m_bFromEvent
                'eck120500

                .PartySourceID = m_iSourceID
                ' CTAF 280900

                .SwiftPartyID = m_lSwiftPartyID

            End With


            m_lReturn = uctPartyPCControl1.Initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            m_lReturn = uctPartyPCControl1.LoadControl
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            m_lReturn = uctPartyPCControl1.GetParty
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            lStatus = uctPartyPCControl1.Status

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'JDW added for CNIC QAS Names
            If Not IsNothing(m_sQASSurname) AndAlso m_sQASSurname.Trim() <> "" Then 'Alix Bergeret - 05/02/2003
                uctPartyPCControl1.Surname = m_sQASSurname
            End If

            ' CJB 290802 Ensure that the initials fields contains all initials!
            If Not String.IsNullOrEmpty(m_sQASForename) Then
                sInitials = m_sQASForename.Substring(0, 1) & m_sQASInitial
            End If

            If Not IsNothing(sInitials) AndAlso sInitials.Trim() <> "" Then 'Alix Bergeret - 05/02/2003

                uctPartyPCControl1.Initials = sInitials
            End If
            If Not IsNothing(m_sQASForename) AndAlso m_sQASForename.Trim() <> "" Then 'Alix Bergeret - 05/02/2003

                uctPartyPCControl1.Forename = m_sQASForename
            End If
            If Not IsNothing(m_sQASTitle) AndAlso m_sQASTitle.Trim() <> "" Then 'Alix Bergeret - 05/02/2003

                uctPartyPCControl1.Title = m_sQASTitle
            End If

            'address bit
            oQASNData = m_oQASData

            If oQASNData.Add1.Length > 0 Then
                '    If Len(m_oQASData.Add1) > 0 Then

                bSuccess = uctPartyPCControl1.AddQASAddress(v_sTitle:=oQASNData.Title, v_sForename:=oQASNData.Forename, v_sSurname:=oQASNData.Surname, v_sInitial:=oQASNData.Initial, v_sPostCode:=oQASNData.Postcode, v_sOrgName:=oQASNData.OrgName, v_sAdd1:=oQASNData.Add1, v_sAdd2:=oQASNData.Add2, v_sAdd3:=oQASNData.Add3, v_sAdd4:=oQASNData.Add4, v_bIsOrg:=oQASNData.IsOrg)

                '02082002 CMG/PB Scalability changes. This line commented out to ensure
                'compilation.  QAS will not work, must revisit this
                'bSuccess = uctPartyPCControl1.AddQASAddress(QAS:=oQASNData)
            End If

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If (UnloadMode <> vbFormCode) Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.

                m_lReturn = uctPartyPCControl1.CancelClick

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

            uctPartyPCControl1.Dispose()

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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 4
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 5
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 6
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 7
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D9 Then
                DirectCast(uctControl.Controls("TabMainTab"), TabControl).SelectedIndex = 8
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try



    End Sub


    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        If Me.WindowState <> FormWindowState.Minimized Then
            'eck 2005 Roadmap
            Me.Height = 529 'VB6.TwipsToPixelsY(7575)
            Me.Width = 736 'VB6.TwipsToPixelsX(10960)
        End If

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim nReturn As Integer

        ' Click event of the OK button.
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            m_lReturn = uctPartyPCControl1.OKClick

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_sShortname = uctPartyPCControl1.IDReference

                m_lPartyCnt = uctPartyPCControl1.PartyCnt

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Check the return value.
                'DC250106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
                nReturn = uctPartyPCControl1.AddPartyHistory()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create party history.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = uctPartyPCControl1.CancelClick

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub
End Class