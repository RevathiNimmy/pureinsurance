Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmWarning
	Inherits System.Windows.Forms.Form
	'
	' History:
	' CJB 080805 PN22978 Changed LoadInterface to not position a new note if form is maximised or minimised
	'            as it will error if we do.
	' CJB 110805 PN23111 Added FState(Me.Tag).Deleted = True statements where required to prevent hidden forms
	'            from loading if we access them in code - we can check this before and not try to access them!
	' CJB 240805 PN23456 Changed mnuEditEditText_Click to only update Warning info if OK was clicked!
	'
	
	Private Const ACClass As String = "frmWarning"
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lEventCnt As Integer
	Private m_dtEventDate As Date
	Private m_sSubject As String = ""
	Private m_sPriorityCode As String = ""
	Private m_lFormLeft As Integer
	Private m_lFormTop As Integer
	Private m_sUserName As String = ""
	Private m_iSubjectId As Integer
	Private m_sEventType As String = ""
	Private m_sDescription As String = ""

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property


	Public Property EventCnt() As Integer
		Get
			Return m_lEventCnt
		End Get
		Set(ByVal Value As Integer)
			m_lEventCnt = Value
		End Set
	End Property
	Public Property EventDate() As Date
		Get
			Return m_dtEventDate
		End Get
		Set(ByVal Value As Date)
			m_dtEventDate = Value
		End Set
	End Property
	Public Property Subject() As String
		Get
			Return m_sSubject
		End Get
		Set(ByVal Value As String)
			m_sSubject = Value
		End Set
	End Property
	Public Property PriorityCode() As String
		Get
			Return m_sPriorityCode
		End Get
		Set(ByVal Value As String)
			m_sPriorityCode = Value
		End Set
	End Property
	Public Property FormLeft() As Integer
		Get
			Return m_lFormLeft
		End Get
		Set(ByVal Value As Integer)
			m_lFormLeft = Value
		End Set
	End Property
	Public Property FormTop() As Integer
		Get
			Return m_lFormTop
		End Get
		Set(ByVal Value As Integer)
			m_lFormTop = Value
		End Set
	End Property
	Public Property Username() As String
		Get
			Return m_sUserName
		End Get
		Set(ByVal Value As String)
			m_sUserName = Value
		End Set
	End Property
	Public Property SubjectId() As Integer
		Get
			Return m_iSubjectId
		End Get
		Set(ByVal Value As Integer)
			m_iSubjectId = Value
		End Set
	End Property
	Public Property EventType() As String
		Get
			Return m_sEventType
		End Get
		Set(ByVal Value As String)
			m_sEventType = Value
		End Set
	End Property
	Public Property Description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
    'Private objFState() As FormState = Nothing
    'Public WriteOnly Property FState() As FormState()
    '    Set(ByVal value As FormState())
    '        Me.objFState = value
    '    End Set
    'End Property

    Public Function LoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Text = EventDate & " - " & Subject
            lblDescription.Text = Description

            If Me.WindowState = FormWindowState.Normal Then 'PN22978
                Me.SetBounds(VB6.TwipsToPixelsX(FormLeft), VB6.TwipsToPixelsY(FormTop), VB6.TwipsToPixelsX(2895), VB6.TwipsToPixelsY(2335))
            End If

            Select Case PriorityCode
                Case "Red"
                    Me.BackColor = Color.FromArgb(255, 128, 128)
                Case "Amber"
                    Me.BackColor = Color.FromArgb(255, 192, 128)
                Case "Green"
                    Me.BackColor = Color.FromArgb(128, 255, 128)
            End Select
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub frmWarning_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Button = MouseButtons.Right Then
            Ctx_mnuEdit.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End If
    End Sub

    Private Sub frmWarning_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            'PN49570
            If Me.WindowState = FormWindowState.Maximized Then
                'When form gets maximized then it retain focus
                Exit Sub
            End If

            'For iLoop As Integer = 0 To Application.OpenForms.Count - 1
            '    If Application.OpenForms.Item(iLoop).Name <> "frmWarning" Then
            '        'If Application.OpenForms.Item(iLoop).GetType.ToString.Contains("iPMBClientManager.") Then
            '        Application.OpenForms.Item(iLoop).Activate()
            '        'End If
            '    End If

            'Next

            For Each frm As Form In Me.MdiParent.MdiChildren
                If frm.Name <> "frmWarning" Then
                    frm.Activate()
                End If
            Next

        End If
    End Sub

    Private Sub frmWarning_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        'make them stick
        Me.WindowState = FormWindowState.Normal
        m_lFormTop = CInt(VB6.PixelsToTwipsY(Me.Top))
        m_lFormLeft = CInt(VB6.PixelsToTwipsX(Me.Left))


        m_lReturn = objCM.g_oEvent.PositionWarnings(v_vEventCnt:=m_lEventCnt, v_vStickyTop:=m_lFormTop, v_vStickyLeft:=m_lFormLeft)

        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmWarning_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        'If isInitializingComponent Then
        '	Exit Sub
        'End If
        'Try 
        '	lblDescription.SetBounds(lblDescription.Left, lblDescription.Top, Me.ClientRectangle.Width - VB6.TwipsToPixelsX(180), Me.ClientRectangle.Height - VB6.TwipsToPixelsY(180))

        'Catch 
        'End Try
    End Sub

    Private Sub frmWarning_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True
    End Sub

    Private Sub lblDescription_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lblDescription.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = MouseButtons.Right Then
            Ctx_mnuEdit.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End If
    End Sub

    Public Sub mnuEditComplete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditComplete.Click

        m_lReturn = objCM.g_oEvent.CloseWarnings(v_vEventCnt:=m_lEventCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to delete Warning", "Error", MessageBoxButtons.OK)
        Else
            Me.Hide()
            objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True
        End If
    End Sub

    Public Sub mnuEditEditText_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditEditText.Click

        Dim bIsCompleted As Boolean

        Dim oNotes As New iPMBNote.Interface_Renamed
        m_lReturn = CType(oNotes, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=".Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditEditText_Click")
            Exit Sub
        End If

        With oNotes
            .EventCnt = m_lEventCnt
            .NoteDate = m_dtEventDate
            .UserName = objCM.g_oObjectManager.UserName
            .EventLogSubjectId = m_iSubjectId
            .Context = m_sEventType
            .Description = m_sDescription
            .PriorityCode = m_sPriorityCode '2005StickyNotes
            .IsCompleted = 0
        End With

        m_lReturn = oNotes.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        m_lReturn = oNotes.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=".Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditEditText_Click")
            Exit Sub
            oNotes.Dispose()
            oNotes = Nothing
        End If

        ' Only update if ok was pressed !   PN23456
        If oNotes.Status = gPMConstants.PMEReturnCode.PMOK Then
            bIsCompleted = oNotes.IsCompleted

            m_sDescription = oNotes.Description
            lblDescription.Text = m_sDescription

            PriorityCode = oNotes.PriorityCode
            Select Case PriorityCode
                Case "Red"
                    Me.BackColor = Color.FromArgb(255, 128, 128)
                Case "Amber"
                    Me.BackColor = Color.FromArgb(255, 192, 128)
                Case "Green"
                    Me.BackColor = Color.FromArgb(128, 255, 128)
            End Select
        End If

		oNotes.Dispose()
        oNotes = Nothing

        ' If the warning was completed, hide the sticky
        If bIsCompleted Then
            Me.Hide()
            objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True
        End If
    End Sub
	
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)
	End Sub
End Class
