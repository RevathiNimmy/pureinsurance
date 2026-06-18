Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Friend Partial Class frmWriteOffReason
	Inherits System.Windows.Forms.Form
	
    Private m_lReturnValue As gPMConstants.PMEReturnCode
	
	Public WriteOnly Property message() As String
		Set(ByVal Value As String)
			lblMessage.Text = Value
		End Set
	End Property
	
	Public ReadOnly Property WriteOffReasonID() As Integer
		Get
			Return cboWriteOffReasonID.ItemId
		End Get
	End Property
	
	Public ReadOnly Property ReturnValue() As Integer
		Get
			Return m_lReturnValue
		End Get
    End Property

    Public WriteOnly Property WhereCondition() As String
        Set(value As String)
            If ToSafeString(value).Trim.Length > 0 Then
                Me.cboWriteOffReasonID.WhereClause = value
            End If
        End Set
    End Property

    Public WriteOnly Property Title() As String
        Set(value As String)
            If ToSafeString(value).Trim.Length > 0 Then
                Me.Text = value
            End If
        End Set
    End Property
	
	Private Sub Cancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Cancel.Click
		m_lReturnValue = gPMConstants.PMEReturnCode.PMCancel
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturnValue = gPMConstants.PMEReturnCode.PMTrue
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		Me.Hide()
	End Sub
	
	Private Sub frmWriteOffReason_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		End If
    End Sub

    Private Sub frmWriteOffReason_Load(sender As Object, e As EventArgs) Handles Me.Load
        cboWriteOffReasonID.RefreshList()
    End Sub
End Class