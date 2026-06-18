Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmQASFindName
	Inherits System.Windows.Forms.Form
	Private Sub frmQASFindName_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	'*****************************************************
	'
	'     QAS Names form.
	'     Uses uctaddresscontrol to access QAS names system
	'
	'     Brings back address and name data
	'
	'     JDW 20/08/01
	'
	'******************************************************
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim ACClass As Object = Nothing
		Try 
			
			m_bQASCancel = True
			Me.Close()
		
		Catch excep As System.Exception
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to cancel form", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdcancel_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim ACClass As Object = Nothing
		Try 
			
			'CJB 03/09/2002 CNIC change to ensure that if there is QAS failure that manually
			'entered details will still get transferred...Comment out check if address selected
			'If m_bChosenAddress Then
			'Transfer details
			m_sTitle = txtTitle.Text
			m_sSurname = txtSurname.Text
			m_sForename = txtForename.Text
			m_sInitial = txtInitial.Text
			
			m_sOrgName = txtOrgName.Text
			
			m_oQASN.Add1 = uctPMAddressControl1.AddressLine1
			m_oQASN.Add2 = uctPMAddressControl1.AddressLine2
			m_oQASN.Add3 = uctPMAddressControl1.AddressLine3
			m_oQASN.Add4 = uctPMAddressControl1.AddressLine4
			m_oQASN.Postcode = uctPMAddressControl1.PostCode
			
			'End If
			
			m_bQASCancel = False
			
			Me.Close()
		
		Catch excep As System.Exception
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process QAS data", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	

	Private Sub frmQASFindName_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim ACClass As Object = Nothing
		Try 
			
			'set up control to access QAS Names
			uctPMAddressControl1.QASDatabaseID = 3
			m_bChosenAddress = False
			If m_sPartyType4QAS = "PC" Then
				lblorgname.Visible = False
				txtOrgName.Visible = False
			Else
				lbltitle.Visible = False
				lblForename.Visible = False
				lblsurname.Visible = False
				lblinitial.Visible = False
				
				txtTitle.Visible = False
				txtForename.Visible = False
				txtSurname.Visible = False
				txtInitial.Visible = False
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub uctPMAddressControl1_ChosenAddress(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMAddressControl1.ChosenAddress
        Dim ACClass As Object = Nothing
		Try 
			
			'get names data
			
			If m_sPartyType4QAS = "PC" Then
				'02082002 CMG/PB Commented to allow scalability
				'compilation - no code available in VSS
				txtTitle.Text = uctPMAddressControl1.Title
				txtForename.Text = uctPMAddressControl1.Forename
				txtInitial.Text = uctPMAddressControl1.Initial
				txtSurname.Text = uctPMAddressControl1.Surname
			Else
				txtOrgName.Text = uctPMAddressControl1.OrgName
			End If
			
			'set flag to proceed further with new client
			m_bChosenAddress = True
		
		Catch excep As System.Exception
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed process chosen address", vApp:=ACApp, vClass:=ACClass, vMethod:="uctPMAddressControl1_ChosenAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub

    Private Sub frmQASFindName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            TabStrip1.SelectedIndex = 0
        End If
    End Sub
End Class
