Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSecurityQuestion
	Inherits System.Windows.Forms.Form
	Private Sub frmSecurityQuestion_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	' ***************************************************************** '
	' Form Name: frmSecurityQuestion
	'
	' Date: 31/10/2003
	'
	' Description: Security Question Popup
	'
	' Edit History: DD 31/10/2003 Created
	'
	' ***************************************************************** '
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmSecurityQuestion"
	
	Private Const ACEnabledColor As Integer = -2147483630
	Private Const ACDisabledColor As Integer = -2147483631
	
	Private m_lPartyCnt As Integer
	Private m_sPartyType As String = ""
	Private m_sPassword As String = ""
	Private m_sPostcode As String = ""
	Private m_bProceed As Boolean
	Private m_bCorrect As Boolean
	Private m_lStatus As Integer
	
	Private m_lReturn As Integer
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyType() As String
		Set(ByVal Value As String)
			m_sPartyType = Value
		End Set
	End Property
	
	
	Public Property Password() As String
		Get
			Return m_sPassword
		End Get
		Set(ByVal Value As String)
			m_sPassword = Value
		End Set
	End Property
	
	Public WriteOnly Property Postcode() As String
		Set(ByVal Value As String)
			m_sPostcode = Value
		End Set
	End Property
	
	Public ReadOnly Property Proceed() As Boolean
		Get
			Return m_bProceed
		End Get
	End Property
	
	Public ReadOnly Property Correct() As Boolean
		Get
			Return m_bCorrect
		End Get
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bCorrect = False
		m_bProceed = False
		
		Me.Hide()
		Exit Sub
	End Sub
	
	'DJM 20/01/2004 : Changed procedure so that postcode is always required.
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		'Initialise
		m_bCorrect = False
		m_bProceed = False
		
		'If postcode is incorrect then do not proceed.
		If m_sPostcode <> "" And StrippedPostcode(txtPostcode.Text) <> StrippedPostcode(m_sPostcode) Then
			'The postcode is incorrect
			MessageBox.Show("The postal code is incorrect.", "Incorrect Postal Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub
		End If
		
		'MKW PN17529 `.8.6 to 1.8.7 Catchup. START
		'AR20041222 - PN17584 Ensure a password has been set up
		If m_sPassword = "" And txtPassword.Text = "" Then
			MessageBox.Show("Please enter a password/phrase for this client.", "Password/Phrase not entered", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub
		End If
		'MKW PN17529 `.8.6 to 1.8.7 Catchup. END
		
		' SET 27/05/2004 ISS11879
		If m_sPassword = "" And txtPassword.Text <> "" Then
			'The customer has set their password for the first time

			m_lReturn = g_oBusiness.UpdateFSAPartyPassword(m_lPartyCnt, m_sPartyType, txtPassword.Text)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Password for the Client.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				'Not much we can do here so just let the user proceed
				Me.Hide()
				Exit Sub
			End If
			
			MessageBox.Show("The new security password/phrase has been saved.", "Set security password/phrase", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			'Return entered password
			m_sPassword = txtPassword.Text.Trim()
			
			m_lStatus = ACNewPasswordEntered
			
		ElseIf m_sPassword = "" And txtPassword.Text = "" Then 
			m_lStatus = ACNoPasswordEntered
		ElseIf chkPasswordOK.CheckState = CheckState.Checked Then 
			m_lStatus = ACPasswordOK
		ElseIf chkKnowPhoneNo.CheckState = CheckState.Checked Then 
			m_lStatus = ACTelephoneNumberRecognised
		ElseIf chkKnowVoice.CheckState = CheckState.Checked Then 
			m_lStatus = ACVoiceRecognised
		Else
			m_lStatus = ACNowtSelected
			MessageBox.Show("Atleast one question must be answered in order to proceed.", "Security Question", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub
		End If
		
		m_bCorrect = True
		m_bProceed = True
		
		'hide form and get out
		Me.Hide()
		Exit Sub
		
		'*****************************************************************************************
		
		
		'    If m_sPassword = "" And txtPassword.Text <> "" Then
		'        'The customer has set their password for the first time
		'        m_lReturn = g_oBusiness.UpdateFSAPartyPassword(m_lPartyCnt, _
		''                                    m_sPartyType, txtPassword.Text)
		'
		'        ' Check for errors.
		'        If (m_lReturn& <> PMTrue) Then
		'            LogMessage _
		''                    iType:=PMLogOnError, _
		''                    sMsg:="Failed to update the Password for the Client.", _
		''                    vApp:=ACApp, _
		''                    vClass:=ACClass, _
		''                    vMethod:="cmdOK_Click", _
		''                    vErrNo:=Err.Number, _
		''                    vErrDesc:=Err.Description
		'
		'            'Not much we can do here so just let the user proceed
		'            m_lStatus = iNewPasswordEntered
		'            Me.Hide
		'            Exit Sub
		'        End If
		'
		'        MsgBox "The new security password/phrase has been saved.", _
		''            vbInformation, "Set security password/phrase"
		'
		'        m_bCorrect = True
		'        m_bProceed = True
		'
		'    ElseIf m_sPassword <> "" And UCase(txtPassword.Text) <> UCase(m_sPassword) Then
		'        'The password is incorrect
		'        If MsgBox("The password/phrase is incorrect." & vbCrLf & vbCrLf & _
		''                "Do you want to continue to the client details?", vbExclamation + vbYesNo, _
		''                "Incorrect security password/phrase") = vbYes Then
		'            m_bCorrect = False
		'            m_bProceed = True
		'        Else
		'            Exit Sub
		'        End If
		'    Else
		'        'If password is correct or no password set up then proceed.
		'        m_bCorrect = True
		'        m_bProceed = True
		'    End If
		'
		'    'Return entered password
		'    m_sPassword = Trim(txtPassword.Text)
		'
		'    'hide form and get out
		'    Me.Hide
		'    Exit Sub
	End Sub
	

	Private Sub frmSecurityQuestion_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

		Try 
			
			If m_sPassword = "" Then
				MessageBox.Show("The customer has not set a security password/phrase." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &  _
				                "Please ask them to set one now.", "Set security password/phrase", MessageBoxButtons.OK, MessageBoxIcon.Information)
				
				txtPostcode.Enabled = True
				lblPostcode.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
				txtPassword.Enabled = True
				lblPassword.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
				
				' resize the form and position the controls
				Height = VB6.TwipsToPixelsY(2400) '2800
				cmdOK.Top = VB6.TwipsToPixelsY(1905)
				pnlAnswers.Visible = False
			Else
				'DJM 20/01/2004 : Still ask for postcode.
				txtPostcode.Enabled = False
				lblPostcode.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
				txtPassword.Enabled = False
				lblPassword.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
				' SET 18/05/2004 ISS11879 - show password and postcode values
				txtPostcode.Text = m_sPostcode
				txtPassword.Text = m_sPassword
				
				' resize the form and position the controls
				Height = VB6.TwipsToPixelsY(3975)
				pnlAnswers.Top = VB6.TwipsToPixelsY(1300)
				pnlAnswers.Visible = True
			End If
			
			cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 885)
			cmdCancel.Top = cmdOK.Top
			
			iPMFunc.CenterForm(Me)
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	Private Function StrippedPostcode(ByVal sPostcode As String) As String
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: StrippedPostcode
		' PURPOSE: Strips out spaces and converts the Postcode to Upper case
		' so that comparisons will be simpler
		' AUTHOR: Danny Davis
		' DATE: 31 October 2003, 10:55:25
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As String = String.Empty
		
		Try
		
		result = sPostcode.Replace(" ", "").ToUpper()
		
	
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------

		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="StrippedPostcode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
                    Return result
		End Select
		
		Finally

		
		End Try
		Return result
	End Function
End Class
