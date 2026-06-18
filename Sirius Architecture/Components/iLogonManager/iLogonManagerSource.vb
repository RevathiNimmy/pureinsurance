Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms

'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSource
	Inherits System.Windows.Forms.Form
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private Const ACClass As String = "frmSource"
	
	Private m_iSourceID As Integer
	Private m_sSourceName As String = ""
	
	Private m_vSources( ,  ) As Object
	
	' array constants
	Private Const SOURCE_ID As Integer = 0
	Private Const SOURCE_NAME As Integer = 1
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
   
    Public WriteOnly Property Sources() As Object
        Set(ByVal Value As Object)
            m_vSources = Value
        End Set
    End Property

	
	Public ReadOnly Property SourceID() As Integer
		Get
			Return m_iSourceID
		End Get
	End Property
	
	Public ReadOnly Property SourceName() As String
		Get
			Return m_sSourceName
		End Get
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		If ListBoxHelper.GetSelectedIndex(lstSource) = -1 Then
			MessageBox.Show("Please select a branch", "iLogonManager", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
	Private Sub frmSource_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			lstSource.Focus()
			
		End If
	End Sub
	

	Private Sub frmSource_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		m_iSourceID = -1
		
		lstSource.Items.Clear()
		
		For iLoop As Integer = m_vSources.GetLowerBound(1) To m_vSources.GetUpperBound(1)
			lstSource.Items.Add(CStr(m_vSources(SOURCE_NAME, iLoop)))
			VB6.SetItemData(lstSource, lstSource.Items.Count - 1, CInt(m_vSources(SOURCE_ID, iLoop)))
		Next 
		
	End Sub
	
	Private Sub frmSource_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_iSourceID < 0 Then
			MessageBox.Show("Please select a branch", "iLogonManager", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			Cancel = True
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub lstSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstSource.SelectedIndexChanged
		
		m_iSourceID = VB6.GetItemData(lstSource, ListBoxHelper.GetSelectedIndex(lstSource))
		m_sSourceName = VB6.GetItemString(lstSource, ListBoxHelper.GetSelectedIndex(lstSource)).Trim()
		
	End Sub
	
	Private Sub lstSource_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstSource.DoubleClick
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
End Class