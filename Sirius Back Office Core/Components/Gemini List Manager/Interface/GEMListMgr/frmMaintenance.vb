Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMaintenance
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmMaintenance
	'
	' Date: 10/02/1999
	'
	' Description: Maintenance form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	
	Private Const ACClass As String = "frmMaintenance"
	
	Private m_sText As New FixedLengthString(70)
	Private m_sABICode As New FixedLengthString(10)
	Private m_sCommand As New FixedLengthString(1)
	
	Private m_iStatus As Integer
	Private m_bDataChanged As Boolean
	
	Public Function Maintain(ByRef r_vListItem() As Object, ByRef r_iMode As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No 236
			'Set the list item values
            m_sText.Value = r_vListItem(GEMListMgrConst.LSTText)
            m_sABICode.Value = r_vListItem(GEMListMgrConst.LSTABICode)
            m_sCommand.Value = r_vListItem(GEMListMgrConst.LSTCommand)
			
			'Set the status mode
			m_iStatus = r_iMode
			txtText.Text = m_sText.Value.Trim()
			txtABICode.Text = m_sABICode.Value.Trim()
			
			'Set the deleted mode
			If m_sCommand.Value = GEMListMgrConst.LSTDeleted Then
				chkDeleted.CheckState = CheckState.Checked
			Else
				chkDeleted.CheckState = CheckState.Unchecked
			End If
			
			Me.ShowDialog()
			
			'If Status is edit then get new data
			If m_iStatus <> gPMConstants.PMEReturnCode.PMCancel Then
				r_iMode = m_iStatus

				r_vListItem(GEMListMgrConst.LSTText) = m_sText.Value.Trim()

				r_vListItem(GEMListMgrConst.LSTABICode) = m_sABICode.Value.Trim()

				r_vListItem(GEMListMgrConst.LSTCommand) = m_sCommand.Value
			End If
			
			Return result
			
			Me.Close()
		
		Catch 
		End Try
		
		
		
		
		Return result
	End Function
	
	Private Sub chkDeleted_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDeleted.CheckStateChanged
		
		m_bDataChanged = True
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_iStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
		
	End Sub
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		If txtText.Text = "" Then
			Exit Sub
		End If
		
		'Apply changes to data
		If m_bDataChanged Then
			
			'Get the data
			m_iStatus = gPMConstants.PMEComponentAction.PMEdit
			m_sText.Value = txtText.Text.Substring(0, 70)
			m_sABICode.Value = txtABICode.Text.Substring(0, 10)
			
			'Set the command char to deleted
			If chkDeleted.CheckState = CheckState.Checked Then
				m_sCommand.Value = GEMListMgrConst.LSTDeleted
			Else
				m_sCommand.Value = GEMListMgrConst.LSTAmmended
			End If
		End If
		
		Me.Hide()
		
	End Sub
	
	
	Private isInitializingComponent As Boolean
	Private Sub txtABICode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtABICode.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_bDataChanged = True
		
		If Strings.Len(txtABICode.Text) > 10 Then
			txtABICode.Text = txtABICode.Text.Substring(0, 10)
		End If
		
	End Sub
	
	Private Sub txtText_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtText.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_bDataChanged = True
		
	End Sub
End Class