Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Xml.Linq
Partial Friend Class frmLicenceLimit
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmLicenceLimit
    '
    ' Date: 08 March 1999
    '
    ' Description: Enter Licence Limit Details
    '
    ' Edit History:
    '
    'RFC080399 - Created for - Update Licence Limit added.
    ' ***************************************************************** '


    Private Const ACClass As String = "frmLicenceLimit"
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iNewLicenceLimit As Integer
    Private m_oBusiness As bPMLicenceAdmin.LicenceAdmin
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property NewLicenceLimit() As Integer
        Get
            Return m_iNewLicenceLimit
        End Get
    End Property
    Public WriteOnly Property Business() As bPMLicenceAdmin.LicenceAdmin
        Set(ByVal Value As bPMLicenceAdmin.LicenceAdmin)
            m_oBusiness = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: UpdateLicenceLimit
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function UpdateLicenceLimit() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the Business Object to Check & Update the Licence Limit
            '	lReturn = m_oBusiness.UpdateLicenceLimit(v_iNewLicenceLimit:=m_iNewLicenceLimit, v_sNewLicenceKey:=txtLicenceKey.Text)

            ' Check that a Licence Key has been supplied
            'DAK050100
            'If m_iNewLicenceLimit > 0 And txtLicenceKey.Text.Trim() = "" Then
            '	Interaction.MsgBox("Licence Key MUST be entered.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Invalid Licence Key")
            '	txtLicenceKey.Focus()
            '	Exit Sub
            'End If

            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                Case gPMConstants.PMEReturnCode.PMInvalidRequest
                    Interaction.MsgBox("The Licence Key you have supplied does NOT match the Licence Limit entered.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Invalid Licence Key")
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    Interaction.MsgBox("An Error Occurred Updating the Licence Key.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Error")
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLicenceLimitFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLicenceLimit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Hide()
    End Sub
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim locate As String = txtLocate.Text
        Dim store As String = txtStore.Text
        Dim finalFileName As String = ""
        If String.IsNullOrEmpty(locate) Then
            MessageBox.Show("Please locate the supplied licence file with extension .lic ", "Locate File Error", MessageBoxButtons.OK)
            Exit Sub
        Else
            Dim locateArray As String() = locate.Split("\")
            Dim fileName As String = locateArray(locateArray.Length - 1)
            Dim extArray As String() = fileName.Split(".")
            Dim ext As String = extArray(extArray.Length - 1)
            If ext.ToLower() <> "lic" Then
                MessageBox.Show("Please locate the supplied licence file with extension .lic ", "Locate File Error", MessageBoxButtons.OK)
                Exit Sub
            End If
            finalFileName = fileName
        End If
        If String.IsNullOrEmpty(store) Then
            MessageBox.Show("Please select the folder to save the licence file", "Store File Error", MessageBoxButtons.OK)
            Exit Sub
        Else
            Dim storeArray As String() = store.Split("\")
            Dim fileName As String = storeArray(storeArray.Length - 1)
            Dim extArray As String() = fileName.Split(".")
            If extArray.Length > 1 Then
                Dim ext As String = extArray(extArray.Length - 1)
                If ext.ToLower() <> "lic" Then
                    MessageBox.Show("Please locate the supplied licence file with extension .lic ", "Locate File Error", MessageBoxButtons.OK)
                    Exit Sub
                End If
            Else
                store = store + "\" + finalFileName
            End If
        End If
        If (locate.ToLower() <> store.ToLower()) Then
            System.IO.File.Copy(locate, store, True)
        End If
        gPMFunctions.SetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusArchitecture, PMERegSettingLevel.pmeRSLSetup, gPMConstants.kRegKeyLicenseKeyPath, store)

        'm_iNewLicenceLimit = CInt(txtNewLicenceLimit.Text)

        ' Check that a Licence Key has been supplied
        'DAK050100
        'If m_iNewLicenceLimit > 0 And txtLicenceKey.Text.Trim() = "" Then
        '	Interaction.MsgBox("Licence Key MUST be entered.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Invalid Licence Key")
        '	txtLicenceKey.Focus()
        '	Exit Sub
        'End If


        ' If it was Updated OK, Finish
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()

    End Sub

    Private Sub frmLicenceLimit_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            'txtNewLicenceLimit.Focus()
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        End If
    End Sub

    Private Sub lblLicenceKey_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub btnLocateFile_Click(sender As Object, e As EventArgs) Handles btnLocateFile.Click
        Dim OpenFileDialog1 As New OpenFileDialog()
        OpenFileDialog1.Title = "Please select the Pure Licence file"
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.Filter = "Licence File|*.lic"
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        If (result = DialogResult.OK) Then
            txtLocate.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnStore_Click(sender As Object, e As EventArgs) Handles btnStore.Click
        Dim folderDlg As FolderBrowserDialog = New FolderBrowserDialog()
        folderDlg.ShowNewFolderButton = True
        folderDlg.Description = "Please select the folder to store the Licence file"
        'folderDlg.RootFolder = "C:\"
        ' Show the FolderBrowserDialog.
        Dim result As DialogResult = folderDlg.ShowDialog()
        If result = DialogResult.OK Then
            txtStore.Text = folderDlg.SelectedPath
            Dim root As Environment.SpecialFolder = folderDlg.RootFolder
        End If
    End Sub

    Private Sub frmLicenceLimit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmLicenceLimit_MenuComplete(sender As Object, e As EventArgs) Handles Me.MenuComplete

    End Sub
End Class
