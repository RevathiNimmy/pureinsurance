Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmExportFolder
	Inherits System.Windows.Forms.Form
	Private Sub frmExportFolder_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Const ACClass As String = "frmExportFolder"
	
	Private m_lReturn As DialogResult
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property FolderName() As String
		Get
			
			Return txtFolderName.Text
			
		End Get
		Set(ByVal Value As String)
			
			txtFolderName.Text = Value
			
		End Set
	End Property
	
	Private Sub cmdBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowse.Click
		
		Dim sTmp As String = ""
		
		m_lReturn = BrowseFolder(sFolder:=sTmp, sTitle:="Browse", hWndParent:=0)
		
		
		txtFolderName.Text = sTmp
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Const kMethodName As String = "cmdOK_Click"
		
		Try
		
		
		
		m_lReturn = MessageBox.Show("Are you sure you wish to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		
		If m_lReturn <> System.Windows.Forms.DialogResult.Yes Then
			Exit Sub
		End If
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()

		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
		Finally


		End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Const kMethodName As String = "cmdOK_Click"

        Dim sFolderName As String = ""


        'Dim oFSO As FileSystemObject
        Dim oFSO As System.IO.DirectoryInfo
        Try


        oFSO = New System.IO.DirectoryInfo(txtFolderName.Text)
        'Validate entries

        If Not oFSO.Exists Then 'Trim(txtFolderName) = "" Or
            MessageBox.Show("Please enter a valid Folder Name.", "Documaster Export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtFolderName.Focus()
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
        Finally
        oFSO = Nothing

        End Try
    End Sub
End Class