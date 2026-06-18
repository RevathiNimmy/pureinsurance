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
<System.Runtime.InteropServices.ProgId("TypeTable_NET.TypeTable")> _
Partial Public Class TypeTable
    Inherits System.Windows.Forms.UserControl
    Public Event ItemDescriptionChange()
    Public Event FirstItemChange()
    Public Event ItemCodeChange()
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
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()

    ' Legal values for Table
    Public Enum actTable
        actDocumentType
        actPostingStatus
        actLedgerType
        actPurgeFrequency
        actAccountType
        actCashListType
        actMediaType
        actAllocationStatus
        actMapType
        actPaymentType
        actBatchStatus
        actTaxType
        actCashListStatus
    End Enum

    'Default Property Values:
    Const m_def_Table As Integer = 0
    Const m_def_ItemId As Integer = 0
    Const m_def_ItemCode As String = ""
    Const m_def_ItemDescription As String = ""
    Const m_def_DefaultItemId As Integer = 0
    Const m_def_FirstItem As String = ""

    'Property Variables:
    Dim m_Table As actTable
    Dim m_ItemId As Integer
    Dim m_ItemCode As String = ""
    Dim m_ItemDescription As String = ""
    Dim m_DefaultItemId As Integer
    Dim m_FirstItem As String = ""

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboTypeTable,cboTypeTable,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp


    ' Public source and language ID's from the
    ' Object Manager.
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer

    ' Public instance of the object manager.
    Private m_oObjectManager As bObjectManager.ObjectManager
    Private m_oTypeTableBusiness As Object

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

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    <Browsable(True)> _
    Public Property Sorted() As Boolean
        Get
            Return cboTypeTable.Sorted
        End Get
        Set(ByVal Value As Boolean)
            'If DesignMode Then Throw New System.Exception("382")
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
            Return cboTypeTable.Font
        End Get
        Set(ByVal Value As Font)
            cboTypeTable.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle

    <Browsable(True)> _
    <Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    Public Property BackStyle() As Integer
        Get


            'developer guide no solution 14 
            'Return MyBase.BackStyle
        End Get
        Set(ByVal Value As Integer)


            'developer guide no solution 14 
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

            'developer guide no solution 15
            'Return cboTypeTable.WhatsThisHelpID
        End Get
        Set(ByVal Value As Integer)


            'developer guide no solution 15
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


    <Browsable(True)> _
    <Description("The TypeTable to be bound to the control")> _
    <Category("Data")> _
    Public Property Table() As actTable
        Get
            Return m_Table
        End Get
        Set(ByVal Value As actTable)
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
                Case actTable.actDocumentType
                    Return "DocumentType"
                Case actTable.actPostingStatus
                    Return "PostingStatus"
                Case actTable.actLedgerType
                    Return "LedgerType"
                Case actTable.actPurgeFrequency
                    Return "PurgeFrequency"
                Case actTable.actAccountType
                    Return "AccountType"
                Case actTable.actCashListType
                    Return "CashListType"
                Case actTable.actMediaType
                    Return "MediaType"
                Case actTable.actAllocationStatus
                    Return "AllocationStatus"
                Case actTable.actMapType
                    Return "MapType"
                Case actTable.actPaymentType
                    Return "PaymentType"
                Case actTable.actBatchStatus
                    Return "BatchStatus"
                Case actTable.actTaxType
                    Return "TaxType"
                Case actTable.actCashListStatus
                    Return "CashListStatus"
                Case Else
                    Return ""
            End Select
        End Get
        Set(ByVal Value As String)
            Select Case Value
                Case "DocumentType"
                    Table = actTable.actDocumentType
                Case "PostingStatus"
                    Table = actTable.actPostingStatus
                Case "LedgerType"
                    Table = actTable.actLedgerType
                Case "PurgeFrequency"
                    Table = actTable.actPurgeFrequency
                Case "AccountType"
                    Table = actTable.actAccountType
                Case "CashListType"
                    Table = actTable.actCashListType
                Case "MediaType"
                    Table = actTable.actMediaType
                Case "AllocationStatus"
                    Table = actTable.actAllocationStatus
                Case "MapType"
                    Table = actTable.actMapType
                Case "PaymentType"
                    Table = actTable.actPaymentType
                Case "BatchStatus"
                    Table = actTable.actBatchStatus
                Case "TaxType"
                    Table = actTable.actTaxType
                Case "CashListStatus"
                    Table = actTable.actCashListStatus
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
            'If DesignMode Then Throw New System.Exception("382")

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


    <Browsable(False)> _
    <Description("The code entry from the TypeTable\r\nFetched from the business on each property get")> _
    <Category("Data")> _
    Public Property ItemCode() As String
        Get
            Dim result As String = String.Empty
            Dim bIsDeleted As Boolean
            Dim dtEffectiveDate As Date
            Dim sDescription As String = ""
            If Not DesignMode Then


                m_lReturn = CType(GetTypeTableEntry(m_ItemId, bIsDeleted, dtEffectiveDate, sDescription, m_ItemCode), gPMConstants.PMEReturnCode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_ItemCode
                    RaiseEvent ItemCodeChange()
                End If

            End If
            Return result
        End Get
        Set(ByVal Value As String)
            'If DesignMode Then Throw New System.Exception("382")
            'If Not DesignMode Then Throw New System.Exception("393")
            m_ItemCode = Value
            RaiseEvent ItemCodeChange()
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

    'Public Property Let ItemDescription(ByVal New_ItemDescription As String)
    '  If Ambient.UserMode = False Then Err.Raise 382
    '  If Ambient.UserMode Then Err.Raise 393
    '  m_ItemDescription = New_ItemDescription
    '  PropertyChanged "ItemDescription"
    'End Property


    <Browsable(True)> _
    <Description("String to be entered in the list before the TypeTable entries")> _
    <Category("Data")> _
    Public Property FirstItem() As String
        Get
            Return m_FirstItem
        End Get
        Set(ByVal Value As String)
            'developer guide no. 131
            If (Not String.IsNullOrEmpty(Value)) Then
                If (Value.IndexOf("("c) + 1) = 0 Then
                    m_FirstItem = "(" & Value & ")"
                Else
                    m_FirstItem = Value
                End If
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
        If cboTypeTable.SelectedIndex > -1 Then


            With cboTypeTable
                m_ItemId = VB6.GetItemData(cboTypeTable, .SelectedIndex)
                RaiseEvent ItemIdChange()
                m_ItemCode = ""
                RaiseEvent ItemCodeChange()
                m_ItemDescription = VB6.GetItemString(cboTypeTable, .SelectedIndex)
                RaiseEvent ItemDescriptionChange()
            End With
        End If
        RaiseEvent Click(Me, Nothing)

    End Sub

    Private Sub TypeTable_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub UserControl_Initialize()

        Try

            ' Have we been here before ?
            If m_oObjectManager Is Nothing And Not IsBuildMachine() Then

                ' Create an instance of the object manager.
                m_oObjectManager = New bObjectManager.ObjectManager()

                ' Call the initialise method.
                m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to call the initialise method.

                    ' Set the object manager to nothing.
                    m_oObjectManager = Nothing

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Exit Sub
                End If

                ' Store the language ID from the object manager
                ' to the public variables, to enable us to use
                ' them throughout the object.
                With m_oObjectManager
                    m_iLanguageID = .LanguageID
                    m_iSourceID = .SourceID
                End With

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub TypeTable_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub
    'refer developer guide no.42
    Private Sub TypeTable_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub TypeTable_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub

    Private Sub TypeTable_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub TypeTable_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub TypeTable_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
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


        'developer guide no solution 2
        Font = Me.Font
        m_Table = actTable.actDocumentType
        m_ItemId = m_def_ItemId
        m_ItemCode = m_def_ItemCode
        m_ItemDescription = m_def_ItemDescription
        m_FirstItem = m_def_FirstItem

    End Sub

    'Load property values from storage


    'developer guide no solution 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)




        cboTypeTable.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        cboTypeTable.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))
        ' cboTypeTable.Sorted = PropBag.ReadProperty("Sorted", False)


        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer guide no solution 2
        Font = PropBag.ReadProperty("Font", Me.Font)



        'developer guide no solution 15
        'MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)



        MyBase.BorderStyle = PropBag.ReadProperty("BorderStyle", 0)


        ToolTip1.SetToolTip(cboTypeTable, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developer guide no solution 15
        'cboTypeTable.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_Table = PropBag.ReadProperty("Table", m_def_Table)


        m_ItemId = CInt(PropBag.ReadProperty("ItemId", m_def_ItemId))


        m_DefaultItemId = CInt(PropBag.ReadProperty("DefaulttemId", m_def_DefaultItemId))


        m_ItemCode = CStr(PropBag.ReadProperty("ItemCode", m_def_ItemCode))


        m_ItemDescription = CStr(PropBag.ReadProperty("ItemDescription", m_def_ItemDescription))


        m_FirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))


        ' Read the list of type table entries
        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub TypeTable_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboTypeTable.Height
        cboTypeTable.Width = MyBase.Width
    End Sub

    Private Sub UserControl_Terminate()

        If Not (m_oTypeTableBusiness Is Nothing) Then

            m_oTypeTableBusiness.Dispose()
            m_oTypeTableBusiness = Nothing
        End If

    End Sub

    'Write property values to storage


    'developer guide no solution 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboTypeTable.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboTypeTable.ForeColor), &H80000008)

        PropBag.WriteProperty("Sorted", cboTypeTable.Sorted, False)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, True)


        'developer guide no solution 2
        PropBag.WriteProperty("Font", Font, Me.Font)


        'developer guide no solution 14
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)


        PropBag.WriteProperty("BorderStyle", MyBase.BorderStyle, 0)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboTypeTable), "")


        'developer guide no solution 15
        'PropBag.WriteProperty("WhatsThisHelpID", cboTypeTable.WhatsThisHelpID, 0)

        PropBag.WriteProperty("Table", m_Table, m_def_Table)

        PropBag.WriteProperty("ItemId", m_ItemId, m_def_ItemId)

        PropBag.WriteProperty("DefaultItemId", m_DefaultItemId, m_def_DefaultItemId)

        PropBag.WriteProperty("ItemCode", m_ItemCode, m_def_ItemCode)

        PropBag.WriteProperty("ItemDescription", m_ItemDescription, m_def_ItemDescription)

        PropBag.WriteProperty("FirstItem", m_FirstItem, m_def_FirstItem)

    End Sub
    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        Dim sTableName As String = ""
        If Not DesignMode And Not IsBuildMachine() Then
            sTableName = TableName
            If sTableName <> "" Then
                cboTypeTable.Items.Clear()
                ' Entry at top of list for (All) or (None) etc
                If m_FirstItem <> "" Then
                    Dim cboTypeTable_NewIndex As Integer = -1
                    cboTypeTable_NewIndex = cboTypeTable.Items.Add(m_FirstItem)
                    VB6.SetItemData(cboTypeTable, cboTypeTable_NewIndex, 0)
                End If
                m_lReturn = CType(GetLookupValues(v_sTypeTable:=sTableName, v_dtEffectiveDate:=DateTime.Now, ctlLookup:=cboTypeTable), gPMConstants.PMEReturnCode)
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

    Function GetTypeTableBusiness() As Integer

        Dim result As Integer = 0
        Try

            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oTypeTableBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                Dim temp_m_oTypeTableBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oTypeTableBusiness, "bACTTypeTable.Form", vInstanceManager:="ClientManager")
                m_oTypeTableBusiness = temp_m_oTypeTableBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypeTable business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness")
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type table business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'developer guide no.101
    Private Function GetLookupValues(ByVal v_sTypeTable As String, ByVal v_dtEffectiveDate As Date, ByRef ctlLookup As ComboBox) As Integer

        ' To hold the lookup results
        Dim result As Integer = 0
        Dim vLookupValues, vLookupDetails As Object


        ' Lookup value contants.
        'Const ACValueTableName As Integer = 0
        'Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetTypeTableBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set the business to look at the particular table in question

            m_oTypeTableBusiness.TypeTableName = v_sTypeTable

            ' Get all of the lookup values with effective date passed


            m_lReturn = m_oTypeTableBusiness.GetLookupValues(dtEffectiveDate:=v_dtEffectiveDate, vTableArray:=vLookupValues, iLanguageID:=m_iLanguageID, vResultArray:=vLookupDetails)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", oDicParms:=oDict)

                Return result
            End If

            ' Now load the values into the combo/list box passed in




            For lCntr As Integer = CInt(vLookupValues(ACValueStartPos, 0)) To CInt((CDbl(vLookupValues(ACValueStartPos, 0)) + CDbl(vLookupValues(ACValueNumber, 0))) - 1)

                'developer guide no.29
                ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(vLookupDetails(ACDetailDesc, lCntr)), CInt(vLookupDetails(ACDetailKey, lCntr))))
            Next lCntr

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetTypeTableEntry(ByVal v_lTypeTableID As Integer, ByRef r_bIsDeleted As Boolean, ByRef r_dtEffectiveDate As Date, ByRef r_sDescription As String, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetTypeTableBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oTypeTableBusiness.GetDetails(vTypeTableID:=v_lTypeTableID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oTypeTableBusiness.getnext(vIsDeleted:=r_bIsDeleted, vEffectiveDate:=r_dtEffectiveDate, vDescription:=r_sDescription, vCode:=r_sCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get type table entry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class