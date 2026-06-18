Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("cboPMUserGroupByTask_NET.cboPMUserGroupByTask")> _
Partial Public Class cboPMUserGroupByTask
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event UserGroupnameChange()
    Public Event FirstItemChange()
    Public Event UserGroupIDChange()
    Public Event SingleUserGroupIDChange()
    Public Event DefaultTaskGroupIDChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event ItemDataChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()

    'Default Property Values:
    Const m_def_PMTaskGroupID As Integer = 0
    Const m_def_UserGroupID As Integer = 0
    Const m_def_UserGroupname As String = ""
    Const m_def_DefaultTaskGroupID As Integer = 0
    Const m_def_SingleUserGroupID As Integer = 0
    Const m_def_FirstItem As String = ""

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oUserGroupLookupBusiness As bPMUserGroup.Lookup
    'Private m_oUserGroupLookupBusiness As bPMUserGroup.Lookup
    Private m_vUserGroupsArray(,) As Object

    'Property Variables:
    Private m_lPMTaskGroupID As Integer
    Private m_lUserGroupID As Integer
    Private m_sUserGroupname As String = ""
    Private m_lDefaultTaskGroupID As Integer
    Private m_lSingleUserGroupID As Integer
    Private m_sFirstItem As String = ""

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboUserGroups,cboUserGroups,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "UserGroupLookup"

    'RSB CQ2065
    Private m_sToolTips() As String
    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboUserGroups.BackColor
        End Get
        Set(ByVal Value As Color)
            cboUserGroups.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboUserGroups.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboUserGroups.ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    <Browsable(True)> _
    <Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)
            MyBase.Enabled = Value
            cboUserGroups.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled
    'developer guide no solution no. 35 (ByVal value As Boolean) added
    Public Property Sorted() As Boolean
        Get
            Return cboUserGroups.Sorted
        End Get
        Set(ByVal value As Boolean)
            cboUserGroups.Sorted = True
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboUserGroups.Font
        End Get
        Set(ByVal Value As Font)
            cboUserGroups.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle
    'developers guide no solution no. 14
    '<Browsable(False)> _
    '<Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    '	Public ReadOnly Property BackStyle() As Integer
    '	Get

    '		Return MyBase.BackStyle
    '	End Get
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BorderStyle
    <Browsable(False)> _
    <Description("Returns/sets the border style for an object.")> _
    Public Shadows ReadOnly Property BorderStyle() As Integer
        Get

            Return MyBase.BorderStyle
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,ActiveControl
    <Browsable(False)> _
    <Description("Returns the control that has focus.")> _
    Public Shadows ReadOnly Property ActiveControl() As Object
        Get
            Return MyBase.ActiveControl
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,ItemData

    Private Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboUserGroups, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboUserGroups, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,List

    <Browsable(False)> _
    <Description("Returns/sets the items contained in a control's list portion.")> _
    Public Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboUserGroups, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboUserGroups, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    ' RDC 04092002
    <Browsable(False)> _
    Public ReadOnly Property UserGroups() As Object
        Get
            'developer guide no. 146
            Return m_vUserGroupsArray
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboUserGroups.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboUserGroups.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            cboUserGroups.SelectedIndex = Value
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboUserGroups)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboUserGroups, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,WhatsThisHelpID
    'developers guide no solution no 15
    '	<Browsable(True)> _
    '	<Description("Returns/sets an associated context number for an object.")> _
    '	Public Property WhatsThisHelpID() As Integer
    '		Get

    '			Return cboUserGroups.WhatsThisHelpID
    '		End Get
    '		Set(ByVal Value As Integer)

    '			cboUserGroups.WhatsThisHelpID = Value
    '			RaiseEvent WhatsThisHelpIDChange()
    '		End Set
    '	End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,NewIndex
    'UPGRADE_NOTE: (7001) The following declaration (get NewIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function NewIndex() As Integer
    'Dim cboUserGroups_NewIndex As Integer = -1
    'Return cboUserGroups_NewIndex
    'End Function

    ' BB040298 Set the Type of Control to Combo for Form Control
    <Browsable(False)> _
    Public ReadOnly Property TypeOfControl() As Integer
        Get
            Return gPMConstants.PMEControlType.PMCombo
        End Get
    End Property


    <Browsable(True)> _
    <Description("The name of the table to ge the lookup values from.")> _
    <Category("Data")> _
    Public Property PMTaskGroupID() As Integer
        Get
            Return m_lPMTaskGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lPMTaskGroupID = Value
        End Set
    End Property


    <Browsable(True)> _
    <Description("Set this to the Id of the item that you want selected by default.")> _
    Public Property DefaultTaskGroupID() As Integer
        Get
            Return m_lDefaultTaskGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lDefaultTaskGroupID = Value
            RaiseEvent DefaultTaskGroupIDChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("If you only want to retrieve a single item, set the id here. Note: The Id specified here will also become the default id. Think performance! Why get the whole list when you only want to diaplay one value.")> _
    Public Property SingleUserGroupID() As Integer
        Get
            Return m_lSingleUserGroupID
        End Get
        Set(ByVal Value As Integer)

            m_lSingleUserGroupID = Value
            RaiseEvent SingleUserGroupIDChange()

            ' If a single Item has been set then this is also the Default Item
            DefaultTaskGroupID = SingleUserGroupID

        End Set
    End Property


    <Browsable(False)> _
    <Description("The Item id of the type entry")> _
    <Category("Data")> _
    Public Property UserGroupID() As Integer
        Get
            Return m_lUserGroupID
        End Get
        Set(ByVal Value As Integer)
            'If DesignMode Then Throw New System.Exception("382")

            m_lUserGroupID = Value
            RaiseEvent UserGroupIDChange()
            With cboUserGroups
                For nIndex As Integer = 0 To .Items.Count - 1
                    If VB6.GetItemData(cboUserGroups, nIndex) = m_lUserGroupID Then
                        .SelectedIndex = nIndex
                        Exit For
                    End If
                Next nIndex
            End With

        End Set
    End Property

    <Browsable(False)> _
    <Description("Description of the Type entry")> _
    <Category("Data")> _
    Public ReadOnly Property ItemUserGroupname(Optional ByVal v_vUserGroupID As Object = Nothing) As String
        Get
            Dim nIndex As Integer

            If Information.IsNothing(v_vUserGroupID) Then
                Return m_sUserGroupname
            Else

                nIndex = IndexOfItem(CInt(v_vUserGroupID))
                If nIndex < 0 Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboUserGroups, nIndex)
                End If
            End If
        End Get
    End Property


    <Browsable(True)> _
    <Description("String to be entered in the list before the TypeTable entries")> _
    <Category("Data")> _
    Public Property FirstItem() As String
        Get
            Return m_sFirstItem
        End Get
        Set(ByVal Value As String)
            ' developer guide no. 36
            m_sFirstItem = Value
            RaiseEvent FirstItemChange()
            If Not DesignMode Then
                RefreshList()
            End If
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub

    Private Sub cboUserGroups_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroups.SelectedIndexChanged

        With cboUserGroups
            If (cboUserGroups.SelectedIndex <> -1) Then
                m_lUserGroupID = VB6.GetItemData(cboUserGroups, .SelectedIndex)
                RaiseEvent UserGroupIDChange()
                m_sUserGroupname = VB6.GetItemString(cboUserGroups, .SelectedIndex)
                RaiseEvent UserGroupnameChange()
            End If
        End With
        RaiseEvent Click(Me, Nothing)

        'RSB CQ2065
        'developer guide no.  PM todolist
        'If cboUserGroups.SelectedIndex <> -1 Then
        '	ToolTip1.SetToolTip(cboUserGroups, m_sToolTips(cboUserGroups.SelectedIndex))
        'End If

    End Sub

    Private Sub cboPMUserGroupByTask_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboPMUserGroupByTask_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub
    'developer guide no.  todolist
    'Private Sub cboPMUserGroupByTask_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
    '	Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '	RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
    '	If KeyAscii = 0 Then
    '		eventArgs.Handled = True
    '	End If
    '	eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub
    Private Sub cboPMUserGroupByTask_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

    End Sub

    Private Sub cboPMUserGroupByTask_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboPMUserGroupByTask_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMUserGroupByTask_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMUserGroupByTask_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,AddItem
    'UPGRADE_NOTE: (7001) The following declaration (AddItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
    'cboUserGroups.Items.Insert(Index, Item)
    'End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUserGroups,cboUserGroups,-1,RemoveItem
    'UPGRADE_NOTE: (7001) The following declaration (RemoveItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub RemoveItem(ByRef Index As Integer)
    'cboUserGroups.Items.RemoveAt(CShort(Index))
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'developers guide no solution no. 2
        'Font = Ambient.Font
        Font = Me.Font
        m_lPMTaskGroupID = m_def_PMTaskGroupID
        m_lUserGroupID = m_def_UserGroupID
        m_sUserGroupname = m_def_UserGroupname
        m_sFirstItem = m_def_FirstItem
    End Sub

    'Load property values from storage


    'developers guide no solution no. 1
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        Dim dtDefEffectiveDate As Date = DateTime.Now




        cboUserGroups.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboUserGroups.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        cboUserGroups.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developers guide no solution no. 2
        'Font = PropBag.ReadProperty("Font", Ambient.Font)
        Font = PropBag.ReadProperty("Font", Me.Font)



        'developers guide no solution no. 14
        ' MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboUserGroups, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developers guide no solution no. 15
        'cboUserGroups.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_lPMTaskGroupID = CInt(PropBag.ReadProperty("PMTaskGroupID", m_def_PMTaskGroupID))


        m_lUserGroupID = CInt(PropBag.ReadProperty("UserGroupID", m_def_UserGroupID))


        m_lDefaultTaskGroupID = CInt(PropBag.ReadProperty("DefaultTaskGroupID", m_def_DefaultTaskGroupID))


        m_lSingleUserGroupID = CInt(PropBag.ReadProperty("SingleUserGroupID", m_def_SingleUserGroupID))


        m_sUserGroupname = CStr(PropBag.ReadProperty("UserGroupname", m_def_UserGroupname))


        m_sFirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        ' Read the list of type PMTaskGroupID entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub cboPMUserGroupByTask_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboUserGroups.Height
        cboUserGroups.Width = MyBase.Width
    End Sub

    'Write property values to storage


    'developers guide no solution no. 1
    '	Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        Dim dtDefEffectiveDate As Date = DateTime.Now


        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboUserGroups.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboUserGroups.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboUserGroups.Sorted, False)

        PropBag.WriteProperty("Enabled", Enabled, True)


        'developers guide no solution no. 2
        'PropBag.WriteProperty("Font", Font, Ambient.Font)
        PropBag.WriteProperty("Font", Font, Me.Font)


        'developers guide no solution no. 14
        '	PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboUserGroups), "")


        'developers guide no solution no. 15
        'PropBag.WriteProperty("WhatsThisHelpID", cboUserGroups.WhatsThisHelpID, 0)

        PropBag.WriteProperty("PMTaskGroupID", m_lPMTaskGroupID, m_def_PMTaskGroupID)

        PropBag.WriteProperty("UserGroupID", m_lUserGroupID, m_def_UserGroupID)

        PropBag.WriteProperty("DefaultTaskGroupID", m_lDefaultTaskGroupID, m_def_DefaultTaskGroupID)

        PropBag.WriteProperty("SingleUserGroupID", m_lSingleUserGroupID, m_def_SingleUserGroupID)

        PropBag.WriteProperty("UserGroupname", m_sUserGroupname, m_def_UserGroupname)

        PropBag.WriteProperty("FirstItem", m_sFirstItem, m_def_FirstItem)
    End Sub
    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Try

            If Not DesignMode Then
                cboUserGroups.Items.Clear()
                ' Entry at top of list for (All) or (None) etc
                If m_sFirstItem <> "" Then
                    Dim cboUserGroups_NewIndex As Integer = -1
                    cboUserGroups_NewIndex = cboUserGroups.Items.Add(m_sFirstItem)
                    VB6.SetItemData(cboUserGroups, cboUserGroups_NewIndex, 0)
                End If
                m_lReturn = CType(GetUserGroups(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp + ", " + "Failed to get Users for PMTaskGroupID : " & PMTaskGroupID)
                End If
                ' Having filled the combo set it to it's default position
                If m_lDefaultTaskGroupID <> 0 Then
                    UserGroupID = m_lDefaultTaskGroupID
                Else
                    If cboUserGroups.Items.Count > 0 Then
                        cboUserGroups.SelectedIndex = 0
                    End If
                End If
            End If

        Catch excep As System.Exception



            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Sub

        End Try

    End Sub

    Private Function IndexOfItem(ByVal v_lUserGroupID As Integer) As Integer
        With cboUserGroups
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboUserGroups, nIndex) = v_lUserGroupID Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    Private Function GetUserGroupLookupBusiness() As Integer

        Dim result As Integer = 0
        Try
            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oUserGroupLookupBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                Dim temp_m_oUserGroupLookupBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oUserGroupLookupBusiness, "bPMUserGroup.Lookup", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oUserGroupLookupBusiness = temp_m_oUserGroupLookupBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypePMTaskGroupID business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupLookupBusiness")
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type PMTaskGroupID business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupLookupBusiness", excep:=excep)

            Return result

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                m_oObjectManager = Nothing
                m_oUserGroupLookupBusiness = Nothing
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' Initialise the Object Manager Etc
    Private Function Initialise() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Have we been here before ?
            If Not (m_oObjectManager Is Nothing) Then
                Return result
            End If

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                m_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With m_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetUserGroups() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            m_lReturn = CType(GetUserGroupLookupBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If PMTaskGroupID < 1 Then
                ' Get the Users

                m_lReturn = m_oUserGroupLookupBusiness.GetAllEffectiveGroups(v_dtEffectiveDate:=DateTime.Now, r_vUserGroupsArray:=m_vUserGroupsArray)
            Else

                m_lReturn = m_oUserGroupLookupBusiness.GetAllEffectiveGroupsByTask(v_lPMTaskGroupID:=PMTaskGroupID, v_dtEffectiveDate:=DateTime.Now, r_vUserGroupsArray:=m_vUserGroupsArray)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroups")
                Return result
            End If

            ' Add to the drop down list box.
            If Information.IsArray(m_vUserGroupsArray) Then
                For lRow As Integer = m_vUserGroupsArray.GetLowerBound(1) To m_vUserGroupsArray.GetUpperBound(1)
                    ' If we only want to add one user
                    If m_lSingleUserGroupID > 0 Then
                        ' Check to see if this is the user to add
                        If m_lSingleUserGroupID = CInt(m_vUserGroupsArray(0, lRow)) Then
                            Dim cboUserGroups_NewIndex As Integer = -1
                            cboUserGroups_NewIndex = cboUserGroups.Items.Add(CStr(m_vUserGroupsArray(2, lRow)))
                            VB6.SetItemData(cboUserGroups, cboUserGroups_NewIndex, CInt(m_vUserGroupsArray(0, lRow)))

                            'RSB CQ2065 - populate the tooltip array with the full length text
                            ReDim Preserve m_sToolTips(cboUserGroups.Items.Count)
                            m_sToolTips(cboUserGroups_NewIndex) = CStr(m_vUserGroupsArray(2, lRow))

                        End If
                    Else
                        ' Add All Users returned.
                        'cboUserGroups_NewIndex = cboUserGroups.Items.Add(CStr(m_vUserGroupsArray(2, lRow)))
                        '	VB6.SetItemData(cboUserGroups, cboUserGroups_NewIndex, CInt(m_vUserGroupsArray(0, lRow)))

                        'RSB CQ2065 - populate the tooltip array with the full length text
                        '	ReDim Preserve m_sToolTips(cboUserGroups.Items.Count)
                        '	m_sToolTips(cboUserGroups_NewIndex) = CStr(m_vUserGroupsArray(2, lRow))
                        Dim cboUserGroups_NewIndex As Integer
                        'developer guide no. added 25 Jan 2010
                        cboUserGroups_NewIndex = cboUserGroups.Items.Add(New VB6.ListBoxItem(m_vUserGroupsArray(2, lRow), CInt(m_vUserGroupsArray(0, lRow))))
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroups", excep:=excep)

            Return result

        End Try
    End Function
End Class
