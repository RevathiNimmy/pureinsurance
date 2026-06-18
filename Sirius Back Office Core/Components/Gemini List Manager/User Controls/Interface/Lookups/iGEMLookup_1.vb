Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Lookup_NET.Lookup")> _
Public Partial Class Lookup
	Inherits System.Windows.Forms.UserControl
	Public Event ItemDescriptionChange()
	Public Event FirstItemChange()
	Public Event InsurerNoChange()
	Public Event ItemIdChange()
	Public Event DefaultItemIdChange()
	Public Event TableChange()
	Public Event WhatsThisHelpIDChange()
	Public Event ToolTipTextChange()
	Public Event ListIndexChange()
	Public Event ListChange()
	Public Event ItemDataChange()
	Public Event BorderStyleChange()
	Public Event BackStyleChange()
	Public Event FontChange()
	Public Event SortedChange()
	Public Event ObjectIDChange()
	Public Event EnabledChange()
	Public Event ForeColorChange()
	Public Event BackColorChange()
	' BB040298 Set the Type of Control to Combo for Form Control
	
	
	' Legal values for Table
	Public Enum voyTable
		gemrenewal_status
		gemrenewal_action
		geminsurer
		gemschemes
		newpolarisobjects
		newpolarisproperties
		gemII_scheme_group
		'MN160799 - Addded for HKJ
		HKJnewpolarisobjects
		HKJnewpolarisproperties
	End Enum
	
	'Default Property Values:
	Const m_def_Table As Integer = 0
	Const m_def_ItemId As Integer = 0
	Const m_def_InsurerNo As Integer = 0
	Const m_def_ObjectID As Integer = 0
	
	'Const m_def_ItemCode = ""
	Const m_def_ItemDescription As String = ""
	Const m_def_DefaultItemId As Integer = 0
	Const m_def_FirstItem As String = ""
	
	'Property Variables:
	Dim m_Table As voyTable
	Dim m_ItemId As Integer
	Dim m_InsurerNo As Integer
	Dim m_lObjectID As Integer
	
	'Dim m_ItemCode As String
	Dim m_ItemDescription As String = ""
	Dim m_DefaultItemId As Integer
	Dim m_FirstItem As String = ""
	
	'Event Declarations:
	Event AfterUpdate(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,AfterUpdate
	Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboTypeTable,cboTypeTable,-1,Click
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
	
	<Browsable(True)> _
	<Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
	Public Shadows Property Enabled() As Boolean
		Get
			Return MyBase.Enabled
		End Get
		Set(ByVal Value As Boolean)
			MyBase.Enabled = Value
			RaiseEvent EnabledChange()
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property ObjectID() As Integer
		Get
			
			Return m_lObjectID
			
		End Get
		Set(ByVal Value As Integer)
			m_lObjectID = Value
			RaiseEvent ObjectIDChange()
			
			RefreshList()
			
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,Enabled
	
	<Browsable(True)> _
	Public Property Sorted() As Boolean
		Get
			Return cboTypeTable.Sorted
		End Get
		Set(ByVal Value As Boolean)
			If DesignMode Then Throw New System.Exception("382")
			'cboTypeTable.Sorted = New_Sorted
			RaiseEvent SortedChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,Font
	
	<Browsable(True)> _
	<Description("Returns a Font object.")> _
	Public Overrides Property Font() As Font
		Get
			Return MyBase.Font
		End Get
		Set(ByVal Value As Font)
			MyBase.Font = Value
			RaiseEvent FontChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,BackStyle
	
	<Browsable(True)> _
	<Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
	Public Property BackStyle() As Integer
		Get

            ' Return MyBase.BackStyle

		End Get
		Set(ByVal Value As Integer)

            'TODO:to be handled at runtime
            'MyBase.BackStyle = Value
            RaiseEvent BackStyleChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,BorderStyle
	
	<Browsable(True)> _
	<Description("Returns/sets the border style for an object.")> _
	Public Shadows Property BorderStyle() As Integer
		Get

			Return MyBase.BorderStyle
		End Get
		Set(ByVal Value As Integer)

			MyBase.BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
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
	
	<Browsable(True)> _
	<Description("Returns/sets the items contained in a control's list portion.")> _
	Public Property List(ByVal Index As Integer) As String
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
			cboTypeTable.SelectedIndex = Value
			RaiseEvent ListIndexChange()
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
	
	<Browsable(True)> _
	<Description("Returns/sets an associated context number for an object.")> _
	Public Property WhatsThisHelpID() As Integer
        Get
            'TODO:No solution founded yet

            'Return cboTypeTable.WhatsThisHelpID
        End Get
		Set(ByVal Value As Integer)
            'TODO:No solution founded yet

            'cboTypeTable.WhatsThisHelpID = Value
			RaiseEvent WhatsThisHelpIDChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=cboTypeTable,cboTypeTable,-1,NewIndex
	<Browsable(False)> _
	<Description("Returns the index of the item most recently added to a control.")> _
	Public ReadOnly Property NewIndex() As Integer
		Get
			Dim cboTypeTable_NewIndex As Integer = -1
			Return cboTypeTable_NewIndex
		End Get
	End Property
	
	' BB040298 Set the Type of Control to Combo for Form Control
	<Browsable(False)> _
	Public ReadOnly Property TypeOfControl() As Integer
		Get
			Return gPMConstants.PMEControlType.PMCombo
		End Get
	End Property
	
	
	<Browsable(True)> _
	<Description("The TypeTable to be bound to the control")> _
	<Category("Data")> _
	Public Property Table() As voyTable
		Get
			Return m_Table
		End Get
		Set(ByVal Value As voyTable)
			m_Table = Value
			RaiseEvent TableChange()
			RefreshList()
		End Set
	End Property
	
	<Browsable(True)> _
	<Description("The name of the selected table")> _
	<Category("Data")> _
	Public Property TableName() As String
		Get
			
			Select Case m_Table
				Case voyTable.gemrenewal_status
					Return "renewal_status"
				Case voyTable.gemrenewal_action
					Return "renewal_action"
				Case voyTable.geminsurer
					Return "insurer"
				Case voyTable.gemschemes
					Return "schemes"
				Case voyTable.newpolarisobjects
					Return "NP_Objects"
				Case voyTable.newpolarisproperties
					Return "NP_Properties"
					
				Case voyTable.gemII_scheme_group
					Return "Scheme_Group"
					
					'MN160799 - Needed for HKJ
				Case voyTable.HKJnewpolarisproperties
					
					Return "HKJNP_Properties"
					
				Case voyTable.HKJnewpolarisobjects
					
					Return "HKJNP_Objects"
					
				Case Else
					Return ""
			End Select
		End Get
		Set(ByVal Value As String)
			
			Select Case Value
				Case "renewal_status"
					Table = voyTable.gemrenewal_status
				Case "renewal_action"
					Table = voyTable.gemrenewal_action
				Case "insurer"
					Table = voyTable.geminsurer
				Case "schemes"
					Table = voyTable.gemschemes
				Case "NP_Objects"
					Table = voyTable.newpolarisobjects
				Case "NP_Properties"
					Table = voyTable.newpolarisproperties
					
				Case "scheme_group"
					Table = voyTable.gemII_scheme_group
					
				Case "HKJNP_Properties"
					
					Table = voyTable.HKJnewpolarisproperties
					
				Case "HKJNP_Objects"
					
					Table = voyTable.HKJnewpolarisobjects
					
				Case Else
					' Ignore it
			End Select
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property DefaultItemId() As Integer
		Get
			Return m_DefaultItemId
		End Get
		Set(ByVal Value As Integer)
			m_DefaultItemId = Value
			RaiseEvent DefaultItemIdChange()
		End Set
	End Property
	
	
	<Browsable(False)> _
	<Description("The Item id of the type entry")> _
	<Category("Data")> _
	Public Property ItemId() As Integer
		Get
			Return m_ItemId
		End Get
		Set(ByVal Value As Integer)
			If DesignMode Then Throw New System.Exception("382")
			
			m_ItemId = Value
			RaiseEvent ItemIdChange()
			With cboTypeTable
				For nIndex As Integer = 0 To .Items.Count - 1
					If VB6.GetItemData(cboTypeTable, nIndex) = m_ItemId Then
						.SelectedIndex = nIndex
						Exit For
					End If
				Next nIndex
			End With
			
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property InsurerNo() As Integer
		Get
			Return m_InsurerNo
		End Get
		Set(ByVal Value As Integer)
			m_InsurerNo = Value
			RaiseEvent InsurerNoChange()
		End Set
	End Property
	<Browsable(False)> _
	<Description("Description of the Type entry")> _
	<Category("Data")> _
	Public ReadOnly Property ItemDescription(Optional ByVal v_vItemId As Object = Nothing) As String
		Get
			Dim nIndex As Integer

			If Information.IsNothing(v_vItemId) Then
				Return m_ItemDescription
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
			Return m_FirstItem
		End Get
		Set(ByVal Value As String)
			If (Value.IndexOf("("c) + 1) = 0 Then
				m_FirstItem = "(" & Value & ")"
			Else
				m_FirstItem = Value
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
	
	Private Sub cboTypeTable_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTypeTable.SelectedIndexChanged
		
		With cboTypeTable
			m_ItemId = VB6.GetItemData(cboTypeTable, .SelectedIndex)
			RaiseEvent ItemIdChange()
			'    m_ItemCode = ""
			'    PropertyChanged "ItemCode"
			m_ItemDescription = VB6.GetItemString(cboTypeTable, .SelectedIndex)
			RaiseEvent ItemDescriptionChange()
		End With
		
		RaiseEvent Click(Me, Nothing)
		
	End Sub
	
	Private Sub Lookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
		RaiseEvent DblClick(Me, Nothing)
		
	End Sub
	
	Private Sub Lookup_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
		
	End Sub
	
    'Private Sub Lookup_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
    Private Sub Lookup_KeyPress(ByVal eventSender As Object, ByVal eventArgs As Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'developer guide no.79
        'RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub Lookup_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub Lookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
    End Sub

    Private Sub Lookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
    End Sub

    Private Sub Lookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,AddItem
    Public Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
        cboTypeTable.Items.Insert(Index, Item)
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboTypeTable,cboTypeTable,-1,RemoveItem
    Public Sub RemoveItem(ByRef Index As Integer)
        cboTypeTable.Items.RemoveAt(CShort(Index))
    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()

        'developer guide no solution no.2
        '  Font = Ambient.Font
        Font = MyBase.Font
        m_Table = voyTable.gemrenewal_status
        m_ItemId = m_def_ItemId
        m_InsurerNo = m_def_InsurerNo
        m_lObjectID = m_def_ObjectID

        '  m_ItemCode = m_def_ItemCode
        m_ItemDescription = m_def_ItemDescription
        m_FirstItem = m_def_FirstItem

    End Sub

    'Load property values from storage


    'developer guide no solutions no.1
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)



        cboTypeTable.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboTypeTable.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))
        'cboTypeTable.Sorted = PropBag.ReadProperty("Sorted", False)


        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer guide no solutions no.2
        'Font = PropBag.ReadProperty("Font", Ambient.Font)
        Font = PropBag.ReadProperty("Font", MyBase.Font)



        'TODO:to be handled at runtime
        'MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboTypeTable, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'TODO:solution yet to find
        ' cboTypeTable.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_Table = PropBag.ReadProperty("Table", m_def_Table)


        m_ItemId = CInt(PropBag.ReadProperty("ItemId", m_def_ItemId))


        m_InsurerNo = CInt(PropBag.ReadProperty("InsurerNo", m_def_InsurerNo))


        m_lObjectID = CInt(PropBag.ReadProperty("ObjectID", m_def_ObjectID))


        m_DefaultItemId = CInt(PropBag.ReadProperty("DefaultItemId", m_def_DefaultItemId))
        '  m_ItemCode = PropBag.ReadProperty("ItemCode", m_def_ItemCode)


        m_ItemDescription = CStr(PropBag.ReadProperty("ItemDescription", m_def_ItemDescription))


        m_FirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        ' Read the list of type table entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub Lookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboTypeTable.Height
        cboTypeTable.Width = MyBase.Width
    End Sub

    Private Sub UserControl_Terminate()
        Terminate()
    End Sub

    'Write property values to storage


    'developer guide no solutions no.1
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboTypeTable.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboTypeTable.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboTypeTable.Sorted, False)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, True)


        'developer guide no solutions no.2
        'PropBag.WriteProperty("Font", Font, Ambient.Font)
        PropBag.WriteProperty("Font", Font, MyBase.Font)


        'TODO:to be handled at runtime
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboTypeTable), "")


        'TODO:to be handled at runtime
        ' PropBag.WriteProperty("WhatsThisHelpID", cboTypeTable.WhatsThisHelpID, 0)

        PropBag.WriteProperty("Table", m_Table, m_def_Table)

        PropBag.WriteProperty("ItemId", m_ItemId, m_def_ItemId)

        PropBag.WriteProperty("InsurerNo", m_InsurerNo, m_def_InsurerNo)

        PropBag.WriteProperty("ObejectID", m_lObjectID, m_def_ObjectID)

        PropBag.WriteProperty("DefaultItemId", m_DefaultItemId, m_def_DefaultItemId)
        '  Call PropBag.WriteProperty("ItemCode", m_ItemCode, m_def_ItemCode)

        PropBag.WriteProperty("ItemDescription", m_ItemDescription, m_def_ItemDescription)

        PropBag.WriteProperty("FirstItem", m_FirstItem, m_def_FirstItem)
    End Sub
    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Dim sTableName As String = ""
        If Not DesignMode Then
            sTableName = TableName
            If sTableName <> "" Then
                cboTypeTable.Items.Clear()
                ' Entry at top of list for (All) or (None) etc
                If m_FirstItem <> "" Then
                    Dim cboTypeTable_NewIndex As Integer = -1
                    cboTypeTable_NewIndex = cboTypeTable.Items.Add(m_FirstItem)
                    VB6.SetItemData(cboTypeTable, cboTypeTable_NewIndex, 0)
                End If
                m_lReturn = CType(GetLookupValues(v_sTypeTable:=sTableName, v_dtEffectiveDate:=DateTime.Now, ctlLookup:=cboTypeTable, v_vInsurerNo:=m_InsurerNo, v_vObjectID:=m_lObjectID), gPMConstants.PMEReturnCode)

                ' Having filled the combo set it to it's default position
                If m_DefaultItemId <> 0 Then
                    ItemId = m_DefaultItemId
                Else
                    If cboTypeTable.Items.Count > 0 Then
                        cboTypeTable.SelectedIndex = 0
                    End If
                End If
            End If
        End If


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
End Class