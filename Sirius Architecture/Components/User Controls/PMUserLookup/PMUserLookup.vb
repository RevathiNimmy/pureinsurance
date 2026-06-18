Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("cboPMUserLookup_NET.cboPMUserLookup")> _
Partial Public Class cboPMUserLookup
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event UsernameChange()
    Public Event FirstItemChange()
    Public Event UserIDChange()
    Public Event SingleUserIDChange()
    Public Event DefaultUserIDChange()
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
    Const m_def_PMUserGroupID As Integer = 0
    Const m_def_UserID As Integer = 0
    Const m_def_Username As String = ""
    Const m_def_DefaultUserID As Integer = 0
    Const m_def_SingleUserID As Integer = 0
    Const m_def_FirstItem As String = ""

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oUserLookupBusiness As bPMUserGroup.Lookup
    Private m_vUsersArray(,) As Object

    'Property Variables:
    Private m_lPMUserGroupID As Integer
    Private m_lUserID As Integer
    Private m_sUsername As String = ""
    Private m_lDefaultUserID As Integer
    Private m_lSingleUserID As Integer
    Private m_sFirstItem As String = ""
    Private m_lPartyCnt As Integer

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboUsers,cboUsers,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "UserLookup"


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboUsers.BackColor
        End Get
        Set(ByVal Value As Color)
            cboUsers.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboUsers.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboUsers.ForeColor = Value
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
            cboUsers.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled
    '<Browsable(False)> _
    'developer guide no. 35
    Public Property Sorted() As Boolean
        Get
            Return cboUsers.Sorted
        End Get
        Set(ByVal Value As Boolean)
            cboUsers.Sorted = Value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboUsers.Font
        End Get
        Set(ByVal Value As Font)
            cboUsers.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle
    <Browsable(False)> _
    <Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    Public ReadOnly Property BackStyle() As Integer
        Get

            'TODO
            'Return MyBase.BackStyle
        End Get
    End Property

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
    'MappingInfo=cboUsers,cboUsers,-1,ItemData

    Private Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboUsers, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboUsers, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,List

    Private Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboUsers, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboUsers, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboUsers.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboUsers.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            cboUsers.SelectedIndex = Value
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboUsers)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboUsers, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,WhatsThisHelpID
    'TODO
    '<Browsable(True)> _
    '<Description("Returns/sets an associated context number for an object.")> _
    'Public Property WhatsThisHelpID() As Integer
    '	Get

    '		Return cboUsers.WhatsThisHelpID
    '	End Get
    '	Set(ByVal Value As Integer)

    '		cboUsers.WhatsThisHelpID = Value
    '		RaiseEvent WhatsThisHelpIDChange()
    '	End Set
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,NewIndex
    'UPGRADE_NOTE: (7001) The following declaration (get NewIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function NewIndex() As Integer
    'Dim cboUsers_NewIndex As Integer = -1
    'Return cboUsers_NewIndex
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
    Public Property PMUserGroupID() As Integer
        Get
            Return m_lPMUserGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lPMUserGroupID = Value
        End Set
    End Property

    <Browsable(True)> _
    <Description("Set this property if we want users attached to a specific Party.")> _
    <Category("Data")> _
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property


    <Browsable(True)> _
    <Description("Set this to the Id of the item that you want selected by default.")> _
    Public Property DefaultUserID() As Integer
        Get
            Return m_lDefaultUserID
        End Get
        Set(ByVal Value As Integer)
            m_lDefaultUserID = Value
            RaiseEvent DefaultUserIDChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("If you only want to retrieve a single item, set the id here. Note: The Id specified here will also become the default id. Think performance! Why get the whole list when you only want to diaplay one value.")> _
    Public Property SingleUserID() As Integer
        Get
            Return m_lSingleUserID
        End Get
        Set(ByVal Value As Integer)

            m_lSingleUserID = Value
            RaiseEvent SingleUserIDChange()

            ' If a single Item has been set then this is also the Default Item
            DefaultUserID = SingleUserID

        End Set
    End Property


    <Browsable(False)> _
    <Description("The Item id of the type entry")> _
    <Category("Data")> _
    Public Property UserID() As Integer
        Get
            Return m_lUserID
        End Get
        Set(ByVal Value As Integer)
            'If DesignMode Then Throw New System.Exception("382")

            m_lUserID = Value
            RaiseEvent UserIDChange()
            With cboUsers
                For nIndex As Integer = 0 To .Items.Count - 1
                    If VB6.GetItemData(cboUsers, nIndex) = m_lUserID Then
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
    Public ReadOnly Property ItemUsername(Optional ByVal v_vUserID As Object = Nothing,Optional ByVal v_bRefresh As Boolean =True) As String
        Get
            Dim nIndex As Integer

            If Information.IsNothing(v_vUserID) Then
                Return m_sUsername
            Else

                nIndex = IndexOfItem(CInt(v_vUserID))

                'DJM 04/02/2004 : Refresh the list (if set to all users) and try again.
                If nIndex < 0 And cboUsers.SelectedIndex = 0 Then
                      If v_bRefresh  then
                        RefreshList()
                        Refresh()
                      End If

                    nIndex = IndexOfItem(CInt(v_vUserID))
                End If

                If nIndex < 0 Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboUsers, nIndex)
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
            If (Value.IndexOf("("c) + 1) = 0 Then
                m_sFirstItem = "(" & Value & ")"
            Else
                m_sFirstItem = Value
            End If
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

    Private Sub cboUsers_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUsers.SelectedIndexChanged

        With cboUsers
            'if added as cboUsers.SelectedIndex is -1 it throws error
            If (cboUsers.SelectedIndex <> -1) Then
                m_lUserID = VB6.GetItemData(cboUsers, .SelectedIndex)
                RaiseEvent UserIDChange()
                m_sUsername = VB6.GetItemString(cboUsers, .SelectedIndex)
                RaiseEvent UsernameChange()
            End If
        End With
        RaiseEvent Click(Me, Nothing)

    End Sub

    Private Sub cboPMUserLookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboPMUserLookup_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub

    'TODO
    'Private Sub cboPMUserLookup_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
    '	Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '	RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
    '	If KeyAscii = 0 Then
    '		eventArgs.Handled = True
    '	End If
    '	eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub
    Private Sub cboPMUserLookup_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboPMUserLookup_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboPMUserLookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMUserLookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMUserLookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,AddItem
    'UPGRADE_NOTE: (7001) The following declaration (AddItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
    'cboUsers.Items.Insert(Index, Item)
    'End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboUsers,cboUsers,-1,RemoveItem
    'UPGRADE_NOTE: (7001) The following declaration (RemoveItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub RemoveItem(ByRef Index As Integer)
    'cboUsers.Items.RemoveAt(CShort(Index))
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'developer guide no 2. no solution
        'Font = Ambient.Font
        Font = Me.Font
        m_lPMUserGroupID = m_def_PMUserGroupID
        m_lUserID = m_def_UserID
        m_sUsername = m_def_Username
        m_sFirstItem = m_def_FirstItem
    End Sub

    'Load property values from storage


    'developer guide no 1. no solution
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        Dim dtDefEffectiveDate As Date = DateTime.Now




        cboUsers.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboUsers.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        cboUsers.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer guide no.2 no solution
        'Font = PropBag.ReadProperty("Font", Ambient.Font)



        'Tarun
        'MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboUsers, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developer guide no 15. no solution
        'cboUsers.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_lPMUserGroupID = CInt(PropBag.ReadProperty("PMUserGroupID", m_def_PMUserGroupID))


        m_lUserID = CInt(PropBag.ReadProperty("UserID", m_def_UserID))


        m_lDefaultUserID = CInt(PropBag.ReadProperty("DefaultUserID", m_def_DefaultUserID))


        m_lSingleUserID = CInt(PropBag.ReadProperty("SingleUserID", m_def_SingleUserID))


        m_sUsername = CStr(PropBag.ReadProperty("Username", m_def_Username))


        m_sFirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        ' Read the list of type PMUserGroupID entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub cboPMUserLookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboUsers.Height
        cboUsers.Width = MyBase.Width
    End Sub

    'Write property values to storage
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        Dim dtDefEffectiveDate As Date = DateTime.Now


        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboUsers.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboUsers.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboUsers.Sorted, False)

        PropBag.WriteProperty("Enabled", Enabled, True)


        'developer guide no 2. no solution
        'PropBag.WriteProperty("Font", Font, Ambient.Font)


        'TODO
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboUsers), "")


        'developer guide no 15. no solution.
        'PropBag.WriteProperty("WhatsThisHelpID", cboUsers.WhatsThisHelpID, 0)

        PropBag.WriteProperty("PMUserGroupID", m_lPMUserGroupID, m_def_PMUserGroupID)

        PropBag.WriteProperty("UserID", m_lUserID, m_def_UserID)

        PropBag.WriteProperty("DefaultUserID", m_lDefaultUserID, m_def_DefaultUserID)

        PropBag.WriteProperty("SingleUserID", m_lSingleUserID, m_def_SingleUserID)

        PropBag.WriteProperty("Username", m_sUsername, m_def_Username)

        PropBag.WriteProperty("FirstItem", m_sFirstItem, m_def_FirstItem)
    End Sub
    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Try

            If Not DesignMode Then
                cboUsers.Items.Clear()
                ' Entry at top of list for (All) or (None) etc
                If m_sFirstItem <> "" Then
                    Dim cboUsers_NewIndex As Integer = -1
                    cboUsers_NewIndex = cboUsers.Items.Add(m_sFirstItem)
                    VB6.SetItemData(cboUsers, cboUsers_NewIndex, 0)
                End If
                m_lReturn = CType(GetUsers(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp + ", " + "Failed to get Users for PMUserGroupID : " & PMUserGroupID)
                End If
                ' Having filled the combo set it to it's default position
                If m_lDefaultUserID <> 0 Then
                    UserID = m_lDefaultUserID
                Else
                    If cboUsers.Items.Count > 0 Then
                        cboUsers.SelectedIndex = 0
                    End If
                End If
            End If

        Catch excep As System.Exception



            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Sub

        End Try

    End Sub

    Private Function IndexOfItem(ByVal v_lUserID As Integer) As Integer
        With cboUsers
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboUsers, nIndex) = v_lUserID Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    Private Function GetUserLookupBusiness() As Integer

        Dim result As Integer = 0
        Try
            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oUserLookupBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                m_lReturn = m_oObjectManager.GetInstance(m_oUserLookupBusiness, "bPMUserGroup.Lookup", vInstanceManager:=gPMConstants.PMGetViaClientManager)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypePMUserGroupID business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserLookupBusiness")
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type PMUserGroupID business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserLookupBusiness", excep:=excep)

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
                m_oUserLookupBusiness = Nothing
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
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

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
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetUsers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            m_lReturn = CType(GetUserLookupBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If PMUserGroupID < 1 Then
                ' Get the Users

                m_lReturn = m_oUserLookupBusiness.GetAllEffectiveUsers(v_dtEffectiveDate:=DateTime.Now, r_vAllUsersArray:=m_vUsersArray)
            Else

                m_lReturn = m_oUserLookupBusiness.GetGroupEffectiveUsers(v_lPMUserGroupID:=PMUserGroupID, v_dtEffectiveDate:=DateTime.Now, r_vGroupUsersArray:=m_vUsersArray, v_iPartyCnt:=m_lPartyCnt)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsers")
                Return result
            End If

            ' Add to the drop down list box.
            If Information.IsArray(m_vUsersArray) Then
                For lRow As Integer = m_vUsersArray.GetLowerBound(1) To m_vUsersArray.GetUpperBound(1)
                    ' If we only want to add one user
                    If m_lSingleUserID > 0 Then
                        ' Check to see if this is the user to add
                        If m_lSingleUserID = CInt(m_vUsersArray(0, lRow)) Then
                            'Dim cboUsers_NewIndex As Integer = -1
                            'cboUsers_NewIndex = cboUsers.Items.Add(CStr(m_vUsersArray(1, lRow)))
                            'VB6.SetItemData(cboUsers, cboUsers_NewIndex, CInt(m_vUsersArray(0, lRow)))
                            Dim cboUsers_NewIndex As Integer = -1

                            cboUsers_NewIndex = cboUsers.Items.Add(New VB6.ListBoxItem(m_vUsersArray(1, lRow), CInt(m_vUsersArray(0, lRow))))

                        End If
                    Else
                        ' Add All Users returned.
                        'cboUsers_NewIndex = cboUsers.Items.Add(m_vUsersArray(1, lRow))
                        'VB6.SetItemData(cboUsers, cboUsers_NewIndex, CInt(m_vUsersArray(0, lRow)))
                        'TODO
                        'Dim index As Integer = cboUsers.Items.Add(m_vUsersArray(1, lRow))
                        Dim cboUsers_NewIndex As Integer = -1
                        'VB6.SetItemData(cboUsers, index, CInt(m_vUsersArray(0, lRow)))
                        cboUsers_NewIndex = cboUsers.Items.Add(New VB6.ListBoxItem(m_vUsersArray(1, lRow), CInt(m_vUsersArray(0, lRow))))


                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsers", excep:=excep)

            Return result

        End Try
    End Function
End Class
