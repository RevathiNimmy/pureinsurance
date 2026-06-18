Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSelectAddress
	Inherits System.Windows.Forms.Form
	
	Private m_lState As gPMConstants.PMEReturnCode
    'developer guide no. 33
    Private m_asEmailAddress As Object
    Private Const vbFormCode As Integer = 0
	Private Const ACClass As String = "frmSelectAddress"
	Private Const FORM_CAPTION As String = "Select Email Addresses"
	
	
	Public Property EmailAddress() As String()
		Get
			Return VB6.CopyArray(GetAddresses())
		End Get
		Set(ByVal Value() As String)
			m_asEmailAddress = Value
		End Set
	End Property
	
	Public ReadOnly Property State() As Integer
		Get
			Return m_lState
		End Get
	End Property
	
	Private Sub RenderForm()
		
		Dim oItem As ListViewItem
		
		Me.Text = FORM_CAPTION
		
		With Me.lvwEmailAddress
			.Columns.Add("H1", "Email Address", CInt(VB6.TwipsToPixelsX(3000)))
			.Columns.Add("H2", "Description", CInt(VB6.TwipsToPixelsX(2600)))
		End With
		
		Me.lvwEmailAddress.SmallImageList = Me.ImageList1
		
		If Information.IsArray(m_asEmailAddress) Then
			For lLoop As Integer = m_asEmailAddress.GetLowerBound(1) To m_asEmailAddress.GetUpperBound(1)

				oItem = Me.lvwEmailAddress.Items.Add("I" & lLoop, m_asEmailAddress(ARRAY_EMAIL_EMAILADDRESS, lLoop), "")
				ListViewHelper.GetListViewSubItem(oItem, 1).Text = m_asEmailAddress(ARRAY_EMAIL_DESCRIPTION, lLoop)
			Next lLoop
		End If
		
		Me.lvwEmailAddress.FocusedItem.Selected = False
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lState = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lState = gPMConstants.PMEReturnCode.PMOk
		Me.Hide()
		
	End Sub
	

	Private Sub frmSelectAddress_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const ACMethod As String = "Form_Load"
		Try 
			
			RenderForm()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The method " & ACMethod & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub frmSelectAddress_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		


        If UnloadMode <> vbFormCode Then

            m_lState = gPMConstants.PMEReturnCode.PMCancel
            Cancel = 1
            Me.Hide()
        End If

        eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmSelectAddress_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Me.lvwEmailAddress.Width = Me.Width - VB6.TwipsToPixelsX(135)
		Me.lvwEmailAddress.Height = Me.Height - VB6.TwipsToPixelsY(945)
		Me.cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(840)
		Me.cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(840)
		Me.cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(2670)
		Me.cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(1350)
		
	End Sub
	
	Private Sub lvwEmailAddress_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwEmailAddress.BeforeLabelEdit
		Dim Cancel As Boolean = eventArgs.CancelEdit
		
		Cancel = 1
		
	End Sub
	
	Private Function GetAddresses() As String()
		
		Const ACMethod As String = "GetAddresses"
		Try 
            'developer guide no. 33
            Dim asAddress As Object
			Dim lAddressCount As Integer
			
			
			For lLoop As Integer = 1 To Me.lvwEmailAddress.Items.Count
				If Me.lvwEmailAddress.Items.Item(lLoop - 1).Selected Then
					
					If lAddressCount = 0 Then
						asAddress = Array.CreateInstance(GetType(String), New Integer(){ARRAY_EMAIL_UPPER - ARRAY_EMAIL_LOWER + 1, 1}, New Integer(){ARRAY_EMAIL_LOWER, 0})
					Else
						asAddress = ArraysHelper.RedimPreserve(Of String(, ))(asAddress, New Integer(){ARRAY_EMAIL_UPPER - ARRAY_EMAIL_LOWER + 1, lAddressCount + 1}, New Integer(){ARRAY_EMAIL_LOWER, 0})
					End If
					
					asAddress(ARRAY_EMAIL_EMAILADDRESS, lAddressCount) = Me.lvwEmailAddress.Items.Item(lLoop - 1).Text
					asAddress(ARRAY_EMAIL_DESCRIPTION, lAddressCount) = ListViewHelper.GetListViewSubItem(Me.lvwEmailAddress.Items.Item(lLoop - 1), 1).Text
					
					lAddressCount += 1
					
				End If
			Next lLoop
			
			
			Return asAddress
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The method " & ACMethod & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Function
End Class
