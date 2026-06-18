Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSalvageDetails
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmSalvageDetails"
	
	Private Const PMTaxArray_TaxTypeID As Integer = 0
	Private Const PMTaxArray_TaxTypeDesc As Integer = 1
	Private Const PMTaxArray_TaxBandID As Integer = 2
	Private Const PMTaxArray_TaxBandDesc As Integer = 3
	Private Const PMTaxArray_TaxIsValue As Integer = 4
	Private Const PMTaxArray_TaxRate As Integer = 5
	Private Const PMTaxArray_TaxTypeCode As Integer = 6
	
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lInitialReserveDecimalCount As Integer
	Private m_lRevisedReserveDecimalCount As Integer
	
	Private m_lPerilId As Integer
	Private m_cInitialReserve As Decimal
	Private m_cRevisedReserve As Decimal
	Private m_nFindClaimMode As Integer
	Private m_cReceiptAmount As Decimal
	Private m_lRecoveryTypeID As Integer
	Private m_cNewreserve As Decimal
	Private m_cCheckreserve As Decimal
	Private m_cReceivedTodate As Decimal
	
	Private m_lLossCurrencyId As Integer
	Private m_sLossCurrencyDesc As String = ""
	Private m_lCurrencyId As Integer
	Private m_dReceiptToLossRate As Double
	Private m_cReceiptAmountLoss As Decimal
	Private m_cRevisedReserveLoss As Decimal
	Private m_lClaimCompanyID As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_bDisableCurrency As Boolean
	Private m_vTaxArray( ,  ) As Object
	Private m_cTaxAmount As Decimal
	Private m_sTaxTypeCode As String = ""
	Private m_sTaxTypeDesc As String = ""
	Private m_sTaxBandDesc As String = ""
	
	Private m_oCurrencyConvert As Object
	Private m_oInsuranceFile As Object
	Private m_oUserAuthorities As Object
	
	
	Public Property TaxAmount() As Decimal
		Get
			Return m_cTaxAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cTaxAmount = Value
		End Set
	End Property
	
	Public Property ReceivedTodate() As Decimal
		Get
			
			Return m_cReceivedTodate
			
		End Get
		Set(ByVal Value As Decimal)
			
			m_cReceivedTodate = Value
			
		End Set
	End Property
	
	Public Property PerilID() As Integer
		Get
			
			Return m_lPerilId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPerilId = Value
			
		End Set
	End Property
	Public Property RecoveryTypeID() As Integer
		Get
			
			Return m_lRecoveryTypeID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lRecoveryTypeID = Value
			
		End Set
	End Property
	
	Public Property InitialReserve() As Decimal
		Get
			Return m_cInitialReserve
		End Get
		Set(ByVal Value As Decimal)
			m_cInitialReserve = Value
		End Set
	End Property
	
	Public Property RevisedReserve() As Decimal
		Get
			Return m_cRevisedReserve
		End Get
		Set(ByVal Value As Decimal)
			m_cRevisedReserve = Value
		End Set
	End Property
	
	Public Property RevisedReserveLoss() As Decimal
		Get
			Return m_cRevisedReserveLoss
		End Get
		Set(ByVal Value As Decimal)
			m_cRevisedReserveLoss = Value
		End Set
	End Property
	
	Public Property ReceiptAmount() As Decimal
		Get
			Return m_cReceiptAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cReceiptAmount = Value
		End Set
	End Property
	
	Public Property ReceiptAmountLoss() As Decimal
		Get
			Return m_cReceiptAmountLoss
		End Get
		Set(ByVal Value As Decimal)
			m_cReceiptAmountLoss = Value
		End Set
	End Property
	
	Public Property InsuranceFileCnt() As Integer
		Get
			Return m_lInsuranceFileCnt
		End Get
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	
	Public Property DisableCurrency() As Boolean
		Get
			Return m_bDisableCurrency
		End Get
		Set(ByVal Value As Boolean)
			m_bDisableCurrency = Value
		End Set
	End Property
	
	Public Property TaxTypeCode() As String
		Get
			Return m_sTaxTypeCode
		End Get
		Set(ByVal Value As String)
			m_sTaxTypeCode = Value
		End Set
	End Property
	
	
	Public Property TaxTypeDesc() As String
		Get
			Return m_sTaxTypeDesc
		End Get
		Set(ByVal Value As String)
			m_sTaxTypeDesc = Value
		End Set
	End Property
	
	Public Property TaxBandDesc() As String
		Get
			Return m_sTaxBandDesc
		End Get
		Set(ByVal Value As String)
			m_sTaxBandDesc = Value
		End Set
	End Property
	

	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property ClaimMode() As Integer
		Get
			Return m_nFindClaimMode
		End Get
		Set(ByVal Value As Integer)
			m_nFindClaimMode = Value
		End Set
	End Property
	
	Public Property NewReserve() As Decimal
		Get
			Return m_cNewreserve
		End Get
		Set(ByVal Value As Decimal)
			m_cNewreserve = Value
		End Set
	End Property
	
	Public Property LossCurrencyId() As Integer
		Get
			Return m_lLossCurrencyId
		End Get
		Set(ByVal Value As Integer)
			m_lLossCurrencyId = Value
		End Set
	End Property
	
	Public Property LossCurrencyDesc() As String
		Get
			Return m_sLossCurrencyDesc
		End Get
		Set(ByVal Value As String)
			m_sLossCurrencyDesc = Value
		End Set
	End Property
	
	Public Property ReceiptCurrencyId() As Integer
		Get
			Return m_lCurrencyId
		End Get
		Set(ByVal Value As Integer)
			m_lCurrencyId = Value
		End Set
	End Property
	
	Public Property ReceiptToLossRate() As Double
		Get
			Return m_dReceiptToLossRate
		End Get
		Set(ByVal Value As Double)
			m_dReceiptToLossRate = Value
		End Set
	End Property
	
	Private Sub cboCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Click
		m_lCurrencyId = cboCurrency.CurrencyId
		
		m_lReturn = GetCurrencyRate()
		
		txtCurrencyRate.Text = CStr(m_dReceiptToLossRate)
		
	End Sub
	

	Private isInitializingComponent As Boolean
	Private Sub cboTaxBand_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxBand.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		m_sTaxBandDesc = cboTaxBand.Text
		
		CalculateTax()
		
	End Sub
	
	Private Sub cboTaxBand_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxBand.SelectionChangeCommitted
		
		m_sTaxBandDesc = cboTaxBand.Text
		
		CalculateTax()
		
	End Sub
	

	Private Sub cboTaxType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxType.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			' Populate tax bands available for selected tax type
			If cboTaxType.SelectedIndex > -1 Then
				FillTaxBandCombo(VB6.GetItemData(cboTaxType, cboTaxType.SelectedIndex))
			Else
				cboTaxBand.Items.Clear()
			End If
			
			CalculateTax()
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	Private Sub cboTaxType_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxType.SelectionChangeCommitted
		
		Try 
			
			' Populate tax bands available for selected tax type
			If cboTaxType.SelectedIndex > -1 Then
				FillTaxBandCombo(VB6.GetItemData(cboTaxType, cboTaxType.SelectedIndex))
			Else
				cboTaxBand.Items.Clear()
			End If
			
			CalculateTax()
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	Private Sub cboTaxType_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboTaxType.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		Try 
			
			' Stop typing
			KeyAscii = 0
		
		Catch 
		End Try
		
		
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Dim bForceLostFocus As Boolean = iPMFunc.ForceLostFocus(cmdOK)
		
		If Not bForceLostFocus Then
			
			If ClaimMode = gPMConstants.PMEComponentAction.PMAdd Then
				txtInitialReserve.Focus()
			Else
				txtRevisedReserve.Focus()
			End If
			
			Exit Sub
		End If
		
		
		Dim bCheckMandatory As Boolean = CheckMandatory()
		If Not bCheckMandatory Then
			Exit Sub
		End If
		
		Dim lRecoveryTypeId As Integer = VB6.GetItemData(cboRecoveryType, cboRecoveryType.SelectedIndex)
        Dim frmInterface As New frmInterface
		'Add Mode
		If g_lButton = ACAddButton Then
			m_lReturn = CheckRecoveryTypeID(lRecoveryTypeId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				DisplayMessage(ACInvalidRecoveryType, Mid(lblRecoveryType.Name, 4))
				
				Exit Sub
			End If
		End If
		
		'Edit Mode
		If g_lButton = ACEditButton Then
			
			If lRecoveryTypeId <> m_lRecoveryTypeID Then
				'Vallidation to Check Recovery Type Id is In frminterface
				m_lReturn = CheckRecoveryTypeID(lRecoveryTypeId)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					DisplayMessage(ACInvalidRecoveryType, Mid(lblRecoveryType.Name, 4))
					
					Exit Sub
				End If
			End If
		End If
		
		If txtInitialReserve.Text.Trim() = Nothing Then
			txtInitialReserve.Text = CStr(0)
		End If
		
		

		m_cInitialReserve = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtInitialReserve.Text))
		
		m_lRecoveryTypeID = lRecoveryTypeId
		
		Select Case ClaimMode
			Case gPMConstants.PMEComponentAction.PMEdit
				
				If txtRevisedReserve.Text.Trim() = Nothing Then
					txtRevisedReserve.Text = CStr(0)
				End If
				If Conversion.Val(txtRevisedReserve.Text) <> 0 Then
					frmInterface.DataChanged = True
				ElseIf cboRecoveryType.Enabled Then 
					frmInterface.DataChanged = True
				End If

				m_cRevisedReserve = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtRevisedReserve.Text))
				
				If txtRevisedReserveLoss.Text.Trim() = Nothing Then
					txtRevisedReserveLoss.Text = CStr(0)
				End If
				

				m_cRevisedReserveLoss = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtRevisedReserveLoss.Text))
				
			Case gPMConstants.PMEComponentAction.PMView
				
				If txtRevisedReserve.Text.Trim() = Nothing Then
					txtRevisedReserve.Text = CStr(0)
				End If
				

				m_cReceiptAmount = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtRevisedReserve.Text))
				
				If txtRevisedReserveLoss.Text.Trim() = Nothing Then
					txtRevisedReserveLoss.Text = CStr(0)
				End If
				

				m_cReceiptAmountLoss = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtRevisedReserveLoss.Text))
				
		End Select
		
		If txtCurrentReserve.Text.Trim() = Nothing Then
			txtCurrentReserve.Text = CStr(0)
		End If
		

		m_cNewreserve = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtCurrentReserve.Text))
		
		Me.Hide()
		
	End Sub
	
	' ***************************************************************** '
	' Name: FormLoad
	'
	' Description: Loads all required details of the form
	'
	' Date:15/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '

	Private Sub frmSalvageDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' Date :15/07/2000
	'
	' Edit History : Pandu
	' MKR 15/02/2005 : PN 18724 : Hiding Fields not required for Broking
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		Dim result As Integer = 0
		Const c_lStart As Integer = 360
		Const c_lStep As Integer = 480
		Const c_lEnd As Integer = 400
		
		Dim lCurrentTop As Integer
		Dim bAllowOverride As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			If Information.IsArray(g_vSalvageRecoveryTypes) Then
				
				'Load Details of Recovery Type in the combo


				LoadDataInCombo(cboRecoveryType, g_vSalvageRecoveryTypes, g_vSalvageRecoveryTypes.GetLowerBound(1), g_vSalvageRecoveryTypes.GetUpperBound(1) + 1)
				
			End If
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Update the interface details with the
			' property members.
			m_lReturn = PropertiesToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Get the claim company id
			m_lReturn = GetClaimCompany()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetClaimCompany failed.")
			End If
			
			lblRecoveryType.Font = VB6.FontChangeBold(lblRecoveryType.Font, True)
			
			txtLossCurrency.Text = m_sLossCurrencyDesc
			
			If ClaimMode = gPMConstants.PMEComponentAction.PMAdd Then
				lblInitialReserve.Font = VB6.FontChangeBold(lblInitialReserve.Font, True)
				
				'Hide controls
				lblCurrentReserve.Visible = False
				txtCurrentReserve.Visible = False
				lblRevisedReserve.Visible = False
				txtRevisedReserve.Visible = False
				lblRevisedReserveLoss.Visible = False
				txtRevisedReserveLoss.Visible = False
				lblLossCurrency.Visible = False
				txtLossCurrency.Visible = False
				lblCurrency.Visible = False
				cboCurrency.Visible = False
				lblCurrencyRate.Visible = False
				txtCurrencyRate.Visible = False
				lblTaxType.Visible = False
				cboTaxType.Visible = False
				lblTaxBand.Visible = False
				cboTaxBand.Visible = False
				lblTaxAmount.Visible = False
				txtTaxAmount.Visible = False
				lblNetPayment.Visible = False
				txtNetPayment.Visible = False
				
				'Position shown controls
				lCurrentTop = c_lStart
				lblRecoveryType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				cboRecoveryType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblInitialReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtInitialReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblLossCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtLossCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
			Else
				'Position shown controls
				lCurrentTop = c_lStart
				lblRecoveryType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				cboRecoveryType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblInitialReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtInitialReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblCurrentReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtCurrentReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblRevisedReserveLoss.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtRevisedReserveLoss.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblRevisedReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtRevisedReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblLossCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtLossCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				
				lCurrentTop += c_lStep
				lblCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
				cboCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblCurrencyRate.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtCurrencyRate.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblTaxType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				cboTaxType.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblTaxBand.Top = VB6.TwipsToPixelsY(lCurrentTop)
				cboTaxBand.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblTaxAmount.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtTaxAmount.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
				lCurrentTop += c_lStep
				lblNetPayment.Top = VB6.TwipsToPixelsY(lCurrentTop)
				txtNetPayment.Top = VB6.TwipsToPixelsY(lCurrentTop)
				
			End If
			
			If (g_lButton <> ACAddButton) And (ClaimMode <> gPMConstants.PMEComponentAction.PMAdd) Then
				cboRecoveryType.Enabled = False
			End If
			
			bAllowOverride = False
			m_lReturn = GetUserAuthorities(r_bAllowOverride:=bAllowOverride)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetUserAuthorities failed")
			End If
			
			If Not bAllowOverride Then
				txtCurrencyRate.Enabled = False
			End If
			
			If m_lCurrencyId = 0 Then
				m_lCurrencyId = m_lLossCurrencyId
			End If
			
			cboCurrency.CompanyId = m_lClaimCompanyID
			cboCurrency.Refresh()
			cboCurrency.CurrencyId = m_lCurrencyId
			
			m_lReturn = GetCurrencyRate()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetCurrencyRate failed")
			End If
			
			txtCurrencyRate.Text = CStr(m_dReceiptToLossRate)
			
			If m_bDisableCurrency Then
				cboCurrency.Enabled = False
			End If
			
			For lLoop As Integer = 0 To cboTaxType.Items.Count - 1
				If VB6.GetItemString(cboTaxType, lLoop) = m_sTaxTypeDesc Then
					cboTaxType.SelectedIndex = lLoop
					Exit For
				End If
			Next 
			
			For lLoop As Integer = 0 To cboTaxBand.Items.Count - 1
				If VB6.GetItemString(cboTaxBand, lLoop) = m_sTaxBandDesc Then
					cboTaxBand.SelectedIndex = lLoop
					Exit For
				End If
			Next 
			
			'Resize the form to fit all shown controls
			lCurrentTop += c_lEnd
			Me.Height = VB6.TwipsToPixelsY(lCurrentTop + 1725)
			cmdCancel.Top = VB6.TwipsToPixelsY(lCurrentTop + 900)
			cmdOK.Top = VB6.TwipsToPixelsY(lCurrentTop + 900)
			tabMainTab.Height = VB6.TwipsToPixelsY(lCurrentTop + 675)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' Date :14/08/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitleGeneral, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            lblRecoveryType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRecoveryType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblInitialReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then

                lblRevisedReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageRevisedReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Else

                lblRevisedReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageSalvageAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            End If


            lblCurrentReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrentReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    txtInitialReserve.Enabled = True

                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView

                    txtInitialReserve.Enabled = False
                    txtRevisedReserve.Enabled = True
                    txtCurrentReserve.Enabled = False

                    txtInitialReserve.BackColor = SystemColors.Control
                    txtCurrentReserve.BackColor = SystemColors.Control

            End Select



            If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then

                lblRevisedReserveLoss.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisedReserveLoss, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Else

                lblRevisedReserveLoss.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageRevisedReserveLoss, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            End If


            lblLossCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceiptCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblCurrencyRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceiptToLossRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblTaxType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTaxType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblTaxBand.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTaxBand, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblTaxAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTaxAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblNetPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNetAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.


            If Not False Or Not (Convert.IsDBNull(m_lRecoveryTypeID) Or IsNothing(m_lRecoveryTypeID)) Then


                For ncount As Integer = 0 To cboRecoveryType.Items.Count - 1

                    If m_lRecoveryTypeID = VB6.GetItemData(cboRecoveryType, ncount) Then
                        cboRecoveryType.SelectedIndex = ncount
                        Exit For
                    End If

                Next

            End If


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    txtInitialReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve))

                Case gPMConstants.PMEComponentAction.PMEdit

                    txtInitialReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve))
                    txtRevisedReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve))

                    txtCurrentReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cNewreserve))

                Case gPMConstants.PMEComponentAction.PMView

                    txtInitialReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve))
                    txtRevisedReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cReceiptAmount))

                    txtCurrentReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cNewreserve))

            End Select


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadDataInCombo
    '
    ' Description: Fills the data from variant array into combobox
    '               INPUTS : Combo Control to be filled
    '                       2D - Array Containing the Record values
    '                       Index in the Array where the Records of the
    '                           Table Start from
    '                       Number of records to enter
    ' ***************************************************************** '
    Private Function LoadDataInCombo(ByRef cboControl As ComboBox, ByVal vntData(,) As Object, ByVal vnStart As Integer, ByVal vnCount As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether an array has been passed
            If Information.IsArray(vntData) Then

                'clear the combobox
                cboControl.Items.Clear()

                'Load the data from the Array to the combobox
                For lCount As Integer = vnStart To vnStart + vnCount - 1

                    Dim cboControl_NewIndex As Integer = -1

                    cboControl_NewIndex = cboControl.Items.Add(CStr(vntData(1, lCount)))

                    VB6.SetItemData(cboControl, cboControl_NewIndex, CInt(vntData(0, lCount)))

                Next lCount

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load data in combobox", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDataInCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Boolean

        Dim result As Boolean = False
        Try


            If cboRecoveryType.Text = "" Then ' RecoveryType Combo box
                DisplayMessage(ACMandatoryFieldMsg, Mid(lblRecoveryType.Name, 4))
                Return False
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    If txtInitialReserve.Text = "" Then ' txtInitialReserve text box
                        DisplayMessage(ACMandatoryFieldMsg, Mid(lblInitialReserve.Name, 4))
                        Return False
                    Else
                        '   If all the Mandatory fields are having values SET the CheckMandatory = True
                        result = True
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for Mandatory Fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         DisplayMessage
    '
    ' Description:  This function is used to display he Error Messages for this Form.
    '               We are passing two parameters MessageCount which is the
    '               Constant defined in the Resource file
    '                The Title is the Error Message Text for the same.
    '
    ' ***************************************************************** '

    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.


            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString))

            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmSalvageDetails_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ''*****Start of Code change Internal Bug 8


        'If Me.Width < 5925 Then Me.Width = 5925
        'If Me.Height < 4080 Then Me.Height = 4080

        '*****Start of Code change Internal Bug 8

    End Sub

    Private Sub frmSalvageDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If

        If Not (m_oInsuranceFile Is Nothing) Then

            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If

        If Not (m_oUserAuthorities Is Nothing) Then

            m_oUserAuthorities.Dispose()
            m_oUserAuthorities = Nothing
        End If
    End Sub

    Private Sub txtCurrencyRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim dbNumericTemp As Double
        If Double.TryParse(txtCurrencyRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            m_dReceiptToLossRate = CDbl(txtCurrencyRate.Text)

            txtRevisedReserve_TextChanged(txtRevisedReserve, New EventArgs())
        End If
    End Sub

    Private Sub txtCurrentReserve_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrentReserve.Enter

        iPMFunc.SelectText(txtCurrentReserve)

    End Sub

    Private Sub txtInitialReserve_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialReserve.Enter
        iPMFunc.SelectText(txtInitialReserve)
    End Sub

    Private Sub txtInitialReserve_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtInitialReserve.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)


        If KeyAscii <> 46 Then

            If KeyAscii <> 8 Then
                If KeyAscii <> 32 Then

                    If KeyAscii < 48 Or KeyAscii > 57 Then

                        KeyAscii = 0

                        DisplayMessage(ACInvalidPositiveNumbers, Mid(lblInitialReserve.Name, 4))

                    End If
                End If

            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtInitialReserve_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialReserve.Leave

        If txtInitialReserve.Text <> "" Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtInitialReserve.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                DisplayMessage(ACInvalidCurrencyMsg, Mid(lblInitialReserve.Name, 4))

                txtInitialReserve.Text = ""
                txtInitialReserve.Focus()

            Else
                txtInitialReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtInitialReserve.Text.Trim())
            End If
        End If

    End Sub

    Private Sub txtRevisedReserve_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRevisedReserve.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim cPaymentAmount, cLossAmount As Decimal
        Dim dPaymentLossRate As Double

        Dim dbNumericTemp As Double
        If Double.TryParse(txtRevisedReserve.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            cPaymentAmount = CDec(txtRevisedReserve.Text)
        End If

        Dim dbNumericTemp2 As Double
        If Double.TryParse(txtCurrencyRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            dPaymentLossRate = CDbl(txtCurrencyRate.Text)
        End If

        If cPaymentAmount <> 0 And dPaymentLossRate <> 0 Then
            cLossAmount = cPaymentAmount * dPaymentLossRate
        End If

        txtRevisedReserveLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cLossAmount), 2)

        CalculateTax()
    End Sub

    Private Sub txtRevisedReserve_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRevisedReserve.Enter
        iPMFunc.SelectText(txtRevisedReserve)

        txtRevisedReserve.Tag = txtRevisedReserve.Text
    End Sub

    Private Sub txtRevisedReserve_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRevisedReserve.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then

            If KeyAscii <> 45 Then

                If KeyAscii <> 46 Then

                    If KeyAscii <> 8 Then
                        If KeyAscii <> 32 Then

                            If KeyAscii < 48 Or KeyAscii > 57 Then

                                KeyAscii = 0
                                DisplayMessage(ACInvalidNumberMsg, Mid(lblRevisedReserve.Name, 4))

                            End If
                        End If

                    End If
                End If

            End If

        End If

        'RWH(27/06/01) Removed section below to allow -ve numbers.
        ' If ClaimMode = PMView Then
        '     If (KeyAscii <> 46) Then
        '            If (KeyAscii <> 8) Then
        '                If (KeyAscii <> 32) Then
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidPositiveNumbers, "Salvage Amount")
        '                    End If
        '                End If
        '            End If
        '    End If
        'End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtRevisedReserve_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRevisedReserve.Leave

        Dim cTempCurrentReserve As Decimal
        Dim sInfoMessage As String = ""
        Dim iReply As DialogResult

        If txtRevisedReserve.Text <> "" Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtRevisedReserve.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                DisplayMessage(ACInvalidCurrencyMsg, Mid(lblInitialReserve.Name, 4))

                txtRevisedReserve.Text = ""

                txtRevisedReserve.Focus()

                Exit Sub

            End If

        End If
        If txtInitialReserve.Text.Trim() = Nothing Then
            txtInitialReserve.Text = CStr(0)
        End If

        If txtRevisedReserve.Text.Trim() = Nothing Then
            txtRevisedReserve.Text = CStr(0)
            txtRevisedReserve.Tag = CStr(0)
        End If

        If txtCurrentReserve.Text.Trim() = Nothing Then
            txtCurrentReserve.Text = CStr(0)
        End If


        Select Case ClaimMode
            Case gPMConstants.PMEComponentAction.PMEdit


                cTempCurrentReserve = CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtCurrentReserve.Text)) + CDec(txtRevisedReserve.Text.Trim()) ' - |
                txtCurrentReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cTempCurrentReserve))

                txtRevisedReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtRevisedReserve.Text.Trim())

            Case gPMConstants.PMEComponentAction.PMView

                iReply = System.Windows.Forms.DialogResult.Yes

                'don't validate if we haven't change anything
                If Convert.ToString(txtRevisedReserve.Tag) = txtRevisedReserve.Text Then
                    Exit Sub
                End If

                'TN20010628 - change txtInitialReserve to txtCurrentReserve

                If CDbl(CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=txtCurrentReserve.Text)) - CDec(txtRevisedReserve.Text.Trim())) < 0 Then

                    'AJM 25/04/01 display info message and get confirmation if salvage amount
                    '             greater than initial reserve

                    sInfoMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACInvalidSalvageAmount, gPMConstants.PMEResourseFileDataType.PMResString))

                    iReply = MessageBox.Show(sInfoMessage, "Salvage Amount", MessageBoxButtons.YesNo)

                End If



                If iReply <> System.Windows.Forms.DialogResult.Yes Then
                    txtRevisedReserve.Focus()
                    Exit Sub

                Else

                    'RWH(25/06/01) Update to use Current Reserve instead of Initial to recalculate Current.
                    txtCurrentReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(CDec(txtCurrentReserve.Text.Trim()) - CDec(txtRevisedReserve.Text.Trim())))

                    txtRevisedReserve.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtRevisedReserve.Text.Trim())

                End If

        End Select

    End Sub
	
	Public Function CheckRecoveryTypeID(ByRef lRecoveryTypeId As Integer) As Integer
		
        Dim result As Integer = 0
        Dim frmInterface As New frmInterface
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If frmInterface.lvwRecovery.Items.Count > 0 Then
			
			
			For ncount As Integer = 1 To frmInterface.lvwRecovery.Items.Count
				

				If lRecoveryTypeId = CDbl(m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(frmInterface.lvwRecovery.Items.Item(ncount - 1).Tag))) Then
					
					result = gPMConstants.PMEReturnCode.PMFalse
					
					Exit For
					
				End If
				
			Next ncount
			
		End If
		
		Return result
	End Function
	
	'********************************************************************************
	' Name:         FillTaxTypeCombo
	' Author:       Alix Bergeret
	' Date:         14/05/2003
	' Description:  -
	'********************************************************************************
	Public Function FillTaxTypeCombo(ByRef r_oBusiness As Object) As Integer
		
		Dim result As Integer = 0
		Dim sPrevious As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Empty lists
			cboTaxType.Items.Clear()
			cboTaxBand.Items.Clear()
			
			' Get items from DB

			m_lReturn = r_oBusiness.GetTaxTypesTaxBands(m_vTaxArray)
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
					If sPrevious <> CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim() Then
						' Add tax type description
						Dim cboTaxType_NewIndex As Integer = -1
						cboTaxType_NewIndex = cboTaxType.Items.Add(CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim())
						' Add tax type ID
						VB6.SetItemData(cboTaxType, cboTaxType_NewIndex, CInt(m_vTaxArray(PMTaxArray_TaxTypeID, lIndex)))
					End If
					sPrevious = CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim()
				Next lIndex
			ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then 
				
			Else
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load tax types and bands from DB.", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxTypeCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillTaxTypeCombo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxTypeCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	'********************************************************************************
	' Name:         FillTaxBandCombo
	' Author:       Alix Bergeret
	' Date:         15/05/2003
	' Description:  -
	'********************************************************************************
	Private Function FillTaxBandCombo(ByVal v_lTaxTypeID As Integer) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Empty list
			cboTaxBand.Items.Clear()
			
			' Loop through array
			If Information.IsArray(m_vTaxArray) Then
				For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
					If v_lTaxTypeID = gPMFunctions.NullToLong(CStr(m_vTaxArray(PMTaxArray_TaxTypeID, lIndex))) Then
						' Add tax type description
						Dim cboTaxBand_NewIndex As Integer = -1
						cboTaxBand_NewIndex = cboTaxBand.Items.Add(CStr(m_vTaxArray(PMTaxArray_TaxBandDesc, lIndex)).Trim())
						' Add tax type ID
						VB6.SetItemData(cboTaxBand, cboTaxBand_NewIndex, CInt(m_vTaxArray(PMTaxArray_TaxBandID, lIndex)))
						
						m_sTaxTypeCode = CStr(m_vTaxArray(PMTaxArray_TaxTypeCode, lIndex))
						m_sTaxTypeDesc = CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex))
					End If
				Next lIndex
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillTaxBandCombo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxBandCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	'********************************************************************************
	' Name:         CalculateTax
	' Author:       Alix Bergeret
	' Date:         15/05/2003
	' Description:  -
	'********************************************************************************
	Private Function CalculateTax() As Integer
		
		Dim result As Integer = 0
		Dim cNetPayment, cThisPayment As Decimal
		Dim iIsValue As Integer
		Dim dRate As Double
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' First we check we have all the needed values
			Dim dbNumericTemp As Double
			If txtRevisedReserve.Text.Trim() = "" Or Not Double.TryParse(txtRevisedReserve.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				m_cTaxAmount = 0
				cNetPayment = 0
				txtTaxAmount.Text = CStr(m_cTaxAmount)
				txtNetPayment.Text = CStr(cNetPayment)
				Return result
			End If
			If cboTaxType.SelectedIndex = -1 Or cboTaxBand.SelectedIndex = -1 Then
				m_cTaxAmount = 0
				cNetPayment = 0
				txtTaxAmount.Text = CStr(m_cTaxAmount)
				txtNetPayment.Text = CStr(cNetPayment)
				Return result
			End If
			
			' Get this payment value
			cThisPayment = CDec(txtRevisedReserve.Text)
			
			' Get rate from array
			If Information.IsArray(m_vTaxArray) Then
				For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
					If VB6.GetItemData(cboTaxBand, cboTaxBand.SelectedIndex) = CDbl(m_vTaxArray(PMTaxArray_TaxBandID, lIndex)) Then
						iIsValue = gPMFunctions.NullToInteger(CStr(m_vTaxArray(PMTaxArray_TaxIsValue, lIndex)))
						dRate = gPMFunctions.NullToDouble(CStr(m_vTaxArray(PMTaxArray_TaxRate, lIndex)))
						Exit For
					End If
				Next lIndex
			End If
			
			' Calculate tax
			If iIsValue = 1 Then
				m_cTaxAmount = dRate
			Else
				m_cTaxAmount = cThisPayment * dRate / 100
			End If
			
			' Work out net payment
			cNetPayment = cThisPayment - m_cTaxAmount
			
			' Display tax amount and net payment
			txtTaxAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cTaxAmount))
			
			txtNetPayment.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cNetPayment))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateTax failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateTax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	'Get the current currency rate for converting from payment currency to loss currency.
	Private Function GetCurrencyRate() As Integer
		Dim result As Integer = 0
		Dim dPaymentRate, dLossRate As Double
		Dim sLossCurrencyDesc As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get business object
			m_lReturn = CreateCurrencyConvert()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateCurrencyConvert failed.")
			End If
			
			'Get payment rate to base

			m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=m_lCurrencyId, v_lCompanyID:=m_lClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dPaymentRate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
			End If
			
			'Get loss rate to base

			m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=m_lLossCurrencyId, v_lCompanyID:=m_lClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dLossRate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
			End If
			
			'Calculate payment rate to loss
			m_dReceiptToLossRate = dPaymentRate / dLossRate
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyRate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Function CreateCurrencyConvert() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oCurrencyConvert Is Nothing Then
				'Get Currency Convert Object.
				Dim temp_m_oCurrencyConvert As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
				m_oCurrencyConvert = temp_m_oCurrencyConvert
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create instance of bACTCurrencyConvert.Form")
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCurrencyConvert failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCurrencyConvert", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private Function GetClaimCompany() As Integer
		Dim result As Integer = 0
		Dim vInsuranceFile As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get insurance file object
			m_lReturn = CreateInsuranceFile()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateInsuranceFile failed.")
			End If
			
			'Get company id from insurance file

			m_lReturn = m_oInsuranceFile.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetDetails failed.")
			End If
			

			m_lReturn = m_oInsuranceFile.GetNext(r_vFieldArray:=vInsuranceFile)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetNext failed.")
			End If
			

            m_lClaimCompanyID = CInt(vInsuranceFile(5))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCompany failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Function CreateInsuranceFile() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oInsuranceFile Is Nothing Then
				'Get Insurance File Object.
				Dim temp_m_oInsuranceFile As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
				m_oInsuranceFile = temp_m_oInsuranceFile
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create instance of bSIRInsuranceFile.Business")
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateInsuranceFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInsuranceFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Function GetUserAuthorities(ByRef r_bAllowOverride As Boolean) As Integer
		Dim result As Integer = 0
		Dim vOverrideDate, vOverrideRate As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get UserAuthorities business object
			m_lReturn = CreateUserAuthorities()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, CreateUserAuthorities")
			End If
			
			'Get authority details for current user.

			m_lReturn = m_oUserAuthorities.GetDetails(vUserID:=g_oObjectManager.UserID)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, m_oUserAuthorities.GetDetails")
			End If
			
			'Get override options for current user.

			m_lReturn = m_oUserAuthorities.GetNext(vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, m_oUserAuthorities.GetNext")
			End If
			
			'If CInt(vOverrideDate) = 1 Or CInt(vOverrideRate) = 1 Then

			r_bAllowOverride = CInt(vOverrideRate) = 1
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Function CreateUserAuthorities() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oUserAuthorities Is Nothing Then
				Dim temp_m_oUserAuthorities As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
				m_oUserAuthorities = temp_m_oUserAuthorities
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create bACTUserAuthorities.Business")
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUserAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Class
