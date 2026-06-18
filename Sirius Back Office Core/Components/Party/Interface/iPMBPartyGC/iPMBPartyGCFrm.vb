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
    ' Edit History:
    ' TF031298 - Menu & Toolbar activity
    ' SP050199 - TradingSinceDate and companyReg are now non-mandatory
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

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

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAddressCount As Integer

    ' {* USER DEFINED CODE (End) *}
    Private m_bFromEvent As Boolean



    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    'Data Retreived via Get Details from Party & PartyPC
    'eck120500
    Private m_iSourceID As Integer

    Private m_lPartyCnt As Integer
    Private m_iPartyTypeId As Integer
    Private m_sShortName As String = ""
    Private m_sName As String = ""

    Private m_iIsAlsoAgent As Integer
    Private m_iIsProspect As Integer

    Private m_lAgentCnt As Integer
    Private m_lConsultantCnt As Integer
    Private m_iAreaId As Integer
    Private m_sFileCode As String = ""
    Private m_iCurrencyId As Integer
    Private m_sPaymentMethodCode As String = ""
    Private m_lReminderTypeID As Integer
    Private m_lServiceLevelId As Integer
    Private m_sCreditCardCode As String = ""
    Private m_sPaymentTermCode As String = ""
    Private m_lCCJs As Integer
    Private m_sResolved As String = ""

    Private m_sCompanyReg As String = ""
    Private m_dtTradingSinceDate As Date
    Private m_lPartyBusinessID As Integer
    Private m_lLocation As Integer
    Private m_lNoOfOffices As Integer
    Private m_lNoOfEmployees As Integer
    Private m_dtFinancialYear As Date
    Private m_sTradeCode As String = ""


    'References from Party Lookups
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""
    Private m_vAssociates As Object

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_sMainPostCode As String = ""
    Private m_vAddresses As Object
    Private m_vAddressTypes As Object
    Private m_vContacts As Object
    Private m_sAddressLine1 As String = ""

    'Flag to indicate whether we need to check the headoffice id matches
    'the headoffice ref as user may change the reference directly
    Private m_bVerifyHeadOfficeCnt As Boolean
    Private m_bVerifyAgentCnt As Boolean
    Private m_bVerifyConsultantCnt As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    ' Declare an instance of the address interface.
    Private m_oAddress As Object

    ' Declare an instance of the contact interface.
    Private m_oContact As Object

    ' Declare an instance of the conviction interface.
    Private m_oConviction As Object

    ' Declare an instance of the prospecting interface.
    Private m_oProspect As Object

    ' Agent
    Private m_bChangedProspect As Boolean
    Private m_lCurrentAgent As Integer
    Private m_sCurrentAgentRef As String = ""
    Private m_sCurrentAgentName As String = ""
    Private m_bVerifyCurrentAgentCnt As Boolean

    ' RFC301001 - SwiftPartID added as part of SBO/SFU Merge
    ' SwiftPartyID
    Private m_lSwiftPartyID As Integer

    'developer guide no. 187(latest guide)
    Private uctControl As New PartyGCControl.uctPartyGCControl

    'DC260106 PN27052
    Private m_bApplyWasClicked As Boolean

    Public ReadOnly Property uctPartyGCControl1() As Object
        Get

            ' developer guide no. 187 (latest guide)
            Return uctControl
        End Get
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    ' RFC301001 - SwiftPartID added as part of SBO/SFU Merge
    ' SwiftPartyID
    Public Property SwiftPartyID() As Integer
        Get
            Return m_lSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lSwiftPartyID = Value
        End Set
    End Property

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
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property FromEvent() As Boolean
        Set(ByVal Value As Boolean)

            m_bFromEvent = Value

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
    Public Property LongName() As String
        Get

            Return m_sName

        End Get
        Set(ByVal Value As String)

            'For some reason this wont compile if use 'name' as the property
            'name.
            m_sName = Value

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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    'eck011001
    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the Apply button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            m_lReturn = uctPartyGCControl1.OKClick

            m_sShortName = uctPartyGCControl1.ShortName


            m_lPartyCnt = uctPartyGCControl1.PartyCnt

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK

                'DC260106 PN27052
                m_bApplyWasClicked = True

                cmdApply.Visible = False


                uctPartyGCControl1.Task = gPMConstants.PMEComponentAction.PMEdit


                m_lReturn = uctPartyGCControl1.ApplyParty()


                m_lReturn = uctPartyGCControl1.GetParty()

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

        uctPartyGCControl1.ShowHelpScreen(cmdHelp, ScreenHelpID)

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
        Dim oQASNData As MainModule.QASNamesData = MainModule.QASNamesData.CreateInstance()
        Dim bsuccess As Boolean
        '
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
            'eck 2005 Roadmap
            Me.Height = VB6.TwipsToPixelsY(7575)
            Me.Width = VB6.TwipsToPixelsX(10920)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = iPMFunc.CreateUserControl(v_sProgID:="uctPartyGC.uctPartyGCControl", v_sObjectName:="uctPartyGCControl1", v_oForm:=Me, r_oContainer:=picHolder, r_oControl:=uctControl)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create user control - PartyGCControl.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            With uctPartyGCControl1

                .Task = m_iTask

                .Status = gPMConstants.PMEReturnCode.PMTrue

                .TransactionType = ""

                .EffectiveDate = DateTime.Today

                .ProcessMode = 0

                .PartyCnt = m_lPartyCnt

                .FromEvent = m_bFromEvent
                'eck120500

                .PartySourceID = m_iSourceID

                ' RFC301001 - SwiftPartID added as part of SBO/SFU Merge
                ' CTAF 280900

                .SwiftPartyID = m_lSwiftPartyID

            End With


            m_lReturn = uctPartyGCControl1.Initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            m_lReturn = uctPartyGCControl1.LoadControl
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            m_lReturn = uctPartyGCControl1.GetParty
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'JDW added for CNIC QAS Names
            If Not IsNothing(m_sQASOrgName) Then
                If m_sQASOrgName.Trim() <> "" Then 'Alix Bergeret - 04/02/2003

                    uctPartyGCControl1.OrgName = m_sQASOrgName
                End If
            End If

            'address bit
            oQASNData = m_oQASData

            If oQASNData.Add1.Length > 0 Then
                'bsuccess = uctPartyGCControl1.AddQASAddress(QAS:=oQASNData)

                bsuccess = uctPartyGCControl1.AddQASAddress(v_sAdd1:=oQASNData.Add1, v_sAdd2:=oQASNData.Add2, v_sAdd3:=oQASNData.Add3, v_sAdd4:=oQASNData.Add4, v_sPostCode:=oQASNData.Postcode)
            End If


            lStatus = uctPartyGCControl1.Status

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Const vbFormCode As Integer = 0
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
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.

                m_lReturn = uctPartyGCControl1.CancelClick

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            ' Terminate the control

            uctPartyGCControl1.Dispose()

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
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 4
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 5
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 6
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 7
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D9 Then
                DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 8
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            m_lReturn = uctPartyGCControl1.OKClick

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_sShortName = uctPartyGCControl1.ShortName

                ' CTAF 220600 Return Party_Cnt too

                m_lPartyCnt = uctPartyGCControl1.PartyCnt

                ' Check the return value.
                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                m_lReturn = uctPartyGCControl1.AddPartyHistory()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create party history.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

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

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = uctPartyGCControl1.CancelClick

            ' Check the return value.
            'DC260106 PN27052 if apply then cancel still ask if custom data is to be entered
            If m_bApplyWasClicked Then
                'DC250106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            Else
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    ' PRIVATE Events (End)

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            'eck 2005 roadmap
            Me.Height = 529
            Me.Width = 736
        End If

    End Sub

    Private Sub cmdApply_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Leave
        DirectCast(uctControl.Controls("tabMainTab"), TabControl).SelectedIndex = 0
    End Sub

    Private Sub cmdExtractClientData_Click(sender As Object, e As EventArgs) Handles cmdExtractClientData.Click

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nPasswordAction As PMEReturnCode = PMEReturnCode.PMCancel
        Dim sPassword As String = String.Empty
        Dim sFilePath As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim oPasswordForm As frmPassword
        Dim oGis As bGIS.Application = Nothing
        Try
            'Get the password
            oPasswordForm = New frmPassword()
            iPMFunc.CenterForm(oPasswordForm)

            oPasswordForm.ShowDialog()
            sPassword = oPasswordForm.txtPassword.Text.Trim()
            nPasswordAction = oPasswordForm.Status

            oPasswordForm.Close()
            oPasswordForm = Nothing

            If nPasswordAction <> gPMConstants.PMEReturnCode.PMOK Then
                nResult = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            oGis = New bGIS.Application
            nResult = g_oObjectManager.GetInstance(oObject:=oGis, sClassName:="bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExtractClientData_Click")
                MessageOnExtractFailure()
            End If

            Using sfdExtractClientData As New SaveFileDialog()
                sfdExtractClientData.Title = "Save As"
                sfdExtractClientData.AddExtension = True
                sfdExtractClientData.OverwritePrompt = True
                sfdExtractClientData.CheckPathExists = True
                sfdExtractClientData.ValidateNames = True
                sfdExtractClientData.FileName = "ClientDataExtract"
                sfdExtractClientData.Filter = "Zip Files|*.zip"
                sfdExtractClientData.DefaultExt = ".zip"
                If sfdExtractClientData.ShowDialog() = DialogResult.OK Then
                    sFilePath = sfdExtractClientData.FileName
                Else
                    Exit Sub
                End If

                Dim abExtractedClientData As Byte() = Nothing
                nResult = oGis.ExtractData(PartyCnt(), sPassword, abExtractedClientData)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS ExtractData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExtractClientData_Click")
                    MessageOnExtractFailure()
                End If
                If abExtractedClientData Is Nothing OrElse abExtractedClientData.Length = 0 Then
                    MessageOnExtractFailure()
                Else
                    File.WriteAllBytes(sFilePath, abExtractedClientData)
                End If

            End Using
        Catch ex As Exception
            MessageOnExtractFailure(ex.Message)
        Finally
            If oGis IsNot Nothing Then
                oGis.Dispose()
                oGis = Nothing
            End If
        End Try
    End Sub

    Private Sub MessageOnExtractFailure(Optional ByVal sAdditionalMsg As String = "")
        MessageBox.Show(String.Format("There was an error generating data file. Please try again.{0}", Environment.NewLine & sAdditionalMsg),
                        "Extract File", MessageBoxButtons.OK)
    End Sub
End Class