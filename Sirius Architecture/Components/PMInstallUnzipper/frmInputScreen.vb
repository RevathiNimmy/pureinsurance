Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInputScreen
	Inherits System.Windows.Forms.Form
	Private Sub frmInputScreen_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_sMessage As String = "" ' message prompt for user
	Private m_sFolderPath As String = "" 'data entered by user
	Private m_sDriveLetter As String = ""
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lReturn As Integer
	
	Private m_vDiskList( ,  ) As Object
	
	Private Const ARRAY_LETTER As Integer = 0
	Private Const ARRAY_SPACE As Integer = 1
	Private Const ARRAY_TYPE As Integer = 2
	Private Const ARRAY_NAME As Integer = 3
	
	Private Const DISK_TYPE_FIXED As Integer = 2
	Private Const DISK_TYPE_NETWORK As Integer = 3
	
	Private Const SIZE_GIGABYTE As Integer = 1048576
	Private Const SIZE_MEGABYTE As Integer = 1024
	
	Public WriteOnly Property Message() As String
		Set(ByVal Value As String)
			m_sMessage = Value
		End Set
	End Property
	
	
	Public Property FolderPath() As String
		Get
			Return m_sFolderPath
		End Get
		Set(ByVal Value As String)
			m_sFolderPath = Value
		End Set
	End Property
	
	Public ReadOnly Property DriveLetter() As String
		Get
			Return m_sDriveLetter
		End Get
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public Function Start() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' get list of disks available to this machine
			m_lReturn = GetDiskList()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to get list of available drives", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			' add disks to listview
			m_lReturn = PopulateListView()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to populate the listview", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			lblMessage.Text = m_sMessage
			txtFolderPath.Text = m_sFolderPath
			
			Me.ShowDialog()
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lReturn = MessageBox.Show("Ok to abort the installation process?", "PMInstallUnzipper", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
		
		If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
			Exit Sub
		End If
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Close()
		
	End Sub
	

	Private Sub frmInputScreen_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		'm_sFolderPath = "\PMSetup"
		'm_lReturn = Start
	End Sub
	
	Private Sub frmInputScreen_Closed(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles MyBase.Closing
		
		If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
			m_lReturn = MessageBox.Show("Abort the installation process?", "PMInstallUnzipper", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
			
			If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
				eventArgs.Cancel = True
				Exit Sub
			End If
		End If
		
		m_sFolderPath = txtFolderPath.Text.Trim()
		
		If m_sFolderPath = "" Or m_sDriveLetter = "" Then
			MessageBox.Show("Folder name and drive letter must be selected", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			eventArgs.Cancel = True
			
			Exit Sub
		End If
		
		If (m_sFolderPath.IndexOf(":\") + 1) = 0 Then
			MessageBox.Show("Folder path incorrectly specified", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			eventArgs.Cancel = True
			
			Exit Sub
		End If
		
	End Sub
	
	Private Sub lvwDiskList_ItemClick(ByVal Item As ListViewItem)
		
		
		m_sDriveLetter = Item.Text
		
		Dim iPos As Integer = (m_sFolderPath.IndexOf(":"c) + 1)
		
		If iPos > 0 Then
			m_sFolderPath = m_sDriveLetter & Mid(m_sFolderPath, iPos)
			txtFolderPath.Text = m_sFolderPath
			Exit Sub
		End If
		
		iPos = (m_sFolderPath.IndexOf("\"c) + 1)
		
		If iPos > 0 Then
			m_sFolderPath = m_sDriveLetter & "\" & Mid(m_sFolderPath, iPos)
			txtFolderPath.Text = m_sFolderPath
			Exit Sub
		End If
		
		'    m_sFolderPath = m_sDriveLetter & ":\" & Mid(m_sFolderPath, iPos)
		txtFolderPath.Text = m_sFolderPath
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub txtFolderPath_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFolderPath.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_sFolderPath = txtFolderPath.Text.Trim()
		
	End Sub
	
	Private Sub txtFolderPath_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFolderPath.Enter
		
		
		Dim iPos As Integer = (txtFolderPath.Text.IndexOf("\"c) + 1)
		
		txtFolderPath.SelectionStart = iPos
		txtFolderPath.SelectionLength = Strings.Len(txtFolderPath.Text)
		
	End Sub
	
	Private Sub txtFolderPath_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFolderPath.Leave
		
		txtFolderPath.Text = txtFolderPath.Text.Trim()
		
	End Sub
	
	' get disks available to this machine
	Private Function GetDiskList() As Integer
		
		Dim result As Integer = 0
		Dim iLoop As Integer
		Dim sLetter, sName As String
		Dim lType, lSpace As Integer
		Dim oDrive As Scripting.Drive
		Dim oFSO As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' use FSO
			oFSO = New Object()
			
			iLoop = 0
			
			For	Each oDrive2 As Scripting.Drive In oFSO.Drives
				oDrive = oDrive2
				sLetter = oDrive.DriveLetter
				sName = oDrive.VolumeName
				
				lType = CInt(oDrive.DriveType)
				lSpace = (CInt(oDrive.FreeSpace / 1024))
				
				' only local hard drives and network drives are usable
				If lType = DISK_TYPE_FIXED Or lType = DISK_TYPE_NETWORK Then
					ReDim Preserve m_vDiskList(3, iLoop)
					

					m_vDiskList(ARRAY_LETTER, iLoop) = sLetter

					m_vDiskList(ARRAY_SPACE, iLoop) = lSpace

					m_vDiskList(ARRAY_TYPE, iLoop) = lType

					m_vDiskList(ARRAY_NAME, iLoop) = sName
					
					iLoop += 1
				End If
				
			Next oDrive2
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oDrive = Nothing
			oFSO = Nothing
			
			Return result
		
		Catch 
			
			
			
			Select Case Information.Err().Number
				Case 71 'disk not ready


				Case Else
			End Select
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
		
	End Function
	
	' add disks to listview
	Private Function PopulateListView() As Integer
		
		Dim result As Integer = 0
        Dim sngSize As Single
		Dim sIcon, sSubItem As String
		Dim oItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			'report format
			lvwDiskList.View = View.Details
			
			'columns
			lvwDiskList.Columns.Clear()
			lvwDiskList.Columns.Add("Drive", "Drive", CInt(VB6.TwipsToPixelsX(600)))
			lvwDiskList.Columns.Add("Name", "Name", CInt(VB6.TwipsToPixelsX(1560)))
			lvwDiskList.Columns.Add("Type", "Type", CInt(VB6.TwipsToPixelsX(800)))
			lvwDiskList.Columns.Add("Space", "Free space", CInt(VB6.TwipsToPixelsX(1000)))
			
			' numerical free space colun
			lvwDiskList.Columns.Item(3).TextAlign = HorizontalAlignment.Right
			
			' load the items
			lvwDiskList.Items.Clear()
			
			For lLoop As Integer = m_vDiskList.GetLowerBound(1) To m_vDiskList.GetUpperBound(1)
				
				' select icon from image list

				If CDbl(m_vDiskList(ARRAY_TYPE, lLoop)) = DISK_TYPE_FIXED Then
					sIcon = "Local"
				Else
					sIcon = "Network"
				End If
				


				oItem = lvwDiskList.Items.Add("d" & lLoop, CStr(m_vDiskList(ARRAY_LETTER, lLoop)), sIcon)
				
				' show available space in appropriate format

				sngSize = CSng(m_vDiskList(ARRAY_SPACE, lLoop))
				
				If sngSize >= SIZE_GIGABYTE Then
					' gigabyte
					sSubItem = StringsHelper.Format(sngSize / (SIZE_GIGABYTE), "#.00") & "gb"
				ElseIf sngSize >= SIZE_MEGABYTE Then 
					' megabyte
					sSubItem = StringsHelper.Format(sngSize / SIZE_MEGABYTE, "#.00") & "mb"
				Else
					' kilobyte
					sSubItem = StringsHelper.Format(sngSize, "#.00") & "kb"
				End If
				
				' display drive type and free space

				ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vDiskList(ARRAY_NAME, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 2).Text = sIcon
				ListViewHelper.GetListViewSubItem(oItem, 3).Text = sSubItem
				
			Next 
			
			' select item(1)
			lvwDiskList.Items.Item(0).Selected = True
			
			' set and display defaults
			m_sDriveLetter = lvwDiskList.Items.Item(0).Text
			m_sFolderPath = m_sDriveLetter & ":" & m_sFolderPath
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class