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
<System.Runtime.InteropServices.ProgId("CurrencyLookup_NET.CurrencyLookup")> _
Partial Public Class CurrencyLookup
    Inherits System.Windows.Forms.UserControl
    Public Event DefaultCurrencyIdChange()
    Public Event CompanyIdChange()
    Public Event RestrictToChange()
    Public Event CurrencyIdChange()
    Public Event TextChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event ItemDataChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event FirstItemChange()

    ' Values for Restrict To Property
    Public Enum RestrictToCurrency
        actAllCurrencies = 2
        actCompanyCurrencies = 1
        actNonCompanyCurrencies = 3
        actBaseCurrencies = 4
    End Enum

    'Default Property Values:
    Const m_def_CurrencyId As Integer = 0
    Const m_def_CurrencyCode As String = ""
    Const m_def_CurrencyName As String = ""
    Dim m_def_RestrictTo As RestrictToCurrency = RestrictToCurrency.actAllCurrencies
    Const m_def_CompanyId As Integer = 0
    Const m_def_DefaultCurrencyId As Integer = 0
    Const m_def_FirstItem As String = ""

    'Property Variables:
    Dim m_CurrencyId As Integer
    Dim m_CurrencyCode As String = ""
    Dim m_CurrencyName As String = ""
    Dim m_RestrictTo As RestrictToCurrency
    Dim m_CompanyId As Integer
    Dim m_DefaultCurrencyId As Integer
    Dim m_FirstItem As String = ""

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboCurrency,cboCurrency,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=cboCurrency,cboCurrency,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=cboCurrency,cboCurrency,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=cboCurrency,cboCurrency,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp

    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "CurrencyLookup"


    <Browsable(True)> _
    Public Property FirstItem() As String
        Get
            Return m_FirstItem
        End Get
        Set(ByVal Value As String)
            m_FirstItem = Value
            If Value.Trim <> String.Empty Then 'developer guide no. 131(Guide)
                If m_FirstItem.Substring(0, 1) <> "(" Then
                    m_FirstItem = "(" & m_FirstItem
                End If
                If Not m_FirstItem.EndsWith(")") Then
                    m_FirstItem = m_FirstItem & ")"
                End If
            End If 'developer guide no. 131(Guide)
            RaiseEvent FirstItemChange()
            If Not DesignMode Then
                RefreshList()
            End If

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
            cboCurrency.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboCurrency.Font
        End Get
        Set(ByVal Value As Font)
            cboCurrency.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,List

    <Browsable(True)> _
    <Description("Returns/sets the items contained in a control's list portion.")> _
    Public Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboCurrency, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboCurrency, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboCurrency.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboCurrency.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            cboCurrency.SelectedIndex = Value
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,ItemData

    <Browsable(True)> _
    <Description("Returns/sets a specific number for each item in a ComboBox or ListBox control.")> _
    Public Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboCurrency, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboCurrency, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,Sorted
    <Browsable(False)> _
    <Description("Indicates whether the elements of a control are automatically sorted alphabetically.")> _
    Public ReadOnly Property Sorted() As Boolean
        Get
            Return cboCurrency.Sorted
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboCurrency)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboCurrency, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,WhatsThisHelpID

    <Browsable(True)> _
    <Description("Returns/sets an associated context number for an object.")> _
    Public Property WhatsThisHelpID() As Integer
        Get

            'developer guide no solution no 15
            'Return cboCurrency.WhatsThisHelpID
        End Get
        Set(ByVal Value As Integer)

            'developer guide no solution no 15
            'cboCurrency.WhatsThisHelpID = Value
            RaiseEvent WhatsThisHelpIDChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,Text

    <Browsable(True)> _
    <Description("Returns/sets the text contained in the control.")> _
    Public Overrides Property Text() As String
        Get
            Return cboCurrency.Text
        End Get
        Set(ByVal Value As String)
            cboCurrency.Text = Value
            RaiseEvent TextChange()
        End Set
    End Property


    <Browsable(False)> _
    Public Property CurrencyId() As Integer
        Get
            Return m_CurrencyId
        End Get
        Set(ByVal Value As Integer)
            'todo:to be handled later
            'If DesignMode Then Throw New System.Exception("382")

            m_CurrencyId = Value
            RaiseEvent CurrencyIdChange()
            With cboCurrency
                For nIndex As Integer = 0 To .Items.Count - 1
                    If VB6.GetItemData(cboCurrency, nIndex) = m_CurrencyId Then
                        .SelectedIndex = nIndex
                        Exit For
                    End If
                Next nIndex
            End With

        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property CurrencyCode(Optional ByVal v_vCurrencyId As Object = Nothing) As String
        Get
            Dim nIndex As Integer
            Dim sTextEntry As String = ""


            If Information.IsNothing(v_vCurrencyId) Then
                Return m_CurrencyCode
            Else

                nIndex = IndexOfItem(CInt(v_vCurrencyId))

                If (nIndex < 0) Or (CInt(v_vCurrencyId) < 1) Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboCurrency, nIndex).Substring(0, 3)
                End If
            End If
        End Get
    End Property

    'Public Property Let CurrencyCode(ByVal New_CurrencyCode As String)
    '  If Ambient.UserMode = False Then Err.Raise 382
    '  If Ambient.UserMode Then Err.Raise 393
    '  m_CurrencyCode = New_CurrencyCode
    '  PropertyChanged "CurrencyCode"
    'End Property

    <Browsable(False)> _
    Public ReadOnly Property CurrencyName(Optional ByVal v_vCurrencyId As Object = Nothing) As String
        Get
            Dim nIndex As Integer

            If Information.IsNothing(v_vCurrencyId) Then
                Return m_CurrencyName
            Else

                nIndex = IndexOfItem(CInt(v_vCurrencyId))
                If nIndex < 0 Then
                    Return ""
                Else
                    Return VB6.GetItemString(cboCurrency, nIndex)
                End If
            End If

        End Get
    End Property

    'Public Property Let CurrencyName(ByVal New_CurrencyName As String)
    '  If Ambient.UserMode = False Then Err.Raise 382
    '  If Ambient.UserMode Then Err.Raise 393
    '  m_CurrencyName = New_CurrencyName
    '  PropertyChanged "CurrencyName"
    'End Property


    <Browsable(True)> _
    Public Property RestrictTo() As RestrictToCurrency
        Get
            Return m_RestrictTo
        End Get
        Set(ByVal Value As RestrictToCurrency)
            m_RestrictTo = Value
            RaiseEvent RestrictToChange()
            If Not DesignMode Then
                RefreshList()
            End If
        End Set
    End Property


    <Browsable(False)> _
    Public Property CompanyId() As Integer
        Get
            Return m_CompanyId
        End Get
        Set(ByVal Value As Integer)
            'todo:to be handled later
            'If DesignMode Then Throw New System.Exception("382")
            m_CompanyId = Value
            RaiseEvent CompanyIdChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property DefaultCurrencyId() As Integer
        Get
            Return m_DefaultCurrencyId
        End Get
        Set(ByVal Value As Integer)
            m_DefaultCurrencyId = Value
            RaiseEvent DefaultCurrencyIdChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub

    Private Sub cboCurrency_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.SelectedIndexChanged
        With cboCurrency
            'developer guide no. 131
            If Not String.IsNullOrEmpty(.Text) Then
                m_CurrencyCode = .Text.Substring(0, 3)
                If (.Text.IndexOf(" "c) + 1) = 0 Then
                    m_CurrencyName = .Text
                Else
                    m_CurrencyName = Mid(.Text, .Text.IndexOf(" "c) + 1).Trim()
                End If
                m_CurrencyId = VB6.GetItemData(cboCurrency, .SelectedIndex)
            End If
        End With
        RaiseEvent Click(Me, Nothing)
    End Sub

    Private Sub CurrencyLookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboCurrency_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCurrency.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub

    'developer guide no. 78
    Private Sub cboCurrency_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboCurrency.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboCurrency_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCurrency.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
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
                    g_iLanguageID = .LanguageID
                    g_iSourceID = .SourceID
                End With

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub CurrencyLookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub CurrencyLookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub CurrencyLookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,RemoveItem
    Public Sub RemoveItem(ByRef Index As Integer)
        cboCurrency.Items.RemoveAt(CShort(Index))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboCurrency,cboCurrency,-1,AddItem
    Public Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
        cboCurrency.Items.Insert(Index, Item)
    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_CurrencyId = m_def_CurrencyId
        m_CurrencyCode = m_def_CurrencyCode
        m_CurrencyName = m_def_CurrencyName
        m_RestrictTo = m_def_RestrictTo
        m_CompanyId = m_def_CompanyId
        m_DefaultCurrencyId = m_def_DefaultCurrencyId
    End Sub

    'Load property values from storage


    'developer guide no solution no 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)

        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer guide no solution no 2
        Font = PropBag.ReadProperty("Font", Me.Font)


        ToolTip1.SetToolTip(cboCurrency, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developer guide no solution no 15
        'cboCurrency.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_CurrencyId = CInt(PropBag.ReadProperty("CurrencyId", m_def_CurrencyId))


        m_CurrencyCode = CStr(PropBag.ReadProperty("CurrencyCode", m_def_CurrencyCode))


        m_CurrencyName = CStr(PropBag.ReadProperty("CurrencyName", m_def_CurrencyName))


        m_RestrictTo = PropBag.ReadProperty("RestrictTo", m_def_RestrictTo)


        m_CompanyId = CInt(PropBag.ReadProperty("CompanyId", m_def_CompanyId))


        m_DefaultCurrencyId = CInt(PropBag.ReadProperty("DefaultCurrencyId", m_def_DefaultCurrencyId))


        m_FirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))
        ' Only at Run Time

        If Not DesignMode Then
            RefreshList()
        End If

    End Sub

    Private Sub CurrencyLookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboCurrency.Height
        cboCurrency.Width = MyBase.Width
    End Sub

    Private Sub UserControl_Terminate()

    End Sub

    'Write property values to storage


    'developer guide no solution no 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, True)


        'developer guide no solution no 2
        PropBag.WriteProperty("Font", Font, Me.Font)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboCurrency), "")


        'developer guide no solution no 15
        'PropBag.WriteProperty("WhatsThisHelpID", cboCurrency.WhatsThisHelpID, 0)

        PropBag.WriteProperty("Text", cboCurrency.Text, "")

        PropBag.WriteProperty("CurrencyId", m_CurrencyId, m_def_CurrencyId)

        PropBag.WriteProperty("CurrencyCode", m_CurrencyCode, m_def_CurrencyCode)

        PropBag.WriteProperty("CurrencyName", m_CurrencyName, m_def_CurrencyName)

        PropBag.WriteProperty("RestrictTo", m_RestrictTo, m_def_RestrictTo)

        PropBag.WriteProperty("CompanyId", m_CompanyId, m_def_CompanyId)

        PropBag.WriteProperty("DefaultCurrencyId", m_DefaultCurrencyId, m_def_DefaultCurrencyId)

        PropBag.WriteProperty("FirstItem", m_FirstItem, m_def_FirstItem)

    End Sub

    ' Read or Reread the list of type table entries
    Public Sub RefreshList()

        If Not DesignMode And Not IsBuildMachine() Then
            cboCurrency.Items.Clear()
            If m_FirstItem <> "" Then
                Dim cboCurrency_NewIndex As Integer = -1
                cboCurrency_NewIndex = cboCurrency.Items.Add(m_FirstItem)
                VB6.SetItemData(cboCurrency, cboCurrency_NewIndex, 0)
            End If

            m_lReturn = CType(GetCompanyCurrencies(v_lMode:=m_RestrictTo, v_lDefaultCurrencyId:=m_DefaultCurrencyId, cboCurrency:=cboCurrency, oParent:=MyBase.FindForm(), v_lCompanyID:=m_CompanyId), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Function IndexOfItem(ByVal v_lItemId As Integer) As Integer
        With cboCurrency
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(cboCurrency, nIndex) = v_lItemId Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    'developer guide no. 9 (Guide)
    Private Function GetCompanyCurrencies(ByVal v_lMode As Integer, ByVal v_lDefaultCurrencyId As Integer, ByRef cboCurrency As ComboBox, ByRef oParent As Object, ByVal v_lCompanyID As Integer) As Integer
        Dim result As Integer = 0
        Dim sText, sCode As String
        Dim vCompanyCurrencies(,) As Object
        Dim sMessage As String = ""

        Dim oBusiness As bACTCompanyCurrency.Form

        Dim vValue As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager
            m_lReturn = Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If oBusiness Is Nothing Then
                ' Get a Company Currency Business Object
                Dim temp_oBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_oBusiness, "bACTCompanyCurrency.Form", vInstanceManager:="ClientManager")
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of bACTCompanyCurrency.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    Return result
                End If
            End If

            'Set company id from parent object
            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=CStr(vValue)), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GETPRODUCTOPTIONVALUE Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set default company id
            If vValue <> 1 Then

                oBusiness.CompanyID = 1
            Else

                oBusiness.CompanyID = g_iSourceID
            End If

            'Set company id from parameter
            If v_lCompanyID <> 0 Then

                oBusiness.CompanyID = v_lCompanyID
            End If

            ' Get list of company currencies

            m_lReturn = oBusiness.GetCompanyCurrencies(lNumberOfRecords:=0, vResultArray:=vCompanyCurrencies, vnMode:=v_lMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DJM 02/02/2004 : Don't display error if it didn't find any.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    sMessage = "Unable to retrieve currency details."

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies")

                End If

                Return m_lReturn
            End If


            ' Load the combo box with the list
            With cboCurrency

                ' Work out the length in twips of the longest code
                '      nMaxCodeSize = 0
                '      For nRow = LBound(vCompanyCurrencies, 2) To UBound(vCompanyCurrencies, 2)
                '        nCodeSize = oParent.TextWidth(Trim(vCompanyCurrencies(ACTCurrencyISOCode, nRow)))
                '        If nCodeSize > nMaxCodeSize Then
                '          nMaxCodeSize = nCodeSize
                '        End If
                '      Next nRow


                For nRow As Integer = vCompanyCurrencies.GetLowerBound(1) To vCompanyCurrencies.GetUpperBound(1)

                    ' Add spaces to each code entry to make them line up
                    '        nSpaceSize = oParent.TextWidth(Space(1))

                    sCode = CStr(vCompanyCurrencies(ACTCurrencyISOCode, nRow)).Trim()
                    '        nCodeSize = oParent.TextWidth(sCode)
                    '        sCode = sCode & Space(((nMaxCodeSize - nCodeSize) / nSpaceSize) + 1)

                    sText = sCode & " " & CStr(vCompanyCurrencies(ACTCurrencyDescription, nRow)).Trim()

                    'developer guide no. 29(Guide)
                    cboCurrency.Items.Add(New VB6.ListBoxItem(sText, vCompanyCurrencies(ACTCurrencyId, nRow)))

                Next nRow


                'developer guide no. 29(Guide)
                If cboCurrency.Items.Count > 0 Then
                    If v_lDefaultCurrencyId <> 0 Then

                        'developer guide no. 29(Guide)
                        For nRow As Integer = 0 To cboCurrency.Items.Count - 1
                            If Convert.ToInt32(cboCurrency.Items(nRow).itemdata) = v_lDefaultCurrencyId Then

                                'developer guide no. 28(Guide)
                                cboCurrency.SelectedIndex = nRow
                                Exit For
                            End If
                        Next nRow
                    End If

                    'developer guide no. 28(Guide)
                    If cboCurrency.SelectedIndex < 0 Then

                        'developer guide no. 28(Guide)
                        cboCurrency.SelectedIndex = 0
                    End If
                End If
            End With


            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get company currencies", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyCurrencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
