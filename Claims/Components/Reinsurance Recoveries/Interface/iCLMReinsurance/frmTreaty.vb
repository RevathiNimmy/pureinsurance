Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmTreaty
	Inherits System.Windows.Forms.Form
	
	Public Code As String = ""
    Public TreatyId As Integer
    'Devloper Guide no. 7
    Private Const vbFormCode As Integer = 3
	Public Status As gPMConstants.PMEReturnCode
	Private m_sTransactionType As String = ""
	
	
	
	Public Property TransactionType() As String
		Get
			Return m_sTransactionType
		End Get
		Set(ByVal Value As String)

			m_sTransactionType = CStr(Value)
		End Set
	End Property
	
	' ***************************************************************** '
	'                             EVENTS
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Status = gPMConstants.PMEReturnCode.PMCancel
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Status = gPMConstants.PMEReturnCode.PMOK
		
		TreatyId = cboTreaty.ItemId
		Code = cboTreaty.ItemCaption
		
		Me.Close()
	End Sub
	
	
	' ***************************************************************** '
	'                           FORM EVENTS
	' ***************************************************************** '

    Private Sub frmTreaty_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'developer guide no.(Added to insert the values in the combobox)
        cboTreaty.FirstItem = ""
        If m_sTransactionType = "TX" Then
            cboTreaty.WhereClause = "reinsurance_type_id IN (Select reinsurance_type_id FROM Reinsurance_Type WHERE code = 'XOL')"
            cboTreaty.RefreshList()
        ElseIf m_sTransactionType = "T" Then
            cboTreaty.WhereClause = "reinsurance_type_id IN (Select reinsurance_type_id FROM Reinsurance_Type WHERE code='QUO')"
            cboTreaty.RefreshList()
        End If

        ' Check how we should label the "treaty"
        If g_bIsUnderwritingAgency Then
            lblTreaty.Text = "Facility:"
        End If
    End Sub
	
	Private Sub frmTreaty_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

		If UnloadMode <> vbFormCode Then
            Status = gPMConstants.PMEReturnCode.PMCancel
		End If

	End Sub
End Class