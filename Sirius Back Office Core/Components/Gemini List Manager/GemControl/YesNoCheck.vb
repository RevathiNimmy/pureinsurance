Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("YesNoCheck_NET.YesNoCheck")> _
Public Partial Class YesNoCheck
	Inherits System.Windows.Forms.UserControl
	Public Event CaptionChange()
	Public Event WhatsThisHelpIDChange()
	Public Event ValueChange()
	Public Event AutoCaptionChange()
	Public Event BorderStyleChange()
	Public Event BackStyleChange()
	Public Event FontChange()
	Public Event EnabledChange()
	Public Event ForeColorChange()
	Public Event BackColorChange()
	
	Private m_bEnabled As Boolean ' CL
	
	Public Enum YesNoCheckValues
		YesNoCheckNone = 0
		YesNoCheckNo = 1
		YesNoCheckYes = 2
	End Enum
	
	
	'Default Property Values:
	Const m_def_WhatsThisHelpID As Integer = 0
	Const m_def_Caption As String = ""
	'Const m_def_Caption = ""
	Const m_def_BackStyle As Integer = 0
	Const m_def_BorderStyle As Integer = 0
	Const m_def_AutoCaption As Boolean = True
	Const m_def_Value As Integer = 0
	
	'Property Variables:
	Dim m_WhatsThisHelpID As Integer
	Dim m_Caption As String = ""
	'Dim m_Caption As String
	Dim m_BackStyle As Integer
	Dim m_BorderStyle As BorderStyle
	Dim m_AutoCaption As Boolean
	Dim m_Value As YesNoCheckValues
	
	
	'Event Declarations:
	Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,Click
	Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
	Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyDown
	Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyPress
	Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,KeyUp
	Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
	Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
	Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp
	Event Change(ByVal Sender As Object, ByVal e As EventArgs)
	
	
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,BackColor
	
	<Browsable(True)> _
	<Description("Returns/sets the background color used to display text and graphics in an object.")> _
	Public Overrides Property BackColor() As Color
		Get
			Return MyBase.BackColor
		End Get
		Set(ByVal Value As Color)
			MyBase.BackColor = Value
			RaiseEvent BackColorChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,ForeColor
	
	<Browsable(True)> _
	<Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
	Public Overrides Property ForeColor() As Color
		Get
			Return MyBase.ForeColor
		End Get
		Set(ByVal Value As Color)
			MyBase.ForeColor = Value
			RaiseEvent ForeColorChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,Enabled
	
	<Browsable(True)> _
	<Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
	<Editor()> _
	Public Shadows Property Enabled() As Boolean
		Get
			'Enabled = UserControl.Enabled
			Return m_bEnabled ' CL
		End Get
		Set(ByVal Value As Boolean)
			
			'MsgBox "prop=" & New_Enabled
			m_bEnabled = Value
			MyBase.Enabled = Value ' CL - doesn't work, i dunno why...
			
			'MsgBox "prop2=" & UserControl.Enabled
			
			RaiseEvent EnabledChange()
			
			' Set color
			If Not Value Then
				'picTickCross.BackColor = BackColor
				lblCaption.ForeColor = lblDisabled.ForeColor
			Else
				'picTickCross.BackColor = vbWhite
				lblCaption.ForeColor = lblEnabled.ForeColor
			End If
			
			SetNewValue()
			
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
	
	
	<Browsable(True)> _
	<Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
	Public Property BackStyle() As Integer
		Get
			Return m_BackStyle
		End Get
		Set(ByVal Value As Integer)
			m_BackStyle = Value
			RaiseEvent BackStyleChange()
		End Set
	End Property
	
	
	<Browsable(True)> _
	<Description("Returns/sets the border style for an object.")> _
	Public Shadows Property BorderStyle() As Integer
		Get
			Return m_BorderStyle
		End Get
		Set(ByVal Value As Integer)
			m_BorderStyle = Value
			RaiseEvent BorderStyleChange()
		End Set
	End Property
	
	
	<Browsable(True)> _
	<Editor()> _
	Public Property AutoCaption() As Boolean
		Get
			Return m_AutoCaption
		End Get
		Set(ByVal Value As Boolean)
			m_AutoCaption = Value
			RaiseEvent AutoCaptionChange()
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property Value() As Integer
		Get
			Return m_Value
		End Get
		Set(ByVal Value As Integer)
			m_Value = Value
			
			SetNewValue()
			
			RaiseEvent ValueChange()
		End Set
	End Property
	
	
	<Browsable(True)> _
	<Description("Returns/sets an associated context number for an object.")> _
	Public Property WhatsThisHelpID() As Integer
		Get
			Return m_WhatsThisHelpID
		End Get
		Set(ByVal Value As Integer)
            m_WhatsThisHelpID = Value
			RaiseEvent WhatsThisHelpIDChange()
		End Set
	End Property
	
	
	<Browsable(True)> _
	<Description("Returns/sets the text displayed in an object's title bar or below an object's icon.")> _
	Public Property Caption() As String
		Get
			Return m_Caption
		End Get
		Set(ByVal Value As String)
			m_Caption = Value
			RaiseEvent CaptionChange()
		End Set
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=picTickCross,picTickCross,-1,hWnd
	<Browsable(False)> _
	<Description("Returns a handle (from Microsoft Windows) to an object's window.")> _
	Public ReadOnly Property hWnd() As Integer
		Get
			Return picTickCross.Handle.ToInt32()
		End Get
	End Property
	
	'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
	'MappingInfo=UserControl,UserControl,-1,Refresh
	Public Overrides Sub Refresh()
		MyBase.Refresh()
	End Sub
	
	
	Private Sub lblCaption_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblCaption.Click
		If Not m_bEnabled Then Exit Sub 'cl
		'RaiseEvent Click
		m_Value += 1
		SetNewValue()
	End Sub
	
	
	Private Sub picTickCross_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles picTickCross.Click
		If Not m_bEnabled Then Exit Sub 'cl
		'RaiseEvent Click
		m_Value += 1
		SetNewValue()
		
	End Sub
	
	Private Sub YesNoCheck_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Click
		If Not m_bEnabled Then Exit Sub 'cl
		'RaiseEvent Click
		m_Value += 1
		SetNewValue()
	End Sub
	
	Private Sub YesNoCheck_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
		If Not m_bEnabled Then Exit Sub 'cl
		'RaiseEvent DblClick
	End Sub
	
	
	Private Sub YesNoCheck_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Enter
		If Not m_bEnabled Then Exit Sub 'cl
		shpFocus.Visible = True
	End Sub
	
	Private Sub YesNoCheck_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Leave
		shpFocus.Visible = False
	End Sub
	
	Private Sub UserControl_Initialize()
		
		With picTickCross
			.Left = 0
			.Top = VB6.TwipsToPixelsY(30)
		End With
		
		With lblCaption
			.Top = VB6.TwipsToPixelsY(20)
			.Left = VB6.TwipsToPixelsX(250)
		End With
		
		
		With shpFocus
			.Left = lblCaption.Left - VB6.TwipsToPixelsX(10)
			.Top = lblCaption.Top - VB6.TwipsToPixelsY(10)
			.Width = lblCaption.Width + VB6.TwipsToPixelsX(20)
			.Height = lblCaption.Height + VB6.TwipsToPixelsY(20)
		End With
		
		m_bEnabled = True 'cl
		MyBase.Enabled = True 'cl
		
	End Sub
	
	Private Sub YesNoCheck_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
		
		If eventArgs.KeyCode = Keys.Space Then
			m_Value += 1
			SetNewValue()
		End If
		
		KeyCode = 0
		
	End Sub
    'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no. 78
    'Private Sub YesNoCheck_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles MyBase.KeyPress
    Private Sub YesNoCheck_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs)
        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no. 78
        'Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyAscii)
        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no. 78
        'RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no. 78
            'eventArgs.Handled = True
		End If
        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no. 78
        'eventArgs.KeyChar = Convert.ToChar(KeyAscii)
        eventArgs.KeyAscii = AscW(Convert.ToChar(KeyAscii))
	End Sub
	
	Private Sub YesNoCheck_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
	End Sub
	
	Private Sub YesNoCheck_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
	End Sub
	
	Private Sub YesNoCheck_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
	End Sub
	
	Private Sub YesNoCheck_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
		
	End Sub
	
	'Initialize Properties for User Control

	Private Sub UserControl_InitProperties()

        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.2
        'Font = Ambient.Font
        Font = Me.Font
		m_BackStyle = m_def_BackStyle
        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no.139
        'm_BorderStyle = BorderStyle.None
        m_BorderStyle = Windows.Forms.BorderStyle.None
		m_AutoCaption = m_def_AutoCaption
		m_Value = YesNoCheckValues.YesNoCheckNone
		'    m_Caption = m_def_Caption
		m_WhatsThisHelpID = m_def_WhatsThisHelpID
		m_Caption = m_def_Caption
	End Sub
	
	'Load property values from storage


    'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.1
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
		



		MyBase.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H8000000F)))



		MyBase.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000012)))


		MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.2
        'Font = PropBag.ReadProperty("Font", Ambient.Font)
        Font = PropBag.ReadProperty("Font", Me.Font)


		m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


		m_BorderStyle = PropBag.ReadProperty("BorderStyle", m_def_BorderStyle)


		m_AutoCaption = CBool(PropBag.ReadProperty("AutoCaption", m_def_AutoCaption))


		m_Value = PropBag.ReadProperty("Value", m_def_Value)


		lblCaption.Text = CStr(PropBag.ReadProperty("Caption", ""))
		'   m_Caption = PropBag.ReadProperty("Caption", m_def_Caption)


		m_WhatsThisHelpID = CInt(PropBag.ReadProperty("WhatsThisHelpID", m_def_WhatsThisHelpID))

        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.14
        'lblCaption.WhatsThisHelpID = m_WhatsThisHelpID

        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.14
        'picTickCross.WhatsThisHelpID = m_WhatsThisHelpID
		


		m_Caption = CStr(PropBag.ReadProperty("Caption", m_def_Caption))
		
	End Sub
	
	'Write property values to storage


    'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.1
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
		

		PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(MyBase.BackColor), &H8000000F)

		PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(MyBase.ForeColor), &H80000012)

		PropBag.WriteProperty("Enabled", MyBase.Enabled, True)


        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.2
        'PropBag.WriteProperty("Font", Font, Ambient.Font)
        PropBag.WriteProperty("Font", Font, Me.Font)

		PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

		PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

		PropBag.WriteProperty("AutoCaption", m_AutoCaption, m_def_AutoCaption)

		PropBag.WriteProperty("Value", m_Value, m_def_Value)

		PropBag.WriteProperty("Caption", lblCaption.Text, "")
		'    Call PropBag.WriteProperty("Caption", m_Caption, m_def_Caption)


        'Modified by Archana Tokas on 12/04/2010 05:36:56 CHANGES refer developer guide no solution no.14
        'PropBag.WriteProperty("WhatsThisHelpID", lblCaption.WhatsThisHelpID, 0)

		PropBag.WriteProperty("WhatsThisHelpID", m_WhatsThisHelpID, m_def_WhatsThisHelpID)

		PropBag.WriteProperty("Caption", m_Caption, m_def_Caption)
	End Sub
	
	Sub SetNewValue()
		
		Dim sCaption As String = ""
		
		If m_Value > YesNoCheckValues.YesNoCheckYes Then
			m_Value = m_Value Mod (YesNoCheckValues.YesNoCheckYes + 1)
		End If
		
		
		
		Select Case m_Value
			Case YesNoCheckValues.YesNoCheckNone
				sCaption = New String(" "c, 7)
				picTickCross.Image = Nothing
				
			Case YesNoCheckValues.YesNoCheckNo
				sCaption = "No  "
				'If UserControl.Enabled = True Then
				If m_bEnabled Then
					picTickCross.Image = imgCrossBlack.Image
				Else
					picTickCross.Image = imgCrossGrey.Image
				End If
				
			Case YesNoCheckValues.YesNoCheckYes
				sCaption = "Yes "
				'If UserControl.Enabled = True Then
				If m_bEnabled Then
					picTickCross.Image = imgTickBlack.Image
				Else
					picTickCross.Image = imgTickGrey.Image
				End If
				
		End Select
		
		If m_AutoCaption Then
			lblCaption.Text = sCaption
		End If
		
		
		' Raise a change event
		RaiseEvent Click(Me, Nothing)
		

		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'chkYes.SetFocus
		'
		'Catch exc As System.Exception
			'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		'End Try
		
	End Sub
End Class