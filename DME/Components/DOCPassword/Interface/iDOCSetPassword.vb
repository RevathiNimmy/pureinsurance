Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	Private Sub CmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdCancel.Click
		
		iVerify = gPMConstants.PMEReturnCode.PMCancel
		
        Me.Hide()
		
	End Sub
	
	Private Sub CmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdOK.Click
		
		'enable/disable text boxes and buttons
		TxtpassVer.Visible = True
		LblVerify.Visible = True
		TxtpassVer.Focus()
		CmdOK.Visible = False
		CmdOK2.Visible = True
		
		'set layout of form, set height of frame/form
		Frame1.Height = VB6.TwipsToPixelsY(1575)
		Me.Height = VB6.TwipsToPixelsY(2580)
		CmdOK2.Top = VB6.TwipsToPixelsY(1800)
		CmdCancel.Top = VB6.TwipsToPixelsY(1800)
		CmdHelp.Top = VB6.TwipsToPixelsY(1800)
		VB6.SetDefault(CmdOK2, True)
		CmdOK2.Enabled = True
		
		
	End Sub
	
	Private Sub CmdOK2_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdOK2.Click
		Dim m_lreturn As Object
		
		'if verification is correct then set password
        Dim sPassword, sCorrectPassword, sEncryptPassword As String
		If TxtPass.Text Like TxtpassVer.Text Then
			MessageBox.Show("Password set", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Me.Cursor = Cursors.WaitCursor
			
			'declare variables
			
			'store password
			sPassword = TxtPass.Text
			
			sCorrectPassword = sPassword
			
			'Call EncryptPassword
			'--if stand-alone
			If bIsStandAlone Then

                m_lreturn = m_oDOCPassword.EncryptPassword(sPassword:=sCorrectPassword, sEncryptedPassword:=sEncryptPassword)
				
				'Call AddNodePassword
				'--if not stand-alone
			Else

				m_lreturn = m_oDOCPassword.AddNodePassword(lNodeNum:=lCurrentNum, iNodeLevel:=iCurrentLevel, sPassword:=sCorrectPassword)
			End If
			
			Me.Cursor = Cursors.WaitCursor
			iVerify = gPMConstants.PMEReturnCode.PMOK
			sEncPassword = sCorrectPassword
			Me.Close()
			
		Else
			'Display message box, set form to enter password again
			MessageBox.Show("Verification incorrect,try again", "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Interaction.Beep()
			TxtPass.Text = ""
			TxtpassVer.Text = ""
			TxtPass.Focus()
			CmdOK.Visible = True
			CmdOK2.Visible = False
			TxtpassVer.Visible = False
			LblVerify.Visible = False
			
			'change settings
			Frame1.Height = VB6.TwipsToPixelsY(855)
			Me.Height = VB6.TwipsToPixelsY(1920)
			CmdOK.Top = VB6.TwipsToPixelsY(1080)
			CmdCancel.Top = VB6.TwipsToPixelsY(1080)
			CmdHelp.Top = VB6.TwipsToPixelsY(1080)
			VB6.SetDefault(CmdOK, True)
			CmdOK.Enabled = True
			
			
		End If
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub TxtPass_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TxtPass.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		If Strings.Len(TxtPass.Text) > 0 Then
			CmdOK.Enabled = True
			CmdOK2.Enabled = False
		Else
			CmdOK.Enabled = False
			CmdOK2.Enabled = True
		End If
	End Sub
End Class