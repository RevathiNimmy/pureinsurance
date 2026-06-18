Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmStatus
	Inherits System.Windows.Forms.Form
	Private m_oBusiness As Object
	Private m_lReturn As Integer
	
	Public WriteOnly Property Business() As Object
		Set(ByVal Value As Object)
			m_oBusiness = Value
		End Set
	End Property
	
	Private Sub frmStatus_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			

			m_lReturn = m_oBusiness.UpdateServerRLDF()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Error Updating Server RDLF!", "List Maintenance Status", MessageBoxButtons.OK)
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			m_oBusiness = Nothing
			
			Me.Hide()
			
		End If
	End Sub
	

	Private Sub frmStatus_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		iPMFunc.CenterForm(Me)
	End Sub
End Class