Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Friend Partial Class frmTest
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmTest
	' File Name     : frmTest.frm
	' Date          : 17-10-2002
	' Author        : Ram Chandrabose
	' Description   : Interface used to Test the iSIRPartySummary.dll
	'
	' Edit History  :
	' RAM20021017   : Created
	' ***************************************************************** '
	
	Dim oInterface As iSIRPartySummary.Interface_Renamed
	
	Private Sub cmdSwitchTo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSwitchTo.Click
		
		Dim lReturn As Integer
		
		If oInterface Is Nothing Then
		Else
			' Swith to the Party Summary Interface

			lReturn = CInt(oInterface.switchTo())
		End If
		
	End Sub
	
	Private Sub cmdTerminate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTerminate.Click
		
		Dim lReturn As Integer
		
		If oInterface Is Nothing Then
		Else

            oInterface.Dispose()
			
			oInterface = Nothing
		End If
		
	End Sub
	
	Private Sub cmtTest_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmtTest.Click
		
		' Navigator type constant
		Const PMKeyNamePartyCnt As String = "party_cnt"
		Const PMKeyNameShortName As String = "shortname"
		Const PMKeyNameDisplayMode As String = "display_mode"
		

		Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal

		Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless
		
		
		Dim vkey As Object
		
		
		ReDim vkey(1, 2)
		
		Dim lPartyCnt As Integer = CInt(Conversion.Val(txtPartyCnt.Text))
		If lPartyCnt < 1 Then
			MessageBox.Show("Invalid Party Cnt", Application.ProductName)
			Exit Sub
		End If
		

		vkey(0, 0) = PMKeyNamePartyCnt

		vkey(1, 0) = lPartyCnt
		
		Dim sPartyShortName As String = txtPartyShortName.Text

		vkey(0, 1) = PMKeyNameShortName

		vkey(1, 1) = sPartyShortName
		

		vkey(0, 2) = PMKeyNameDisplayMode
		
		Dim iDisplayMode As Integer = VB6.GetItemData(cboDisplayMode, cboDisplayMode.SelectedIndex)
		Select Case iDisplayMode
			Case 0

				vkey(1, 2) = ACShowFormModeLess
			Case 1

				vkey(1, 2) = ACShowFormModal
		End Select
		
		oInterface = New iSIRPartySummary.Interface_Renamed()
		

		Dim lReturn As Integer = CInt(CType(oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise())
		oInterface.CallingAppName = "Test Stub"


		lReturn = CInt(oInterface.SetKeys(vkey))

		lReturn = CInt(oInterface.Start())
		
	End Sub
	
	

	Private Sub frmTest_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		cboDisplayMode.SelectedIndex = 0
	End Sub
End Class
