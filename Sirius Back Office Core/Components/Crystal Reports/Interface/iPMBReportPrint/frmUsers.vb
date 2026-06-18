Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmUsers
	Inherits System.Windows.Forms.Form
	Private m_lReturn As Integer
	Private m_iStatus As gPMConstants.PMEReturnCode
	
	Private m_vUsers() As Object
	
	
	Public Property Users() As Object
		Get
			
			Return VB6.CopyArray(m_vUsers)
			
		End Get
		Set(ByVal Value As Object)
			
			m_vUsers = Value
			
		End Set
	End Property
	
	Public Property Status() As Integer
		Get
			
			Return m_iStatus
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iStatus = Value
			
		End Set
	End Property
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim vUsers() As Object = Nothing
		
		For i As Integer = 1 To lvwUsers.Items.Count
			If lvwUsers.Items.Item(i - 1).Selected Then
				If Not Information.IsArray(vUsers) Then
					ReDim vUsers(0)
				Else

					ReDim Preserve vUsers(vUsers.GetUpperBound(0) + 1)
				End If


                vUsers(vUsers.GetUpperBound(0)) = lvwUsers.Items.Item(i - 1).Text
			End If
		Next i

		m_vUsers = vUsers
		m_iStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
		
	End Sub
	

	Private Sub frmUsers_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim oListItem As ListViewItem
		
		iPMFunc.CenterForm(Me)
		
		For	Each m_vUsers_item As Object In m_vUsers
			
			'Set oListItem = lvwUsers.ListItems.Add(, , Trim$(m_vUsers(i)), , "Risk")
			oListItem = lvwUsers.Items.Add(CStr(m_vUsers_item).Trim())
			
			
			oListItem = Nothing
			
		Next m_vUsers_item
	End Sub
End Class