Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPartyTax_NET.uctPartyTax")> _
Public Partial Class uctPartyTax
	Inherits System.Windows.Forms.UserControl
	
	' ***************************************************************** '
	' Object Name: uctPMUFees
	'
	' Date: 25-04-2005
	'
	' Description: Main User Control
	'
	' Edit History:
	'   NB: All Initial functionality has been ripped from
	'           iPMBPartyFee.frmUFeeRMStepInterface
	' ***************************************************************** '
	
	Private Const ACClass As String = "uctPartyTax"
	
	
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iUserId As Integer
	Private m_lCurrencyID As Integer
	
	'Default Property Values:
	Private Const m_def_BackColor As Integer = 0
	Private Const m_def_ForeColor As Integer = 0
	Private Const m_def_BackStyle As Integer = 0
	Private Const m_def_BorderStyle As Integer = 0
	Private Const m_def_ShowEdit As Boolean = True
	Private Const m_def_Enabled As Boolean = True
	Private Const m_def_Visible As Boolean = True
	Private Const m_def_SizeHorizontal As Boolean = False
	Private Const m_def_FrameOn As Boolean = False
	
	'Property Variables:
	Private m_BackColor As Integer
	Private m_ForeColor As Integer
	Private m_BackStyle As Integer
	Private m_BorderStyle As Integer
	Private m_ShowEdit As Boolean
	Private m_Enabled As Boolean
	Private m_Visible As Boolean
	Private m_Font As Font
	Private m_bSizeHorizontal As Boolean
	Private m_bFrameOn As Boolean
	
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_iTask As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_lReturn As Integer
	Private m_oBusiness As Object
	Private m_oFormFields As iPMFormControl.FormFields
	Private m_ListViewArray As Object
	Private m_bStatus As Boolean
	
	Public Event Change(ByVal Sender As Object, ByVal e As EventArgs)
	Public Event IsDomiciledForTaxChanged(ByVal Sender As Object, ByVal e As EventArgs)
	
	Private m_bReadOnly As Boolean
	Private m_sTaxNumber As String = ""
	Private m_bIsDomiciledForTax As Boolean
	Private m_bTaxExempt As Boolean
	Private m_dTaxPercentage As Double
	Private m_bIsReinsurer As Boolean
	Private m_bIsBrokingAgent As Boolean
	'020506 Datasure
	Private m_bIsBrokingInsurer As Boolean
	
	<Browsable(False)> _
	Public WriteOnly Property IsReinsurer() As Boolean
		Set(ByVal Value As Boolean)
			m_bIsReinsurer = Value
		End Set
	End Property
	
	'*********************************
	<Browsable(True)> _
	Public Property FrameOn() As Boolean
		Get
			Return m_bFrameOn
		End Get
		Set(ByVal Value As Boolean)
			m_bFrameOn = Value
		End Set
	End Property
	'*********************************
	<Browsable(True)> _
	Public Property SizeHorizontal() As Boolean
		Get
			Return m_bSizeHorizontal
		End Get
		Set(ByVal Value As Boolean)
			m_bSizeHorizontal = Value
		End Set
	End Property
	'*********************************
	<Browsable(False)> _
	Public WriteOnly Property ReadOnly_Renamed() As Boolean
		Set(ByVal Value As Boolean)
			m_bReadOnly = Value
			SetupControl()
		End Set
	End Property
	'*********************************
	<Browsable(True)> _
	Public Property TaxNumber() As String
		Get
			m_sTaxNumber = txtTaxNumber.Text
			Return m_sTaxNumber
		End Get
		Set(ByVal Value As String)
			m_sTaxNumber = Value
			txtTaxNumber.Text = m_sTaxNumber
		End Set
	End Property
	'*********************************
	<Browsable(True)> _
	Public Property IsDomiciledForTax() As Boolean
		Get
			m_bIsDomiciledForTax = (chkIsDomiciledForTax.CheckState = CheckState.Checked)
			Return m_bIsDomiciledForTax
		End Get
		Set(ByVal Value As Boolean)
			m_bIsDomiciledForTax = Value
			If m_bIsDomiciledForTax Then
				chkIsDomiciledForTax.CheckState = CheckState.Checked
			End If
			RaiseEvent IsDomiciledForTaxChanged(Me, Nothing)
		End Set
	End Property
	'*********************************
	<Browsable(True)> _
	Public Property TaxExempt() As Boolean
		Get
			m_bTaxExempt = (chkTaxExempt.CheckState = CheckState.Checked)
			Return m_bTaxExempt
		End Get
		Set(ByVal Value As Boolean)
			m_bTaxExempt = Value
			If m_bTaxExempt Then
				chkTaxExempt.CheckState = CheckState.Checked
			End If
		End Set
	End Property
	'*********************************
	<Browsable(True)> _
	Public Property TaxPercentage() As Double
		Get
			m_dTaxPercentage = gPMFunctions.ToSafeDouble(txtTaxPercentage.Text)
			Return m_dTaxPercentage
		End Get
		Set(ByVal Value As Double)
			m_dTaxPercentage = Value
			txtTaxPercentage.Text = CStr(m_dTaxPercentage)
		End Set
	End Property
	'*********************************
	<Browsable(False)> _
	Public WriteOnly Property IsBrokingAgent() As Boolean
		Set(ByVal Value As Boolean)
			m_bIsBrokingAgent = Value
			If m_bIsBrokingAgent Then
				'PN32427
				'lblIsDomiciledForTax.Caption = "        Tax registered:"
				'020506 Datasure
				lblTaxExempt.Visible = False
				chkTaxExempt.Visible = False
				lblTaxPercentage.Visible = False
				txtTaxPercentage.Visible = False
			End If
		End Set
	End Property
	
	'*********************************
	'020506 Datasure
	<Browsable(False)> _
	Public WriteOnly Property IsBrokingInsurer() As Boolean
		Set(ByVal Value As Boolean)
			m_bIsBrokingInsurer = Value
			If m_bIsBrokingInsurer Then
				lblTaxExempt.Visible = True
				chkTaxExempt.Visible = True
				lblTaxPercentage.Visible = True
				txtTaxPercentage.Visible = True
			End If
		End Set
	End Property
	
    Private Sub chkIsDomiciledForTax_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsDomiciledForTax.CheckStateChanged

        ' for reinsurers tax number is mandatory if
        ' domiciled for tax is true
        If m_bIsReinsurer Then
            If chkIsDomiciledForTax.CheckState = CheckState.Checked Then

                lblTaxNumber.Left -= VB6.TwipsToPixelsX(140)
                lblTaxNumber.Font = VB6.FontChangeBold(lblTaxNumber.Font, True)
            Else
                lblTaxNumber.Left += VB6.TwipsToPixelsX(140)
                lblTaxNumber.Font = VB6.FontChangeBold(lblTaxNumber.Font, False)
            End If
        End If

        RaiseEvent IsDomiciledForTaxChanged(Me, Nothing)

    End Sub

    'Private Sub txtTaxNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtTaxNumber.KeyPress
    '    Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '    If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 8 Then
    '        KeyAscii = 0
    '    End If
    '    If KeyAscii = 0 Then
    '        eventArgs.Handled = True
    '    End If
    '    eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub
	
	Private isInitializingComponent As Boolean
    Private Sub txtTaxPercentage_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxPercentage.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ValidateTaxPercentage()
    End Sub
	

	Private Sub UserControl_InitProperties()
		m_BackColor = m_def_BackColor
		m_ForeColor = m_def_ForeColor
		m_BackStyle = m_def_BackStyle
		m_BorderStyle = m_def_BorderStyle
		m_ShowEdit = m_def_ShowEdit
		m_Enabled = m_def_Enabled
		m_Visible = m_def_Visible


        'developer guide no solution 2
        'm_Font = Ambient.Font
        m_Font = Me.Font
		m_bSizeHorizontal = m_def_SizeHorizontal
		m_bFrameOn = m_def_FrameOn
	End Sub
	


    'developer guide no solution 1
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


		m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


		m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


		m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


		m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


		m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


		m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


		m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))


		m_bSizeHorizontal = CBool(PropBag.ReadProperty("SizeHorizontal", m_def_SizeHorizontal))


		m_bFrameOn = CBool(PropBag.ReadProperty("FrameOn", m_def_FrameOn))


        'developer guide no solution 2
        'm_Font = PropBag.ReadProperty("Font", Ambient.Font)
        m_Font = PropBag.ReadProperty("Font", Me.Font)
	End Sub
	


    'developer guide no solution 1
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

		PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

		PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

		PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

		PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

		PropBag.WriteProperty("ShowEdit", m_ShowEdit, m_def_ShowEdit)

		PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)

		PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)

		PropBag.WriteProperty("SizeHorizontal", m_bSizeHorizontal, m_def_SizeHorizontal)


        'developer guide no solution 2
        'PropBag.WriteProperty("Font", m_Font, Ambient.Font)
        PropBag.WriteProperty("Font", m_Font, Me.Font)

		PropBag.WriteProperty("FrameOn", m_bFrameOn, m_def_FrameOn)
	End Sub
	
	Private Sub uctPartyTax_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
        Dim lFrameBorder As Integer
		
		Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width))
        Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height))
		
		fraPartyTax.Top = 0
		fraPartyTax.Left = 0
		
        fraPartyTax.Height = VB6.TwipsToPixelsY(lNewHeight - 45)
        fraPartyTax.Width = VB6.TwipsToPixelsX(lNewWidth - 45)
		
		If m_bFrameOn Then
			lFrameBorder = 100

            fraPartyTax.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        Else
            lFrameBorder = 0

            fraPartyTax.BorderStyle = Windows.Forms.BorderStyle.None
        End If
		
		If m_bSizeHorizontal Then
			
			lblTaxNumber.Left = VB6.TwipsToPixelsX(165 + lFrameBorder)
			lblTaxNumber.Top = VB6.TwipsToPixelsY(180 + lFrameBorder)
			
			lblIsDomiciledForTax.Left = VB6.TwipsToPixelsX(5805 + lFrameBorder)
			lblIsDomiciledForTax.Top = VB6.TwipsToPixelsY(180 + lFrameBorder)
			
			lblTaxExempt.Left = VB6.TwipsToPixelsX(7995 + lFrameBorder)
			lblTaxExempt.Top = VB6.TwipsToPixelsY(180 + lFrameBorder)
			
			lblTaxPercentage.Left = VB6.TwipsToPixelsX(9480 + lFrameBorder)
			lblTaxPercentage.Top = VB6.TwipsToPixelsY(180 + lFrameBorder)
			
			txtTaxNumber.Left = VB6.TwipsToPixelsX(1320 + lFrameBorder)
			txtTaxNumber.Top = VB6.TwipsToPixelsY(120 + lFrameBorder)
			
			chkIsDomiciledForTax.Left = VB6.TwipsToPixelsX(7680 + lFrameBorder)
			chkIsDomiciledForTax.Top = VB6.TwipsToPixelsY(120 + lFrameBorder)
			
			chkTaxExempt.Left = VB6.TwipsToPixelsX(9120 + lFrameBorder)
			chkTaxExempt.Top = VB6.TwipsToPixelsY(120 + lFrameBorder)
			
			txtTaxPercentage.Left = VB6.TwipsToPixelsX(10920 + lFrameBorder)
			txtTaxPercentage.Top = VB6.TwipsToPixelsY(120 + lFrameBorder)
			
		Else
			
			lblTaxNumber.Left = VB6.TwipsToPixelsX(885 + lFrameBorder)
			lblTaxNumber.Top = VB6.TwipsToPixelsY(180 + lFrameBorder)
			
			lblIsDomiciledForTax.Left = VB6.TwipsToPixelsX(165 + lFrameBorder)
			lblIsDomiciledForTax.Top = VB6.TwipsToPixelsY(580 + lFrameBorder)
			
			lblTaxExempt.Left = VB6.TwipsToPixelsX(915 + lFrameBorder)
			lblTaxExempt.Top = VB6.TwipsToPixelsY(980 + lFrameBorder)
			
			lblTaxPercentage.Left = VB6.TwipsToPixelsX(600 + lFrameBorder)
			lblTaxPercentage.Top = VB6.TwipsToPixelsY(1380 + lFrameBorder)
			
			txtTaxNumber.Left = VB6.TwipsToPixelsX(2040 + lFrameBorder)
			txtTaxNumber.Top = VB6.TwipsToPixelsY(120 + lFrameBorder)
			
			chkIsDomiciledForTax.Left = VB6.TwipsToPixelsX(2040 + lFrameBorder)
			chkIsDomiciledForTax.Top = VB6.TwipsToPixelsY(520 + lFrameBorder)
			
			chkTaxExempt.Left = VB6.TwipsToPixelsX(2040 + lFrameBorder)
			chkTaxExempt.Top = VB6.TwipsToPixelsY(920 + lFrameBorder)
			
			txtTaxPercentage.Left = VB6.TwipsToPixelsX(2040 + lFrameBorder)
			txtTaxPercentage.Top = VB6.TwipsToPixelsY(1320 + lFrameBorder)
			
		End If
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: SetupControl
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 17-08-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
	Private Function SetupControl(Optional ByVal v_lMode As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupControl"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		txtTaxNumber.Enabled = Not m_bReadOnly
		txtTaxPercentage.Enabled = Not m_bReadOnly
		chkIsDomiciledForTax.Enabled = Not m_bReadOnly
		chkTaxExempt.Enabled = Not m_bReadOnly
		
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: ValidateTaxPercentage
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ValidateTaxPercentage() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ValidateTaxPercentage"
		
		Dim lReturn, lPos As Integer
		Dim sLastCharacter As String = ""
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If txtTaxPercentage.Text <> "" Then
			
			sLastCharacter = txtTaxPercentage.Text.Substring(Strings.Len(txtTaxPercentage.Text) - 1)
			
			lPos = (("0123456789.").IndexOf(sLastCharacter) + 1)
			
			If lPos = 0 Then
				MessageBox.Show("Percentage must be > 0.01 and < 99.99", "Tax Percent Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
				txtTaxPercentage.Text = ""
            ElseIf gPMFunctions.ToSafeDouble(txtTaxPercentage.Text) >= 100 Or gPMFunctions.ToSafeDouble(txtTaxPercentage.Text) < 0 Then
                MessageBox.Show("Percentage must be > 0.01 and < 99.99", "Tax Percent Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtTaxPercentage.Text = ""
			End If
			
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function

End Class
