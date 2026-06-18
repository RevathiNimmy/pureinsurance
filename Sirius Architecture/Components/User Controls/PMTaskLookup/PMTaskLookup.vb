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
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("cboPMTaskLookup_NET.cboPMTaskLookup")> _
Partial Public Class cboPMTaskLookup
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event TasknameChange()
    Public Event FirstItemChange()
    Public Event TaskIDChange()
    Public Event SingleTaskIDChange()
    Public Event DefaultTaskIDChange()
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
    Const m_def_TaskID As Integer = 0
    Const m_def_Taskname As String = ""
    Const m_def_DefaultTaskID As Integer = 0
    Const m_def_SingleTaskID As Integer = 0
    Const m_def_FirstItem As String = ""

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oTaskLookupBusiness As bPMTaskGroup.Lookup
    'Private m_oTaskLookupBusiness As bPMTaskGroup.Lookup
    'Private m_vUsersArray( ,  ) As Object
    Private m_vUsersArray As Object

    'Property Variables:
    Private m_lPMTaskGroupID As Integer
    Private m_lTaskID As Integer
    Private m_sTaskname As String = ""
    Private m_lDefaultTaskID As Integer
    Private m_lSingleTaskID As Integer
    Private m_sFirstItem As String = ""
    Private m_bShowRequiresKeys As Boolean

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboTasks,cboTasks,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "TaskLookup"


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboTasks.BackColor
        End Get
        Set(ByVal Value As Color)
            cboTasks.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboTasks.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboTasks.ForeColor = Value
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
            cboTasks.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled
    <Browsable(False)> _
    Public ReadOnly Property Sorted() As Boolean
        Get
            Return cboTasks.Sorted
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboTasks.Font
        End Get
        Set(ByVal Value As Font)
            cboTasks.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle
    <Browsable(False)> _
    <Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    Public ReadOnly Property BackStyle() As Integer
        Get

            'devoloper guide no 14(No solutions)
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
    'MappingInfo=cboTasks,cboTasks,-1,ItemData

    'Private Property ItemData(ByVal Index As Integer) As Integer 'tarun modiifed
    Public Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboTasks, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboTasks, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,List

    '    Private Property List(ByVal Index As Integer) As String 'tarun modiifed
    Public Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboTasks, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboTasks, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboTasks.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboTasks.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            cboTasks.SelectedIndex = Value
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboTasks)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboTasks, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,WhatsThisHelpID
    'Tarun	
    '<Browsable(True)> _
    '<Description("Returns/sets an associated context number for an object.")> _
    'Public Property WhatsThisHelpID() As Integer
    '	Get

    '		Return cboTasks.WhatsThisHelpID
    '	End Get
    '	Set(ByVal Value As Integer)

    '		cboTasks.WhatsThisHelpID = Value
    '		RaiseEvent WhatsThisHelpIDChange()
    '	End Set
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,NewIndex
    'UPGRADE_NOTE: (7001) The following declaration (get NewIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function NewIndex() As Integer
    'Dim cboTasks_NewIndex As Integer = -1
    'Return cboTasks_NewIndex
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
    Public Property DefaultTaskID() As Integer
        Get
            Return m_lDefaultTaskID
        End Get
        Set(ByVal Value As Integer)
            m_lDefaultTaskID = Value
            RaiseEvent DefaultTaskIDChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("If you only want to retrieve a single item, set the id here. Note: The Id specified here will also become the default id. Think performance! Why get the whole list when you only want to diaplay one value.")> _
    Public Property SingleTaskID() As Integer
        Get
            Return m_lSingleTaskID
        End Get
        Set(ByVal Value As Integer)

            m_lSingleTaskID = Value
            RaiseEvent SingleTaskIDChange()

            ' If a single Item has been set then this is also the Default Item
            DefaultTaskID = SingleTaskID

        End Set
    End Property


    <Browsable(False)> _
    <Description("The Item id of the type entry")> _
    <Category("Data")> _
    Public Property TaskID() As Integer
        Get
            Return m_lTaskID
        End Get
        Set(ByVal Value As Integer)
            'If DesignMode Then Throw New System.Exception("382")

            m_lTaskID = Value
            RaiseEvent TaskIDChange()
            With cboTasks
                For nIndex As Integer = 0 To .Items.Count - 1
                    If VB6.GetItemData(cboTasks, nIndex) = m_lTaskID Then
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
    Public ReadOnly Property ItemTaskname(Optional ByVal v_vTaskID As Object = Nothing) As String
        Get
            Dim nIndex As Integer

            If Information.IsNothing(v_vTaskID) Then
                Return m_sTaskname
            Else

                nIndex = IndexOfItem(CInt(v_vTaskID))
                If nIndex < 0 Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboTasks, nIndex)
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
            'developer guide no.36
            m_sFirstItem = Value
            RaiseEvent FirstItemChange()
            If Not DesignMode Then
                RefreshList()
            End If
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ShowRequiresKeys() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowRequiresKeys = Value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub

    Private Sub cboTasks_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTasks.SelectedIndexChanged
        'developer guide no. 111
        If (cboTasks.SelectedIndex > -1) Then
            With cboTasks

                m_lTaskID = VB6.GetItemData(cboTasks, .SelectedIndex)
                RaiseEvent TaskIDChange()
                m_sTaskname = VB6.GetItemString(cboTasks, .SelectedIndex)
                RaiseEvent TasknameChange()
            End With
            RaiseEvent Click(Me, Nothing)
        End If
    End Sub

    Private Sub cboPMTaskLookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboPMTaskLookup_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub
    'Tarun
    'Private Sub cboPMTaskLookup_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
    '	Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '	RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
    '	If KeyAscii = 0 Then
    '		eventArgs.Handled = True
    '	End If
    '	eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub
    Private Sub cboPMTaskLookup_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboPMTaskLookup_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboPMTaskLookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
    End Sub

    Private Sub cboPMTaskLookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
    End Sub

    Private Sub cboPMTaskLookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,AddItem
    'UPGRADE_NOTE: (7001) The following declaration (AddItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
    'cboTasks.Items.Insert(Index, Item)
    'End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTasks,cboTasks,-1,RemoveItem
    'UPGRADE_NOTE: (7001) The following declaration (RemoveItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub RemoveItem(ByRef Index As Integer)
    'cboTasks.Items.RemoveAt(CShort(Index))
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'Tarun Commneted
        'Font = Ambient.Font
        Font = Me.Font
        m_lPMTaskGroupID = m_def_PMTaskGroupID
        m_lTaskID = m_def_TaskID
        m_sTaskname = m_def_Taskname
        m_sFirstItem = m_def_FirstItem
    End Sub

    'Load property values from storage


    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        Dim dtDefEffectiveDate As Date = DateTime.Now




        cboTasks.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboTasks.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        cboTasks.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'Tarun
        'Font = PropBag.ReadProperty("Font", Ambient.Font)
        Font = PropBag.ReadProperty("Font", Me.Font)



        'Tarun
        ' MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboTasks, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'Tarun
        'cboTasks.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_lPMTaskGroupID = CInt(PropBag.ReadProperty("PMTaskGroupID", m_def_PMTaskGroupID))


        m_lTaskID = CInt(PropBag.ReadProperty("TaskID", m_def_TaskID))


        m_lDefaultTaskID = CInt(PropBag.ReadProperty("DefaultTaskID", m_def_DefaultTaskID))


        m_lSingleTaskID = CInt(PropBag.ReadProperty("SingleTaskID", m_def_SingleTaskID))


        m_sTaskname = CStr(PropBag.ReadProperty("Taskname", m_def_Taskname))


        m_sFirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        ' Read the list of type PMTaskGroupID entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub cboPMTaskLookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboTasks.Height
        cboTasks.Width = MyBase.Width
    End Sub

    'Write property values to storage


    'Tarun
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        Dim dtDefEffectiveDate As Date = DateTime.Now


        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboTasks.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboTasks.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboTasks.Sorted, False)

        PropBag.WriteProperty("Enabled", Enabled, True)


        'Tarun Commneted
        'PropBag.WriteProperty("Font", Font, Ambient.Font)


        'Tarun Commneted
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboTasks), "")


        'Tarun Commneted
        'PropBag.WriteProperty("WhatsThisHelpID", cboTasks.WhatsThisHelpID, 0)

        PropBag.WriteProperty("PMTaskGroupID", m_lPMTaskGroupID, m_def_PMTaskGroupID)

        PropBag.WriteProperty("TaskID", m_lTaskID, m_def_TaskID)

        PropBag.WriteProperty("DefaultTaskID", m_lDefaultTaskID, m_def_DefaultTaskID)

        PropBag.WriteProperty("SingleTaskID", m_lSingleTaskID, m_def_SingleTaskID)

        PropBag.WriteProperty("Taskname", m_sTaskname, m_def_Taskname)

        PropBag.WriteProperty("FirstItem", m_sFirstItem, m_def_FirstItem)
    End Sub
    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Try

            If Not DesignMode Then
                cboTasks.Items.Clear()
                ' Entry at top of list for (All) or (None) etc
                If m_sFirstItem <> "" Then
                    Dim cboTasks_NewIndex As Integer = -1
                    cboTasks_NewIndex = cboTasks.Items.Add(m_sFirstItem)
                    VB6.SetItemData(cboTasks, cboTasks_NewIndex, 0)
                End If
                m_lReturn = CType(GetTasks(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((Constants.vbObjectError + gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp + ", " + "Failed to get Tasks for PMTaskGroupID : " & PMTaskGroupID)
                End If
                ' Having filled the combo set it to it's default position
                If m_lDefaultTaskID <> 0 Then
                    TaskID = m_lDefaultTaskID
                Else
                    If cboTasks.Items.Count > 0 Then
                        cboTasks.SelectedIndex = 0
                    End If
                End If
            End If

        Catch excep As System.Exception



            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Sub

        End Try

    End Sub

    Private Function IndexOfItem(ByVal v_lTaskID As Integer) As Integer
        With cboTasks
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboTasks, nIndex) = v_lTaskID Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    Private Function GetTaskLookupBusiness() As Integer

        Dim result As Integer = 0
        Try
            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oTaskLookupBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                Dim temp_m_oTaskLookupBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oTaskLookupBusiness, "bPMTaskGroup.Lookup", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oTaskLookupBusiness = temp_m_oTaskLookupBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypePMTaskGroupID business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskLookupBusiness")
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type PMTaskGroupID business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskLookupBusiness", excep:=excep)

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
                m_oTaskLookupBusiness = Nothing
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

    Private Function GetTasks() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            m_lReturn = CType(GetTaskLookupBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If PMTaskGroupID < 1 Then
                ' Get the Tasks

                m_lReturn = m_oTaskLookupBusiness.GetAllEffectiveTasks(v_dtEffectiveDate:=DateTime.Now, r_vAllTasksArray:=m_vUsersArray)
            Else

                m_lReturn = m_oTaskLookupBusiness.GetGroupEffectiveTasks(v_lPMTaskGroupID:=PMTaskGroupID, v_dtEffectiveDate:=DateTime.Now, r_vGroupTasksArray:=m_vUsersArray)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTasks")
                Return result
            End If

            ' Add to the drop down list box.
            If Information.IsArray(m_vUsersArray) Then
                For lRow As Integer = m_vUsersArray.GetLowerBound(1) To m_vUsersArray.GetUpperBound(1)
                    ' If we only want to add one task
                    If m_lSingleTaskID > 0 Then
                        ' Check to see if this is the task to add
                        If m_lSingleTaskID = CInt(m_vUsersArray(0, lRow)) Then
                            Dim cboTasks_NewIndex As Integer = -1
                            cboTasks_NewIndex = cboTasks.Items.Add(CStr(m_vUsersArray(2, lRow)))
                            VB6.SetItemData(cboTasks, cboTasks_NewIndex, CInt(m_vUsersArray(0, lRow)))
                        End If
                    Else
                        ' Add All Tasks returned.
                        'Include if no keys or if we want the keys or it's the default
                        If (m_vUsersArray(3, lRow) = 0) Or m_bShowRequiresKeys Then
                            'Put 2 in here, as we return code and caption, let's display the latter
                            'developer guide no
                            Dim indx As Integer
                            indx = cboTasks.Items.Add(CStr(m_vUsersArray(2, lRow)))
                            VB6.SetItemData(cboTasks, indx, m_vUsersArray(0, lRow))
                        End If
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTasks", excep:=excep)

            Return result

        End Try
    End Function
End Class
