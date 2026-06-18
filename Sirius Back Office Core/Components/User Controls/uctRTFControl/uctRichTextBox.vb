Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctRichTextBox_NET.uctRichTextBox")> _
Partial Public Class uctRichTextBox
    Inherits System.Windows.Forms.UserControl
    Public Event MaxLengthChange()
    Public Event ShowToolbarChange()
    Public Event BulletIndentChange()
    Public Event TextRTFChange()
    Public Event TextChange()
    Public Event kToolbarButtonFontChange()
    Public Event ForeColorChange()
    Public Event EnabledChange()
    Public Event BorderStyleChange()
    Public Event BackColorChange()
    Private checkPrint As Integer
    Private richtextboxprintctrl1 As RichTextBoxPrintCtrl

    Private Const ACClass As String = "uctRichTextBox"

    ' Constants used to define marking effects for each rich-text box,
    ' used to ensure the effects are consistent.
    Private Const SpellCheckColorBackGround As Integer = SSCE_BACKGROUND_MARK_COLOR Or SSCE_BACKGROUND_CONTEXT_MENU

    Private SpellCheckErrorColor As Integer = ColorTranslator.ToOle(Color.Red)

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Const WM_PASTE As Integer = &H302S

    'Default Property Values:
    Const m_def_mDataField As String = "0"
    Const m_def_Enabled As Boolean = True
    Const m_def_ForeColor As Integer = 0
    Const m_def_TextRTF As String = ""
    Const m_def_Show_Toolbar As Boolean = True
    Const m_def_IndentSize As Integer = 500
    Const m_def_Printername As String = ""

    Private m_bBold As Boolean
    Private m_bItalic As Boolean
    Private m_bUnderline As Boolean
    Private m_bStrikeThru As Boolean

    Private m_bRedoUndoInProgress As Boolean
    Private m_iCurrentStackMemberIndex As Integer
    Private m_asStack() As String = New String() {}
    Private m_bShowToolbar As Boolean
    Private m_bEnterPressed As Boolean
    'Property Variables:
    Private m_bEnabled As Boolean
    Private m_lForeColor As Integer
    Private m_bFontUnderline As Boolean
    Private m_bFontStrikethru As Boolean
    Private m_dFontSize As Single
    Private m_sFontName As String = ""
    Private m_bFontItalic As Boolean
    Private m_bFontBold As Boolean
    Private m_sTextRTF As String = ""
    Private m_bSpellCheck As Boolean
    Private m_sPrinterName As String = ""
    '*******************************************************************
    ' Exposed Properties
    '*******************************************************************
    ' spellcheck            - default to false
    ' backcolor             - backcolor
    ' borderstyle           - border style
    ' enabled               - enabled
    ' forecolor             - selected font color
    ' font                  - font
    ' text                  - text
    ' textrtf               - rtf text
    ' bulletindent          - size of the bullet indent
    ' showtoolbar           - show / hide toolbar
    ' maxlength             - max length of rtf field
    ' printername           - "" means use default printer
    '*******************************************************************

    ' printername

    <Browsable(True)> _
    Public Property PrinterName() As String
        Get
            Return m_sPrinterName
        End Get
        Set(ByVal Value As String)
            m_sPrinterName = Value
        End Set
    End Property

    ' spellcheck

    <Browsable(True)> _
    Public Property SpellCheck() As Boolean
        Get
            Return m_bSpellCheck
        End Get
        Set(ByVal Value As Boolean)
            m_bSpellCheck = Value
        End Set
    End Property

    ' backcolor

    <Browsable(True)> _
    <Description("Returns/sets the background color of an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return rtfEdit.BackColor
        End Get
        Set(ByVal Value As Color)
            rtfEdit.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    ' borderstyle

    <Browsable(True)> _
    <Description("Returns/sets the border style for an object.")> _
    Public Shadows Property BorderStyle() As BorderStyle
        Get
            Return rtfEdit.BorderStyle
        End Get
        Set(ByVal Value As BorderStyle)

            Try
                rtfEdit.BorderStyle = Value
                RaiseEvent BorderStyleChange()

            Catch exc As System.Exception
                'Developer Guide No 32
            End Try
        End Set
    End Property

    ' enabled

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

    ' forecolor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return ColorTranslator.FromOle(m_lForeColor)
        End Get
        Set(ByVal Value As Color)
            m_lForeColor = ColorTranslator.ToOle(Value)
            RaiseEvent ForeColorChange()
        End Set
    End Property

    ' font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return rtfEdit.Font
        End Get
        Set(ByVal Value As Font)
            rtfEdit.Font = Value
            RaiseEvent kToolbarButtonFontChange()
        End Set
    End Property

    ' text
    <Browsable(True)> _
    <Description("Returns/sets the text contained in an object.")> _
    Public Overrides Property text() As String
        Get
            Return rtfEdit.Text
        End Get
        Set(ByVal Value As String)
            rtfEdit.Text = Value
            RaiseEvent TextChange()
        End Set
    End Property

    ' textrtf

    <Browsable(True)> _
    Public Property TextRTF() As String
        Get
            Return rtfEdit.Rtf
        End Get
        Set(ByVal Value As String)

            rtfEdit.Rtf = Value
            RaiseEvent TextRTFChange()
        End Set
    End Property

    ' bulletindent

    <Browsable(True)> _
    Public Property BulletIndent() As Integer
        Get
            'Developer Guide No 70
            Return rtfEdit.BulletIndent
        End Get
        Set(ByVal Value As Integer)
            'developer table no. 70
            rtfEdit.BulletIndent = Value
            RaiseEvent BulletIndentChange()
        End Set
    End Property

    ' showtoolbar

    <Browsable(True)> _
    Public Property ShowToolbar() As Boolean
        Get
            Return m_bShowToolbar
        End Get
        Set(ByVal Value As Boolean)

            m_bShowToolbar = Value
            tlbMain.Visible = Value

            ' resize the user control to fill the area when then
            ' toolbar is hidden
            If Not m_bShowToolbar Then
                rtfEdit.Height = MyBase.Height
                rtfEdit.Width = MyBase.Width
                rtfEdit.Top = 0
                rtfEdit.Left = 0
            Else
                rtfEdit.Top = tlbMain.Height
            End If

            RaiseEvent ShowToolbarChange()

        End Set
    End Property
    ' maxlength
    <Browsable(True)> _
    Public Property MaxLength() As Integer
        Get
            Return rtfEdit.MaxLength
        End Get
        Set(ByVal Value As Integer)
            rtfEdit.MaxLength = Value
            RaiseEvent MaxLengthChange()
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property Locked() As Boolean
        Set(ByVal Value As Boolean)
            rtfEdit.ReadOnly = Value
            If rtfEdit.ReadOnly = True Then
                With tlbMain
                    .Items.Item(kToolbarButtonBold).Enabled = False
                    .Items.Item(kToolbarButtonItalic).Enabled = False
                    .Items.Item(kToolbarButtonUnderline).Enabled = False
                    .Items.Item(kToolbarButtonBullet).Enabled = False
                    .Items.Item(kToolbarButtonLeft).Enabled = False
                    .Items.Item(kToolbarButtonRight).Enabled = False
                    .Items.Item(kToolbarButtonCenter).Enabled = False
                    .Items.Item(kToolbarButtonIncreaseIndent).Enabled = False
                    .Items.Item(kToolbarButtonDecreaseIndent).Enabled = False
                    .Items.Item(kToolbarButtonFont).Enabled = False
                    .Items.Item(kToolbarButtonCut).Enabled = False
                    .Items.Item(kToolbarButtonCopy).Enabled = False
                    .Items.Item(kToolbarButtonUndo).Enabled = False
                    .Items.Item(kToolbarButtonRedo).Enabled = False
                    .Items.Item(kToolbarButtonPaste).Enabled = True
                End With
            End If
        End Set
    End Property
    ''' <summary>
    ''' User Control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UserControl_Initialize()
        ReDim m_asStack(100)
        With tlbMain
            .Items.Item(kToolbarButtonCut).Enabled = False
            .Items.Item(kToolbarButtonCopy).Enabled = False
            .Items.Item(kToolbarButtonUndo).Enabled = False
            .Items.Item(kToolbarButtonRedo).Enabled = False
        End With
        SSCEVB_SetKey(CInt("1576248155"))
        SSCEVB_SetIniFile("ssce.ini")
    End Sub


    Private Sub UserControl_InitProperties()
        ' do nothing
    End Sub

    'Load property values from storage


    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))

        rtfEdit.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))
        rtfEdit.BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", 1))
        rtfEdit.SelectionColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor)))
        rtfEdit.Font = PropBag.ReadProperty(kToolbarButtonFont, Me.butFont)
        rtfEdit.MaxLength = CInt(PropBag.ReadProperty("MaxLength", 0))
        ShowToolbar = CBool(PropBag.ReadProperty("ShowToolbar", m_def_Show_Toolbar))
        rtfEdit.BulletIndent = PropBag.ReadProperty("BulletIndent", m_def_IndentSize)
        m_sPrinterName = CStr(PropBag.ReadProperty("PrinterName", m_def_Printername))

    End Sub

    'Write property values to storage


    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        PropBag.WriteProperty("Enabled", MyBase.Enabled, m_def_Enabled)
        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(rtfEdit.BackColor), &H80000005)
        PropBag.WriteProperty("BorderStyle", rtfEdit.BorderStyle, 1)
        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(rtfEdit.SelectionColor), m_def_ForeColor)
        PropBag.WriteProperty(kToolbarButtonFont, rtfEdit.Font, Me.butFont)
        PropBag.WriteProperty("MaxLength", rtfEdit.MaxLength, 0)
        PropBag.WriteProperty("ShowToolbar", m_bShowToolbar, m_def_Show_Toolbar)
        PropBag.WriteProperty("BulletIndent", rtfEdit.BulletIndent, m_def_IndentSize)
        PropBag.WriteProperty("PrinterName", m_sPrinterName, m_def_Printername)

    End Sub

    Private Sub uctRichTextBox_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try
            rtfEdit.SetBounds(0, tlbMain.Height, MyBase.ClientRectangle.Width, MyBase.ClientRectangle.Height - tlbMain.Height)
        Catch
        End Try

    End Sub

    Private Sub uctRichTextBox_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Enter

        rtfEdit.Focus()

    End Sub

    Private Sub tlbMain_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles butPrint.Click, butPreview.Click, butNew.Click, butSave.Click, butOpen.Click, _tlbMain_Button6.Click, butCut.Click, butCopy.Click, butPaste.Click, _tlbMain_Button10.Click, butUndo.Click, butRedo.Click, _tlbMain_Button13.Click, butBold.Click, butItalic.Click, butUnderline.Click, butStrikeThru.Click, _tlbMain_Button18.Click, butColor.Click, butFont.Click, _tlbMain_Button21.Click, butLeft.Click, butCenter.Click, butRight.Click, _tlbMain_Button25.Click, butBullet.Click, butDecreaseIndent.Click, butIncreaseIndent.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)


        Select Case Button.Name
            ' Redo / Undo
            Case kToolbarButtonUndo
                ToolbarActionUndo()
            Case kToolbarButtonRedo
                ToolbarActionRedo()
                ' Selected Text Cut / Paste
            Case kToolbarButtonCut
                ToolbarActionCut()
            Case kToolbarButtonCopy
                ToolbarActionCopy()
            Case kToolbarButtonPaste
                ToolbarActionPaste()

                ' Selected Text Formatting
            Case kToolbarButtonColor
                ToolbarActionSelectFontColor()
            Case kToolbarButtonFont
                ToolbarActionSelectFont()
            Case kToolbarButtonStrikeThru
                ToolbarActionSetStrikeThrough()
            Case kToolbarButtonUnderline
                ToolbarActionSetUnderline()
            Case kToolbarButtonItalic
                ToolbarActionSetItalic()
            Case kToolbarButtonBold
                ToolbarActionSetBold()
            Case kToolbarButtonLeft
                ToolbarActionAlign(HorizontalAlignment.Left)
            Case kToolbarButtonCenter
                ToolbarActionAlign(HorizontalAlignment.Center)
            Case kToolbarButtonRight
                ToolbarActionAlign(HorizontalAlignment.Right)
            Case kToolbarButtonIncreaseIndent
                ToolbarActionIncreaseIndent()
            Case kToolbarButtonDecreaseIndent
                ToolbarActionDecreaseIndent()
            Case kToolbarButtonBullet
                ToolbarActionSetBullets()

                ' Print / Preview
            Case kToolbarButtonPrint
                ToolbarActionPrint()
            Case kToolbarButtonPreview
                ToolbarActionPrintPreview()

            Case Else
                MessageBox.Show("ToolbarAction Not Supported", Application.ProductName)

        End Select

    End Sub

    Private Sub rtfEdit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles rtfEdit.KeyDown
        m_bEnterPressed = e.KeyValue = 13
    End Sub

    Private Sub rtfEdit_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles rtfEdit.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If Shift = ShiftConstants.CtrlMask And eventArgs.KeyCode = Keys.Z Then
            Undo()
        End If

        If Shift = ShiftConstants.CtrlMask And eventArgs.KeyCode = Keys.Y Then
            Redo()
        End If

        If rtfEdit.ReadOnly = False Then
            If rtfEdit.SelectedText <> "" Then
                tlbMain.Items.Item(kToolbarButtonCopy).Enabled = True
                tlbMain.Items.Item(kToolbarButtonCut).Enabled = True
            Else
                tlbMain.Items.Item(kToolbarButtonCopy).Enabled = False
                tlbMain.Items.Item(kToolbarButtonCut).Enabled = False
            End If
        End If
        SetButtons()

    End Sub

    Private Sub rtfEdit_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles rtfEdit.TextChanged
        Dim ResizeOnOutOfBoundsError As Boolean = False

        ' if the spellcheck is enabled
        If m_bSpellCheck And Not m_bEnterPressed Then
            SSCEVB_CheckCtrlBackgroundNotify(rtfEdit.Handle.ToInt32(), SpellCheckColorBackGround, SpellCheckErrorColor)
        End If

        ' if this is not a redo / undo ToolbarAction
        Try
            If Not m_bRedoUndoInProgress Then
                If m_asStack.Length <> 0 Then
                    If m_asStack(m_iCurrentStackMemberIndex) <> rtfEdit.Rtf Then

                        m_iCurrentStackMemberIndex += 1

                        ResizeOnOutOfBoundsError = True

                        m_asStack(m_iCurrentStackMemberIndex) = rtfEdit.Rtf

                        '            Debug.Print "'*******************************************"
                        '            Debug.Print m_iCurrentStackMemberIndex
                        '            Debug.Print rtfEdit.TextRTF
                        '            Debug.Print rtfEdit.text
                        '            Debug.Print "'*******************************************"
                        If rtfEdit.ReadOnly = False Then
                            tlbMain.Items.Item(kToolbarButtonUndo).Enabled = True
                        End If
                    End If
                End If
            End If

        Catch excep As System.Exception
            If Not ResizeOnOutOfBoundsError Then
                Throw excep
            End If

            If ResizeOnOutOfBoundsError Then


                ' if subscript out of range
                If Information.Err().Number = 9 Then

                    If m_iCurrentStackMemberIndex > m_asStack.GetUpperBound(0) Then
                        ' resize the stack to handle more ToolbarActions
                        ReDim Preserve m_asStack(m_asStack.GetUpperBound(0) + 100)
                    End If

                    ' retry assignment



                End If


            End If
        End Try

    End Sub

    Private Sub rtfEdit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles rtfEdit.Enter

        If m_bSpellCheck Then
            SSCEVB_CheckCtrlBackgroundNotify(rtfEdit.Handle.ToInt32(), SpellCheckColorBackGround, SpellCheckErrorColor)
        End If

    End Sub

    Private Sub rtfEdit_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles rtfEdit.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim xPos, yPos As Integer
        If m_bSpellCheck Then

            If ((Button And MouseButtons.Right) <> 0) And (Shift = 0) Then

                ' Right mouse clicked. The mouse position is specified in twips.
                ' Translate to window coordinates (relative to the rich-text box)

                xPos = x
                yPos = y

                SSCEVB_CheckCtrlBackgroundMenu(rtfEdit.Handle.ToInt32(), xPos, yPos, SpellCheckColorBackGround, SpellCheckErrorColor)

            End If

        End If
        If rtfEdit.ReadOnly = False Then

            If rtfEdit.SelectedText <> "" Then
                tlbMain.Items.Item(kToolbarButtonCopy).Enabled = True
                tlbMain.Items.Item(kToolbarButtonCut).Enabled = True
            Else
                tlbMain.Items.Item(kToolbarButtonCopy).Enabled = False
                tlbMain.Items.Item(kToolbarButtonCut).Enabled = False
            End If
        End If
        SetButtons()

    End Sub

    Private Sub rtfEdit_SelectionChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles rtfEdit.SelectionChanged

        If m_bSpellCheck And Not m_bEnterPressed Then
            SSCEVB_CheckCtrlBackgroundNotify(rtfEdit.Handle.ToInt32(), SpellCheckColorBackGround, SpellCheckErrorColor)
        End If

    End Sub

    '************************************************************************
    ' Methods
    '***********************************************************************

    ' ***************************************************************** '
    ' Name: ToolbarActionSetBold
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSetBold()

        Const kMethodName As String = "ToolbarActionSetBold"

        Dim lReturn, lSubValue As Integer

        Try



        If m_bBold Then
            rtfEdit.SelectionFont = VB6.FontChangeBold(rtfEdit.SelectionFont, False)
            m_bBold = False
            CType(tlbMain.Items.Item(kToolbarButtonBold), ToolStripButton).Checked = False
        Else
            rtfEdit.SelectionFont = VB6.FontChangeBold(rtfEdit.SelectionFont, True)
            m_bBold = True
            CType(tlbMain.Items.Item(kToolbarButtonBold), ToolStripButton).Checked = True
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

       
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionAlign
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionAlign(ByVal Alignment As HorizontalAlignment)

        Const kMethodName As String = "ToolbarActionAlign"

        Dim lSubValue As Integer

        Try



        rtfEdit.SelectionAlignment = Alignment

        SetButtons()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

       
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionCopy
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionCopy()

        Const kMethodName As String = "ToolbarActionCopy"

        Dim lReturn, lSubValue As Integer

        Try

        My.Computer.Clipboard.Clear()
        'add this line as per vbcode

        My.Computer.Clipboard.SetText(rtfEdit.SelectedText, 1)

        tlbMain.Items.Item(kToolbarButtonPaste).Enabled = True
        tlbMain.Items.Item(kToolbarButtonCut).Enabled = False


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

      
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionCut
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionCut()

        Const kMethodName As String = "ToolbarActionCut"

        Dim lReturn, lSubValue As Integer

        Try

        My.Computer.Clipboard.Clear()

        Clipboard.SetText(rtfEdit.SelectedText, 1)

        rtfEdit.SelectedText = ""

        tlbMain.Items.Item(kToolbarButtonPaste).Enabled = True
        tlbMain.Items.Item(kToolbarButtonCopy).Enabled = False



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

      
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionDecreaseIndent
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionDecreaseIndent()

        Const kMethodName As String = "ToolbarActionDecreaseIndent"

        Dim lReturn, lSubValue As Integer

        Try

        'Developer Guide No 70
        rtfEdit.SelectionIndent = rtfEdit.SelectionIndent - rtfEdit.BulletIndent


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally
      
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionSetBullets
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSetBullets()

        Const kMethodName As String = "ToolbarActionSetBullets"

        Dim lReturn, lSubValue As Integer

        Try


        rtfEdit.SelectionBullet = Not rtfEdit.SelectionBullet


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

       
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionPrint
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionPrint()

        Const kMethodName As String = "ToolbarActionPrint"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try

        If PrintDialog1.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

      
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionPrintPreview
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionPrintPreview()

        Const kMethodName As String = "ToolbarActionPrintPreview"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try

        PrintPreviewDialog1.ShowDialog()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: ToolbarActionSelectFontColor
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSelectFontColor()

        Const kMethodName As String = "ToolbarActionSelectFontColor"

        Dim lReturn, lSubValue As Integer

        Try



        CommonDialog1Color.ShowDialog()
        rtfEdit.SelectionColor = CommonDialog1Color.Color


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

     
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionSelectFont
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSelectFont()

        Const kMethodName As String = "ToolbarActionSelectFont"

        Dim lReturn, lSubValue As Integer

        Try

        CommonDialog1Font.ShowEffects = True

        With rtfEdit
            CommonDialog1Font.Font = FontChangeName(CommonDialog1Font.Font, .SelectionFont.Name)
            CommonDialog1Font.Font = FontChangeSize(CommonDialog1Font.Font, .SelectionFont.SizeInPoints)
            CommonDialog1Font.Font = FontChangeBold(CommonDialog1Font.Font, .SelectionFont.Bold)
            CommonDialog1Font.Font = FontChangeStrikeout(CommonDialog1Font.Font, .SelectionFont.Strikeout)
            CommonDialog1Font.Font = FontChangeUnderline(CommonDialog1Font.Font, .SelectionFont.Underline)
            CommonDialog1Font.Font = FontChangeItalic(CommonDialog1Font.Font, .SelectionFont.Italic)
            CommonDialog1Font.Color = .SelectionColor
            'CommonDialog1Color.Color = .SelectionColor
        End With

        If CommonDialog1Font.ShowDialog() = DialogResult.OK Then

            With rtfEdit
                .SelectionFont = VB6.FontChangeName(.SelectionFont, CommonDialog1Font.Font.Name)
                .SelectionFont = VB6.FontChangeSize(.SelectionFont, CommonDialog1Font.Font.Size)
                .SelectionFont = VB6.FontChangeBold(.SelectionFont, CommonDialog1Font.Font.Bold)
                .SelectionFont = VB6.FontChangeItalic(.SelectionFont, CommonDialog1Font.Font.Italic)
                .SelectionFont = VB6.FontChangeStrikeout(.SelectionFont, CommonDialog1Font.Font.Strikeout)
                .SelectionFont = VB6.FontChangeUnderline(.SelectionFont, CommonDialog1Font.Font.Underline)
                .SelectionColor = CommonDialog1Font.Color 'CommonDialog1Color.Color
            End With
        End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionIncreaseIndent
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionIncreaseIndent()

        Const kMethodName As String = "ToolbarActionIncreaseIndent"

        Dim lReturn, lSubValue As Integer

        Try

        rtfEdit.SelectionIndent = rtfEdit.SelectionIndent + rtfEdit.BulletIndent


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

      
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionSetItalic
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSetItalic()

        Const kMethodName As String = "ToolbarActionSetItalic"

        Dim lReturn, lSubValue As Integer

        Try



        If m_bItalic Then
            rtfEdit.SelectionFont = VB6.FontChangeItalic(rtfEdit.SelectionFont, False)
            m_bItalic = False
            CType(tlbMain.Items.Item(kToolbarButtonItalic), ToolStripButton).Checked = False
        Else
            rtfEdit.SelectionFont = VB6.FontChangeItalic(rtfEdit.SelectionFont, True)
            m_bItalic = True
            CType(tlbMain.Items.Item(kToolbarButtonItalic), ToolStripButton).Checked = True
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionPaste
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionPaste()

        Const kMethodName As String = "ToolbarActionPaste"

        Dim lReturn, lSubValue As Integer

        Try
        'added this line as per vbcode
        rtfEdit.SelectedText = Clipboard.GetText(1)




        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

       
        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: ToolbarActionRedo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionRedo()

        Const kMethodName As String = "ToolbarActionRedo"

        Dim lReturn, lSubValue As Integer

        Try



        Redo()
        If rtfEdit.ReadOnly = False Then
            tlbMain.Items.Item(kToolbarButtonUndo).Enabled = True
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: ToolbarActionSetStrikeThrough
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSetStrikeThrough()

        Const kMethodName As String = "ToolbarActionSetStrikeThrough"

        Dim lReturn, lSubValue As Integer

        Try



        If m_bStrikeThru Then
            rtfEdit.SelectionFont = VB6.FontChangeStrikeout(rtfEdit.SelectionFont, False)
            m_bStrikeThru = False
            CType(tlbMain.Items.Item(kToolbarButtonStrikeThru), ToolStripButton).Checked = False
        Else
            rtfEdit.SelectionFont = VB6.FontChangeStrikeout(rtfEdit.SelectionFont, True)
            m_bStrikeThru = True
            CType(tlbMain.Items.Item(kToolbarButtonStrikeThru), ToolStripButton).Checked = True
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        
        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: ToolbarActionSetUnderline
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionSetUnderline()

        Const kMethodName As String = "ToolbarActionSetUnderline"

        Dim lReturn, lSubValue As Integer

        Try



        If m_bUnderline Then
            rtfEdit.SelectionFont = VB6.FontChangeUnderline(rtfEdit.SelectionFont, False)
            m_bUnderline = False
            CType(tlbMain.Items.Item(kToolbarButtonUnderline), ToolStripButton).Checked = False
        Else
            rtfEdit.SelectionFont = VB6.FontChangeUnderline(rtfEdit.SelectionFont, True)
            m_bUnderline = True
            CType(tlbMain.Items.Item(kToolbarButtonUnderline), ToolStripButton).Checked = True
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetButtons
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub SetButtons()

        Const kMethodName As String = "SetButtons"

        Dim lReturn, lSubValue As Integer
        Dim Alignment As HorizontalAlignment

        Try

        If Not IsNothing(rtfEdit.SelectionFont) Then

            If rtfEdit.SelectionFont.Bold Then
                m_bBold = True
                CType(tlbMain.Items.Item(kToolbarButtonBold), ToolStripButton).Checked = True
            Else
                m_bBold = False
                CType(tlbMain.Items.Item(kToolbarButtonBold), ToolStripButton).Checked = False
            End If

            If rtfEdit.SelectionFont.Italic Then
                m_bItalic = True
                CType(tlbMain.Items.Item(kToolbarButtonItalic), ToolStripButton).Checked = True
            Else
                m_bItalic = False
                CType(tlbMain.Items.Item(kToolbarButtonItalic), ToolStripButton).Checked = False
            End If

            If rtfEdit.SelectionFont.Underline Then
                m_bUnderline = True
                CType(tlbMain.Items.Item(kToolbarButtonUnderline), ToolStripButton).Checked = True
            Else
                m_bUnderline = False
                CType(tlbMain.Items.Item(kToolbarButtonUnderline), ToolStripButton).Checked = False
            End If

            If rtfEdit.SelectionFont.Strikeout Then
                m_bStrikeThru = True
                CType(tlbMain.Items.Item(kToolbarButtonStrikeThru), ToolStripButton).Checked = True
            Else
                m_bStrikeThru = False
                CType(tlbMain.Items.Item(kToolbarButtonStrikeThru), ToolStripButton).Checked = False
            End If
        End If

        Alignment = rtfEdit.SelectionAlignment

        CType(tlbMain.Items.Item(kToolbarButtonLeft), ToolStripButton).Checked = Alignment = HorizontalAlignment.Left

        CType(tlbMain.Items.Item(kToolbarButtonRight), ToolStripButton).Checked = Alignment = HorizontalAlignment.Right

        CType(tlbMain.Items.Item(kToolbarButtonCenter), ToolStripButton).Checked = Alignment = HorizontalAlignment.Center


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ToolbarActionUndo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub ToolbarActionUndo()

        Const kMethodName As String = "ToolbarActionUndo"

        Dim lReturn, lSubValue As Integer

        Try



        Undo()

        If rtfEdit.ReadOnly = False Then
            tlbMain.Items.Item(kToolbarButtonRedo).Enabled = True
        End If

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        ' If you want to rollback a transToolbarAction or something, do it here

        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Undo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub Undo()

        Const kMethodName As String = kToolbarButtonUndo
        Dim lSubValue As Integer
        'if the Index is = to 0, then It shouldn't undo anymore
        If m_iCurrentStackMemberIndex = 0 Then Exit Sub

        'This is the basic undo stuff.
        m_bRedoUndoInProgress = True
        m_iCurrentStackMemberIndex -= 1

        Try

            rtfEdit.Rtf = m_asStack(m_iCurrentStackMemberIndex)
            m_bRedoUndoInProgress = False
            Exit Sub
        Catch exc As System.Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lsubvalue, excep:=exc)
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Redo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Sub Redo()
        Dim lSubValue As Integer
        Const kMethodName As String = kToolbarButtonRedo

        'This is the basic redo
        m_bRedoUndoInProgress = True
        m_iCurrentStackMemberIndex += 1


        Try

            rtfEdit.Rtf = m_asStack(m_iCurrentStackMemberIndex)
            m_bRedoUndoInProgress = False
            Exit Sub

        Catch exc As System.Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=exc)
        End Try
    End Sub


    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        checkPrint = 0
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        ' Print the content of the RichTextBox. Store the last character printed.
        richtextboxprintctrl1 = New RichTextBoxPrintCtrl

        richtextboxprintctrl1.Rtf = rtfEdit.Rtf

        checkPrint = richtextboxprintctrl1.Print(checkPrint, richtextboxprintctrl1.TextLength, e)

        ' Look for more pages
        If checkPrint < RichTextBoxPrintCtrl1.TextLength Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If
    End Sub

    Private Sub uctRichTextBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
