Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Windows.Forms
Friend Partial Class frmExistingLogin
	Inherits System.Windows.Forms.Form
	
	Dim f_ActiveFlag As Integer
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Tag = CStr(0)
		Me.Hide()
		
	End Sub
	
	Private Sub cmdCancel_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles cmdCancel.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		message(Me, "Leave without overriding a logon")
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		
		Dim sMsg As String = "Are you sure you wish to override existing login?"
		If Interaction.MsgBox(sMsg, MB_ICONQUESTION + MB_YESNO + MB_DEFBUTTON2, "Override Existing Login") = System.Windows.Forms.DialogResult.Yes Then
			Me.Tag = CStr(outUser.get_ItemData(outUser.ListIndex))
			Me.Hide()
		End If
		
	End Sub
	
	Private Sub cmdOK_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles cmdOK.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		message(Me, "Override selected logon, with your current logon")
		
	End Sub
	
	Private Sub FillUserList()
		
		Dim sSQLQuery As String = ""
		Dim ssUserList As DAO.Snapshot
		
		Try 
			
			If Convert.ToString(Me.Tag).Trim() <> "DMSAPI" And Convert.ToString(Me.Tag).Trim() <> "SU" Then
				sSQLQuery = "SELECT login.*, user.title FROM login, user "
				sSQLQuery = sSQLQuery & "WHERE login.user_name = user.user_name AND "
				sSQLQuery = sSQLQuery & "login.user_name = '" & Convert.ToString(Me.Tag) & "' ORDER BY login.date"
			Else
				sSQLQuery = "SELECT login.* FROM login "
				sSQLQuery = sSQLQuery & "WHERE login.user_name = '" & Convert.ToString(Me.Tag) & "' ORDER BY login.date"
			End If
			
			ssUserList = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			outUser.Clear()
			
			While Not ssUserList.EOF
				If Convert.ToString(Me.Tag).Trim() <> "DMSAPI" And Convert.ToString(Me.Tag).Trim() <> "SU" Then
                    outUser.AddItem(ssUserList("login.user_name").Value.Trim() & " (" & ssUserList("user.title").Value.Trim() & ")")
				Else
                    outUser.AddItem(ssUserList("login.user_name").Value.Trim())
				End If
				outUser.set_Indent(CShort(outUser.ListCount - 1), 1)
                outUser.set_ItemData(CShort(outUser.ListCount - 1), ssUserList.Fields("login.login_num").Value)
				outUser.set_PictureType(CShort(outUser.ListCount - 1), MSOUTLINE_PICTURE_CLOSED)
				outUser.AddItem(StringsHelper.Format(ssUserList("login.date"), "dddddd hh:mm:ss am/pm"))
				outUser.set_Indent(CShort(outUser.ListCount - 1), 2)
				outUser.set_PictureType(CShort(outUser.ListCount - 1), MSOUTLINE_PICTURE_OPEN)
				
				ssUserList.MoveNext()
			End While
			
			ssUserList.Close()
			ssUserList = Nothing
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub frmExistingLogin_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			If Not (f_ActiveFlag) Then
				f_ActiveFlag = True
				Application.DoEvents()
				
				Me.Cursor = Cursors.WaitCursor
				message(Me, "Please Wait... Retrieving Existing Logons")
				
				' Display title
				' This is a bit crap, but we must make
				' an exception for DMSAPI
				If Convert.ToString(Me.Tag).Trim() = "DMSAPI" Then
					fra3ExistingLogins.Text = "Login limit for user '" & Convert.ToString(Me.Tag).Trim() & "' is, 1"
				Else
					fra3ExistingLogins.Text = "Login limit for user '" & Convert.ToString(Me.Tag).Trim() & "' is, " & CStr(GetUserLimit(Convert.ToString(Me.Tag)))
				End If
				
				' Get all existing logins
				FillUserList()
				
				message(Me, "")
				Me.Cursor = Cursors.Default
			End If
			
		End If
	End Sub
	

	Private Sub frmExistingLogin_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        'TODO
        'CenterForm(Me)
		f_ActiveFlag = False
		
	End Sub
	
	Private Sub fra3ExistingLogins_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles fra3ExistingLogins.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		message(Me, "Copyright ©" & DateTime.Today.Year & " SSP Sirius Limited")
		
	End Sub
	
	Private Function GetUserLimit(ByRef sUserName As String) As Integer
		
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			' Get the user limit
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT login_limit FROM user WHERE user_name = '" & sUserName & "'")
			DAO_DBEngine_definst.FreeLocks()
			
			
			If ssSnapShot.RecordCount = 1 Then
                Return ssSnapShot("login_limit").Value
			Else
				Return 0
			End If
		
		Catch 
		End Try
		
		
		
		Return 0
		
	End Function
	
	Private Sub outUser_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles outUser.ClickEvent
		
		cmdOK.Enabled = Not (outUser.ListIndex = -1 Or outUser.get_Indent(outUser.ListIndex) > 1)
		
	End Sub
	
	Private Sub outUser_Collapse(ByVal eventSender As Object, ByVal eventArgs As AxMSOutl.OutlineEvents_CollapseEvent) Handles outUser.Collapse
		
		outUser.ListIndex = eventArgs.ListIndex
		
	End Sub
	
	Private Sub outUser_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles outUser.DoubleClick
		
		If outUser.get_Indent(outUser.ListIndex) < 2 Then
			If outUser.get_Expand(outUser.ListIndex) Then
				outUser.set_Expand(outUser.ListIndex, False)
			Else
				outUser.set_Expand(outUser.ListIndex, True)
			End If
		End If
		
	End Sub
	
	Private Sub outUser_ExpandEvent(ByVal eventSender As Object, ByVal eventArgs As AxMSOutl.OutlineEvents_ExpandEvent) Handles outUser.ExpandEvent
		
		outUser.ListIndex = eventArgs.ListIndex
		
	End Sub
	
	Private Sub outUser_KeyUpEvent(ByVal eventSender As Object, ByVal eventArgs As AxMSOutl.OutlineEvents_KeyUpEvent) Handles outUser.KeyUpEvent
		
		outUser_ClickEvent(outUser, New EventArgs())
		
	End Sub
	
	Private Sub pan3ExistingLogins_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pan3ExistingLogins.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		message(Me, "Copyright ©" & DateTime.Today.Year & " SSP Sirius Limited")
		
	End Sub
	Private Sub frmExistingLogin_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub
End Class