Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmEditAdmin
	Inherits System.Windows.Forms.Form
	Private m_bUpdateAccess As Boolean
	
	Public ReadOnly Property UpdateAccess() As Boolean
		Get
			Return m_bUpdateAccess
		End Get
	End Property
	
	
	' Added validation so the file access levels  for copy, move or
	' delete cannot be higher than the folder access levels
	' this applies to all the foloowing lost_focus events
	
	'folder lost focus
	Private Sub cboFolderCopy_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFolderCopy.Leave
		If CInt(cboFileCopy.Text) < CDbl(cboFolderCopy.Text) Then
			cboFileCopy.Text = CStr(CInt(cboFolderCopy.Text))
		End If
	End Sub
	
	Private Sub cboFolderDelete_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFolderDelete.Leave
		If CInt(cboFileDelete.Text) < CDbl(cboFolderDelete.Text) Then
			cboFileDelete.Text = CStr(CInt(cboFolderDelete.Text))
		End If
	End Sub
	
	Private Sub cboFolderMove_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFolderMove.Leave
		If CInt(cboFileMove.Text) < CDbl(cboFolderMove.Text) Then
			cboFileMove.Text = CStr(CInt(cboFolderMove.Text))
		End If
	End Sub
	
	' File lost focus
	Private Sub cboFileCopy_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFileCopy.Leave
		If CInt(cboFileCopy.Text) < CDbl(cboFolderCopy.Text) Then
			cboFileCopy.Text = CStr(CInt(cboFolderCopy.Text))
		End If
	End Sub
	
	Private Sub cboFileDelete_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFileDelete.Leave
		If CInt(cboFileDelete.Text) < CDbl(cboFolderDelete.Text) Then
			cboFileDelete.Text = CStr(CInt(cboFolderDelete.Text))
		End If
	End Sub
	
	Private Sub cboFileMove_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFileMove.Leave
		If CInt(cboFileMove.Text) < CDbl(cboFolderMove.Text) Then
			cboFileMove.Text = CStr(CInt(cboFolderMove.Text))
		End If
	End Sub
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_bUpdateAccess = False
		Me.Close()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' ask user if they are sure
		Dim m_lReturn As DialogResult = MessageBox.Show("Are you sure you want to set the access levels to these values?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		
		' if no then exit
		If m_lReturn = System.Windows.Forms.DialogResult.No Then Exit Sub
		
		' else set values and exit
		g_iFolderMoveLevel = CInt(cboFolderMove.Text)
		g_iFolderCopyLevel = CInt(cboFolderCopy.Text)
		g_iFolderDeleteLevel = CInt(cboFolderDelete.Text)
		
		g_iFileMoveLevel = CInt(cboFileMove.Text)
		g_iFileCopyLevel = CInt(cboFileCopy.Text)
		g_iFileDeleteLevel = CInt(cboFileDelete.Text)
		
		m_bUpdateAccess = True
		Me.Close()
		
	End Sub
	

	Private Sub frmEditAdmin_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' set all access levels to current values
		cboFolderMove.SelectedIndex = g_iFolderMoveLevel - 1
		cboFolderCopy.SelectedIndex = g_iFolderCopyLevel - 1
		cboFolderDelete.SelectedIndex = g_iFolderDeleteLevel - 1
		
		cboFileMove.SelectedIndex = g_iFileMoveLevel - 1
		cboFileCopy.SelectedIndex = g_iFileCopyLevel - 1
		cboFileDelete.SelectedIndex = g_iFileDeleteLevel - 1
		
		
	End Sub
End Class