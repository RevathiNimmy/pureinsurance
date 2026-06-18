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
<System.Runtime.InteropServices.ProgId("cboPMLookup_NET.cboPMLookup")> _
Partial Public Class cboPMLookup
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event ItemCodeChange()
    Public Event ItemDescriptionChange()
    Public Event FirstItemChange()
    Public Event ItemIdChange()
    Public Event SingleItemIdChange()
    Public Event WhereClauseChange()
    Public Event DefaultItemIdChange()
    Public Event PMLookupProductFamilyChange()
    Public Event TableChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event ItemDataChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()


    ' RAW 15/07/2003 : CQ258 : Handle entries flagged as deleted
    ' RAW 25/07/2003 : CQ258 : pass a new LookupType value to GetLookupValues to include delete items

    'Default Property Values:
    Const m_def_Table As String = "None"
    Const m_def_ItemId As Integer = 0
    Const m_def_ItemDescription As String = ""
    Const m_def_DefaultItemId As Integer = 0
    Const m_def_SingleItemId As Integer = 0
    Const m_def_FirstItem As String = ""
    Const m_kdef_SortColumnName As String = ""



    Dim m_def_PMLookupProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
    ' RDC 18112002
    Const m_def_ItemCode As String = ""
    Const m_def_WhereClause As String = ""

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oTypeTableBusiness As bPMLookup.Business
    Private m_vLookupItems(,) As Object

    'Property Variables:
    Private m_sTableName As String = ""
    Private m_lItemId As Integer
    Private m_sCaption As String = ""
    ' RDC 18112002
    Private m_sItemCode As String = ""
    Private m_lDefaultItemId As Integer
    Private m_lSingleItemId As Integer
    Private m_sFirstItem As String = ""
    Private m_sSortColumnName As String = ""

    Private m_lPMLookupProductFamily As gPMConstants.PMEProductFamily
    Private m_sWhereClause As String = ""

    ' RAW 15/07/2003 : CQ258 : added
    Private m_colDeletedEntries As Collection
    Private m_bLoadInProgress As Boolean
    ' This variable is set to differentiate when click events is fired as a result of user action or setting ListIndex property from code
    Private m_bListIndexSetByCode As Boolean
    ' RAW 15/07/2003 : CQ258 : end

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboTypeTable,cboTypeTable,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressUserEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "TypeTable"
    Private sLookupDisplayedText As String = ""

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboTypeTable.BackColor
        End Get
        Set(ByVal Value As Color)
            cboTypeTable.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboTypeTable.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboTypeTable.ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    'tarun commneted
    '<Browsable(True)> _
    '<Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
    'Public Shadows Property Enabled() As Boolean
    '	Get
    '		Return MyBase.Enabled
    '	End Get
    '	Set(ByVal Value As Boolean)
    '		MyBase.Enabled = Value
    '		cboTypeTable.Enabled = Value
    '		RaiseEvent EnabledChange()

    '		'force update of ToolTipText if necessary


    '		If ToolTip1.GetToolTip(cboTypeTable) <> ReflectionHelper.GetMember(Extender, "ToolTipText") And Value Then


    '			ToolTip1.SetToolTip(cboTypeTable, ReflectionHelper.GetMember(Extender, "ToolTipText"))
    '		End If

    '	End Set
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled
    'develoepr guide no solution no. 35
    '<Browsable(False)> _
    'Public ReadOnly Property Sorted() As Boolean
    '	Get
    '		Return cboTypeTable.Sorted
    '	End Get
    'End Property
    <Browsable(False)> _
    Public Property Sorted() As Boolean
        Get
            Return cboTypeTable.Sorted
        End Get
        Set(ByVal value As Boolean)
            cboTypeTable.Sorted = value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboTypeTable.Font
        End Get
        Set(ByVal Value As Font)
            cboTypeTable.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle
    '<Browsable(False)> _
    'tarun
    '<Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    'Public ReadOnly Property BackStyle() As Integer
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

    ' RAW 15/07/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property IsItemDeleted(ByVal v_lItemId As Integer) As Boolean
        Get

            Dim result As Boolean = False


            Try
                If m_colDeletedEntries IsNot Nothing Then
                    If CStr(m_colDeletedEntries.Item(CStr(v_lItemId))) <> "" Then
                        If Information.Err().Number = 0 Then
                            result = True
                        End If
                    End If
                End If

                Return result

            Catch exc As System.Exception
                'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
            End Try
        End Get
    End Property

    ' RAW 15/07/2003 : CQ258 : added
    <Browsable(False)> _
    Public ReadOnly Property IsLoadInProgress() As Boolean
        Get
            Return m_bLoadInProgress
        End Get
    End Property

    ' RAW 15/07/2003 : CQ258 : added
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
    Public Shadows ReadOnly Property ActiveControl() As Object
        Get
            Return MyBase.ActiveControl
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,ItemData

    <Browsable(True)> _
    <Description("Returns/sets a specific number for each item in a ComboBox or ListBox control.")> _
    Public Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboTypeTable, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboTypeTable, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,List

    Private Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboTypeTable, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboTypeTable, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboTypeTable.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboTypeTable.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            ' RDC 20112002
            If Value >= -1 And cboTypeTable.Items.Count > 0 Then
                m_bListIndexSetByCode = True ' RAW 15/07/2003 : CQ258 : added
                cboTypeTable.SelectedIndex = Value
                m_bListIndexSetByCode = False ' RAW 15/07/2003 : CQ258 : added
                RaiseEvent ListIndexChange()
            End If
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboTypeTable)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboTypeTable, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,WhatsThisHelpID

    'tarun commneted
    '<Browsable(True)> _
    '<Description("Returns/sets an associated context number for an object.")> _
    'Public Property WhatsThisHelpID() As Integer
    '	Get

    '		Return cboTypeTable.WhatsThisHelpID
    '	End Get
    '	Set(ByVal Value As Integer)

    '		cboTypeTable.WhatsThisHelpID = Value
    '		RaiseEvent WhatsThisHelpIDChange()
    '	End Set
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,NewIndex
    'UPGRADE_NOTE: (7001) The following declaration (get NewIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function NewIndex() As Integer
    'Dim cboTypeTable_NewIndex As Integer = -1
    'Return cboTypeTable_NewIndex
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
    Public Property TableName() As String
        Get
            Return m_sTableName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sTableName = Value.Trim()
            RaiseEvent TableChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property PMLookupProductFamily() As Integer
        Get
            Return m_lPMLookupProductFamily
        End Get
        Set(ByVal Value As Integer)

            Try

                ' Store the Lookup Product Family
                m_lPMLookupProductFamily = Value
                RaiseEvent PMLookupProductFamilyChange()

            Catch excep As System.Exception



                Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
                Exit Property

            End Try

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


    <Browsable(False)> _
    <Description("The Item id of the type entry")> _
    <Category("Data")> _
    Public Property ItemId() As Integer
        Get
            Return m_lItemId
        End Get
        Set(ByVal Value As Integer)
            'Akash: Commented because showing exception when droping control on the form
            'If DesignMode Then Throw New System.Exception("382")

            Dim nIndex As Integer

            With cboTypeTable

                For nIndex = 0 To .Items.Count - 1

                    If VB6.GetItemData(cboTypeTable, nIndex) = Value Then
                        m_bListIndexSetByCode = True ' RAW 15/07/2003 : CQ258 : added
                        .SelectedIndex = nIndex
                        m_bListIndexSetByCode = False ' RAW 15/07/2003 : CQ258 : added
                        m_lItemId = Value
                        RaiseEvent ItemIdChange()
                        Exit Property
                    End If
                Next nIndex

                ' RAW 15/07/2003 : CQ258 : added
                ' if we have reached here then we have not found it
                ' if it has been deleted then add it to the combo and select it
                If IsItemDeleted(Value) Then
                    ' add deleted entry to combo
                    Dim cboTypeTable_NewIndex As Integer = -1
                    cboTypeTable_NewIndex = cboTypeTable.Items.Add(CStr(m_colDeletedEntries.Item(CStr(Value))))
                    nIndex = cboTypeTable_NewIndex
                    VB6.SetItemData(cboTypeTable, nIndex, Value)
                    ' select it
                    m_bListIndexSetByCode = True ' RAW 15/07/2003 : CQ258 : added
                    .SelectedIndex = nIndex
                    m_bListIndexSetByCode = False ' RAW 15/07/2003 : CQ258 : added

                    m_lItemId = Value
                    RaiseEvent ItemIdChange()
                End If
                ' RAW 15/07/2003 : CQ258 : end
            End With

        End Set
    End Property

    ' RDC 18112002
    <Browsable(False)> _
    Public ReadOnly Property ItemCode() As String
        Get
            Return m_sItemCode
        End Get
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
                    Return VB6.GetItemString(cboTypeTable, nIndex)
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
            'developer guide no. 131
            If (Not String.IsNullOrEmpty(Value)) Then
                If (Value.IndexOf("("c) + 1) = 0 Then
                    m_sFirstItem = "(" & Value & ")"
                Else
                    m_sFirstItem = Value
                End If
            End If
            RaiseEvent FirstItemChange()
            If Not DesignMode And Not IsBuildMachine() Then
                RefreshList()
            End If
        End Set
    End Property

    <Browsable(True)> _
    <Description("Column name of table to sort")> _
    <Category("Data")> _
    Public Property SortColumnName() As String
        Get
            Return m_sSortColumnName
        End Get
        Set(ByVal value As String)
            m_sSortColumnName = value
        End Set
    End Property




    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub

    Private Sub cboTypeTable_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTypeTable.SelectedIndexChanged


        With cboTypeTable
            sLookupDisplayedText = .Text
            ' RAW 15/07/2003 : CQ258 : added test for invalid index
            If .SelectedIndex < 0 Then
                m_lItemId = 0
                RaiseEvent ItemIdChange()
                m_sCaption = ""
                RaiseEvent ItemDescriptionChange()
                m_sItemCode = ""
                RaiseEvent ItemCodeChange()
            Else
                m_lItemId = VB6.GetItemData(cboTypeTable, .SelectedIndex)
                RaiseEvent ItemIdChange()
                m_sCaption = VB6.GetItemString(cboTypeTable, .SelectedIndex)
                RaiseEvent ItemDescriptionChange()

                ' RDC 18112002
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

        End With
        RaiseEvent Click(Me, Nothing)

    End Sub


    Private Sub cboPMLookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboPMLookup_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboPMLookup_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboPMLookup_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub cboPMLookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMLookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub cboPMLookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,AddItem
    'UPGRADE_NOTE: (7001) The following declaration (AddItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
    'cboTypeTable.Items.Insert(Index, Item)
    'End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,RemoveItem
    'UPGRADE_NOTE: (7001) The following declaration (RemoveItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub RemoveItem(ByRef Index As Integer)
    'cboTypeTable.Items.RemoveAt(CShort(Index))
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'developers guid no solution no 2
        'Font = Ambient.Font
        Font = MyBase.Font
        m_sTableName = m_def_Table
        m_lItemId = m_def_ItemId
        m_sCaption = m_def_ItemDescription
        m_sFirstItem = m_def_FirstItem
        m_lPMLookupProductFamily = m_def_PMLookupProductFamily
        ' RDC 18112002
        m_sItemCode = m_def_ItemCode
        m_sSortColumnName = m_kdef_SortColumnName
    End Sub

    'Load property values from storage


    'developers guid no 1. no solution.
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        Dim dtDefEffectiveDate As Date = DateTime.Now




        cboTypeTable.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboTypeTable.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        cboTypeTable.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developers guid no.2 no solution
        Font = PropBag.ReadProperty("Font", MyBase.Font)



        'developers guid no.14 no solution
        '	MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboTypeTable, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developers guid no.15 no solution
        'cboTypeTable.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_sTableName = CStr(PropBag.ReadProperty("Table", m_def_Table))


        m_lItemId = CInt(PropBag.ReadProperty("ItemId", m_def_ItemId))


        m_lDefaultItemId = CInt(PropBag.ReadProperty("DefaultItemId", m_def_DefaultItemId))


        m_lSingleItemId = CInt(PropBag.ReadProperty("SingleItemId", m_def_SingleItemId))


        m_sCaption = CStr(PropBag.ReadProperty("ItemDescription", m_def_ItemDescription))


        m_sFirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))

        m_sSortColumnName = CStr(PropBag.ReadProperty("SortColumnName", m_kdef_SortColumnName))

        PMLookupProductFamily = CInt(PropBag.ReadProperty("PMLookupProductFamily", m_def_PMLookupProductFamily))
        ' RDC 18112002

        m_sItemCode = CStr(PropBag.ReadProperty("ItemCode", m_def_ItemCode))

        m_sWhereClause = CStr(PropBag.ReadProperty("WhereClause", m_def_WhereClause))

        'Read the list of type table entries
        If Not DesignMode And Not IsBuildMachine() Then
            RefreshList()
        End If

    End Sub

    Private Sub cboPMLookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboTypeTable.Height
        cboTypeTable.Width = MyBase.Width
    End Sub

    'Write property values to storage


    'developers guid no.1 no solution
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        Dim dtDefEffectiveDate As Date = DateTime.Now


        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboTypeTable.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboTypeTable.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboTypeTable.Sorted, False)

        PropBag.WriteProperty("Enabled", Enabled, True)


        'developers guid no.2 no solution.
        PropBag.WriteProperty("Font", Font, MyBase.Font)


        'developers guid no solution no 14
        '	PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboTypeTable), "")


        'developers guid no solution no 15
        '	PropBag.WriteProperty("WhatsThisHelpID", cboTypeTable.WhatsThisHelpID, 0)

        PropBag.WriteProperty("Table", m_sTableName, m_def_Table)

        PropBag.WriteProperty("ItemId", m_lItemId, m_def_ItemId)

        PropBag.WriteProperty("DefaultItemId", m_lDefaultItemId, m_def_DefaultItemId)

        PropBag.WriteProperty("SingleItemId", m_lSingleItemId, m_def_SingleItemId)

        PropBag.WriteProperty("ItemDescription", m_sCaption, m_def_ItemDescription)

        PropBag.WriteProperty("FirstItem", m_sFirstItem, m_def_FirstItem)

        PropBag.WriteProperty("PMLookupProductFamily", m_lPMLookupProductFamily, m_def_PMLookupProductFamily)

        ' RDC 18112002

        PropBag.WriteProperty("ItemCode", m_sItemCode, m_def_ItemCode)

        PropBag.WriteProperty("WhereClause", m_sWhereClause, m_def_WhereClause)

        PropBag.WriteProperty("SortColumnName", m_sSortColumnName, m_kdef_SortColumnName)

    End Sub

    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Try

            If Not DesignMode And Not IsBuildMachine() Then
                If TableName <> "None" And TableName <> "" Then

                    ' RAW 15/07/2003 : CQ258 : added
                    m_bLoadInProgress = True

                    cboTypeTable.Items.Clear()
                    ' RAW 15/07/2003 : CQ258 : added
                    m_colDeletedEntries = Nothing
                    m_colDeletedEntries = New Collection()
                    ' RAW 15/07/2003 : CQ258 : added

                    ' Entry at top of list for (All) or (None) etc
                    If m_sFirstItem <> "" Then
                        Dim cboTypeTable_NewIndex As Integer = -1
                        cboTypeTable_NewIndex = cboTypeTable.Items.Add(m_sFirstItem)
                        VB6.SetItemData(cboTypeTable, cboTypeTable_NewIndex, 0)
                    End If
                    m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 15/07/2003 : CQ258 : added
                        m_bLoadInProgress = False
                        Throw New System.Exception((Constants.vbObjectError + gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp + ", " + "Failed to get Lookup Values for Table : " & TableName)
                    End If
                    ' Having filled the combo set it to it's default position
                    If m_lDefaultItemId <> 0 Then
                        ItemId = m_lDefaultItemId
                    Else
                        If cboTypeTable.Items.Count > 0 Then
                            m_bListIndexSetByCode = True ' RAW 15/07/2003 : CQ258 : added
                            cboTypeTable.SelectedIndex = 0
                            m_bListIndexSetByCode = False ' RAW 15/07/2003 : CQ258 : added
                        End If
                    End If
                End If
            End If

            'force update of ToolTipText if necessary


            'developers guid no solution no.3
            '			If ToolTip1.GetToolTip(cboTypeTable) <> Extender.ToolTipText Then
            If ToolTip1.GetToolTip(cboTypeTable) <> Me.ToolTipText Then


                'developers guid no solution no.3
                'ToolTip1.SetToolTip(cboTypeTable, Extender.ToolTipText)
                ToolTip1.SetToolTip(cboTypeTable, Me.ToolTipText)
            End If

            ' RAW 15/07/2003 : CQ258 : added
            m_bLoadInProgress = False

        Catch excep As System.Exception



            ' RAW 15/07/2003 : CQ258 : added
            m_bLoadInProgress = False

            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Sub



        End Try

    End Sub

    Private Function IndexOfItem(ByVal v_lItemId As Integer) As Integer
        With cboTypeTable
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboTypeTable, nIndex) = v_lItemId Then
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
                'changes as bPMLookup object is required
                'Dim temp_m_oTypeTableBusiness As Object
                Dim temp_m_oTypeTableBusiness As bPMLookup.Business
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oTypeTableBusiness, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oTypeTableBusiness = temp_m_oTypeTableBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypeTable business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness")
                    Return result
                End If

                ' Set the Lookup Product Family

                m_oTypeTableBusiness.PMLookupProductFamily = PMLookupProductFamily

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

            If IsBuildMachine() Then
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


            ' RAW 15/07/2003 : CQ258 : added
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
        Dim vTableArray(,) As Object

        Dim iLookupType As gPMConstants.PMELookupType
        Dim sCaption, sID As String
        Dim vIsDeleted As String = ""
        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)
        Dim iLanguageId As Integer
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)

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

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = TableName

            ' Set the Type of Lookup
            If SingleItemId < 1 Then
                ' RAW 25/07/2003 : CQ258 : replaces PMLookupAllEffective
                iLookupType = gPMConstants.PMELookupType.PMLookupAllWithDeleted
            Else
                iLookupType = gPMConstants.PMELookupType.PMLookupSingle

                vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = SingleItemId
            End If


            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0) = m_sWhereClause

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)
            'This portion of code is not mentioned in the Tech spec, but is required to get the language id that is being used currently
            m_lReturn = CType(gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)
            'Replaced g_iLanguageID with iLanguageId
            ' Get the lookup values

            m_lReturn = m_oTypeTableBusiness.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageId, dtEffectiveDate:=DateTime.Now, vResultArray:=m_vLookupItems, sSortColumnName:=m_sSortColumnName)
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (Not mentioned in the TechSpec)
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

                    ' RAW 15/07/2003 : CQ258 : added code to handle deleted entries
                    ' extract details from result set
                    sCaption = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRow))
                    sID = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lRow))

                    vIsDeleted = CStr(m_vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lRow))

                    If Convert.IsDBNull(vIsDeleted) Or IsNothing(vIsDeleted) Then
                        vIsDeleted = CStr(0)
                    ElseIf CBool(vIsDeleted) Then
                        vIsDeleted = CStr(1)
                    ElseIf StringsHelper.ToDoubleSafe(vIsDeleted) <> 1 Then
                        vIsDeleted = CStr(0)
                    End If

                    If StringsHelper.ToDoubleSafe(vIsDeleted) = 1 Then
                        ' add to collection of deleted entries
                        m_colDeletedEntries.Add(sCaption, sID)
                    Else
                        ' add entry to combo
                        Dim cboTypeTable_NewIndex As Integer = -1
                        cboTypeTable_NewIndex = cboTypeTable.Items.Add(sCaption)
                        VB6.SetItemData(cboTypeTable, cboTypeTable_NewIndex, CInt(sID))
                    End If
                    ' RAW 15/07/2003 : CQ258 : end


                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", excep:=excep)

            Return result

        End Try
    End Function

    Private Function IsBuildMachine() As Boolean
        Static vIsRunning, bIsBuildMachine As Boolean

        Dim sValue As String = ""

        'DD 26/5/2004
        'Returns true if this machine is a build machine
        'PMLookup will always try to populate the combo if embedded on another control
        'eg. Address control. Which means that a build machine would have to be logged into
        'Sirius to compile other projects. If this registry setting is on then
        'it will never load any data which is fine for just compiling.


        If vIsRunning.Equals(False) Then
            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, gPMConstants.PMERegSettingLevel.pmeRSLClient, "IsBuildMachine", sValue)

            vIsRunning = True
            bIsBuildMachine = (sValue <> "")
        End If

        Return bIsBuildMachine
    End Function

    Private Sub cboTypeTable_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTypeTable.LostFocus
        Dim iIndex As Integer
        iIndex = cboTypeTable.FindString(sLookupDisplayedText)
        If iIndex >= 0 Then cboTypeTable.SelectedIndex = iIndex
    End Sub

End Class
