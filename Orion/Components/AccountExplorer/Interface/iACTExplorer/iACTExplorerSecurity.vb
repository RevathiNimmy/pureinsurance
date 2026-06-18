Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSecurity
	Inherits System.Windows.Forms.Form
	
	
	' RAW 09/04/2003 : IAG ENDVR633 : tidied up handling of uctPickList
	
	
    'Object properties
	Private m_sFolderName As String = ""
	Private m_lNodeId As Integer
	
	Public WriteOnly Property FolderName() As String
		Set(ByVal Value As String)
			'Standard Property.
			m_sFolderName = Value
		End Set
	End Property
	
	Public WriteOnly Property NodeId() As Integer
		Set(ByVal Value As Integer)
			'Standard Property.
			m_lNodeId = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		'Save the picklists
		uctPickListView.Save()
		uctPickListUpdate.Save()
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Me.Close()
	End Sub
	

	Private Sub frmSecurity_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		'Set form caption
		Me.Text = Me.Text.Replace("[Folder Name]", m_sFolderName)
		
		'Setup the view picklist
		' RAW 09/04/2003 : IAG ENDVR633 : changed to use class from uctPicklist
		Dim Key As New uctPickList.PickListKey ' RAW 09/04/2003 : IAG ENDVR633 : changed to use class from uctPicklist
		Key.KeyName = "node_id"
		uctPickListView.ForeignKeys.Add(Key, Key:="NodeId")

		uctPickListView.ForeignKeys.Item("NodeId").Value = m_lNodeId
		' RAW 09/04/2003 : IAG ENDVR633 : added error handling
        'developer guide no. 68
        MessageBox.Show("Failed to load view list", "Account Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'm_lErrorNumber = PMFalse
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Exit Sub
        'End If

        'Setup the update picklist
        uctPickListUpdate.ForeignKeys.Add(Key, Key:="NodeId")

        uctPickListUpdate.ForeignKeys.Item("NodeId").Value = m_lNodeId
        ' RAW 09/04/2003 : IAG ENDVR633 : added error handling
        'developer guide no. 68
        MessageBox.Show("Failed to load update list", "Account Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'm_lErrorNumber = PMFalse
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Exit Sub
        'End If
		
	End Sub
End Class