Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmPassword
	Inherits System.Windows.Forms.Form
	Private m_sUserName As String = ""
	Private m_sPassword As String = ""
	Private m_sBrokerID As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public ReadOnly Property UserName() As String
		Get
			Return m_sUserName
		End Get
	End Property
	
	Public ReadOnly Property Password() As String
		Get
			Return m_sPassword
		End Get
	End Property
	
	Public ReadOnly Property BrokerID() As String
		Get
			Return m_sBrokerID
		End Get
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        If CBool(gPMFunctions.ToSafeString(txtPassword.Text <> "").Trim()) And CBool(gPMFunctions.ToSafeString(txtBrokerID.Text <> "").Trim()) Then
            m_sUserName = txtUsername.Text
            m_sPassword = txtPassword.Text
            m_sBrokerID = txtBrokerID.Text
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Hide()
        Else
            MessageBox.Show("Please enter a valid username, password and broker id to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
	End Sub
End Class