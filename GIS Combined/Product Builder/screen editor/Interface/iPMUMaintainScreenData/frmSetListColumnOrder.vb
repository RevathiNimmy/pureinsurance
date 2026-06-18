Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmSetListColumnOrder
	Inherits System.Windows.Forms.Form
	Public vSetListColumnOrderArray As Object
	
	Event ClickOK()
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		ListViewSetListColumnOrder.Items.Clear()
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDown.Click
		Dim sTempCaption As String = ""
		Dim vTempTag As Object
		
        Dim iIndex As Integer = ListViewSetListColumnOrder.SelectedItems(0).Index + 1
		
		If iIndex < ListViewSetListColumnOrder.Items.Count Then
			
			sTempCaption = ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex), 2).Text


			vTempTag = Convert.ToString(ListViewSetListColumnOrder.Items.Item(iIndex).Tag)
			
			ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex), 2).Text = ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 1), 2).Text

			ListViewSetListColumnOrder.Items.Item(iIndex).Tag = Convert.ToString(ListViewSetListColumnOrder.Items.Item(iIndex - 1).Tag)
			
			ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 1), 2).Text = sTempCaption


			ListViewSetListColumnOrder.Items.Item(iIndex - 1).Tag = CStr(vTempTag)
			ListViewSetListColumnOrder.Refresh()
			iIndex += 1
		End If
		ListViewSetListColumnOrder.Items.Item(iIndex - 1).Selected = True
		ListViewSetListColumnOrder.Focus()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		RaiseEvent ClickOK()
		Me.Close()
	End Sub
	
	Private Sub cmdUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUp.Click
		
		Dim sTempCaption As String = ""
		Dim vTempTag As Object
		
        'Dim iIndex As Integer = ListViewSetListColumnOrder.FocusedItem.Index + 1
        Dim iIndex As Integer = ListViewSetListColumnOrder.SelectedItems(0).Index + 1
		
		If iIndex > 1 Then
			sTempCaption = ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 2), 2).Text


			vTempTag = Convert.ToString(ListViewSetListColumnOrder.Items.Item(iIndex - 2).Tag)
			
			ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 2), 2).Text = ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 1), 2).Text

			ListViewSetListColumnOrder.Items.Item(iIndex - 2).Tag = Convert.ToString(ListViewSetListColumnOrder.Items.Item(iIndex - 1).Tag)
			
			ListViewHelper.GetListViewSubItem(ListViewSetListColumnOrder.Items.Item(iIndex - 1), 2).Text = sTempCaption


			ListViewSetListColumnOrder.Items.Item(iIndex - 1).Tag = CStr(vTempTag)
			ListViewSetListColumnOrder.Refresh()
			iIndex -= 1
		End If
		
		ListViewSetListColumnOrder.Items.Item(iIndex - 1).Selected = True
		ListViewSetListColumnOrder.Focus()
		
	End Sub
	
	
	Private Sub frmSetListColumnOrder_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
            ListViewSetListColumnOrder.FullRowSelect = True
            ListViewSetListColumnOrder.Select()
            ListViewSetListColumnOrder.Items.Item(0).Selected = True
            'ListViewSetListColumnOrder.Focus()


		End If
	End Sub
End Class