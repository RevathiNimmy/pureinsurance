Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'Develoer Guide no 129
Imports SharedFiles
Friend Partial Class frmPaymentProcessed
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmPaymentsProcessed"
	Private m_vPaymentArray( ,  ) As Object
	Private m_lReturn As Integer
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Me.Close()
	End Sub
	

	Private Sub frmPaymentProcessed_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lReturn = DisplayData()
	End Sub
	
	Private Function DisplayData() As Integer
		Dim result As Integer = 0
        Const kMethodName As String = "DisplayData"
        Dim oListItem As ListViewItem
        Try

            For iIndex As Integer = m_vPaymentArray.GetLowerBound(1) To m_vPaymentArray.GetUpperBound(1)
                oListItem = lvwPaymentDesc.Items.Add(CStr(m_vPaymentArray(InterfaceMain.ACListPaymentSummary.PSMediaType, iIndex)))
                ListViewHelper.GetListViewSubItem(oListItem, InterfaceMain.ACListPaymentSummary.PSPaymentCount).Text = CStr(m_vPaymentArray(InterfaceMain.ACListPaymentSummary.PSPaymentCount, iIndex))
                ListViewHelper.GetListViewSubItem(oListItem, InterfaceMain.ACListPaymentSummary.PSPaymentValue).Text = CStr(m_vPaymentArray(InterfaceMain.ACListPaymentSummary.PSPaymentValue, iIndex))
            Next

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
		Return result
	End Function
    'Developer guide No 31
   
    Public WriteOnly Property PaymentArray() As Object
        Set(ByVal Value As Object)
            m_vPaymentArray = Value
        End Set
    End Property
End Class
