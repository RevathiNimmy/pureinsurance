Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms

Imports SharedFiles
Partial Public Class FrmExternalWorkFlowConfiguration
    Inherits System.Windows.Forms.Form
    Private Const ACClass As String = "FrmExternalWorkFlowConfiguration"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_iUser As Integer
    Private m_sUsername As String = ""
    Private m_lReturn As Integer
    Private m_bInitialised As Boolean
    Private m_vSourceArray As Object
    Private m_vUserGroupInfo(,) As Object

    Private m_bSiriusUser As Boolean
    Private m_vDateDeleted As Object
    Private m_bFormLoading As Boolean
    Private m_vSystemSecurityModel As Object
    Private m_bBackGroundJobEnableForFailure As Boolean
    Private m_oBusiness As Object
    Private m_nExternalWorkflowConfigid As Integer

#Region "Public Property"

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
    Public Property SourceArray() As Object
        Get
            Return m_vSourceArray
        End Get
        Set(ByVal Value As Object)


            m_vSourceArray = Value
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
    ''' <summary>
    ''' Get And Set the Background Job Flag
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BackGroundJobEnableForFailure() As Boolean
        Get
            Return m_bBackGroundJobEnableForFailure
        End Get
        Set(ByVal Value As Boolean)
            m_bBackGroundJobEnableForFailure = Value
        End Set
    End Property

    ''' <summary>
    ''' Get And Set the Background Job  ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExternalWorkflowConfig_id() As Integer
        Get
            Return m_nExternalWorkflowConfigid
        End Get
        Set(ByVal Value As Integer)
            m_nExternalWorkflowConfigid = Value
        End Set
    End Property

#End Region
#Region "Private Methods"

    ''' <summary>
    ''' PopulateUserGroups
    ''' Date:10/07/2014
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulateUserGroups() As Integer

        Dim nResult As Integer = 0
        Dim sTemp As String = ""
        Dim iTemp As Integer
        Dim oListitem As ListViewItem
        Dim sKey As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetExternalWorkflowConfigurationUserGroupInfo(r_vUserGroupInfo:=m_vUserGroupInfo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no user groups fo nothing
            If Not Information.IsArray(m_vUserGroupInfo) Then
                Return nResult
            End If

            'set the flag value
            If Not m_vUserGroupInfo Is Nothing AndAlso m_vUserGroupInfo.Length > 0 Then
                m_bBackGroundJobEnableForFailure = (m_vUserGroupInfo(5, 0)).Trim()
                m_nExternalWorkflowConfigid = (m_vUserGroupInfo(6, 0))
            End If

            lvwGroups.Items.Clear()
            lvwSelectedGroups.Items.Clear()

            For lRow As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)

                sTemp = CStr(m_vUserGroupInfo(2, lRow)).Trim()
                sKey = CStr(m_vUserGroupInfo(0, lRow)).Trim()

                If CStr(m_vUserGroupInfo(3, lRow)) <> "0" Then

                    iTemp = CInt(m_vUserGroupInfo(4, lRow))

                    If iTemp = 1 Then
                        oListitem = lvwSelectedGroups.Items.Add("K" & sKey, sTemp, "")
                        oListitem.ImageKey = "user"
                        oListitem.Tag = m_vUserGroupInfo(7, lRow)
                    Else
                        oListitem = lvwSelectedGroups.Items.Add("K" & sKey, sTemp, "")
                        oListitem.ImageKey = "user"
                        oListitem.Tag = m_vUserGroupInfo(7, lRow)
                    End If

                Else
                    oListitem = lvwGroups.Items.Add("K" & sKey, sTemp, "")
                    oListitem.ImageKey = "user"
                    oListitem.Tag = m_vUserGroupInfo(7, lRow)
                End If

            Next lRow

            'Enable/Disable the checkbox
            If BackGroundJobEnableForFailure Then
                chkbackgroundjobforfailure.Checked = True
            Else
                chkbackgroundjobforfailure.Checked = False
            End If

            Return nResult


        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' This methos is used to Update the UserGroups
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateUserGroups() As Integer
        Dim oListitem As ListViewItem
        Dim iIsSupervisor As Integer

        Try

            'Remove Any Unselected Groups
            'For i As Integer = lvwGroups.Items.Count To 1 Step -1

            '    For j As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)
            '        If CInt(CStr(m_vUserGroupInfo(0, j)).Trim()) = CInt(Mid(lvwGroups.Items.Item(i - 1).Name, 2, 5)) And CStr(m_vUserGroupInfo(3, j)).Trim() <> "0" Then

            m_lReturn = g_oBusiness.UpdateExternalWorkflowConfigurationUserGroupInfo(r_lPMUserGroupId:=0, r_iMode:=0)

            '        End If

            '    Next j

            'Next i

            'Add/Update Selected Groups
            'For i As Integer = lvwSelectedGroups.Items.Count To 1 Step -1

            '    oListitem = lvwSelectedGroups.Items.Item(i - 1)

            '    For j As Integer = m_vUserGroupInfo.GetLowerBound(1) To m_vUserGroupInfo.GetUpperBound(1)

            '        If CInt(CStr(m_vUserGroupInfo(0, j)).Trim()) = CInt(Mid(oListitem.Name, 2, 5)) Then

            '            If oListitem.ImageKey = "supervisor" Then
            '                iIsSupervisor = 1
            '            Else
            '                iIsSupervisor = 0
            '            End If

            '            If CStr(m_vUserGroupInfo(3, j)).Trim() = "0" Or (iIsSupervisor <> CInt(CStr(m_vUserGroupInfo(4, j)).Trim())) Then

            '                'New User Group Selected

            '                m_lReturn = g_oBusiness.UpdateExternalWorkflowConfigurationUserGroupInfo(r_lPMUserGroupId:=CInt(Mid(oListitem.Name, 2, 5)), r_iIsSupervisor:=iIsSupervisor, r_iMode:=1)

            '            End If

            '        End If

            '    Next j

            'Next i
            If lvwSelectedGroups.Items.Count > 0 Then
                For i As Integer = lvwSelectedGroups.Items.Count To 1 Step -1

                    oListitem = lvwSelectedGroups.Items.Item(i - 1)
                    m_lReturn = g_oBusiness.UpdateExternalWorkflowConfigurationUserGroupInfo(r_lPMUserGroupId:=CInt(Mid(oListitem.Name, 2, 5)), r_iIsSupervisor:=iIsSupervisor, r_iMode:=1)

                Next
            End If
            Return m_lReturn

        Catch excep As System.Exception

            m_lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn

        End Try
    End Function
    Private isInitializingComponent As Boolean

    Private Sub cmdAddAllGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllGroups.Click

        Dim oListitem As ListViewItem
        Dim sTemp As String = ""
        Dim bSelectSystemAdministrationGroups As Boolean = True
        Dim bShowMsgOnce As Boolean = False

        For i As Integer = lvwGroups.Items.Count To 1 Step -1

            'Set it to true for the very first System Administration Group
            If lvwGroups.Items.Item(i - 1).Tag = 1 AndAlso bSelectSystemAdministrationGroups = True Then
                'If there is a System Administration Group in the list, warn the user and set his choice
                'Ok : select System Administration Group as well + Rest of the groups
                'Cancel : only select groups other than System Administration Group
                If bShowMsgOnce = True = False AndAlso _
                MsgBox("Selecting a user group of type System Administration Group is not recommended. " + "Do you wish to continue ?", _
                       MsgBoxStyle.OkCancel, _
                       "System Administration Group") = MsgBoxResult.Cancel Then
                    bSelectSystemAdministrationGroups = False
                    bShowMsgOnce = True
                Else
                    bShowMsgOnce = True
                End If
            End If

            If lvwGroups.Items.Item(i - 1).Tag = 1 And bSelectSystemAdministrationGroups = True Then
                oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(i - 1).Name, lvwGroups.Items.Item(i - 1).Text, "user")
                oListitem.Tag = lvwGroups.Items.Item(i - 1).Tag
                lvwGroups.Items.RemoveAt(i - 1)
            ElseIf lvwGroups.Items.Item(i - 1).Tag <> 1 Then
                oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(i - 1).Name, lvwGroups.Items.Item(i - 1).Text, "user")
                oListitem.Tag = lvwGroups.Items.Item(i - 1).Tag
                lvwGroups.Items.RemoveAt(i - 1)
            End If

        Next i

    End Sub

    Private Sub cmdAddGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddGroup.Click

        Dim oListitem As ListViewItem
        Dim sTemp As String = ""

        If lvwGroups.Items.Count > 0 Then

            If Not Information.IsNothing(lvwGroups.FocusedItem) Then

                If lvwGroups.FocusedItem.Tag = 1 Then

                    If MsgBox("Selecting a user group of type System Administration Group is not recommended. " + "Do you wish to continue ?", _
                       MsgBoxStyle.OkCancel, _
                       lvwGroups.FocusedItem.Text) = MsgBoxResult.Ok Then

                        oListitem = lvwSelectedGroups.Items.Add(lvwGroups.FocusedItem.Name, lvwGroups.FocusedItem.Text, "user")
                        oListitem.Tag = lvwGroups.FocusedItem.Tag
                        lvwGroups.Items.Remove(lvwGroups.FocusedItem)
                    End If

                Else
                    oListitem = lvwSelectedGroups.Items.Add(lvwGroups.FocusedItem.Name, lvwGroups.FocusedItem.Text, "user")
                    oListitem.Tag = lvwGroups.FocusedItem.Tag
                    lvwGroups.Items.Remove(lvwGroups.FocusedItem)
                End If


            Else
                If lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Tag = 1 Then

                    If MsgBox("Selecting a user group of type System Administration Group is not recommended. " + "Do you wish to continue ?", _
                       MsgBoxStyle.OkCancel, _
                       lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Text) = MsgBoxResult.Ok Then
                        oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Name, lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Text, "user")
                        oListitem.Tag = lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Tag
                        lvwGroups.Items.RemoveAt(lvwGroups.Items.Count - 1)
                    End If

                Else
                    oListitem = lvwSelectedGroups.Items.Add(lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Name, lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Text, "user")
                    oListitem.Tag = lvwGroups.Items.Item(lvwGroups.Items.Count - 1).Tag
                    lvwGroups.Items.RemoveAt(lvwGroups.Items.Count - 1)
                End If

            End If
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdDelAllGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelAllGroups.Click

        Dim oListitem As ListViewItem

        For i As Integer = lvwSelectedGroups.Items.Count To 1 Step -1
            oListitem = lvwGroups.Items.Add(lvwSelectedGroups.Items.Item(i - 1).Name, lvwSelectedGroups.Items.Item(i - 1).Text, "")
            oListitem.Tag = lvwSelectedGroups.Items.Item(i - 1).Tag
            lvwSelectedGroups.Items.RemoveAt(i - 1)
        Next
    End Sub

    Private Sub cmdDelGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelGroup.Click

        Dim oListitem As ListViewItem
        If (lvwSelectedGroups.Items.Count > 0) Then
            If Not (lvwSelectedGroups.FocusedItem Is Nothing) Then
                oListitem = lvwGroups.Items.Add(lvwSelectedGroups.FocusedItem.Name, lvwSelectedGroups.FocusedItem.Text, "")
                oListitem.Tag = lvwSelectedGroups.FocusedItem.Tag
                lvwSelectedGroups.Items.Remove(lvwSelectedGroups.FocusedItem)
            Else
                oListitem = lvwGroups.Items.Add(lvwSelectedGroups.Items(lvwSelectedGroups.Items.Count - 1).Name, lvwSelectedGroups.Items(lvwSelectedGroups.Items.Count - 1).Text, "")
                oListitem.Tag = lvwSelectedGroups.Items(lvwSelectedGroups.Items.Count - 1).Tag
                lvwSelectedGroups.Items.RemoveAt(lvwSelectedGroups.Items.Count - 1)
            End If
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lReturn = Save()
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.Close()
        End If


    End Sub

    Private Sub frmUserMaintenance_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRExternalWorkflowConfiguration.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmUserMaintenance_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_bFormLoading = True

        iPMFunc.ShowFormInTaskBar_Detach()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If
        m_lReturn = PopulateUserGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        m_bFormLoading = False
    End Sub
    Private Sub frmUserMaintenance_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
    End Sub
    Private Sub lvwGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGroups.DoubleClick
        cmdAddGroup_Click(cmdAddGroup, New EventArgs())
    End Sub

    Private Sub lvwRiskGroups_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs)
        Dim Cancel As Boolean = eventArgs.CancelEdit
        Cancel = 1
    End Sub
    Private Sub lvwSelectedGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectedGroups.DoubleClick
        cmdDelGroup_Click(cmdDelGroup, New EventArgs())
    End Sub

    Private Sub lvwSelectedGroups_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSelectedGroups.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        If Button <> 2 Then
            Exit Sub
        End If

        Dim oListitem As ListViewItem = lvwSelectedGroups.GetItemAt(x, y)
        If oListitem Is Nothing Then
            Exit Sub
        End If



        mnuSuper.Checked = oListitem.ImageKey = "supervisor"
        Ctx_mnuSupervisor.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

        If mnuSuper.Checked Then


            oListitem.ImageKey = "supervisor"
        Else


            oListitem.ImageKey = "user"
        End If

        oListitem = Nothing

    End Sub
    Private Function SetProductOption(ByVal r_vOption As Object, ByVal r_vBranch As Object, ByVal r_vValue As Object) As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = g_oBusiness.SetProductOptionValue(r_vOption:=r_vOption, r_vBranch:=r_vBranch, r_vValue:=r_vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result



        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProductOption Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProductOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ''' <summary>
    ''' Update the Records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateRecords() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMFalse

            nResult = UpdateUserGroups()

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the BackGround job Flag
            nResult = g_oBusiness.UpdateExternalWorkflowConfigFlag(o_bEnablebackgroundjob_ForFailure:=IIf(chkbackgroundjobforfailure.Checked = True, 1, 0) _
                                                                   , o_lExternalWorkFlowConfigID:=ExternalWorkflowConfig_id)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRecords Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRecords", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    Private Sub frmUserMaintenance_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'If e.Alt And e.KeyCode = Keys.U Then
        '    tabMain.SelectedIndex = 0
        'End If

        'If e.Alt And e.KeyCode = Keys.S Then
        '    tabMain.SelectedIndex = 1
        'End If

        'If tabMain.SelectedIndex = 0 Then
        '    If e.Alt And e.KeyCode = Keys.D1 Then
        '        SSTab1.SelectedIndex = 0
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D2 Then
        '        SSTab1.SelectedIndex = 1
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D3 Then
        '        SSTab1.SelectedIndex = 2
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D4 Then
        '        SSTab1.SelectedIndex = 3
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D5 Then
        '        SSTab1.SelectedIndex = 4
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D6 Then
        '        SSTab1.SelectedIndex = 5
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D7 Then
        '        SSTab1.SelectedIndex = 6
        '    End If
        '    If e.Alt And e.KeyCode = Keys.D8 Then
        '        SSTab1.SelectedIndex = 7
        '    End If
        'End If
        'If tabMain.SelectedIndex = 0 And SSTab1.SelectedIndex = 4 Then
        '    If e.Alt And e.KeyCode = Keys.A Then
        '        SSTab2.SelectedIndex = 0
        '    End If

        '    If e.Alt And e.KeyCode = Keys.P Then
        '        SSTab2.SelectedIndex = 1
        '    End If
        '    If e.Alt And e.KeyCode = Keys.L Then
        '        SSTab2.SelectedIndex = 2
        '    End If
        'End If
    End Sub
#End Region
#Region "Public Methods"
    Public Function InitialForm() As Integer

        'Dim result As Integer = 0
        'Try

        '    result = gPMConstants.PMEReturnCode.PMTrue

        '    m_colDomainUsersData = New Collection()

        '    m_bPasswordChanged = False
        '    m_lReturn = iPMFunc.GetSystemSecurityModel(m_vSystemSecurityModel)
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve System Security Model", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        '        Return gPMConstants.PMEReturnCode.PMFalse
        '    End If
        '    If Convert.ToString(m_vSystemSecurityModel) = "" Then
        '        m_vSystemSecurityModel = 0
        '    End If


        'Return result

        'Catch excep As System.Exception
        '    'Error Section
        '    result = gPMConstants.PMEReturnCode.PMError

        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '    Return result

        'End Try
    End Function
    Public Sub mnuSuper_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSuper.Click
        mnuSuper.Checked = Not (mnuSuper.Checked)
    End Sub
    ''' <summary>
    ''' Save the User Groups
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Save() As Integer
        Dim nResult As Integer = 0
        Dim sNewUser As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMFalse

            'Update Individual User Details
            sNewUser = m_sUsername

            m_lReturn = UpdateRecords()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return nResult
            End If
            nResult = gPMConstants.PMEReturnCode.PMTrue

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Save Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
#End Region

End Class
