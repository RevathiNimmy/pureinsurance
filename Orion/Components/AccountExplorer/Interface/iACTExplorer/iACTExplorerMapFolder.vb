Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmMapFolder
	Inherits System.Windows.Forms.Form
	
	' PWF 04/09/2002 - Change full properties to public (simplification)
	Public MapName As String = ""
	Public FullPath As String = ""
	Public Description As String = ""
	Public TotallingID As Integer
	Public ReportMapID As Integer
	
	' Property members
	Private m_bResult As Boolean
	
	
	Public ReadOnly Property Result() As Boolean
		Get
			' Return the dialog result
			Return m_bResult
		End Get
	End Property
	
	
	Private Sub cboTotallingType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTotallingType.Leave
		TotallingID = cboTotallingType.SelectedIndex
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bResult = False
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_bResult = True
        'Developer Guide No 231
        Me.Hide()
	End Sub
	

	Private Sub frmMapFolder_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		txtMapName.Text = MapName
		lblFullPath.Text = FullPath
		txtDescription.Text = Description
		txtreportMapId.Text = CStr(ReportMapID)
        txtMapName.Focus()
        txtMapName.Select()

		'Temp for Now until totalling table is introduced
		cboTotallingType.Items.Add("Sub Account")
		cboTotallingType.Items.Add("Heading Account")
		cboTotallingType.Items.Add("Prime Account")
		cboTotallingType.Items.Add("Grouped Account")
		cboTotallingType.SelectedIndex = TotallingID
		
	End Sub
	
	Private Sub frmMapFolder_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        'If m_bResult Then
        '	MapName = txtMapName.Text
        '	FullPath = lblFullPath.Text
        '	TotallingID = cboTotallingType.SelectedIndex
        '	Description = txtDescription.Text
        '	ReportMapID = CInt(txtreportMapId.Text)
        'End If
	End Sub
	
	Private Sub txtMapName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMapName.Enter
		txtMapName.SelectionStart = 0
		txtMapName.SelectionLength = Strings.Len(txtMapName.Text)
	End Sub

    Private Sub frmMapFolder_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If m_bResult Then
            MapName = txtMapName.Text
            FullPath = lblFullPath.Text
            TotallingID = cboTotallingType.SelectedIndex
            Description = txtDescription.Text
            ReportMapID = CInt(txtreportMapId.Text)
        End If
    End Sub

    Private Sub frmMapFolder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class