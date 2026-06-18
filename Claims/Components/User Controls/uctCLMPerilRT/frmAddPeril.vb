Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmAddPeril
	Inherits System.Windows.Forms.Form
	Private m_lStatus As Integer
	

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		Me.Hide()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		FileSystem.FileClose()
	End Sub
	

	Private Sub frmAddPeril_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		SetInterfaceDefaults()
		FileSystem.FileClose()
	End Sub
	
	Private Sub SetInterfaceDefaults()
		
		Dim vValue As String = ""
		
		'CMG/PB See if LossSchedule is enabled and set a private boolean
		'RVH Changed to base decision on claimsbuilder system option
		iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
		
		Dim bClaimsBuilder As Boolean = (gPMFunctions.NullToString(vValue) = "1")
		lblDescription.Visible = bClaimsBuilder
		txtDescription.Visible = bClaimsBuilder
		'End CMG
	End Sub
End Class