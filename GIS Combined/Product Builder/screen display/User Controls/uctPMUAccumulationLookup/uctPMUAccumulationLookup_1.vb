Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("cboAccumulation_NET.cboAccumulation")> _
Partial Public Class cboAccumulation
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event ItemCodeChange()
    Public Event ItemDescriptionChange()
    Public Event FirstItemChange()
    Public Event ItemIdChange()
    Public Event SingleItemIdChange()
    Public Event WhereClauseChange()
    Public Event DefaultItemIdChange()
    Public Event AccumulationLevelChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event ItemDataChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()


    ' RAW 04/09/2003 : CQ258 : Handle entries flagged as deleted (as per uctPMLookup)


    'Default Property Values:
    Const m_def_ItemId As Integer = 0
    Const m_def_ItemDescription As String = ""
    Const m_def_DefaultItemId As Integer = 0
    Const m_def_SingleItemId As Integer = 0
    Const m_def_FirstItem As String = ""
    Const m_def_AccumulationLevel As Integer = 0
    Const m_def_ItemCode As String = "" ' RAW 04/09/2003 : CQ258 : added
    Const m_def_WhereClause As String = "" ' RAW 04/09/2003 : CQ258 : added

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oTypeTableBusiness As bSIRAccumulationLookup.Business
    'Private m_oTypeTableBusiness As bSIRAccumulationLookup.Business
    Private m_vLookupItems(,) As Object

    'Property Variables:
    Private m_lItemId As Integer
    Private m_sCaption As String = ""
    Private m_sItemCode As String = "" ' RAW 04/09/2003 : CQ258 : added
    Private m_lDefaultItemId As Integer
    Private m_lSingleItemId As Integer
    Private m_sFirstItem As String = ""
    Private m_lAccumulationLevel As Integer
    Private m_sWhereClause As String = "" ' RAW 04/09/2003 : CQ258 : added

    ' RAW 04/09/2003 : CQ258 : added
    Private m_colDeletedEntries As Collection
    Private m_bLoadInProgress As Boolean
    ' This variable is set to differentiate when click events is fired as a result of user action or setting ListIndex property from code
    Private m_bListIndexSetByCode As Boolean
    ' RAW 04/09/2003 : CQ258 : end


    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboAccumulation,cboAccumulation,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "TypeTable"


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboAccumulation_cboAccumulation.BackColor
        End Get
        Set(ByVal Value As Color)
            cboAccumulation_cboAccumulation.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboAccumulation_cboAccumulation.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboAccumulation_cboAccumulation.ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    <Browsable(True)> _
    <Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
    Public Shadows Property Enabled_Renamed() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)
            MyBase.Enabled = Value
            cboAccumulation_cboAccumulation.Enabled = Value
            RaiseEvent EnabledChange()

            'force update of ToolTipText if necessary


            'developer guide no solution.3
            If ToolTip1.GetToolTip(cboAccumulation_cboAccumulation) <> Me.ToolTipText And Value Then


                'developer guide no solution.3
                ToolTip1.SetToolTip(cboAccumulation_cboAccumulation, Me.ToolTipText)
            End If

        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled
    <Browsable(False)> _
    Public ReadOnly Property Sorted() As Boolean
        Get
            Return cboAccumulation_cboAccumulation.Sorted
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboAccumulation_cboAccumulation.Font
        End Get
        Set(ByVal Value As Font)
            cboAccumulation_cboAccumulation.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle
    <Browsable(False)> _
    <Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    Public ReadOnly Property BackStyle() As Integer
        Get


            'developer no solution.14
            'Return MyBase.BackStyle
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BorderStyle
    <Browsable(False)> _
    <Description("Returns/sets the border style for an object.")> _
    Public Shadows ReadOnly Property BorderStyle_Renamed() As Integer
        Get


            Return MyBase.BorderStyle
        End Get
    End Property

    ' RAW 04/09/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property IsItemDeleted(ByVal v_lItemId As Integer) As Boolean
        Get

            Dim result As Boolean = False


            Try
                If CStr(m_colDeletedEntries.Item(CStr(v_lItemId))) <> "" Then
                    If Information.Err().Number = 0 Then
                        result = True
                    End If
                End If

                Return result

            Catch exc As System.Exception
                'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
            End Try
        End Get
    End Property

    ' RAW 04/09/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property IsLoadInProgress() As Boolean
        Get
            Return m_bLoadInProgress
        End Get
    End Property

    ' RAW 04/09/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property IsListIndexSetByCode() As Boolean
        Get
            Return m_bListIndexSetByCode
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,ActiveControl
    <Browsable(False)> _
    <Description("Returns the control that has focus.")> _
    Public Shadows ReadOnly Property ActiveControl_Renamed() As Object
        Get
            Return MyBase.ActiveControl
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,ItemData

    Private Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboAccumulation_cboAccumulation, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboAccumulation_cboAccumulation, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,List

    Private Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboAccumulation_cboAccumulation, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboAccumulation_cboAccumulation, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboAccumulation_cboAccumulation.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboAccumulation_cboAccumulation.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            m_bListIndexSetByCode = True ' RAW 04/09/2003 : CQ258 : added
            cboAccumulation_cboAccumulation.SelectedIndex = Value
            m_bListIndexSetByCode = False ' RAW 04/09/2003 : CQ258 : added
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboAccumulation_cboAccumulation)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboAccumulation_cboAccumulation, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,WhatsThisHelpID

    <Browsable(True)> _
    <Description("Returns/sets an associated context number for an object.")> _
    Public Property WhatsThisHelpID() As Integer
        Get


            'developer no solution.15
            'Return cboAccumulation.WhatsThisHelpID
        End Get
        Set(ByVal Value As Integer)

            'developer no solution.15
            RaiseEvent WhatsThisHelpIDChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,NewIndex

    'Private Function NewIndex() As Integer
    'Dim cboAccumulation_NewIndex As Integer = -1
    'Return cboAccumulation_NewIndex
    'End Function

    ' BB040298 Set the Type of Control to Combo for Form Control
    <Browsable(False)> _
    Public ReadOnly Property TypeOfControl() As Integer
        Get
            Return gPMConstants.PMEControlType.PMCombo
        End Get
    End Property

    <Browsable(True)> _
    Public Property AccumulationLevel() As Integer
        Get
            Return m_lAccumulationLevel
        End Get
        Set(ByVal Value As Integer)
            m_lAccumulationLevel = Value
            RaiseEvent AccumulationLevelChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("Set this to the Id of the item that you want selected by default.")> _
    Public Property DefaultItemId() As Integer
        Get
            Return m_lDefaultItemId
        End Get
        Set(ByVal Value As Integer)
            m_lDefaultItemId = Value
            RaiseEvent DefaultItemIdChange()
        End Set
    End Property

    ' RAW 04/09/2003 : CQ258 : added
    ' RAW 04/09/2003 : CQ258 : added
    <Browsable(True)> _
    Public Property WhereClause() As String
        Get
            Return m_sWhereClause
        End Get
        Set(ByVal Value As String)

            m_sWhereClause = Value
            RaiseEvent WhereClauseChange()

        End Set
    End Property


    <Browsable(True)> _
    <Description("If you only want to retrieve a single item, set the id here. Note: The Id specified here will also become the default id. Think performance! Why get the whole list when you only want to diaplay one value.")> _
    Public Property SingleItemId() As Integer
        Get
            Return m_lSingleItemId
        End Get
        Set(ByVal Value As Integer)

            m_lSingleItemId = Value
            RaiseEvent SingleItemIdChange()

            ' If a single Item has been set then this is also the Default Item
            DefaultItemId = SingleItemId

        End Set
    End Property

    ' RAW 04/09/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property ItemCode() As String
        Get
            Return m_sItemCode
        End Get
    End Property


    <Browsable(False)> _
    <Description("The Item id of the type entry")> _
    <Category("Data")> _
    Public Property ItemId() As Integer
        Get
            Return m_lItemId
        End Get
        Set(ByVal Value As Integer)
            If DesignMode Then Throw New System.Exception("382")

            Dim nIndex As Integer

            With cboAccumulation_cboAccumulation
                For nIndex = 0 To .Items.Count - 1
                    If VB6.GetItemData(cboAccumulation_cboAccumulation, nIndex) = Value Then
                        m_bListIndexSetByCode = True ' RAW 04/09/2003 : CQ258 : added
                        .SelectedIndex = nIndex
                        m_bListIndexSetByCode = False ' RAW 04/09/2003 : CQ258 : added
                        m_lItemId = Value
                        RaiseEvent ItemIdChange()
                        Exit Property
                    End If
                Next nIndex

                ' RAW 04/09/2003 : CQ258 : added
                ' if we have reached here then we have not found it
                ' if it has been deleted then add it to the combo and select it
                If IsItemDeleted(v_lItemId:=Value) Then
                    ' add deleted entry to combo
                    Dim cboAccumulation_NewIndex As Integer = -1
                    cboAccumulation_NewIndex = cboAccumulation_cboAccumulation.Items.Add(CStr(m_colDeletedEntries.Item(CStr(Value))))
                    nIndex = cboAccumulation_NewIndex
                    VB6.SetItemData(cboAccumulation_cboAccumulation, nIndex, Value)
                    ' select it
                    m_bListIndexSetByCode = True ' RAW 04/09/2003 : CQ258 : added
                    .SelectedIndex = nIndex
                    m_bListIndexSetByCode = False ' RAW 04/09/2003 : CQ258 : added

                    m_lItemId = Value
                    RaiseEvent ItemIdChange()
                End If
                ' RAW 04/09/2003 : CQ258 : end
            End With

        End Set
    End Property

    <Browsable(False)> _
    <Description("Description of the Type entry")> _
    <Category("Data")> _
    Public ReadOnly Property ItemCaption(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            Dim nIndex As Integer

            If Information.IsNothing(v_vItemId) Then
                Return m_sCaption
            Else

                nIndex = IndexOfItem(CInt(v_vItemId))
                If nIndex < 0 Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboAccumulation_cboAccumulation, nIndex)
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

    Private Sub cboAccumulation_cboAccumulation_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccumulation_cboAccumulation.SelectedIndexChanged


        With cboAccumulation_cboAccumulation

            ' RAW 04/09/2003 : CQ258 : added test for invalid index
            If .SelectedIndex < 0 Then
                m_lItemId = 0
                RaiseEvent ItemIdChange()
                m_sCaption = ""
                RaiseEvent ItemDescriptionChange()
                m_sItemCode = "" ' RAW 04/09/2003 : CQ258 : added
                RaiseEvent ItemCodeChange() ' RAW 04/09/2003 : CQ258 : added
            Else
                m_lItemId = VB6.GetItemData(cboAccumulation_cboAccumulation, .SelectedIndex)
                RaiseEvent ItemIdChange()
                m_sCaption = VB6.GetItemString(cboAccumulation_cboAccumulation, .SelectedIndex)
                RaiseEvent ItemDescriptionChange()

                ' RAW 04/09/2003 : CQ258 : added
                If Information.IsArray(m_vLookupItems) Then
                    For lLoop As Integer = m_vLookupItems.GetLowerBound(1) To m_vLookupItems.GetUpperBound(1)
                        If CStr(m_vLookupItems(1, lLoop)) = m_sCaption Then
                            m_sItemCode = CStr(m_vLookupItems(2, lLoop))

                            RaiseEvent ItemCodeChange()
                            Exit For
                        End If
                    Next
                End If
            End If
            ' RAW 04/09/2003 : CQ258 : end
        End With

        RaiseEvent Click(Me, Nothing)

    End Sub


    Private Sub cboAccumulation_cboAccumulation_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccumulation_cboAccumulation.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboAccumulation_cboAccumulation_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboAccumulation_cboAccumulation.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub

    'developer guide no.42
    Private Sub cboAccumulation_cboAccumulation_KeyPress(ByVal eventSender As Object, ByVal eventArgs As Windows.Forms.KeyPressEventArgs) Handles cboAccumulation_cboAccumulation.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'NIIT - Replaced with the Migrated code 1144 
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboAccumulation_cboAccumulation_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboAccumulation_cboAccumulation.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboAccumulation_cboAccumulation_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles cboAccumulation_cboAccumulation.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'developer guide no.75
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboAccumulation_cboAccumulation_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles cboAccumulation_cboAccumulation.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'developer guide no.75
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboAccumulation_cboAccumulation_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles cboAccumulation_cboAccumulation.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'developer guide no.75
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,AddItem

    'Private Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
    'cboAccumulation_cboAccumulation.Items.Insert(Index, Item)
    'End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboAccumulation,cboAccumulation,-1,RemoveItem

    'Private Sub RemoveItem(ByRef Index As Integer)
    'cboAccumulation_cboAccumulation.Items.RemoveAt(CShort(Index))
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'developer guide no solution.2 
        Font = MyBase.Font
        'Font = Ambient.Font
        m_lItemId = m_def_ItemId
        m_sCaption = m_def_ItemDescription
        m_sFirstItem = m_def_FirstItem
        m_lAccumulationLevel = m_def_AccumulationLevel

        m_sItemCode = m_def_ItemCode ' RAW 04/09/2003 : CQ258 : added
    End Sub

    'Load property values from storage


    'developer no solution.1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)


        Dim dtDefEffectiveDate As Date = DateTime.Now




        cboAccumulation_cboAccumulation.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboAccumulation_cboAccumulation.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        cboAccumulation_cboAccumulation.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer no solution. 2
        Font = PropBag.ReadProperty("Font", MyBase.Font)
        'Font = PropBag.ReadProperty("Font", Ambient.Font)



        'developer guide no solution.14
        'MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboAccumulation_cboAccumulation, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developer no solution.15
        'cboAccumulation.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_lItemId = CInt(PropBag.ReadProperty("ItemId", m_def_ItemId))


        m_lDefaultItemId = CInt(PropBag.ReadProperty("DefaultItemId", m_def_DefaultItemId))


        m_lSingleItemId = CInt(PropBag.ReadProperty("SingleItemId", m_def_SingleItemId))


        m_sCaption = CStr(PropBag.ReadProperty("ItemDescription", m_def_ItemDescription))


        m_sFirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        m_lAccumulationLevel = CInt(PropBag.ReadProperty("AccumulationLevel", m_def_AccumulationLevel))


        m_sItemCode = CStr(PropBag.ReadProperty("ItemCode", m_def_ItemCode)) ' RAW 04/09/2003 : CQ258 : added


        m_sWhereClause = CStr(PropBag.ReadProperty("WhereClause", m_def_WhereClause)) ' RAW 04/09/2003 : CQ258 : added

        ' Read the list of type table entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub


    Private Sub cboAccumulation_cboAccumulation_Resize()
        MyBase.Height = cboAccumulation_cboAccumulation.Height
        cboAccumulation_cboAccumulation.Width = MyBase.Width
    End Sub

    'Write property values to storage


    'developer no solution.1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)

        Dim dtDefEffectiveDate As Date = DateTime.Now


        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboAccumulation_cboAccumulation.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboAccumulation_cboAccumulation.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboAccumulation_cboAccumulation.Sorted, False)

        PropBag.WriteProperty("Enabled", Enabled, True)


        'developer guide no solution.2
        PropBag.WriteProperty("Font", Font, MyBase.Font)


        'developer guide no.14 (no solution)
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboAccumulation_cboAccumulation), "")


        'developer guide. 15 (no solution)
        'PropBag.WriteProperty("WhatsThisHelpID", cboAccumulation.WhatsThisHelpID, 0)

        PropBag.WriteProperty("ItemId", m_lItemId, m_def_ItemId)

        PropBag.WriteProperty("DefaultItemId", m_lDefaultItemId, m_def_DefaultItemId)

        PropBag.WriteProperty("SingleItemId", m_lSingleItemId, m_def_SingleItemId)

        PropBag.WriteProperty("ItemDescription", m_sCaption, m_def_ItemDescription)

        PropBag.WriteProperty("FirstItem", m_sFirstItem, m_def_FirstItem)

        PropBag.WriteProperty("AccumulationLevel", m_lAccumulationLevel, m_def_AccumulationLevel)

        PropBag.WriteProperty("ItemCode", m_sItemCode, m_def_ItemCode) ' RAW 04/09/2003 : CQ258 : added

        PropBag.WriteProperty("WhereClause", m_sWhereClause, m_def_WhereClause) ' RAW 04/09/2003 : CQ258 : added

    End Sub

    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Try

            If Not DesignMode Then
                If AccumulationLevel <> 0 Then

                    ' RAW 04/09/2003 : CQ258 : added
                    m_bLoadInProgress = True

                    cboAccumulation_cboAccumulation.Items.Clear()
                    ' RAW 04/09/2003 : CQ258 : added
                    m_colDeletedEntries = Nothing
                    m_colDeletedEntries = New Collection()
                    ' RAW 04/09/2003 : CQ258 : added

                    ' Entry at top of list for (All) or (None) etc
                    If m_sFirstItem <> "" Then
                        Dim cboAccumulation_NewIndex As Integer = -1
                        cboAccumulation_NewIndex = cboAccumulation_cboAccumulation.Items.Add(m_sFirstItem)
                        VB6.SetItemData(cboAccumulation_cboAccumulation, cboAccumulation_NewIndex, 0)
                    End If
                    m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 04/09/2003 : CQ258 : added
                        m_bLoadInProgress = False
                        Throw New System.Exception((Constants.vbObjectError + gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp + ", " + "Failed to get Lookup Values for Level : " & AccumulationLevel)
                    End If
                    ' Having filled the combo set it to it's default position
                    If m_lDefaultItemId <> 0 Then
                        ItemId = m_lDefaultItemId
                    Else
                        If cboAccumulation_cboAccumulation.Items.Count > 0 Then
                            m_bListIndexSetByCode = True ' RAW 04/09/2003 : CQ258 : added
                            cboAccumulation_cboAccumulation.SelectedIndex = 0
                            m_bListIndexSetByCode = False ' RAW 04/09/2003 : CQ258 : added
                        End If
                    End If
                End If
            End If

            'force update of ToolTipText if necessary


            'developer no solution.3
            If ToolTip1.GetToolTip(cboAccumulation_cboAccumulation) <> Me.ToolTipText Then


                'developer no solution.3
                ToolTip1.SetToolTip(cboAccumulation_cboAccumulation, Me.ToolTipText)
            End If

            ' RAW 04/09/2003 : CQ258 : added
            m_bLoadInProgress = False

        Catch excep As System.Exception



            ' RAW 04/09/2003 : CQ258 : added
            m_bLoadInProgress = False

            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        End Try

    End Sub

    Private Function IndexOfItem(ByVal v_lItemId As Integer) As Integer
        With cboAccumulation_cboAccumulation
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboAccumulation_cboAccumulation, nIndex) = v_lItemId Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    Private Function GetTypeTableBusiness() As Integer

        Dim result As Integer = 0
        Try
            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oTypeTableBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                Dim temp_m_oTypeTableBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oTypeTableBusiness, "bSIRAccumulationLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oTypeTableBusiness = temp_m_oTypeTableBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the Accumulation Lookup business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness")
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type table business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness", excep:=excep)

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
                m_oTypeTableBusiness = Nothing

               
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

            ' RAW 04/09/2003 : CQ258 : added
            m_colDeletedEntries = New Collection()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetLookupValues() As Integer

        ' To hold the lookup results
        Dim result As Integer = 0
        Dim vTableArray As Object

        Dim iLookupType As gPMConstants.PMELookupType


        Dim sCaption, sID As String
        Dim vIsDeleted As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            m_lReturn = CType(GetTypeTableBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Format the Input Array
            ReDim vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0)

            ' Set the Table Name

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = AccumulationLevel

            ' Set the Type of Lookup
            If SingleItemId < 1 Then
                ' RAW 04/09/2003 : CQ258 : replaces PMLookupAllEffective
                iLookupType = gPMConstants.PMELookupType.PMLookupAllWithDeleted
            Else
                iLookupType = gPMConstants.PMELookupType.PMLookupSingle

                vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = SingleItemId
            End If


            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0) = m_sWhereClause ' RAW 04/09/2003 : CQ258 : added

            ' Get the lookup values

            m_lReturn = m_oTypeTableBusiness.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=g_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=m_vLookupItems)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                Return result
            End If

            ' Add to the drop down list box.
            ' As we are only working with one Lookup table at a time,
            ' we do not need to bother with the start and no of items
            If Information.IsArray(m_vLookupItems) Then
                For lRow As Integer = m_vLookupItems.GetLowerBound(1) To m_vLookupItems.GetUpperBound(1)

                    ' RAW 04/09/2003 : CQ258 : added code to handle deleted entries
                    ' extract details from result set
                    sCaption = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRow))
                    sID = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lRow))

                    vIsDeleted = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lRow))

                    If Convert.IsDBNull(vIsDeleted) Or IsNothing(vIsDeleted) Then
                        vIsDeleted = CStr(0)
                    ElseIf StringsHelper.ToDoubleSafe(vIsDeleted) <> 1 Then
                        vIsDeleted = CStr(0)
                    End If

                    If StringsHelper.ToDoubleSafe(vIsDeleted) = 1 Then
                        ' add to collection of deleted entries
                        m_colDeletedEntries.Add(sCaption, sID)
                    Else
                        ' add entry to combo
                        Dim cboAccumulation_NewIndex As Integer = -1
                        cboAccumulation_NewIndex = cboAccumulation_cboAccumulation.Items.Add(sCaption)
                        VB6.SetItemData(cboAccumulation_cboAccumulation, cboAccumulation_NewIndex, CInt(sID))
                    End If
                    ' RAW 04/09/2003 : CQ258 : end


                Next lRow
            End If

            'Now we can get rid of the business
            Dispose()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", excep:=excep)

            Return result

        End Try
    End Function
End Class
