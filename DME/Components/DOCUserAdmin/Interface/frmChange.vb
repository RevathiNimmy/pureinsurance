Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmChange
	Inherits System.Windows.Forms.Form
    'Private objfrmInterface As New frmInterface
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' dont bother setting the access level
		' me go bye bye
		Me.Hide()
		
	End Sub
	
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim objfrmInterface As frmInterface = Me.Owner
        Dim li As ListViewItem
        ' set the access level on the main form
        'ListViewHelper.GetListViewSubItem(objfrmInterface.lvwUsers.Items.Item(objfrmInterface.lvwUsers.FocusedItem.Index), 1).Text = cboNewLevel.Text
        li = objfrmInterface.lvwUsers.Items.Item(Me.fraChange.Tag)
        li.SubItems(1).Text = (cboNewLevel.Text)


        ' me hide!
        Me.Hide()

    End Sub
End Class