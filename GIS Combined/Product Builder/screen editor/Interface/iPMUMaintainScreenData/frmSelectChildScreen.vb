Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmSelectChildScreen
	Inherits System.Windows.Forms.Form
	Private Sub frmSelectChildScreen_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_lScreenId As Integer
	
	Public ReadOnly Property lScreenId() As Integer
		Get
			Return m_lScreenId
		End Get
	End Property
	
	
	
	Private Sub cmdListCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdListCancel.Click
		m_lScreenId = -1
		Me.Close()
	End Sub
	
	Private Sub cmdListViewAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdListViewAdd.Click
		m_lScreenId = 0
		Me.Close()
	End Sub
	
	Private Sub cmdListViewEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdListViewEdit.Click
		
		Dim iIndex As Integer
		If Not (ListViewSelectScreen.FocusedItem Is Nothing) Then
			iIndex = ListViewSelectScreen.FocusedItem.Index + 1
			
			If iIndex > 0 Then

				m_lScreenId = Convert.ToString(ListViewSelectScreen.Items.Item(iIndex - 1).Tag)
				Me.Close()
			End If
		End If
	End Sub
End Class