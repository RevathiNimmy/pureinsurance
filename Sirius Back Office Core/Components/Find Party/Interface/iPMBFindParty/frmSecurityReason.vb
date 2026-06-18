Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSecurityReason
	Inherits System.Windows.Forms.Form
	Private Sub frmSecurityReason_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	' ***************************************************************** '
	' Form Name: frmSecurityReason
	'
	' Date: 29/10/2003
	'
	' Description: Security Reason Popup
	'
	' Edit History: DD 29/10/2003 Created
	'
	' ***************************************************************** '
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmSecurityReason"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bIsLogged As Boolean
	Private m_bIsQuestion As Boolean
	
	'MKW080104 PN9424 Include Complaint in FSA reasons START
	Private m_iIsComplaint As Integer
	Public WriteOnly Property IsComplaint() As Integer
		Set(ByVal Value As Integer)
			m_iIsComplaint = Value
		End Set
	End Property
	'MKW080104 PN9424 Include Complaint in FSA reasons END
	
	Public ReadOnly Property IsLogged() As Boolean
		Get
			Return m_bIsLogged
		End Get
	End Property
	
	Public ReadOnly Property IsQuestion() As Boolean
		Get
			Return m_bIsQuestion
		End Get
	End Property
	
	Public ReadOnly Property SecurityReason() As String
		Get
            Dim Reason As String = ""
            If Not (lvwReason.FocusedItem Is Nothing) Then
                Return lvwReason.FocusedItem.Text
            Else
                Return ""
            End If
		End Get
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		If Not (lvwReason.FocusedItem Is Nothing) Then
            'm_bIsLogged = (lvwReason.listViewHelper1.GetListViewSubItem(lvwReason.FocusedItem, 1).Text = "1")
            'm_bIsQuestion = (lvwReason.listViewHelper1.GetListViewSubItem(lvwReason.FocusedItem, 2).Text = "1")
            m_bIsLogged = (ListViewHelper.GetListViewSubItem(lvwReason.FocusedItem, 1).Text = "1")
            m_bIsQuestion = (ListViewHelper.GetListViewSubItem(lvwReason.FocusedItem, 2).Text = "1")
			Me.Hide()
		End If
	End Sub
	

	Private Sub frmSecurityReason_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim vResultArray(,) As Object = Nothing
		Dim itmX As ListViewItem
		
		Try 
			
			'Populate the listview
			'MKW080104 PN9424 Include Complaint in FSA reasons Added parameter
            m_lReturn = g_oBusiness.GetFSAPartyViewReasons(r_vResultArray:=vResultArray, r_iIsComplaint:=Math.Abs(m_iIsComplaint))
			If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get FSA View Reasons.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Exit Sub
			End If
			
			
			For lRow As Integer = 0 To vResultArray.GetUpperBound(1)
				itmX = lvwReason.Items.Add(CStr(vResultArray(1, lRow)).Trim(), CStr(vResultArray(2, lRow)), "")
				
				'Add the logged flag
				ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vResultArray(3, lRow)).Trim()
				
				'Add the question flag
				ListViewHelper.GetListViewSubItem(itmX, 2).Text = CStr(vResultArray(4, lRow)).Trim()
			Next lRow
			
			
			'Set the columns
			lvwReason.Columns.Item(0).Width = CInt(lvwReason.Width - VB6.TwipsToPixelsX(450))
			lvwReason.Columns.Item(1).Width = CInt(0)
			SetExtraListViewProperties(lvwReason.Handle.ToInt32(), v_vShowRowSelect:=True)
			iPMFunc.CenterForm(Me)
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Form_Load failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub lvwReason_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReason.DoubleClick
		cmdOK_Click(cmdOK, New EventArgs())
	End Sub
End Class
