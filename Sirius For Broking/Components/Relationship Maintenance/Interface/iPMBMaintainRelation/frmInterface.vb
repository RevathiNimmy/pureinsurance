Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developers guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmMain"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_vLookupDetails As Object
    Private m_vLookupValues As Object
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Business Object
    Public m_oBusiness As Integer

    ' Data
    'develepors guide no. 101
    'Private m_vDataArray( ,  ) As Object
    Private m_vDataArray As Object
    Private m_vLookupArray(,) As Object
    'develepors guide no. 101
    'Private m_vPartyArray(,) As Object
    Private m_vPartyArray As Object

    Private m_bDataChanged As Boolean

    ' Values for resource values
    Private m_sDeleteCaption As String = ""
    Private m_sUnDeleteCaption As String = ""

    ' PUBLIC Property Procedures (Begin)


    Public Property LookupValues() As Object
        Get
            Return m_vLookupValues
        End Get
        Set(ByVal Value As Object)


            m_vLookupValues = Value
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

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    Private Sub cboPartyRelationshipGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyRelationshipGroup.SelectedIndexChanged
        'CMG/PB 11/07/2002 When a different relationship group is choosen
        'Update the list to show only the relevant relationships
        m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = m_lReturn
            Exit Sub
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = CType(ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim sMsg As String = ""

        If m_bDataChanged Then
            sMsg = "Are you sure you wish to exit? You will lose your changes."
            If MessageBox.Show(sMsg, "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        m_lReturn = CType(DeleteItem(), gPMConstants.PMEReturnCode)
        cmdDelete.Text = "&Delete"

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_lReturn = CType(ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowHelp
    '
    ' Description: Display the help for this screen
    '
    ' History: 11/06/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowHelp(ByVal v_lContextID As Integer) As Integer

        Dim result As Integer = 0
        Dim sHelpFile As String = ""

        Try
            'developer guide no. 200
            Dim dlgHelp As New Object()


            result = gPMConstants.PMEReturnCode.PMTrue

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)


            dlgHelp.HelpFile = sHelpFile

            dlgHelp.HelpContext = v_lContextID

            'developer guide no. 
            dlgHelp.HelpCommand = SharedFiles.GIIFunctions.HelpContext

            dlgHelp.ShowHelp()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowHelp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click


        'TODO
        'm_lReturn = CType(ShowHelp(cmdHelp.HelpContextID), gPMConstants.PMEReturnCode)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        ' Set to busy pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If m_bDataChanged Then
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
        End If

        ' Set to normal pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Close()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' Set the resizer control values
            With uctPMResizer

                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("lvwMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("picRelationGroup", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("LblRelGroup", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cboPartyRelationshipGroup", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .FormMinHeight = 3975
                .FormMinWidth = 6495

            End With

        End If
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ProcessSolutionOptions
    '
    ' Description: Process any solution specific code
    '
    ' History: 17/08/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessSolutionOptions() As Integer

        Dim result As Integer = 0
        Dim vUnderwriting As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get SFU or SFB

            m_lReturn = CType(iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=CStr(vUnderwriting)), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Display / Show the party type
            cboPartyRelationshipGroup.SelectedIndex = 0


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSolutionOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSolutionOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Data hasnt changed
            m_bDataChanged = False

            ' Display the captions
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
            End If

            ' Get the list of data
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
                Exit Sub
            End If

            ' CTAF 170802 - Process Solution Options
            m_lReturn = CType(ProcessSolutionOptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
            End If

        Catch excep As System.Exception



            ' Error Section
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

        'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwMain.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
        lvwMain.FullRowSelect = True
        lvwMain.Scrollable = True
    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetPartner
    '
    ' Description: Get's the description for the partner id passed in
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetPartner(ByVal v_vPartnerID As String, ByRef r_sPartner As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_vPartnerID = "0" Then
                r_sPartner = "(none)"
                Return result
            End If

            ' Loop the array
            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                ' If the ID's match, then return the description
                If CInt(v_vPartnerID) = CInt(m_vDataArray(ACArrayRelationShipTypeID, iLoop1)) Then
                    r_sPartner = CStr(m_vDataArray(ACArrayDescription, iLoop1))
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartner Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartner", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RefreshList
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RefreshList() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sText, sPartner As String
        Dim bInUse As Boolean
        Dim iTypeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list
            lvwMain.Items.Clear()

            ' Exit if no data
            If Not Information.IsArray(m_vDataArray) Then
                Return result
            End If

            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                ' Add the item  only if matches party relationship group
                If CDbl(m_vDataArray(ACArrayPartyRelationshipGroupID, iLoop1)) = VB6.GetItemData(cboPartyRelationshipGroup, cboPartyRelationshipGroup.SelectedIndex) Then
                    sText = CStr(m_vDataArray(ACArrayCode, iLoop1))


                    lstItem = lvwMain.Items.Add("I" & iLoop1, sText, "Icon")
                    'developer guide no.242
                    ListViewHelper.AddListItemSmallIconProperty(lstItem, "Icon")
                    ' Description
                    ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDataArray(ACArrayDescription, iLoop1))

                    ' Partner
                    m_lReturn = CType(GetPartner(v_vPartnerID:=CStr(m_vDataArray(ACArrayComplementaryTypeID, iLoop1)), r_sPartner:=sPartner), gPMConstants.PMEReturnCode)

                    ListViewHelper.GetListViewSubItem(lstItem, 2).Text = sPartner

                    ' Check if it's in use
                    iTypeID = CInt(m_vDataArray(ACArrayRelationShipTypeID, iLoop1))
                    m_lReturn = CType(CheckInUse(v_iTypeID:=iTypeID, r_bInUse:=bInUse), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If bInUse Then
                        ListViewHelper.GetListViewSubItem(lstItem, 3).Text = "Yes"
                    End If

                    ' Ghost the icon if it's marked as deleted
                    If CStr(m_vDataArray(ACArrayIsDeleted, iLoop1)) = "1" Then

                        'developer guide no. 12 (no solution)
                        'lstItem.ghosted = true
                        'lstItem.BackColor = Color.WhiteSmoke
                        lstItem.ForeColor = Color.Gray
                    End If

                    ' Store the index of the array
                    lstItem.Tag = CStr(iLoop1)
                End If

            Next iLoop1

            ' Select the first one
            If lvwMain.Items.Count > 0 Then
                lvwMain.FocusedItem = lvwMain.Items.Item(0)
                'developer guide no. 185
                If lvwMain.SelectedItems.Count > 0 Then
                    lvwMain_ItemClick(lvwMain.SelectedItems.Item(0))
                End If
            Else
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetArrayIndex
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetArrayIndex(ByVal v_iID As Integer, ByRef r_iIndex As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
                If CInt(m_vDataArray(ACArrayRelationShipTypeID, iLoop1)) = v_iID Then
                    r_iIndex = iLoop1
                    Exit For
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetArrayIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetArrayIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateLinks
    '
    ' Description: Type A and B are the new partners, C is the old one.
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateLinks(ByVal v_iTypeA As Integer, ByVal v_iTypeB As Integer, Optional ByVal v_iTypeC As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim iIndex1, iIndex2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop around and remove all links to those two types
            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                If CInt(m_vDataArray(ACArrayComplementaryTypeID, iLoop1)) = v_iTypeA Then
                    m_vDataArray(ACArrayComplementaryTypeID, iLoop1) = CStr(0)
                End If

                If CInt(m_vDataArray(ACArrayComplementaryTypeID, iLoop1)) = v_iTypeB Then
                    m_vDataArray(ACArrayComplementaryTypeID, iLoop1) = CStr(0)
                End If

                If Not False Then
                    If CInt(m_vDataArray(ACArrayComplementaryTypeID, iLoop1)) = v_iTypeC Then
                        m_vDataArray(ACArrayComplementaryTypeID, iLoop1) = CStr(0)
                    End If
                End If

            Next iLoop1

            ' Don't reconnect if we have a type 0 (none)
            If v_iTypeA = 0 Or v_iTypeB = 0 Then
                Return result
            End If

            ' Re-connect those two types

            ' Get the index in the array
            m_lReturn = CType(GetArrayIndex(v_iID:=v_iTypeA, r_iIndex:=iIndex1), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get index in array for type A", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLinks", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the index in the array
            m_lReturn = CType(GetArrayIndex(v_iID:=v_iTypeB, r_iIndex:=iIndex2), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get index in array for type B", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLinks", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' and match them up
            m_vDataArray(ACArrayComplementaryTypeID, iIndex1) = CStr(v_iTypeB)
            m_vDataArray(ACArrayComplementaryTypeID, iIndex2) = CStr(v_iTypeA)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLinks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLinks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNextID
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetNextID(ByRef r_lID As Integer) As Integer

        Dim result As Integer = 0
        Dim lNextID, lCurrentID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lNextID = 0

            For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                lCurrentID = CInt(m_vDataArray(ACArrayRelationShipTypeID, iLoop1))

                If lCurrentID > lNextID Then
                    lNextID = lCurrentID
                End If

            Next iLoop1

            ' Return the next id
            r_lID = lNextID + 1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateCombo
    '
    ' Description: Populates combo with values from Party Relationship Group lookup
    '
    ' History: 11/07/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateCombo() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate the combo box
            If Information.IsArray(m_vLookupArray) Then
                For iLoop1 As Integer = 0 To m_vLookupArray.GetUpperBound(1)
                    cboPartyRelationshipGroup.Items.Add(CStr(m_vLookupArray(3, iLoop1)))
                    VB6.SetItemData(cboPartyRelationshipGroup, iLoop1, CInt(m_vLookupArray(0, iLoop1)))
                Next iLoop1
            End If

            ' Default the combo to client
            ' PW290702 - check if any groups returned
            If cboPartyRelationshipGroup.Items.Count = 0 Then
                ' no relationship groups found
            Else
                cboPartyRelationshipGroup.SelectedIndex = 0
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessDetails
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessDetails(ByVal v_iTask As Integer) As Integer

        Dim result As Integer = 0
        Dim oDetail As frmDetails
        Dim iIndex As Integer
        Dim sTag As String = ""
        Dim lNextFreeID As Integer
        Dim bInUse As Boolean
        Dim iOldPartnerID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the index if in edit mode
            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                iIndex = Convert.ToString(lvwMain.FocusedItem.Tag)
            End If

            ' Get a new instance of the form
            oDetail = New frmDetails()

            ' Load it

            ' Set the task
            oDetail.Task = v_iTask

            ' Pass the array through for the combo box
            'developers guide no 24
            oDetail.DataArray = m_vDataArray
            oDetail.PartyArray = m_vPartyArray

            oDetail.PartyRelationshipGroupId = VB6.GetItemData(cboPartyRelationshipGroup, cboPartyRelationshipGroup.SelectedIndex)

            ' Set the properties if in edit mode
            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                m_lReturn = CType(CheckInUse(v_iTypeID:=CInt(m_vDataArray(ACArrayRelationShipTypeID, iIndex)), r_bInUse:=bInUse), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                With oDetail
                    .InUse = bInUse
                    .RelationshipTypeID = CInt(m_vDataArray(ACArrayRelationShipTypeID, iIndex))
                    .Code = CStr(m_vDataArray(ACArrayCode, iIndex))
                    .Description = CStr(m_vDataArray(ACArrayDescription, iIndex))
                    .PartnerID = CInt(m_vDataArray(ACArrayComplementaryTypeID, iIndex))
                End With

            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                oDetail.InUse = False

            End If

            ' Initialise it
            'developer guide no. 9
            m_lReturn = oDetail.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse's pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Show it
            oDetail.ShowDialog()

            ' Get the values
            If oDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Warn about data changed when exiting
                m_bDataChanged = True

                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                    If Information.IsArray(m_vDataArray) Then
                        iIndex = m_vDataArray.GetUpperBound(1) + 1
                        ReDim Preserve m_vDataArray(7, iIndex)
                    Else
                        iIndex = 0
                        ReDim m_vDataArray(7, iIndex)
                    End If

                    m_lReturn = CType(GetNextID(r_lID:=lNextFreeID), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the next free number so that new relations can be set up to this one
                    m_vDataArray(ACArrayRelationShipTypeID, iIndex) = lNextFreeID

                    ' This is zero so that the business knows to do an add
                    m_vDataArray(ACArrayCaptionID, iIndex) = 0

                End If

                ' Get the properties
                ' Get the old partner

                iOldPartnerID = CInt(m_vDataArray(ACArrayComplementaryTypeID, iIndex))

                m_vDataArray(ACArrayCode, iIndex) = oDetail.Code
                m_vDataArray(ACArrayDescription, iIndex) = oDetail.Description
                m_vDataArray(ACArrayComplementaryTypeID, iIndex) = CStr(oDetail.PartnerID)
                m_vDataArray(ACArrayIsDeleted, iIndex) = "0"
                m_vDataArray(ACArrayPartyRelationshipGroupID, iIndex) = VB6.GetItemData(cboPartyRelationshipGroup, cboPartyRelationshipGroup.SelectedIndex)

                ' Update the links
                m_lReturn = CType(UpdateLinks(v_iTypeA:=CInt(m_vDataArray(ACArrayRelationShipTypeID, iIndex)), v_iTypeB:=CInt(m_vDataArray(ACArrayComplementaryTypeID, iIndex)), v_iTypeC:=iOldPartnerID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Terminate it
            oDetail.Dispose()
            ' CTAF 160800
            ' Unload the form and remove it
            oDetail.Close()

            oDetail = Nothing

            ' Refresh the list
            m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: BusinessToData
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the data off the table

            m_lReturn = g_oBusiness.GetDetails(r_vDetailArray:=m_vDataArray, r_vLookupArray:=m_vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on GetDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Read which relationships are in use

            m_lReturn = g_oBusiness.GetUsedRelations(r_vArray:=m_vPartyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on GetUsedRelations", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' Fill the combo box with valid party relationship groups
            'This will also refreshlist()
            PopulateCombo()

            ' Fill the combo box with valid party relationship groups
            'This will also refreshlist()
            PopulateCombo()

            ' Fill the combo box with valid party relationship groups
            'This will also refreshlist()
            PopulateCombo()

            ' Fill the combo box with valid party relationship groups
            'This will also refreshlist()
            PopulateCombo()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwMain_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwMain.DoubleClick

        ' Edit the item on a double click
        If cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If

    End Sub

    Private Sub lvwMain_ItemClick(ByVal Item As ListViewItem)


        ' UPGRADE_WARNING: (1068) Item.Tag of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
        Dim lID As Integer = Convert.ToString(Item.Tag)

        ' Check for delete/undelete
        If CStr(m_vDataArray(ACArrayIsDeleted, lID)) = "0" Then
            cmdDelete.Text = m_sDeleteCaption
        Else
            cmdDelete.Text = m_sUnDeleteCaption
        End If


    End Sub

    Private Sub lvwMain_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwMain.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide 70
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim bEnable As Boolean

        ' Check to enable or disable buttons
        bEnable = Not (lvwMain.GetItemAt(x, y) Is Nothing)

        cmdEdit.Enabled = bEnable
        cmdDelete.Enabled = bEnable


    End Sub

    ' ***************************************************************** '
    '
    ' Name: DeleteItem
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteItem() As Integer

        Dim result As Integer = 0
        Dim iIndex, iIsDeleted As Integer
        Dim bInUse As Boolean
        Dim sMsg As String = ""
        Dim iID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the index
            iIndex = Convert.ToString(lvwMain.FocusedItem.Tag)

            bInUse = False

            ' Check if it's in use
            iID = CInt(m_vDataArray(ACArrayRelationShipTypeID, iIndex))
            m_lReturn = CType(CheckInUse(v_iTypeID:=iID, r_bInUse:=bInUse), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If it's in use then don't let it be deleted
            If bInUse Then
                sMsg = "Unable to delete this relationship as it is in use by one or more parties."
                MessageBox.Show(sMsg, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' Check if it's relation is in use
            iID = CInt(m_vDataArray(ACArrayComplementaryTypeID, iIndex))
            m_lReturn = CType(CheckInUse(v_iTypeID:=iID, r_bInUse:=bInUse), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If it's in use then don't let it be deleted
            If bInUse Then
                sMsg = "Unable to delete this relationship as it's partner is in use by one or more parties."
                MessageBox.Show(sMsg, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' Warn user first
            If CDbl(m_vDataArray(ACArrayComplementaryTypeID, iIndex)) <> 0 Then

                ' Ask if they're sure they want to delete it
                sMsg = "This type has a relation. Deleting it will break the relationship." & Environment.NewLine & _
                       "Do you wish to continue?"
                m_lReturn = CType(MessageBox.Show(sMsg, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question), gPMConstants.PMEReturnCode)
                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    ' Exit it out if they change their mind
                    Return result
                End If

            End If

            ' Get the property
            iIsDeleted = CInt(m_vDataArray(ACArrayIsDeleted, iIndex))

            ' Invert it
            'iIsDeleted = Not iIsDeleted ' This gives 0-> -1.. what the?!?
            If iIsDeleted = 0 Then
                iIsDeleted = 1
            Else
                iIsDeleted = 0
            End If

            ' Set it
            m_vDataArray(ACArrayIsDeleted, iIndex) = iIsDeleted

            If iIsDeleted = 1 Then
                ' Remove the link
                m_vDataArray(ACArrayComplementaryTypeID, iIndex) = 0
                ' Update the links
                m_lReturn = CType(UpdateLinks(v_iTypeA:=CInt(m_vDataArray(ACArrayRelationShipTypeID, iIndex)), v_iTypeB:=0), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Make sure it's updated
            m_bDataChanged = True

            ' Refresh the list
            m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckInUse
    '
    ' Description: Check if a relation is associated with a party.
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckInUse(ByVal v_iTypeID As Integer, ByRef r_bInUse As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default to not in use
            r_bInUse = False

            ' Check that there's some in use
            If Not Information.IsArray(m_vPartyArray) Then
                Return result
            End If

            ' Loop through the array
            For iLoop1 As Integer = 0 To m_vPartyArray.GetUpperBound(1)
                If CInt(m_vPartyArray(0, iLoop1)) = v_iTypeID Then
                    r_bInUse = True
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInUse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInUse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: InterfaceToData
    '
    ' Description: Save's all the data to the business
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.UpdateDetails(v_vArray:=m_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed on UpdateDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DisplayCaptions
    '
    ' Description: Get's captions from the resource file and sets them
    '              on the form.
    '
    ' History: 25/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'iPMFunc.GetResData is commented as GetResData is added to the same project

            'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Store the value for Delete button

            'm_sDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            m_sDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Store the value for Undelete button

            'm_sUnDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            m_sUnDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'tabMainTab.TabPages(0).Text = iPMFunc.GetResData(g_iLanguageID, ACTabTitle1, gPMConstants.PMEResourseFileDataType.PMResString)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwMain.SelectedIndexChanged
        If (lvwMain.SelectedItems.Count > 0) Then
            lvwMain_ItemClick(CType(sender, ListView).SelectedItems(0))
        End If
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMain.SelectedIndex = 0
        End If
    End Sub

End Class