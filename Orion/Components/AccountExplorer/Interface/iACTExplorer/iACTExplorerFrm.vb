Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23-07-1997
    '
    ' Description: Main interface.
    '
    ' Edit History: KN (CMG) 10/02/03 When Multi tree switched off, accounts
    '               were not being displayed for branches other than where
    '               company_id = 1
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    Const ACImageClsdFold As Integer = 1
    Const ACImageOpenFold As Integer = 2
    Const ACImageClsdLdgr As Integer = 3
    Const ACImageOpenLdgr As Integer = 4
    Const ACImageAccTyp01 As Integer = 5
    Const ACImageClsdFoldMove As Integer = 6 ' Use this for DragDrop Move
    Const ACImageClsdFoldCopy As Integer = 7 ' Use this for DragDrop Copy
    Const ACImageFragments As Integer = 8
    Const ACImageWorld As Integer = 9

    Dim m_bInExploreNode As Boolean ' Flag to prevent unwanted recursion
    Dim m_bShiftDown As Boolean ' Flag to record shift key state
    Dim m_bCtrlDown As Boolean ' Flag to record ctrl key state
    Dim m_bOverExplorer As Boolean ' Flag to record drag drop transition

    ' Constants for Verb / Action arrays
    Const ACVerbFirst As Integer = 0
    Const ACVerbCopy As Integer = 0
    Const ACVerbMove As Integer = 1
    Const ACVerbDelete As Integer = 2
    Const ACVerbRename As Integer = 3
    Const ACVerbMap As Integer = 4
    Const ACVerbUnmap As Integer = 5
    Const ACVerbLast As Integer = 5
    Dim sVerb As Array = Array.CreateInstance(GetType(String), New Integer() {ACVerbLast - ACVerbFirst + 1}, New Integer() {ACVerbFirst})
    Dim sAction As Array = Array.CreateInstance(GetType(String), New Integer() {ACVerbLast - ACVerbFirst + 1}, New Integer() {ACVerbFirst})

    Dim sActionMsg As Array = Array.CreateInstance(GetType(String), New Integer() {ACTExplorerConst.ACExpErrLast - ACTExplorerConst.ACExpErrFirst + 1}, New Integer() {ACTExplorerConst.ACExpErrFirst})

    ' Constants for NodeType array
    Const ACNodeFirst As Integer = 0
    Const ACNodeAccount As Integer = 0
    Const ACNodeMap As Integer = 1
    Const ACNodeFolder As Integer = 2
    Const ACNodeLast As Integer = 2
    Dim sNodeType As Array = Array.CreateInstance(GetType(String), New Integer() {ACNodeLast - ACNodeFirst + 1}, New Integer() {ACNodeFirst})

    ' *** BEGIN Inserted By ResGen ***
    'developer guide no. 50
    Dim frmCreateAccount As frmCreateAccount
    ' Form Constants for Captions

    Const ACInterfaceCaption As Integer = 100

    ' Button Constants for Captions

    Const ACNavigateCaption As Integer = 200
    Const ACHelpCaption As Integer = 201
    Const ACCancelCaption As Integer = 202
    Const ACOKCaption As Integer = 203
    Const ACOKTip As Integer = 204

    ' Message Constants for Captions

    Dim m_Split As Integer


    ' *** END Inserted By ResGen ***
    'developer guide no. 50
    Dim frmMapFolder As frmMapFolder
    ' PRIVATE Data Members (Begin)


    Private m_oBusiness As bACTExplorer.Form

    Private m_oAccount As iACTAccount.Interface_Renamed

    Private m_lAccountID As Integer
    Private m_iLedgerId As Integer
    Private m_iLedgerTypeID As Integer
    Private m_sShortCode As String = ""
    Private m_sFullKey As String = ""
    Private m_sAccountName As String = ""
    Private m_sStartKey As String = ""
    Private m_lMappingID As Integer
    Private m_lReadOnly As Integer
    'eck080500
    Private m_iCompanyID As Integer
    Private m_sCompanyCode As String = ""
    Private m_sCompanyName As String = ""

    'DD 15/07/2002: True if new product option is enabled
    Private m_bEnhancedSecurity As Boolean

    'PWF 08/09/2002: True if multi-tree option is enabled
    Private m_bMultiTree As Boolean

    'PWF 08/09/2002: True if accountsegregation option is enabled
    Private m_bMBCoreAccounts As Boolean

    'DD 20/09/2002: Last Explorer Key (used in Drag-Drop)
    Private m_sLastExplorerKey As String = ""

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    'developer guide no. 108
    Private m_oInterface As Interface_Renamed

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    'sj 21/11/2002 - start
    'PS700
    Private m_iMouseButtonState As MouseButtonConstants
    'sj 21/11/2002 - end

    ' Stores the return value for function calls.
    Private m_lReturn As Integer

    ' Authority Level
    Private m_lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel

    ' Forms
    Private m_frmSecurity As frmSecurity

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
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

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the task flag.
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
    'developer guide no. 108
    Public WriteOnly Property ControllingInterface() As Interface_Renamed
        'developer guide no. 108
        Set(ByVal Value As Interface_Renamed)

            ' Set the controlling interface for this form
            m_oInterface = Value

            ' Set our private reference to the business object
            m_oBusiness = m_oInterface.m_oBusiness

        End Set
    End Property

    Public ReadOnly Property AccountID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAccountID

        End Get
    End Property

    Public Property LedgerID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_iLedgerId

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_iLedgerId = Value

        End Set
    End Property

    Public Property LedgerTypeID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_iLedgerTypeID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_iLedgerTypeID = Value

        End Set
    End Property

    Public Property ShortCode() As String
        Get

            ' Return the objects parameter value.
            Return m_sShortCode

        End Get
        Set(ByVal Value As String)

            ' Set the object parameter value.
            m_sShortCode = Value

        End Set
    End Property


    Public Property StartKey() As String
        Get
            Return m_sStartKey
        End Get
        Set(ByVal Value As String)
            m_sStartKey = Value
        End Set
    End Property

    Public Property FullKey() As String
        Get

            ' Return the objects parameter value.
            Return m_sFullKey

        End Get
        Set(ByVal Value As String)

            ' Set the object parameter value.
            m_sFullKey = Value

        End Set
    End Property

    Public ReadOnly Property AccountName() As String
        Get

            ' Return the objects parameter value.
            Return m_sAccountName

        End Get
    End Property


    Public Property MappingID() As Integer
        Get

            Return m_lMappingID

        End Get
        Set(ByVal Value As Integer)

            m_lMappingID = Value

        End Set
    End Property

    'eck080500


    Public Property ReadOnly_Renamed() As Integer
        Get

            Return m_lReadOnly

        End Get
        Set(ByVal Value As Integer)

            m_lReadOnly = Value

        End Set
    End Property
    'eck080500
    Public Property CompanyID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try


            ' Update the interface details.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToProperties
    '
    ' Description: Updates the property member from the interface.
    '
    ' ***************************************************************** '
    Public Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Dim vNodeData As Object
        Dim lRow, lNodeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (lvwExplorer.FocusedItem Is Nothing) Then


                m_oBusiness.GetNode(GetNodeIDFromKey(lvwExplorer.FocusedItem.Name), vNodeData)

                ' If a node was returned
                If Information.IsArray(vNodeData) Then

                    ' Only one record
                    lRow = 0

                    ' Determine the type of property dialog to use

                    ' This is an account if AccountID defined

                    If CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRow)) > "" Then

                        lNodeId = CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow))

                        m_oBusiness.GetFullPath(lNodeId:=lNodeId, vFullPath:=m_sFullKey)

                        m_sShortCode = CStr(vNodeData(ACTExplorerConst.ACGetNodeShortCode, lRow)).Trim()

                        m_lAccountID = CInt(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRow))

                        m_sAccountName = CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountName, lRow))
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sStatText As String = ""

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

            'DD 12/08/2002: Make Security item optional
            mnuSeperator3.Available = m_bEnhancedSecurity And (m_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin)
            mnuExpTSecurity.Available = m_bEnhancedSecurity And (m_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin)

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            RefreshExplorerTree()

            'Select the root node
            tvwExplorer.SelectedNode = tvwExplorer.Nodes.Item(0)
            tvwExplorer_AfterSelect(tvwExplorer, New TreeViewEventArgs(tvwExplorer.SelectedNode))
            tvwExplorer_NodeMouseClick(tvwExplorer, New TreeNodeMouseClickEventArgs(tvwExplorer.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0))

            ' Display the user authority
            sStatText = "Authority: "

            Select Case m_lPMAuthorityLevel
                Case gPMConstants.PMEAuthorityLevel.pmeALSupervisor
                    sStatText = sStatText & "Supervisor"
                Case gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
                    sStatText = sStatText & "Sys Admin"
                Case gPMConstants.PMEAuthorityLevel.pmeALUser
                    sStatText = sStatText & "User"
            End Select

            sbarMain.Items.Item(0).Text = sStatText

            ' Full row select on the list
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwExplorer.Handle.ToInt32(), v_vShowRowSelect:=True)

            Return result

        Catch excep As System.Exception



            ' Error Section.
            '    Resume
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Display all language specific captions


            'Developer Guide No 76
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub Form_Initialize_Renamed()

        Dim vValue As String = ""

        ' Forms initialise event.

        Try

            ' Add the Hook
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the object parameters to default values
            m_iLedgerId = -1 ' Undefined
            m_iLedgerTypeID = -1
            m_sShortCode = ""
            m_sFullKey = ""

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Initialize language arrays
            InitializeVerbs()
            InitializeMsgs()
            InitializeNodeTypes()

            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, sClassName:="iACTAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oAccount = temp_m_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'MKW090603 PN4574 Only Select Company if no company stored START
            If g_iSourceID = 0 Then
                'PWF 09/10/2002: Moved from all over the place to here
                m_lReturn = GetCompany()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_iCompanyID = 0
                End If
            Else
                m_iCompanyID = g_iSourceID
            End If
            'MKW090603 PN4574 Only Select Company if no company stored END

            'DD 15/07/2002: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity, m_iCompanyID, vValue)
            m_bEnhancedSecurity = (vValue = "1")

            'PWF 08/10/2002: Get product option settings
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iCompanyID, vValue)
            m_bMultiTree = (vValue = "1")

            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiBranchCoreAccounts, m_iCompanyID, vValue)
            m_bMBCoreAccounts = (vValue = "1")

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Remove the Hook
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            AddHandler lvwExplorer.ItemDrag, AddressOf lvwExplorer_ItemDrag
            AddHandler tvwExplorer.DragEnter, AddressOf tvwExplorer_DragEnter
            AddHandler tvwExplorer.DragDrop, AddressOf tvwExplorer_DragDrop

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section

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

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oInterface.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
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

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        ' Forms unload event.

        Try

            'Inform the Interface
            m_lReturn = m_oInterface.OnForm_Unload()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Clear the reference to the interface
            m_oInterface = Nothing
            m_oBusiness = Nothing

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        m_bShiftDown = (Shift And ShiftConstants.ShiftMask) > 0
        m_bCtrlDown = (Shift And ShiftConstants.CtrlMask) > 0

        If eventArgs.KeyCode = Keys.Escape Then
            ' Cancel any drag operations
            If Convert.ToString(tvwExplorer.Tag) <> "" Then
                EndDrag()

                'developer guide no. 68 (No Solution)
                'tvwExplorer.Drag(System.Windows.Forms.DialogResult.Cancel)
            End If
            KeyCode = 0
        Else
            ' If doing drag drop...
            If Convert.ToString(tvwExplorer.Tag) <> "" Then
                ' ...respond to change of Shift state
                ShiftDragIcon()
            End If
        End If

    End Sub

    Private Sub frmInterface_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        m_bShiftDown = (Shift And ShiftConstants.ShiftMask) > 0
        m_bCtrlDown = (Shift And ShiftConstants.CtrlMask) > 0

        ' If doing drag drop...
        If Convert.ToString(tvwExplorer.Tag) <> "" Then
            ' ...respond to change of Shift state
            ShiftDragIcon()
        End If

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Delegate function
        ResizeControls()
    End Sub

    Private Sub InitializeVerbs()
        ' Replace by resource strings
        sVerb(ACVerbCopy) = "Copy"
        sAction(ACVerbCopy) = "Copying"
        sVerb(ACVerbMove) = "Move"
        sAction(ACVerbMove) = "Moving"
        sVerb(ACVerbDelete) = "Delete"
        sAction(ACVerbDelete) = "Deleting"
        sVerb(ACVerbRename) = "Rename"
        sAction(ACVerbRename) = "Renaming"
        sVerb(ACVerbMap) = "Map"
        sAction(ACVerbMap) = "Mapping"
        sVerb(ACVerbUnmap) = "Unmap"
        sAction(ACVerbUnmap) = "Unmapping"
    End Sub

    Private Sub InitializeMsgs()

        sActionMsg(ACTExplorerConst.ACExpErrOK) = "No error."
        sActionMsg(ACTExplorerConst.ACExpErrGeneralFailure) = "General failure."
        sActionMsg(ACTExplorerConst.ACExpErrDuplicateElement) = "An element with the name you specified already exists. " & _
                            "Specify a different element name."
        sActionMsg(ACTExplorerConst.ACExpErrMultipleElementInPath) = "This element is already in the destination path."
        sActionMsg(ACTExplorerConst.ACExpErrElementInStructure) = "This element is referenced in the structure tree."
        sActionMsg(ACTExplorerConst.ACExpErrElementInElement) = "This element is referenced in the fragments tree."
        sActionMsg(ACTExplorerConst.ACExpErrNodeHasAccounts) = "This folder contains accounts or subfolders containing accounts."
    End Sub

    Private Sub InitializeNodeTypes()
        sNodeType(ACNodeAccount) = "Account"
        sNodeType(ACNodeMap) = "Mapping"
        sNodeType(ACNodeFolder) = "Folder"
    End Sub


    Private Sub SetStartNode(ByRef lFromNodeId As Integer, ByRef lFromElementId As Integer, ByRef bNoSiblings As Boolean)

        Dim lParentNodeId As Integer

        ' A start point was specified
        If m_sStartKey > "" Then

            m_oBusiness.GetNodeIdFromFullPath(m_sStartKey, lFromNodeId, r_vElementId:=lFromElementId, r_vParentNodeId:=lParentNodeId)

            ' But we actually want to start from it's parent
            ' and not show siblings
            If lParentNodeId <> 0 Then
                bNoSiblings = False
                lFromNodeId = lParentNodeId
            End If
        ElseIf m_lMappingID > 0 Then

            lFromNodeId = m_oBusiness.GetNodeFromMappingID(m_lMappingID, r_vParentNodeId:=lParentNodeId)

            ' But we actually want to start from it's parent
            ' and not show siblings
            If lParentNodeId <> 0 Then
                bNoSiblings = False
                lFromNodeId = lParentNodeId
            End If
        Else
            lFromNodeId = 0
            lFromElementId = 0
            bNoSiblings = False
        End If

    End Sub
    Private Sub RefreshExplorerTree()

        Dim lFromNodeId, lFromElementId As Integer
        Dim bNoSiblings As Boolean

        Dim lElementID As Integer
        Dim sNewText As String = ""

        Dim vNodeData As Object

        tvwExplorer.Nodes.Clear()

        SetStartNode(lFromNodeId, lFromElementId, bNoSiblings)

        'KN (CMG) 10/02/03 Start
        ' If root node does not exist...
        '    m_oBusiness.GetChildrenOfNode lFromNodeId, 1, vNodeData, lCompanyID:=CLng(g_iCompanyId)

        ' If root node does not exist...
        If m_bMultiTree Then

            m_oBusiness.GetChildrenOfNode(lFromNodeId, 1, vNodeData, lCompanyID:=g_iCompanyId)
        Else

            m_oBusiness.GetChildrenOfNode(lFromNodeId, 1, vNodeData, lCompanyID:=CInt("1"))
        End If
        'KN (CMG) 10/02/03 End

        If Not Information.IsArray(vNodeData) Then

            ' ...create general element and root node
            sNewText = "General"

            m_oBusiness.BeginTrans()

            lElementID = m_oBusiness.InsertElement(sElementName:=sNewText)


            If (lElementID > 0) And m_oBusiness.InsertNode(lParentNodeId:=0, lElementID:=lElementID) Then
                ' Commit the update

                m_oBusiness.CommitTrans()
            Else

                m_oBusiness.RollbackTrans()
            End If

        End If

        ' Parse the tree
        ExploreNode(sFromKey:=MakeNodeKey(lFromNodeId, lFromElementId), iLevel:=0, iExpandLevel:=2, iMaxLevel:=2, vNoSiblings:=bNoSiblings)

    End Sub

    Private Sub ResizeControls()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ResizeControls
        ' PURPOSE: Resizes all the controls on the form due to a form/splitter resize
        ' AUTHOR: Danny Davis (Peter Finney)
        ' DATE: 19/09/2002, 14:57 (07/09/2002)
        ' RETURNS: PMTrue for success
        ' CHANGES: Rewritten for when original element tree was removed.
        ' CHANGES: Re-rewritten for really simple realtime resize method.
        '          See picSplit.MouseMove for more info
        ' ---------------------------------------------------------------------------
        Dim lAreaTop, lAreaLeft, lAreaHeight As Integer


        Try

            ' Calculate areas first
            lAreaTop = CInt(VB6.PixelsToTwipsY(tbarExplorer.Top) + VB6.PixelsToTwipsY(tbarExplorer.Height))
            lAreaLeft = CInt(VB6.PixelsToTwipsX(picSplit.Left) + VB6.PixelsToTwipsX(picSplit.Width))
            lAreaHeight = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - lAreaTop - VB6.PixelsToTwipsY(sbarMain.Height) - VB6.PixelsToTwipsY(lblExplorerTree.Height))

            ' Resize the treeview portion
            lblExplorerTree.SetBounds(0, VB6.TwipsToPixelsY(lAreaTop), picSplit.Left, 0, BoundsSpecified.X Or BoundsSpecified.Y Or BoundsSpecified.Width)
            tvwExplorer.SetBounds(0, VB6.TwipsToPixelsY(lAreaTop) + lblExplorerTree.Height, picSplit.Left, VB6.TwipsToPixelsY(lAreaHeight))

            ' Resize the listview portion
            lblExplorerList.SetBounds(VB6.TwipsToPixelsX(lAreaLeft), VB6.TwipsToPixelsY(lAreaTop), Me.ClientRectangle.Width - VB6.TwipsToPixelsX(lAreaLeft), 0, BoundsSpecified.X Or BoundsSpecified.Y Or BoundsSpecified.Width)
            lvwExplorer.SetBounds(VB6.TwipsToPixelsX(lAreaLeft), VB6.TwipsToPixelsY(lAreaTop) + lblExplorerTree.Height, Me.ClientRectangle.Width - VB6.TwipsToPixelsX(lAreaLeft), VB6.TwipsToPixelsY(lAreaHeight))

            ' Resize the splitter but just it's height
            picSplit.SetBounds(picSplit.Left, VB6.TwipsToPixelsY(lAreaTop), picSplit.Width, Me.ClientRectangle.Height - VB6.TwipsToPixelsY(lAreaTop) - sbarMain.Height)



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case 5
                    ' Just a resize error, controls have probably shrunk too much, ignore

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeControls", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub MoveToNode(ByRef sKey As String)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: MoveToNode
        ' PURPOSE: Moves to the passed node on the tree
        ' AUTHOR: Danny Davis
        ' DATE: 20/09/2002, 14:38
        ' CHANGES:
        ' ---------------------------------------------------------------------------




        Try

            Dim nodX As TreeNode = tvwExplorer.Nodes.Item(sKey)
            If Not (nodX Is Nothing) Then
                RefreshExplorerList(nodX)

                'developer guide no. 35
                'nodX.Selected = True
                tvwExplorer.SelectedNode = nodX
                tvwExplorer.Focus()
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


            Select Case Information.Err().Number
                Case Else
                    MessageBox.Show("iACTExplorer.frmInterface.MoveToNode" & _
                                    "Version: " & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) & _
                                    " At line: " & CStr(Information.Erl()) & "|" & Information.Err().Source & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    Information.Err().Number & ":" & Information.Err().Description, Application.ProductName)

                    GoTo Finally_Renamed
            End Select

Finally_Renamed:
        Catch exc As System.Exception

        End Try



    End Sub


    'Developer Guide No 101
    Private Function MakeNodeKey(ByRef lNodeId As Integer, ByRef lElementID As Integer, Optional ByRef vMapID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing) As String


        Dim sKey As String = "N=" & lNodeId & ", E=" & CStr(lElementID)


        If Not Information.IsNothing(vMapID) Then
            'Developer Guide No 149
            sKey = sKey & ", M=" & Convert.ToString(vMapID)
        End If


        If Not Information.IsNothing(vAccountId) Then

            'Developer Guide No 149
            sKey = sKey & ", A=" & Convert.ToString(vAccountId)
        End If

        Return sKey

    End Function

    Private Function GetNodeIDFromKey(ByVal sKey As String) As Integer
        Return GetValueFromKey(sKey, "N")
    End Function

    Private Function GetElementIDFromKey(ByVal sKey As String) As Integer
        Return GetValueFromKey(sKey, "E")
    End Function

    Private Function GetMapIDFromKey(ByVal sKey As String) As Integer
        Return GetValueFromKey(sKey, "M")
    End Function

    Private Function GetAccountIDFromKey(ByVal sKey As String) As Integer
        Return GetValueFromKey(sKey, "A")
    End Function

    Private Function GetValueFromKey(ByVal sKey As String, ByVal sValue As String) As Integer

        Dim result As Integer = 0
        Dim nEnd As Integer

        Dim nPos As Integer = (sKey.IndexOf(sValue & "=") + 1) + 2

        If nPos Then
            nEnd = Strings.InStr(nPos, sKey, ",")
            If nEnd = 0 Then
                result = CInt(Conversion.Val(sKey.Substring(nPos - 1)))
            Else
                result = CInt(Conversion.Val(sKey.Substring(nPos - 1, Math.Min(sKey.Length, nEnd - nPos))))
            End If
        End If

        Return result
    End Function


    'EK 220300
    Private Function GetFolderLevel(ByRef sPath As String) As Integer

        Dim lCount As Integer

        Dim nPos As Integer = 1

        Do While nPos
            nPos = Strings.InStr(nPos, sPath, "\")
            If nPos Then
                nPos += 1
            End If
            lCount += 1
        Loop

        Return lCount
    End Function

    'EK220300
    'eck080500
    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    ' ***************************************************************** '
    Private Function GetCompany() As Integer
        Dim result As Integer = 0


        Dim m_oBranch As iPMBBranch.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' The first time we only get the company ID

            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID, vSourceName:=m_sCompanyName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Now we can call it again and get the description

            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID, vSourceName:=m_sCompanyName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBranch.Dispose()
            m_oBranch = Nothing
            ' Trim the company name
            m_sCompanyName = m_sCompanyName.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck080500

    Private Function ElementDeletable(ByRef lElementID As Integer) As Integer
        Dim iIsDeletable As Integer

        m_oBusiness.GetElementExtras(lElementId:=lElementID, vTotallingId:=Nothing, vDescription:="", vReportMapId:=Nothing, vAccountMapID:=Nothing, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=Nothing)

        Return iIsDeletable
    End Function

    Private Function MakeMapName(ByRef sMapName As String, ByRef sElementName As String) As String

        Return sMapName

    End Function

    'Developer Guide No 101
    Private Sub ExploreNode(ByRef sFromKey As String, ByRef iLevel As Integer, ByRef iExpandLevel As Integer, ByRef iMaxLevel As Integer, Optional ByRef vNoSiblings As Object = Nothing)

        ' Explore any children of the given node.
        ' iLevel is the current level of iteration.
        ' If iLevel < iExpandLevel then the given node will be expanded.
        ' If iLevel < iMaxLevel then any children will be explored also.

        Dim vNodeData(,) As Object


        Dim nodX As New TreeNode
        Dim sKey, sText As String
        'testit
        Dim sCode As String = ""

        Static lRootCount As Integer

        Dim lFromNodeId As Integer = GetNodeIDFromKey(sFromKey)

        ' Get a two dimensional result array whose columns are identified
        ' by constants with prefix ACGetNode.
        If iExpandLevel = 666 Then
            sCode = "z%"

            m_oBusiness.GetChildrenOfNode(lFromNodeId, 1000, vNodeData, sCode)
        Else
            'KN (CMG) 10/02/03 Start
            'm_oBusiness.GetChildrenOfNode lFromNodeId, 1000, vNodeData, lCompanyID:=g_iCompanyId
            If m_bMultiTree Then

                m_oBusiness.GetChildrenOfNode(lFromNodeId, 1, vNodeData, lCompanyID:=g_iCompanyId)
            Else

                m_oBusiness.GetChildrenOfNode(lFromNodeId, 1, vNodeData, lCompanyID:=CInt("1"))
            End If
            'KN (CMG) 10/02/03 End
        End If
        If Information.IsArray(vNodeData) Then

            ' Populate the list view from the result array

            For lRow As Integer = vNodeData.GetLowerBound(1) To vNodeData.GetUpperBound(1)

                ' Skip if AccountID defined

                If CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRow)) = "" Then
                    ' If MapID defined...

                    If CStr(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)) > "" Then


                        sKey = MakeNodeKey(CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow)), CInt(Conversion.Val(CStr(vNodeData(ACTExplorerConst.ACGetNodeElementID, lRow)))), CInt(Conversion.Val(CStr(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)))))


                        sText = MakeMapName(CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, lRow)), CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRow)))
                    Else


                        sKey = MakeNodeKey(CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow)), CInt(Conversion.Val(CStr(vNodeData(ACTExplorerConst.ACGetNodeElementID, lRow)))))

                        sText = CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRow))
                    End If

                    If iLevel = 0 Then
                        lRootCount += 1

                        ' Do we want root level siblings ?
                        If lRootCount > 1 Then

                            If Not Information.IsNothing(vNoSiblings) Then
                                If vNoSiblings Then
                                    Exit Sub
                                End If
                            End If
                        End If

                        ' Root entry
                        nodX = tvwExplorer.Nodes.Add(sKey, sText, ACImageWorld - 1, ACImageWorld - 1)

                    Else
                        ' Child entry
                        If Not tvwExplorer.Nodes.Find(sFromKey, True)(0).Tag Then
                            ' If MapID defined...

                            If CStr(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)) > "" Then

                                'Developer Guide No. 162
                                nodX = tvwExplorer.Nodes.Find(sFromKey, True)(0).Nodes.Add(sKey, sText, ACImageClsdLdgr - 1, ACImageClsdLdgr - 1)

                                'developer guide no. 16 (No Solution)
                                'nodX.ExpandedImage = ACImageOpenLdgr

                            Else
                                nodX = tvwExplorer.Nodes.Find(sFromKey, True)(0).Nodes.Add(sKey, sText, ACImageClsdFold - 1, ACImageClsdFold - 1)
                            End If
                        End If

                        If iLevel < iExpandLevel Then
                            ' Expand parent node
                            m_bInExploreNode = True ' Set flag
                            tvwExplorer.Nodes.Item(sFromKey).Expand()
                            m_bInExploreNode = False ' Reset flag
                        End If
                    End If

                    If iLevel < iMaxLevel Then
                        ' Explore further recursively
                        iLevel += 1
                        ExploreNode(sKey, iLevel, iExpandLevel, iMaxLevel)
                        iLevel -= 1
                    End If
                End If
            Next lRow
        End If

        If iLevel > 0 Then
            ' Flag that parent node has been explored
            tvwExplorer.Nodes.Find(sFromKey, True)(0).Tag = True
        End If


    End Sub


    Private Sub lvwExplorer_AfterLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwExplorer.AfterLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit
        Dim NewString As String = eventArgs.Label

        Dim vErrorNum As Byte
        Dim sNewText As String = ""
        Dim Index, Count As Integer
        Dim lElementID As Integer
        Dim vNodeData As Object

        ' NewString is the new name of the SelectedItem Element
        ' Which must be unique in relation to its siblings

        With tvwExplorer.SelectedNode


            If Not m_oBusiness.UpdateElement(vNodeId:=GetNodeIDFromKey(.Name), vElementName:=NewString, vErrorNum:=vErrorNum) Then

                ActionErrorMsgBox(ACVerbRename, .Text, sActionMsg(vErrorNum))
                Cancel = True
                'eck180800 Update the Mapping as well
            Else


                m_lReturn = m_oBusiness.UpdateMapping(lMapID:=GetMapIDFromKey(.Name), vMapName:=NewString)

                ' CTAF 251000 - It doesnt matter if the mapping isnt updated
                ' because the folder might not be mapped

                sNewText = NewString
                ' Iterate through all current nodes
                ' and update the labels of any other nodes
                ' defined by the same ElementID
                lElementID = GetElementIDFromKey(.Name)
                Index = .Index
                Count = tvwExplorer.Nodes.Count

                For i As Integer = 1 To Count

                    With tvwExplorer.Nodes.Item(i - 1)

                        If lElementID = GetElementIDFromKey(.Name) Then

                            ' If node is mapped...
                            If GetMapIDFromKey(.Name) Then

                                ' Change text of node to include map name

                                m_oBusiness.GetNode(GetNodeIDFromKey(.Name), vNodeData)


                                .Text = MakeMapName(CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, 0)), CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, 0)))

                                ' If SelectedItem ...
                                If Index = .Index Then

                                    ' Return map name in NewString
                                    NewString = .Text

                                End If

                            Else

                                .Text = sNewText

                            End If

                        End If

                    End With

                Next i

                'End If
                'eck180800
            End If
            '
        End With

        'TODO:MILAN
        lvwExplorer.LabelEdit = False
    End Sub

    Private Sub lvwExplorer_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwExplorer.BeforeLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit
        'Avoid runtime errors

        Try

            If tvwExplorer.Nodes.Item(lvwExplorer.FocusedItem.Name) Is Nothing Then
                'Only folders can be renamed
                Cancel = True
            End If

        Catch exc As System.Exception
        End Try
    End Sub

    Private Sub lvwExplorer_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwExplorer.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwExplorer.Columns(eventArgs.Column)
        ' CF 171299 - Added sorting to columns

        ListViewHelper.SetSortedProperty(lvwExplorer, False)

        If ListViewHelper.GetSortOrderProperty(lvwExplorer) = SortOrder.Ascending Then
            ListViewHelper.SetSortOrderProperty(lvwExplorer, SortOrder.Descending)
        Else
            ListViewHelper.SetSortOrderProperty(lvwExplorer, SortOrder.Ascending)
        End If

        ListViewHelper.SetSortKeyProperty(lvwExplorer, ColumnHeader.Index + 1 - 1)

        ListViewHelper.SetSortedProperty(lvwExplorer, True)
    End Sub

    Private Sub lvwExplorer_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwExplorer.DoubleClick

        Dim lAccountId As Integer

        'JK041298-ensure that an item is selected
        If lvwExplorer.FocusedItem.Selected Then
            'cmdOK_Click
            lAccountId = GetAccountIDFromKey(sKey:=lvwExplorer.FocusedItem.Name)

            ' Expand the folder
            If lAccountId = 0 Then
                ExploreNode(lvwExplorer.FocusedItem.Name, 1, 2, 2)
                MoveToNode(lvwExplorer.FocusedItem.Name)
            End If

        Else
            Exit Sub
        End If

    End Sub

    Private Sub lvwExplorer_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwExplorer.MouseMove
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Developer Guide No. 261
        If eventArgs.Button = MouseButtons.Left Then
            If Not (lvwExplorer.SelectedItems.Count <= 0) Then ' Signal a Drag operation.
                lvwExplorer.SelectedItems(0).Selected = False

                'developer guide no. 70 (No Solution)
                'lvwExplorer.DragIcon = imgExplorer.Images.Item(ACImageClsdFoldMove - 1)


                'developer guide no. 68 (No Solution)
                'lvwExplorer.Drag(vbBeginDrag) '  Drag operation.
                m_dragSource = lvwExplorer
                lvwExplorer.DoDragDrop(lvwExplorer.FocusedItem, DragDropEffects.Move)
                m_sLastExplorerKey = tvwExplorer.SelectedNode.Name
            End If
        End If
    End Sub

    Private Sub lvwExplorer_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwExplorer.MouseUp
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim itmX As ListViewItem

        If m_lReadOnly Then
            Exit Sub
        End If

        'Developer Guide No. 261
        If eventArgs.Button = MouseButtons.Right Then
            'Ensure that there is a selected item
            If Not (lvwExplorer.GetItemAt(x, y) Is Nothing) Then
                ' Display the context menu on right mouse click.
                DisplayListViewMenu(x, y)
            End If
            'Developer Guide No. 261
        ElseIf eventArgs.Button = MouseButtons.Left Then

            itmX = lvwExplorer.GetItemAt(x, y)

            If itmX Is Nothing Then
                lvwExplorer.Tag = ""
            Else
                lvwExplorer.Tag = itmX.Name
                itmX.Selected = True
            End If

            itmX = Nothing
        End If
    End Sub

    Public Sub mnuExpExplorerHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpExplorerHelp.Click

        ' Fire up the help screen
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(Me, ScreenHelpID)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to return Help", Application.ProductName)
            Exit Sub
        End If

    End Sub

    Public Sub mnuExpGroupForGlExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpGroupForGlExport.Click

        Dim iChecked As Integer

        If mnuExpGroupForGlExport.Checked Then
            mnuExpGroupForGlExport.Checked = False
            iChecked = 0
        Else
            mnuExpGroupForGlExport.Checked = True
            iChecked = 1
        End If

        UpdateGroupForGlExport(v_iChecked:=iChecked)
    End Sub

    Public Sub mnuExpHelpTopics_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpHelpTopics.Click

        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(Me, 0)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to return Help", Application.ProductName)
            Exit Sub
        End If

    End Sub

    Public Sub mnuExplorerExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExplorerExit.Click
        Me.Hide()
    End Sub

    Public Sub mnuListVDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuListVDelete.Click

        'JK041298 - Check if item is selected
        If lvwExplorer.FocusedItem.Selected Then
            ListViewDelete()
        Else
            Exit Sub
        End If

    End Sub

    Public Sub mnuListVExtras_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuListVExtras.Click
        AccountExtras()
    End Sub

    Public Sub mnuListVProperties_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuListVProperties.Click

        'JK041298 - Check if item is selected
        If lvwExplorer.FocusedItem.Selected Then
            ListViewProperties()
        Else
            Exit Sub
        End If

    End Sub

    Private Sub picSplit_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles picSplit.MouseMove
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim lPosition As Integer

        ' ---------------------------------------------------------------------------
        ' Very Simple Splitter Control:
        ' - If the mouse button is down, move the splitter by the current movement of
        '   the mouse (checking for appropriate left/right boundaries).
        ' - Fire the ResizeControls event which will resize all controls around the
        '   position of the splitter control
        ' - Done in 2 methods!
        ' ---------------------------------------------------------------------------

        ' Simple splitter movement
        'Developer Guide No. 261
        If eventArgs.Button = MouseButtons.Left Then
            lPosition = CInt(VB6.PixelsToTwipsX(picSplit.Left) + x)

            If lPosition < 600 Then lPosition = 600
            If lPosition > (VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 600) Then lPosition = (CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 600))

            picSplit.Left = VB6.TwipsToPixelsX(lPosition)

            ResizeControls()
        End If
    End Sub

    Private Sub tbarExplorer_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tbarExplorer_Button1.Click, _tbarExplorer_Button2.Click, _tbarExplorer_Button3.Click, _tbarExplorer_Button4.Click, _tbarExplorer_Button5.Click, _tbarExplorer_Button6.Click, _tbarExplorer_Button7.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Select Case Button.Name
            'Case "MapFolder"
            Case "_tbarExplorer_Button1"
                MapFolder()

                'Case "UnmapFolder"
            Case "_tbarExplorer_Button2"
                UnmapFolder()

                'Case "CreateFolder"
            Case "_tbarExplorer_Button4"
                'TODO:MILAN::
                tvwExplorer.LabelEdit = True
                'TODO
                'CType(tbarExplorer.Items.Item("CreateFolder"), ToolStripButton).Checked = True
                CType(tbarExplorer.Items.Item("_tbarExplorer_Button4"), ToolStripButton).Checked = True
                mnuExpTCreateFolder_Click(mnuExpTCreateFolder, New EventArgs())
                'TODO
                'CType(tbarExplorer.Items.Item("CreateFolder"), ToolStripButton).Checked = False
                CType(tbarExplorer.Items.Item("_tbarExplorer_Button4"), ToolStripButton).Checked = False

                'Case "CreateAccount"
            Case "_tbarExplorer_Button5"
                'TODO:MILAN::
                tvwExplorer.LabelEdit = True
                'TODO
                'CType(tbarExplorer.Items.Item("CreateAccount"), ToolStripButton).Checked = True
                CType(tbarExplorer.Items.Item("_tbarExplorer_Button5"), ToolStripButton).Checked = True
                mnuExpTCreateAccount_Click(mnuExpTCreateAccount, New EventArgs())
                'TODO
                'CType(tbarExplorer.Items.Item("CreateAccount"), ToolStripButton).Checked = False
                CType(tbarExplorer.Items.Item("_tbarExplorer_Button5"), ToolStripButton).Checked = False

                'Case "Delete"
            Case "_tbarExplorer_Button7"
                mnuExpTDelete_Click(mnuExpTDelete, New EventArgs())
        End Select

    End Sub
    ' ***************************************************************** '
    '
    ' Name: UpdateGroupForGlExport
    '
    ' Description:
    '
    ' History: 21/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Sub UpdateGroupForGlExport(ByVal v_iChecked As Integer)

        Try

            Dim lElementID, lTotallingID As Integer
            Dim sDescription As String = ""
            Dim lReportMapID As Integer
            Dim iIsDeletable, iGroupForGlExportInd As Integer
            Dim bReturn As Boolean
            If m_lReadOnly Then
                Exit Sub
            End If


            lElementID = GetElementIDFromKey(tvwExplorer.SelectedNode.Name)

            m_oBusiness.GetElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=Nothing, vIsDeletable:=Nothing, vGroupForGLExportInd:=iGroupForGlExportInd)

            If v_iChecked <> iGroupForGlExportInd Then
                iGroupForGlExportInd = v_iChecked

                bReturn = m_oBusiness.UpdateElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=0, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd)
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupForGlExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGroupForGlExport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub MapFolder()

        Dim sDefaultMapName As String = ""
        Dim lNodeId, lElementID, lMapID As Integer
        Dim vNodeData As Object

        'Developer Guide No 101
        Dim vFullPath As Object
        Dim vErrorNum As Object
        'EK 220300
        Dim bResult As Boolean
        Dim lReportMapID, lTotallingID As Integer
        Dim sDescription As String = ""
        Dim iIsDeletable As Integer
        'sj 21/11/2002 - start
        'PS700
        Dim iGroupForGlExportInd As Integer
        'sj 21/11/2002 - end

        If m_lReadOnly Then
            Exit Sub
        End If

        With tvwExplorer.SelectedNode
            lNodeId = GetNodeIDFromKey(.Name)
            lElementID = GetElementIDFromKey(.Name)
            lMapID = GetMapIDFromKey(.Name)
        End With

        ' Get old map name

        m_oBusiness.GetNode(lNodeId, vNodeData)


        Dim sOldMapName As String = CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, 0))

        Dim sElementName As String = CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, 0))

        If sOldMapName = "" Then
            sDefaultMapName = sElementName
        Else
            sDefaultMapName = sOldMapName
        End If

        Dim sNewMapName As String = sOldMapName

        ' Get the full path of the current node

        m_oBusiness.GetFullPath(lNodeId, vFullPath:=vFullPath)
        'EK 220300
        m_oBusiness.GetElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=Nothing, vIsDeletable:=Nothing, vGroupForGLExportInd:=iGroupForGlExportInd)
        ' Display the Map Folder dialog

        'Developer Guide No 69
        frmMapFolder = New frmMapFolder
        With frmMapFolder
            .MapName = sDefaultMapName
            .FullPath = vFullPath
            .TotallingID = lTotallingID
            .Description = sDescription
            .ReportMapID = lReportMapID
            VB6.ShowForm(frmMapFolder, FormShowConstants.Modal, Me)
            'EK 220300 More properties returned
            If .Result Then
                sNewMapName = .MapName.Trim()
                lTotallingID = .TotallingID
                sDescription = .Description
                lReportMapID = .ReportMapID
                bResult = .Result
            End If
        End With

        'PSL 18/02/2003  Issue 2125 Deletable depends on whether there are children or not
        If tvwExplorer.SelectedNode.GetNodeCount(False) <= 0 Then
            iIsDeletable = 1
        Else
            iIsDeletable = 0
        End If

        'EK 220300 Check that changes were OK'd
        If bResult Then
            If lMapID Then
                'EK 220300 Update the Element extras

                If m_oBusiness.UpdateElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=0, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd) Then

                    If sNewMapName <> sOldMapName And sNewMapName > "" Then
                        ' Update the mapping

                        If m_oBusiness.UpdateMapping(lMapID, sNewMapName) Then
                            With tvwExplorer.SelectedNode
                                ' Change node's text
                                .Text = MakeMapName(sNewMapName, sElementName)
                                tvwExplorer.Refresh()
                            End With
                        End If
                    End If
                End If
            Else
                ' Create the mapping

                m_oBusiness.BeginTrans()

                If m_oBusiness.UpdateElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=0, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd) Then

                    lMapID = m_oBusiness.InsertMapping(sNewMapName)

                    If lMapID > 0 Then

                        With tvwExplorer.SelectedNode

                            ' Map node in StructureTree

                            If m_oBusiness.MapNode(lNodeId, lMapID, vErrorNum) Then

                                ' Commit the update

                                m_oBusiness.CommitTrans()

                                'If .Index > 1 Then
                                ' Change node's image

                                .ImageIndex = ACImageClsdLdgr - 1

                                .SelectedImageIndex = ACImageClsdLdgr - 1

                                'developer guide no. 16 (No Solution)
                                '.ExpandedImage = ACImageOpenLdgr
                                'End If

                                .Text = MakeMapName(sNewMapName, .Text)
                                .Name = MakeNodeKey(lNodeId, lElementID, lMapID)
                                tvwExplorer.Refresh()
                                tvwExplorer_AfterSelect(tvwExplorer, New TreeViewEventArgs(tvwExplorer.SelectedNode))
                                tvwExplorer_NodeMouseClick(tvwExplorer, New TreeNodeMouseClickEventArgs(tvwExplorer.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0))
                            Else

                                ActionErrorMsgBox(ACVerbMap, .Text, sActionMsg(vErrorNum))

                                m_oBusiness.RollbackTrans()

                            End If

                        End With
                    End If
                Else


                    m_oBusiness.RollbackTrans()

                End If

            End If

        End If
    End Sub

    Sub UnmapFolder()

        Dim sMapName, sElementName As String
        Dim vNodeData As Object
        Dim lNodeId, lElementID, lMapID As Integer
        Dim vErrorNum As Byte

        If m_lReadOnly Then
            Exit Sub
        End If

        With tvwExplorer.SelectedNode

            lNodeId = GetNodeIDFromKey(.Name)
            lElementID = GetElementIDFromKey(.Name)
            lMapID = GetMapIDFromKey(.Name)

            If lMapID = 0 Then
                Exit Sub
            End If


            m_oBusiness.GetNode(lNodeId, vNodeData)

            sMapName = CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, 0))

            sElementName = CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, 0))


            m_oBusiness.BeginTrans()
            ' Unmap node in StructureTree


            If m_oBusiness.MapNode(lNodeId, 0, vErrorNum) Then

                ' Delete the mapping from StructureMapping

                If m_oBusiness.DeleteMapping(lMapID) Then

                    ' Commit the update

                    m_oBusiness.CommitTrans()

                    'Developer Guide No 111
                    'If .Index > 0 Then
                    ' Change node's image

                    .ImageIndex = ACImageClsdFold - 1

                    .SelectedImageIndex = ACImageClsdFold - 1

                    'developer guide no. 16 (No Solution)
                    '.ExpandedImage = ACImageClsdFold
                    'End If

                    .Text = sElementName
                    .Name = MakeNodeKey(lNodeId, lElementID)
                    tvwExplorer.Refresh()
                    tvwExplorer_AfterSelect(tvwExplorer, New TreeViewEventArgs(tvwExplorer.SelectedNode))
                    tvwExplorer_NodeMouseClick(tvwExplorer, New TreeNodeMouseClickEventArgs(tvwExplorer.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0))
                Else


                    m_oBusiness.RollbackTrans()

                End If

            Else

                ActionErrorMsgBox(ACVerbUnmap, sMapName, sActionMsg(vErrorNum))

                m_oBusiness.RollbackTrans()

            End If

        End With

    End Sub

    Private Sub CreateAccount()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CreateAccount
        ' PURPOSE:
        ' AUTHOR: Peter Finney
        ' DATE: 09-Oct-02, 04:59 PM
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lParentNodeId As Integer
        Dim iLedgerId As Integer
        Dim vFullPath As String = ""
        Dim lAccountId, lElementID, lNodeId As Integer


        Try

            ' Get parent node and it's full path
            lParentNodeId = GetNodeIDFromKey(tvwExplorer.SelectedNode.Name)

            m_oBusiness.GetFullPath(lParentNodeId, vFullPath:=vFullPath)

            'PWF 09/10/2002: Create a proper instance of the form
            frmCreateAccount = New frmCreateAccount()

            ' Set default properties (this is now a NEW form so everything will be blank)
            frmCreateAccount.Business = m_oBusiness
            frmCreateAccount.FullPath = vFullPath
            frmCreateAccount.Company = m_sCompanyName
            frmCreateAccount.CompanyID = m_iCompanyID

            'EK130199 Bug 190
            m_lReturn = frmCreateAccount.SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", [Internal], Failed to create account.")
            End If

            ' Display the Create Account dialog
            VB6.ShowForm(frmCreateAccount, FormShowConstants.Modal, Me)

            'EK 220300
            If frmCreateAccount.Result Then
                If frmCreateAccount.Code.Length Then
                    ' Create the account

                    m_oBusiness.BeginTrans()
                    'eck PN5946 110803 Ledger Id has already been picked from Form

                    '            m_lReturn = m_oBusiness.GetLedgerOfNode( _
                    ''                r_lNodeId:=lParentNodeId, _
                    ''                r_iLedgerId:=iLedgerId)
                    '
                    '            If (m_lReturn <> PMTrue) Then
                    '                m_oBusiness.RollbackTrans
                    '                Exit Sub
                    '            End If

                    'eck080500
                    'mkw110803 PN5367 Changed to save updated branch id to database.
                    'eck PN5946 110803 Get ledger Id from oCreateAccount

                    m_lReturn = m_oBusiness.InsertAccount(r_lAccountID:=lAccountId, r_vAccountName:=frmCreateAccount.AccName, r_vShortCode:=frmCreateAccount.Code, r_vAccountType:=frmCreateAccount.AccountType, r_vLedgerId:=frmCreateAccount.LedgerID, r_vCompanyID:=frmCreateAccount.CompanyID, r_vSubBranchID:=frmCreateAccount.SubBranchID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        m_oBusiness.RollbackTrans()

                        If m_lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                            MessageBox.Show("An Account with the short code '" & frmCreateAccount.Code & "' already exists." & _
                                            Environment.NewLine & "This field must be unique. Please use another.", "Account - Short Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                        Exit Sub
                    End If

                    If lAccountId > 0 Then
                        ' Check for base or multi-branch element?
                        If m_bMBCoreAccounts Then

                            lElementID = m_oBusiness.LookupElementId(frmCreateAccount.Code, m_iCompanyID)
                        Else

                            lElementID = m_oBusiness.LookupElementId(frmCreateAccount.Code)
                        End If

                        'EK 220200
                        If lElementID = 0 Then

                            lElementID = m_oBusiness.InsertElement(frmCreateAccount.Code, vTotallingId:=frmCreateAccount.TotallingID, vDescription:=frmCreateAccount.Description, vReportMapId:=frmCreateAccount.ReportMapID, vAccountMapID:=frmCreateAccount.AccountMapID, vIsDeletable:=1)
                        End If

                        If lElementID > 0 Then

                            lNodeId = m_oBusiness.InsertNode(lParentNodeId:=lParentNodeId, lElementId:=lElementID, vAccountID:=lAccountId)

                            If lNodeId > 0 Then
                                ' Commit the update

                                m_oBusiness.CommitTrans()
                                RefreshExplorerList(tvwExplorer.SelectedNode)
                            Else

                                m_oBusiness.RollbackTrans()
                            End If
                        Else

                            m_oBusiness.RollbackTrans()
                        End If

                    Else

                        m_oBusiness.RollbackTrans()
                    End If
                End If
            End If

            frmCreateAccount.Close()
            frmCreateAccount = Nothing



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFeedback, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", excep:=ex)

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create account", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub
    'EK 220200
    Private Sub AccountExtras()

        Dim iAccountType As Integer
        Dim sAccountName As String = ""
        Dim iLedgerId As Integer
        Dim vFullPath As String = ""
        Dim sDescription As String = ""
        Dim lTotallingID, lReportMapID, lAccountMapID As Integer
        Dim iIsDeletable As Integer
        Dim iSubBranchID As Integer
        'sj 21/11/2002 - start
        'PS700
        Dim iGroupForGlExportInd As Integer
        'sj 21/11/2002 - end

        Dim lAccountId As Integer = GetAccountIDFromKey(lvwExplorer.FocusedItem.Name)


        Dim lElementID As Integer = m_oBusiness.GetElementFromAccountID(v_lAccountId:=lAccountId)

        Dim sElementName As String = m_oBusiness.LookupElementName(lElementId:=lElementID)
        Dim lParentNodeId As Integer = GetNodeIDFromKey(tvwExplorer.SelectedNode.Name)

        ' Get the full path of the parent node

        m_oBusiness.GetFullPath(lParentNodeId, vFullPath:=vFullPath)


        m_oBusiness.GetElementExtras(lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=lAccountMapID, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd)
        ' eck PN5946 110803 Pass ledger ID

        m_oBusiness.GetAccountDetails(lAccountId:=lAccountId, vAccountName:=sAccountName, vAccountType:=iAccountType, vCompanyId:=m_iCompanyID, vLedgerID:=iLedgerId, iSubBranchID:=iSubBranchID)

        frmCreateAccount = New frmCreateAccount()

        ' Display the Create Account dialog
        With frmCreateAccount
            .ExtrasOnly = True
            .Code = sElementName
            .AccName = sAccountName
            .AccountType = iAccountType
            .FullPath = vFullPath
            .TotallingID = lTotallingID
            .Description = sDescription
            .ReportMapID = lReportMapID
            .AccountMapID = lAccountMapID
            ' eck PN5946 110803 Pass ledger ID
            .LedgerID = iLedgerId
            .Business = m_oBusiness
            .ProcessMode = m_lProcessMode
            .SubBranchID = iSubBranchID
            .Company = m_sCompanyName
            .CompanyID = m_iCompanyID

            m_lReturn = .SetInterfaceDefaults()
            m_lReturn = .DisplayLookupDetails()
            VB6.ShowForm(frmCreateAccount, FormShowConstants.Modal, Me)

            If .Result Then
                lTotallingID = .TotallingID
                sDescription = .Description.Trim()
                lReportMapID = .ReportMapID
                lAccountMapID = .AccountMapID
            End If

        End With

        frmCreateAccount.Close()
        frmCreateAccount = Nothing

        With m_oBusiness

            .BeginTrans()


            m_lReturn = .UpdateElementExtras(lElementId:=lElementID, vTotallingId:=lTotallingID, vDescription:=sDescription, vReportMapId:=lReportMapID, vAccountMapID:=lAccountMapID, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd)

            If Not m_lReturn Then

                .RollbackTrans()
            Else

                .CommitTrans()
            End If
        End With


    End Sub

    Private Sub ListViewProperties()

        Dim lAccountId As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            lAccountId = GetAccountIDFromKey(lvwExplorer.FocusedItem.Name)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' This is an account if AccountID defined
            If lAccountId <> 0 Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                ' Use the Account Interface to modify the properties

                m_oAccount.AccountID = lAccountId

                ' CF050399 Set the authority level

                m_oAccount.PMAuthorityLevel = m_lPMAuthorityLevel

                m_lReturn = m_oAccount.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetProcessModes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                m_lReturn = m_oAccount.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start account object.", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If


                If m_oAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                    ' Refresh the list view
                    RefreshExplorerList(tvwExplorer.SelectedNode)
                End If

            Else
                ' Dont have any other kind at this time
            End If

            '    End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display properties.", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub ListViewDelete()

        Dim lAccountId, lNodeId As Integer

        Dim l As DialogResult = MessageBox.Show("Do you wish to delete the selected item", "Account " & _
                                "Explorer", MessageBoxButtons.YesNo)

        If l = System.Windows.Forms.DialogResult.No Then

            Exit Sub

        ElseIf (l = System.Windows.Forms.DialogResult.Yes) Then

            With lvwExplorer.FocusedItem

                lAccountId = GetAccountIDFromKey(.Name)
                lNodeId = GetNodeIDFromKey(.Name)

                ' Is it an account ?
                If lAccountId <> 0 Then


                    If m_oBusiness.DeleteAccount(lAccountId) Then
                        lvwExplorer.Items.RemoveAt(lvwExplorer.FocusedItem.Index)
                    Else
                        ActionErrorMsgBox(ACVerbDelete, .Text, sActionMsg(ACTExplorerConst.ACExpErrNodeHasAccounts))
                    End If

                Else


                    If m_oBusiness.DeleteNode(lNodeId) Then
                        ' Delete node from explorer tree
                        lvwExplorer.Items.RemoveAt(CInt(.Name) - 1)
                        tvwExplorer.Nodes.RemoveAt(CInt(.Name) - 1)
                    Else
                        ActionErrorMsgBox(ACVerbDelete, .Text, sActionMsg(ACTExplorerConst.ACExpErrNodeHasAccounts))
                    End If

                End If

            End With

        End If

    End Sub

    Private Sub EndDrag()

        tvwExplorer.Tag = ""


        'developer guide no. 69 (No Solution)
        'tvwExplorer.DropHighlight = Nothing

        m_bOverExplorer = False

    End Sub

    Private Sub ShiftDragIcon()

        Dim pt As MainModule.POINTAPI = New MainModule.POINTAPI()

        ' Respond to change of Shift state
        SetDragIcon()

        ' If the DragIcon was changed then it is necessary to
        ' simulate a mouse movement to change the visible cursor.
        ' Use API functions GetCursorPos, SetCursorPos.
        GetCursorPos(pt)
        SetCursorPos(pt.x, pt.y) ' Don't change position!

    End Sub

    Private Sub SetDragIcon()

        Dim bCopy As Boolean

        ' Set DragIcon to Copy or Move

        If Convert.ToString(tvwExplorer.Tag) <> "" Then
            bCopy = m_bCtrlDown

            'developer guide no. 70 (No Solution)
            'tvwExplorer.DragIcon = imgExplorer.Images.Item((IIf(bCopy, ACImageClsdFoldCopy, ACImageClsdFoldMove)) - 1)
        End If

    End Sub

    Private Function MoveCopyNodes(ByRef bCopy As Boolean, ByRef tvwDest As TreeView, ByRef nodDest As TreeNode, ByRef tvwSrce As TreeView, ByRef nodSrce As TreeNode) As Boolean

        Dim result As Boolean = False

        If nodDest Is Nothing Then Return result
        If nodSrce Is Nothing Then Return result

        Dim vErrorNum As Byte
        Dim nodX As TreeNode
        Dim sKey As String = ""
        Dim lNodeId As Integer
        Dim idxLast As Integer
        Dim nodY As TreeNode

        Dim lElementID As Integer = GetElementIDFromKey(nodSrce.Name)

        If tvwDest Is tvwExplorer Then
            ' Add node to StructureTree

            lNodeId = m_oBusiness.InsertNode(lParentNodeId:=GetNodeIDFromKey(nodDest.Name), lElementId:=lElementID, vAccountID:=Nothing, vErrorNum:=vErrorNum)
            If lNodeId = 0 Then
                ActionErrorMsgBox(IIf(bCopy, ACVerbCopy, ACVerbMove), nodSrce.Text, sActionMsg(vErrorNum))
                Return result
            End If

            ' Create new child node
            ' Developer Guide No 162
            nodX = tvwDest.Nodes.Find(nodDest.Name, True)(0).Nodes.Add(MakeNodeKey(lNodeId, lElementID), nodSrce.Text, ACImageClsdFold - 1, ACImageClsdFold - 1)
            ' Flag that node has been explored
            nodX.Tag = True
            ' Expand parent node
            m_bInExploreNode = True ' Set flag
            nodDest.Expand()
            m_bInExploreNode = False ' Reset flag
        End If

        ' Copy any child nodes
        With nodSrce
            If .GetNodeCount(False) > 0 Then ' There are children
                ' Get first child node
                nodY = .FirstNode
                ' Set idxLast to the index of the child node's last sibling

                'developer guide no. 34
                idxLast = nodY.LastNode.Index
                Do
                    ' Copy this node (recursive)
                    If Not MoveCopyNodes(bCopy, tvwDest, nodX, tvwSrce, nodY) Then
                        Return result
                    End If
                    ' If this node's index value is idxLast...
                    If idxLast = nodY.Index Then
                        Exit Do
                    Else
                        ' Get next sibling
                        nodY = nodY.NextNode
                    End If
                Loop
            End If
        End With
        Return True
    End Function

    Private Sub ActionErrorMsgBox(ByRef nAction As Integer, ByRef sObject As String, ByRef sMsg As String)
        MessageBox.Show("Cannot " & sVerb(nAction) & " " & sObject & ": " & sMsg, "Error " & sAction(nAction) & " Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Function IsCyclicError(ByRef nodDest As TreeNode, ByRef nodSrce As TreeNode, ByRef bCopy As Boolean) As Boolean
        ' Check for cyclic errors in the case where
        ' nodDest and nodSrce are nodes in the same TreeView control
        ' bCopy=True indicates a copy operation
        ' bCopy=False indicates a move operation
        Dim nodParent As TreeNode

        Dim sMsg As String = ""
        ' Destination cannot be the same as the source node
        If (nodDest Is nodSrce) Or (nodDest Is nodSrce.Parent) Then
            sMsg = "The destination folder is the same as the source folder."
        Else
            ' Destination cannot be a descendant of the source node
            nodParent = nodDest.Parent
            Do While Not (nodParent Is Nothing)
                If nodParent Is nodSrce Then
                    sMsg = "The destination folder is a subfolder of the source folder."
                    Exit Do
                End If
                nodParent = nodParent.Parent
            Loop
        End If

        If sMsg = "" Then
            Return False
        Else
            ActionErrorMsgBox(IIf(bCopy, ACVerbCopy, ACVerbMove), nodSrce.Text, sMsg)
            Return True
        End If
    End Function

    Private Sub tvwExplorer_AfterLabelEdit(ByVal eventSender As Object, ByVal eventArgs As NodeLabelEditEventArgs) Handles tvwExplorer.AfterLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit

        'TODO:MILAN::
        'The LabelProperty sometimes contains nothing which should be checked
        Dim OriginalString As String = eventArgs.Node.Text
        If Not Information.IsNothing(eventArgs.Label) Then
            Dim NewString As String = eventArgs.Label

            Dim vErrorNum As Byte
            Dim sNewText As String = ""
            Dim Index, Count As Integer
            Dim lElementID As Integer
            Dim vNodeData As Object

            ' NewString is the new name of the SelectedItem Element
            ' Which must be unique in relation to its siblings

            With tvwExplorer.SelectedNode

                If Not m_oBusiness.UpdateElement(vNodeId:=GetNodeIDFromKey(.Name), vElementName:=NewString, vErrorNum:=vErrorNum) Then

                    ActionErrorMsgBox(ACVerbRename, .Text, sActionMsg(vErrorNum))

                    'eck180800 Update the Mapping as well
                    eventArgs.CancelEdit = True
                    Cancel = True
                Else


                    m_lReturn = m_oBusiness.UpdateMapping(lMapID:=GetMapIDFromKey(.Name), vMapName:=NewString)

                    ' CTAF 251000 - It doesnt matter if the mapping isnt updated
                    ' because the folder might not be mapped

                    sNewText = NewString
                    ' Iterate through all current nodes
                    ' and update the labels of any other nodes
                    ' defined by the same ElementID
                    lElementID = GetElementIDFromKey(.Name)
                    Index = .Index
                    Count = tvwExplorer.Nodes.Count

                    For i As Integer = 1 To Count

                        With tvwExplorer.Nodes.Item(i - 1)

                            If lElementID = GetElementIDFromKey(.Name) Then

                                ' If node is mapped...
                                If GetMapIDFromKey(.Name) Then

                                    ' Change text of node to include map name

                                    m_oBusiness.GetNode(GetNodeIDFromKey(.Name), vNodeData)


                                    .Text = MakeMapName(CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, 0)), CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, 0)))

                                    ' If SelectedItem ...
                                    If Index = .Index Then

                                        ' Return map name in NewString
                                        NewString = .Text

                                    End If

                                Else

                                    .Text = sNewText

                                End If

                            End If

                        End With

                    Next i

                    'End If
                    'eck180800
                End If
                '
            End With
        End If
        'TODO:MILAN::
        tvwExplorer.LabelEdit = False
    End Sub

    Private Sub tvwExplorer_AfterExpand(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwExplorer.AfterExpand
        Dim Node As TreeNode = eventArgs.Node

        Dim idxLast As Integer
        Dim nodY As TreeNode

        If m_bInExploreNode Then
            Exit Sub ' Ignore if flag is set
        End If


        ' Ensure that any child nodes have been explored
        With Node

            If .GetNodeCount(False) > 0 Then ' There are children

                ' Get first child node
                nodY = .FirstNode
                ' Set idxLast to the index of the child node's last sibling


                'TODO:MILAN:: This behavior is deviating from the one mentioned in Developer Guide.
                'idxLast = nodY.LastSibling.Index
                Dim tempNodY As TreeNode
                Dim lastSibling As TreeNode
                tempNodY = nodY
                Do
                    lastSibling = tempNodY
                    tempNodY = tempNodY.NextNode
                Loop While tempNodY IsNot Nothing

                idxLast = lastSibling.Index

                Do

                    ' If this node has not been explored...
                    'Developer Guide No 98
                    If Not nodY.Tag Then
                        ' Explore it one level
                        'testit
                        If nodY.Text = "Client Ledger" Then
                            ExploreNode(nodY.Name, iLevel:=1, iExpandLevel:=666, iMaxLevel:=1)
                        Else
                            ExploreNode(nodY.Name, iLevel:=1, iExpandLevel:=1, iMaxLevel:=1)
                        End If
                    End If
                    ' If this node's index value is idxLast...
                    If idxLast = nodY.Index Then
                        Exit Do
                    Else
                        ' Get next sibling
                        nodY = nodY.NextNode
                    End If

                Loop

                'Developer guide no. 16 (No Solution)
                If Node.ImageIndex = ACImageClsdFold - 1 Then
                    Node.ImageIndex = ACImageOpenFold - 1
                    Node.SelectedImageIndex = ACImageOpenFold - 1
                ElseIf Node.ImageIndex = ACImageClsdLdgr - 1 Then
                    Node.ImageIndex = ACImageOpenLdgr - 1
                    Node.SelectedImageIndex = ACImageOpenLdgr - 1
                ElseIf Node.ImageIndex = ACImageWorld - 1 Then
                    Node.SelectedImageIndex = ACImageWorld - 1
                End If
            End If
        End With
    End Sub

    Private Sub tvwExplorer_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwExplorer.MouseDown
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        'sj 21/11/2002 - start
        'PS700
        'm_iMouseButtonState = Button
        'sj 21/11/2002 - end

        Dim nodX As TreeNode = tvwExplorer.GetNodeAt(x, y)

        If nodX Is Nothing Then

            tvwExplorer.Tag = ""

        Else

            'Developer Guide No. 261
            If eventArgs.Button = MouseButtons.Left Then
                tvwExplorer.Tag = nodX.Name
            End If



            'Developer guide No 35
            tvwExplorer.SelectedNode = nodX

        End If

        nodX = Nothing



    End Sub

    Private Sub tvwExplorer_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwExplorer.MouseMove
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        If m_lReadOnly Then Exit Sub
        'Developer Guide No. 261
        m_dragSource = tvwExplorer
        'If eventArgs.Button = MouseButtons.Left Then
        '    If Convert.ToString(tvwExplorer.Tag) <> "" Then ' Signal a Drag operation.
        '        SetDragIcon()


        '        'tvwExplorer.Drag(vbBeginDrag) '  Drag operation.
        '        'developer guide no. 68 (No Solution)
        '        'tvwExplorer.Drag = vbBeginDrag

        '        m_dragSource = tvwExplorer
        '        MessageBox.Show(Convert.ToString(m_dragSource))
        '        tvwExplorer.DoDragDrop(tvwExplorer.GetNodeAt(x, y), DragDropEffects.Move)
        '    End If
        'End If

    End Sub

    Private Sub tvwExplorer_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwExplorer.MouseUp
        'Developer Guide No. 261
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If m_lReadOnly Then Exit Sub

        'sj 21/11/2002 - start
        'PS700
        'm_iMouseButtonState = Button
        'sj 21/11/2002 - end

        'Developer Guide No. 261
        If eventArgs.Button = MouseButtons.Right Then
            'Ensure that there is a selected item
            If Not (tvwExplorer.GetNodeAt(x, y) Is Nothing) Then
                ' Display the context menu on right mouse click.
                DisplayExplorerMenu(x, y)
            End If
        Else
            EndDrag()
        End If

    End Sub

    Private Sub tvwExplorer_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs)
        Dim Node As TreeNode = eventArgs.Node
        ' Call this when SelectedItem changed

        ' Refresh the list view
        RefreshExplorerList(Node)


    End Sub

    Private Sub DisplayExplorerMenu(ByVal x As Single, ByVal y As Single)

        'sj 21/11/2002 - start
        'PS700
        Dim iGroupForGlExportInd As Integer
        Dim lElementID As Integer
        Dim iIsDeletable As Integer
        Dim vReportMapId As Object

        'sj 21/11/2002 - end

        ' Show / hide menu items according to corresponding toolbar state
        If m_lReadOnly Then Exit Sub

        With tvwExplorer
            'EK 220300
            mnuExpTCreateFolder.Enabled = Not (GetFolderLevel(.SelectedNode.FullPath) >= 10)

            'sj 21/11/2002 - start
            'PS700
            lElementID = GetElementIDFromKey(.SelectedNode.Name)

            m_oBusiness.GetElementExtras(lElementId:=lElementID, vTotallingId:=Nothing, vDescription:="", vReportMapId:=vReportMapId, vAccountMapID:=Nothing, vIsDeletable:=iIsDeletable, vGroupForGLExportInd:=iGroupForGlExportInd)

            If iIsDeletable > 0 Then
                '        If (ElementDeletable(GetElementIDFromKey(.SelectedItem.Key))) Then
                'sj 21/11/2002 - end
                '            mnuExpTUnmapFolder.Enabled = True
                mnuExpTDelete.Enabled = True
            Else
                '            mnuExpTUnmapFolder.Enabled = False
                mnuExpTDelete.Enabled = False
            End If

            'PSL 18/02/2003 Issue 2125 Mapping does not depened on deletable, and the menu option
            'should depend on whether or not it is already mapped
            If GetMapIDFromKey(tvwExplorer.SelectedNode.Name) > 0 Then
                mnuExpTUnmapFolder.Enabled = True
                'Map is true because you may want to change the mapping
                mnuExpTMapFolder.Enabled = True
            Else
                mnuExpTUnmapFolder.Enabled = False
                mnuExpTMapFolder.Enabled = True
            End If

            'sj 21/11/2002 - start
            'PS700
            mnuExpGroupForGlExport.Checked = iGroupForGlExportInd = 1
            'sj 21/11/2002 - end

        End With

        'With tbarExplorer
        '    mnuExpTMapFolder.Visible = .Buttons("MapFolder").Enabled
        '    mnuExpTUnmapFolder.Visible = .Buttons("UnmapFolder").Enabled
        'End With

        Ctx_mnuExpT.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y) ' , , , mnuExpTCreateFolder

    End Sub

    Private Sub DisplayListViewMenu(ByVal x As Single, ByVal y As Single)

        'DD 12/07/2002
        'Only Accounts can be right clicked on

        Dim lElementID As Integer = m_oBusiness.GetElementFromAccountID(v_lAccountId:=GetAccountIDFromKey(lvwExplorer.FocusedItem.Name))

        If lElementID > 0 Then
            mnuListVProperties.Available = True
            mnuListVExtras.Available = True

            'EK 220300
            If ElementDeletable(lElementID) Then
                mnuListVDelete.Available = True
                mnuListVDelete.Enabled = True
            Else
                mnuListVDelete.Available = False
                mnuListVDelete.Enabled = False
            End If
            Ctx_mnuListV.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        Else
            'We're on a folder so show the folder menu here.

            'developer guide no. 35
            tvwExplorer.SelectedNode = tvwExplorer.Nodes.Find(lvwExplorer.FocusedItem.Name, True)(0)

            DisplayExplorerMenu(x, y)
        End If

    End Sub

    ' Explorer Menu Commands

    Public Sub mnuExpTMapFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTMapFolder.Click
        MapFolder()
    End Sub

    Public Sub mnuExpTUnmapFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTUnmapFolder.Click
        UnmapFolder()
    End Sub

    Public Sub mnuExpTCreateFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTCreateFolder.Click

        Dim lNodeId As Integer
        Dim nodX As TreeNode


        m_oBusiness.BeginTrans()

        Dim sNewText As String = "New Folder"

        Dim lElementID As Integer = m_oBusiness.InsertElement(sElementName:=sNewText)

        If lElementID > 0 Then
            ' Add node to StructureTree

            lNodeId = m_oBusiness.InsertNode(lParentNodeId:=GetNodeIDFromKey(tvwExplorer.SelectedNode.Name), lElementId:=lElementID)

            If lNodeId > 0 Then
                ' Create new child node
                'Developer Guide No 162
                nodX = tvwExplorer.Nodes.Find(tvwExplorer.SelectedNode.Name, True)(0).Nodes.Add(MakeNodeKey(lNodeId, lElementID), sNewText, ACImageClsdFold - 1, ACImageOpenFold - 1)

                ' Flag that node has been explored
                nodX.Tag = True

                ' Expand parent node
                m_bInExploreNode = True ' Set flag
                tvwExplorer.SelectedNode.Expand()
                m_bInExploreNode = False ' Reset flag

                ' Select the new child node
                'developer guide no. 246
                tvwExplorer.SelectedNode = nodX

                tvwExplorer_AfterSelect(tvwExplorer, New TreeViewEventArgs(tvwExplorer.SelectedNode))

                ' Start label edit
                If Not (tvwExplorer.SelectedNode Is Nothing) Then
                    'TODO:MILAN::
                    tvwExplorer.LabelEdit = True
                    tvwExplorer.SelectedNode.BeginEdit()
                End If

                ' Commit the update

                m_oBusiness.CommitTrans()
            Else

                m_oBusiness.RollbackTrans()
            End If
        Else

            m_oBusiness.RollbackTrans()
        End If
    End Sub

    Public Sub mnuExpTCreateAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTCreateAccount.Click
        CreateAccount()
    End Sub

    Public Sub mnuExpTDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTDelete.Click
        Dim sMsg As String = ""
        'JK021298-Check to see if folder contains subfolders or accounts

        If tvwExplorer.SelectedNode.GetNodeCount(False) <= 0 Then
            sMsg = "Are you sure you want to delete " & tvwExplorer.SelectedNode.Text & " ? " & _
                   ""
            If MessageBox.Show(sMsg, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.Yes Then
                'developer guide no. 246
                DeleteExplorerNode(tvwExplorer.SelectedNode)
                RefreshExplorerList(tvwExplorer.SelectedNode)
            End If
        Else
            m_lReturn = MessageBox.Show("This folder contains accounts or subfolders containing " & _
                        "accounts", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Public Sub mnuExpTRename_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTRename.Click
        ' If SelectedItem is mapped...
        Dim vNodeData As Object
        With tvwExplorer.SelectedNode
            If GetMapIDFromKey(.Name) Then
                ' ...change text of node to element name only

                m_oBusiness.GetNode(GetNodeIDFromKey(.Name), vNodeData)

                .Text = CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, 0))
            End If
        End With
        If Not (tvwExplorer.SelectedNode Is Nothing) Then
            'TODO:MILAN::
            tvwExplorer.LabelEdit = True
            tvwExplorer.SelectedNode.BeginEdit()
        End If
    End Sub

    'Display security maintenance form
    Public Sub mnuExpTSecurity_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExpTSecurity.Click

        m_frmSecurity = New iACTExplorer.frmSecurity()

        'Assign the parameters to the security form properties.
        m_frmSecurity.FolderName = tvwExplorer.SelectedNode.Text
        m_frmSecurity.NodeId = GetNodeIDFromKey(tvwExplorer.SelectedNode.Name)

        m_frmSecurity.ShowDialog()

    End Sub

    Private Function DeleteExplorerNode(ByRef nodX As TreeNode) As Boolean

        Dim result As Boolean = False

        If nodX Is Nothing Then Return result
        If nodX.Index = 1 Then Return result ' Cannot delete root

        Dim lNodeId As Integer
        With nodX
            ' Delete node from StructureTree
            lNodeId = GetNodeIDFromKey(.Name)


            If m_oBusiness.DeleteNode(lNodeId) Then
                ' Delete node from explorer tree
                'Developer Guide No 262
                tvwExplorer.Nodes.Find(nodX.Parent.Name, True)(0).Nodes.RemoveByKey(.Name)

                result = True
            Else
                ActionErrorMsgBox(ACVerbDelete, .Text, sActionMsg(ACTExplorerConst.ACExpErrNodeHasAccounts))
            End If
        End With
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (ClearElementParent) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClearElementParent(ByRef nodParent As TreeNode) As Boolean
    ' Clear ParentID in Elements table
    'Dim result As Boolean = False
    '
    'Dim vErrorNum As Byte
    'Dim idxLast As Integer
    'Dim nodChild As TreeNode
    '
    ' Clear the ParentID in the elements table

    'If Not m_oBusiness.UpdateElement(vElementId:=GetElementIDFromKey(nodParent.Name), vParentID:=0, vErrorNum:=vErrorNum) Then
    'ActionErrorMsgBox(ACVerbDelete, nodParent.Text, sActionMsg(vErrorNum))
    'Return result
    'End If
    '
    'If nodParent.GetNodeCount(False) > 0 Then ' There are children
    ' Get first child node
    'nodChild = nodParent.FirstNode
    ' Set idxLast to the index of the child node's last sibling

    'idxLast = nodChild.LastSibling.Index
    'Do 
    ' Clear this node's parent (recursive)
    'If Not ClearElementParent(nodChild) Then
    'Return result
    'End If
    ' If this node's index value is idxLast...
    'If idxLast = nodChild.Index Then
    'Exit Do
    'Else
    ' Get next sibling
    'nodChild = nodChild.NextNode
    'End If
    'Loop 
    'End If
    'Return True
    'End Function

    Private Sub RefreshExplorerList(ByVal nodX As TreeNode)

        Dim lFromNodeId As Integer
        Dim vNodeData(,) As Object
        'eck020801
        Dim sCode As String = ""
        Dim lstX As New ListViewItem
        Dim sKey, sText, sShortCode As String

        Try

            ' Show any children of the given node
            ' (including accounts) in the list view

            ' CTAF 291099 - Set cursor to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'sj 21/11/2002 - start
            'PS700
            ' Clear the list view
            '    lvwExplorer.ListItems.Clear
            '
            '    lFromNodeId = GetNodeIDFromKey(nodX.Key)
            '
            '    ' Add text of current node to panel
            '    lblExplorerList.Caption = "Contents of " & nodX.Text
            'eck020801
            'sj 21/11/2002 - end

            If nodX.Text = "Client Ledger" Then
                'sj 21/11/2002 - start
                'PS700
                If m_iMouseButtonState <> MouseButtonConstants.RightButton Then
                    'sj 21/11/2002 - end
                    sCode = Interaction.InputBox("Enter Client Code:", "Client Search")
                    If sCode = "" Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    Else
                        sCode = sCode & "%"
                    End If
                    'sj 21/11/2002 - start
                    'PS700
                Else
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
                'sj 21/11/2002 - end
            End If

            'sj 21/11/2002 - start
            'PS700
            ' Clear the list view
            lvwExplorer.Items.Clear()

            lFromNodeId = GetNodeIDFromKey(nodX.Name)

            ' Add text of current node to panel
            lblExplorerList.Text = "Contents of " & nodX.Text
            'eck020801
            'sj 21/11/2002 - end

            ' Get a two dimensional result array whose columns are identified
            ' by constants with prefix ACGetNode.
            'eck020809 added new parameter

            m_oBusiness.GetChildrenOfNode(lFromNodeId, 1000, vNodeData, sCode)

            ' CTAF 291099 - Disable interface while refreshing
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwExplorer)

            If Information.IsArray(vNodeData) Then
                ' Populate the list view from the result array

                For lRow As Integer = vNodeData.GetLowerBound(1) To vNodeData.GetUpperBound(1)


                    ' If MapID defined...

                    If CStr(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)) > "" Then



                        sKey = MakeNodeKey(CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow)), CInt(vNodeData(ACTExplorerConst.ACGetNodeElementID, lRow)), CInt(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)))


                        sText = MakeMapName(CStr(vNodeData(ACTExplorerConst.ACGetNodeMapName, lRow)), CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRow)))
                    Else


                        sKey = MakeNodeKey(CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow)), CInt(vNodeData(ACTExplorerConst.ACGetNodeElementID, lRow)))

                        sText = CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRow))
                    End If
                    ' This is an account if AccountID defined


                    If CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRow)) > "" Then
                        ' AccountID defined
                        ' Assign the element name to the first column.

                        sShortCode = CStr(vNodeData(ACTExplorerConst.ACGetNodeShortCode, lRow)).Trim()
                        If sShortCode > "" Then
                            sText = sShortCode
                        End If



                        sKey = MakeNodeKey(CInt(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRow)), CInt(vNodeData(ACTExplorerConst.ACGetNodeElementID, lRow)), vAccountId:=CInt(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRow)))

                        lstX = lvwExplorer.Items.Add(sKey, sText, ACImageAccTyp01 - 1) ' + vNodeData(ACGetNodeAccountType, lRow))
                        '                    ACImageAccTyp01 + vNodeData(ACGetNodeAccountType, lRow))
                        ' Assign the type to the second column
                        'lstX.SubItems(1) = sNodeType(ACNodeAccount)

                        If IsNothing(frmCreateAccount) Then
                            frmCreateAccount = New frmCreateAccount()
                        End If
                        'ListViewHelper.GetListViewSubItem(lstX, 1).Text = frmCreateAccount.uctAccountType.ItemDescription(CInt(vNodeData(ACTExplorerConst.ACGetNodeAccountType, lRow)))
                        ListViewHelper.GetListViewSubItem(lstX, 1).Text = frmCreateAccount.uctAccountType.ItemDescription(ToSafeInteger(vNodeData(ACTExplorerConst.ACGetNodeAccountType, lRow)))

                        ListViewHelper.GetListViewSubItem(lstX, 2).Text = gPMFunctions.NullToString(CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountName, lRow))).Trim()

                        ListViewHelper.GetListViewSubItem(lstX, 3).Text = gPMFunctions.NullToString(CStr(vNodeData(ACTExplorerConst.ACGetNodeSubBranchName, lRow))).Trim()
                    ElseIf CStr(vNodeData(ACTExplorerConst.ACGetNodeMapID, lRow)) > "" Then
                        ' MapID defined
                        ' Assign the name to the first column.

                        lstX = lvwExplorer.Items.Add(sKey, sText, ACImageClsdLdgr - 1)
                        ' Assign the type to the second column
                        ListViewHelper.GetListViewSubItem(lstX, 1).Text = sNodeType(ACNodeMap)
                    Else
                        ' Assign the name to the first column.

                        lstX = lvwExplorer.Items.Add(sKey, sText, ACImageClsdFold - 1)
                        ' Assign the type to the second column
                        ListViewHelper.GetListViewSubItem(lstX, 1).Text = sNodeType(ACNodeFolder)
                    End If
                Next lRow
            End If

            ' CTAF 291099 - Re-Enable list view
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchEnd()

            ' CTAF 291099 - Set cursor to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch excep As System.Exception



            ' CTAF 291099 - Set cursor to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to refresh explorer list.", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshExplorerList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    'Developer guide no. 16 (No Solution)
    'Starts
    Private Sub tvwExplorer_AfterCollapse(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwExplorer.AfterCollapse

        Dim node As TreeNode
        node = e.Node

        If node.ImageIndex = ACImageOpenFold - 1 Then
            node.ImageIndex = ACImageClsdFold - 1
            node.SelectedImageIndex = ACImageClsdFold - 1
        ElseIf node.ImageIndex = ACImageOpenLdgr - 1 Then
            node.ImageIndex = ACImageClsdLdgr - 1
            node.SelectedImageIndex = ACImageClsdLdgr - 1
        ElseIf node.ImageIndex = ACImageWorld - 1 Then
            node.SelectedImageIndex = ACImageWorld - 1
        End If
    End Sub
    'Ends

    Private Sub tvwExplorer_NodeMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwExplorer.NodeMouseClick
        Dim Node As TreeNode = e.Node
        ' Call this when SelectedItem changed

        ' Refresh the list view
        If e.Button = Windows.Forms.MouseButtons.Right And Node.Text = "Client Ledger" Then
        Else
            RefreshExplorerList(Node)
        End If


        ' UnmapFolder is already mapped
        'tbarExplorer.Items.Item("UnmapFolder").Enabled = (GetMapIDFromKey(Node.Name) > 0)
        tbarExplorer.Items.Item("_tbarExplorer_Button2").Enabled = (GetMapIDFromKey(Node.Name) > 0)

        If (Node.ImageIndex = ACImageWorld - 1) Then
            Node.SelectedImageIndex = ACImageWorld - 1
        End If
    End Sub


    'TreeView DragDrop Event Handlers
    Private Sub tvwExplorer_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles tvwExplorer.ItemDrag
        tvwExplorer.DoDragDrop(e.Item, DragDropEffects.Move)
        m_dragSource = sender
    End Sub
    'Tree View Drag Drop Eventhandlers
    Private Sub tvwExplorer_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvwExplorer.DragEnter
        If e.Data.GetDataPresent(GetType(ListView.SelectedListViewItemCollection)) Then
            e.Effect = DragDropEffects.Move
        End If
        If (Convert.ToString(tvwExplorer.Tag) <> "") Or m_dragSource Is lvwExplorer Then
            ' Set DropHighlight to the mouse's coordinates.

            'tvwExplorer.DropHighlight = tvwExplorer.GetNodeAt(x, y)
            ' Set transition flag
            'm_bOverExplorer = (State <> 1)
            ' Set DragIcon to Copy or Move
            SetDragIcon()
        End If
    End Sub

    Private Sub tvwExplorer_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvwExplorer.DragDrop
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: tvwExplorer_DragDrop
        ' PURPOSE: Handle the final drop
        ' AUTHOR: Danny Davis
        ' DATE: 23/09/2002, 14:23
        ' CHANGES: Add drag/drop between list and tree
        ' ---------------------------------------------------------------------------



        Dim nodSrce, nodDest As TreeNode
        Dim bCopy As Boolean
        Dim vErrorNum As Byte


        'nodDest = tvwExplorer.DropHighlight
        If e.Data.GetDataPresent(GetType(ListView.SelectedListViewItemCollection).ToString(), False) Then
            Dim loc As Point = (CType(sender, TreeView)).PointToClient(New Point(e.X, e.Y))
            nodDest = (CType(sender, TreeView)).GetNodeAt(loc)
            Dim tnNew As TreeNode
            'If m_dragSource Is tvwExplorer Then
            If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = True Then
                If (Convert.ToString(tvwExplorer.Tag) <> "") And Not (nodDest Is Nothing) Then
                    bCopy = m_bCtrlDown
                    nodSrce = tvwExplorer.Nodes.Item(Convert.ToString(tvwExplorer.Tag))
                    ' Source node must be fully explored
                    ExploreNode(nodSrce.Name, iLevel:=1, iExpandLevel:=0, iMaxLevel:=999)
                    ' Check for cyclic error
                    If Not IsCyclicError(nodDest, nodSrce, True) Then
                        If bCopy Then
                            ' Require transaction around MoveCopyNodes

                            m_oBusiness.BeginTrans()
                            If MoveCopyNodes(bCopy, tvwExplorer, nodDest, tvwExplorer, nodSrce) Then

                                m_oBusiness.CommitTrans()
                            Else

                                m_oBusiness.RollbackTrans()
                            End If
                        Else

                            If Not m_oBusiness.MoveNode(lSrceNodeID:=GetNodeIDFromKey(Convert.ToString(tvwExplorer.Tag)), lDestNodeID:=GetNodeIDFromKey(nodDest.Name), vErrorNum:=vErrorNum) Then
                                ActionErrorMsgBox(ACVerbMove, nodSrce.Text, sActionMsg(vErrorNum))
                            Else
                                ' Move node on the TreeView

                                'nodSrce.Parent = nodDest
                            End If
                        End If
                    End If
                End If
                EndDrag()
            ElseIf e.Data.GetDataPresent(GetType(ListView.SelectedListViewItemCollection)) Then

                'Drop the List item in here

                If Not m_oBusiness.MoveNode(lSrceNodeID:=GetNodeIDFromKey(lvwExplorer.FocusedItem.Name), lDestNodeID:=GetNodeIDFromKey(nodDest.Name), vErrorNum:=vErrorNum) Then
                    ActionErrorMsgBox(ACVerbMove, nodSrce.Text, sActionMsg(vErrorNum))
                Else
                    'Avoid runtime error here
                    Try
                        'Check if item is a node on the tree
                        nodSrce = tvwExplorer.Nodes.Item(lvwExplorer.FocusedItem.Name)

                    Catch
                    End Try


                    'Return to the parent

                    'tvwExplorer.DropHighlight = Nothing
                    MoveToNode(tvwExplorer.SelectedNode.Name)

                    If Not (nodSrce Is Nothing) Then
                        ' Move node on the TreeView

                        'nodSrce.Parent = nodDest
                    End If

                End If
            End If
        End If
        nodDest = Nothing
        nodSrce = Nothing


        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------


Catch_Renamed:
        Select Case Information.Err().Number
            Case Else
                MessageBox.Show("iACTExplorer.frmInterface.tvwExplorer_DragDrop" & _
                                "Version: " & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) & _
                                " At line: " & CStr(Information.Erl()) & "|" & Information.Err().Source & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                Information.Err().Number & ":" & Information.Err().Description, Application.ProductName)

                GoTo Finally_Renamed
        End Select

Finally_Renamed:

        m_dragSource = Nothing

    End Sub

    'ListView Drag Drop Event Handlers
    Private Sub lvwExplorer_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwExplorer.DragDrop
        'If (e.Data.GetDataPresent(GetType(TreeNode))) Then
        '    Dim tn As TreeNode = DirectCast(e.Data.GetData(GetType(TreeNode)), TreeNode)
        '    lvwExplorer.Items.Add(tn.Text, tn.ImageIndex)
        '    tvwExplorer.Nodes.Remove(tn)
        'End If

        Dim itmX As ListViewItem
        'Developer Guie No 17
        Dim vErrorNum As Byte

        If m_dragSource Is lvwExplorer Then

            'itmX = lvwExplorer.DropHighlight
            If Not (itmX Is Nothing) Then
                If GetAccountIDFromKey(itmX.Name) > 0 Then

                    'lvwExplorer.DropHighlight = Nothing

                    'lvwExplorer.Drag(System.Windows.Forms.DialogResult.Cancel)
                Else
                    'Drop the List item in here


                    If Not m_oBusiness.MoveNode(lSrceNodeID:=GetNodeIDFromKey(sender.SelectedItem.Key), lDestNodeID:=GetNodeIDFromKey(itmX.Name), vErrorNum:=vErrorNum) Then
                        ActionErrorMsgBox(ACVerbMove, itmX.Text, sActionMsg(vErrorNum))
                    Else
                        'Return to the parent
                        RefreshExplorerList(tvwExplorer.SelectedNode)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub lvwExplorer_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwExplorer.DragEnter
        If (e.Data.GetDataPresent(GetType(TreeNode))) Then
            e.Effect = DragDropEffects.Move
        End If
        m_dragSource = Nothing
    End Sub

    Private Sub lvwExplorer_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwExplorer.DragOver
        If (e.Data.GetDataPresent(GetType(TreeNode))) Then
            e.Effect = DragDropEffects.Move
        End If
        m_dragSource = Nothing
    End Sub
    Private Sub lvwExplorer_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles lvwExplorer.ItemDrag
        lvwExplorer.DoDragDrop(lvwExplorer.SelectedItems, DragDropEffects.Move)
    End Sub
End Class
