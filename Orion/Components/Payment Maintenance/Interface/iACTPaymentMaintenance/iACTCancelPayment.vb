Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmCancelPayment
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify
	Private Const ACClass As String = "frmCancelPayment"
	
	Private m_oGeneral As iACTPaymentMaintenance.General
	
	Public m_vCancelArray( ,  ) As Object
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	Private m_vExistingCancelPayment As Object
	'Property Global Variable
	Private m_lTransDetailID As Integer
	Private m_crAmount As Decimal
	'PN: 45889
	Private m_sMediaRef As String = ""
	Private m_sBankAccountNo As String = ""
	Private m_sPolicyHolder As String = ""
	Private m_dtPolicyDate As Date
	Private m_sMediaType As String = ""
	Private m_sBankSortCode As String = ""
	Private m_sDocumentRef As String = ""
	Private m_sClientCode As String = ""
    Private m_lCashListItemID As Integer
    Private m_lPartyCnt As Integer
    'developer guide no. 101
    Private m_vInsuranceFileCnt As Object
	Private m_lClaimId As Integer
	Private m_lEventTypeId As Integer
	Private m_lTransCurrencyId As Integer
	Private m_iHasPaymentAuthority As Integer
	Private m_lPaymentCurrency As Integer
	Private m_crPaymentAmount As Decimal
	Private m_crAmt As Decimal
	Private m_sPolicyAndClaimRef As String = ""
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_oPMUser As Object
	
	Private m_vDocumentIds As Object
	Private m_lAccountId As Integer
	Private m_vTransIds As Object
	
	'Properties Start
	
	Public Property ExistingCancelPayment() As Object
		Get
			Return m_vExistingCancelPayment
		End Get
		Set(ByVal Value As Object)


			m_vExistingCancelPayment = Value
		End Set
	End Property
	
	
	Public Property TransDetailID() As Integer
		Get
			Return m_lTransDetailID
		End Get
		Set(ByVal Value As Integer)
			m_lTransDetailID = Value
		End Set
	End Property
	
	Public WriteOnly Property Amount() As Decimal
		Set(ByVal Value As Decimal)
			m_crAmount = Value
		End Set
	End Property
	
	'PN: 45889
	Public WriteOnly Property MediaRef() As String
		Set(ByVal Value As String)
			m_sMediaRef = Value
		End Set
	End Property
	
	'PN: 45889
	Public WriteOnly Property BankAcccountNo() As String
		Set(ByVal Value As String)
			m_sBankAccountNo = Value
		End Set
	End Property
	
	Public WriteOnly Property PolicyHolder() As String
		Set(ByVal Value As String)
			m_sPolicyHolder = Value
		End Set
	End Property
	
	Public WriteOnly Property PaymentDate() As Date
		Set(ByVal Value As Date)
			m_dtPolicyDate = Value
		End Set
	End Property
	
	Public WriteOnly Property MediaType() As String
		Set(ByVal Value As String)
			m_sMediaType = Value
		End Set
	End Property
	
	Public WriteOnly Property BankSortCode() As String
		Set(ByVal Value As String)
			m_sBankSortCode = Value
		End Set
	End Property
	
	Public WriteOnly Property DocumentRef() As String
		Set(ByVal Value As String)
			m_sDocumentRef = Value
		End Set
	End Property
	
	Public WriteOnly Property ClientCode() As String
		Set(ByVal Value As String)
			m_sClientCode = Value
		End Set
	End Property
	
	
	Public Property CashListItemID() As String
		Get
            Return CStr(m_lCashListItemID)
		End Get
		Set(ByVal Value As String)
            m_lCashListItemID = CInt(Value)
		End Set
	End Property
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
    'developer guide no. 101
    Public WriteOnly Property InsuranceFileCnt() As Object
        Set(ByVal Value As Object)
            m_vInsuranceFileCnt = Value
        End Set
    End Property
	Public WriteOnly Property ClaimId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimId = Value
		End Set
	End Property
	
	Public WriteOnly Property EventTypeId() As Integer
		Set(ByVal Value As Integer)
			m_lEventTypeId = Value
		End Set
	End Property
	
	Public WriteOnly Property TransCurrencyId() As Integer
		Set(ByVal Value As Integer)
			m_lTransCurrencyId = Value
		End Set
	End Property
	
	Public WriteOnly Property HasPaymentAuthority() As Integer
		Set(ByVal Value As Integer)
			m_iHasPaymentAuthority = Value
		End Set
	End Property
	
	Public WriteOnly Property PaymentCurrency() As Integer
		Set(ByVal Value As Integer)
			m_lPaymentCurrency = Value
		End Set
	End Property
	
	Public WriteOnly Property PaymentAmount() As Decimal
		Set(ByVal Value As Decimal)
			m_crPaymentAmount = Value
		End Set
	End Property
	
	
	Public WriteOnly Property PolicyClaimRef() As String
		Set(ByVal Value As String)
			m_sPolicyAndClaimRef = Value
		End Set
	End Property
	
	Public Function FillCancelGrid() As Integer
		
		Dim result As Integer = 0
        Dim lTransDetailID As Long
		Dim oListItem As ListViewItem
		Dim sPolicyClaimNumber, dAmount, sDocRef, sDocType As String
		Dim dtTransactionDate As Date
		
		
        Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
        lTransDetailID = gPMFunctions.ToSafeLong(TransDetailID)
		

        m_lReturn = g_oBusiness.FillCancelPaymentGrid(v_lTransDetailID:=lTransDetailID, vResultArray:=m_vCancelArray)
		
		' Clear the search details.
		lvwCancelPayment.Items.Clear()
		
		' Check that search details are valid before
		' continuing.
		If Not Information.IsArray(m_vCancelArray) Then
			result = gPMConstants.PMEReturnCode.PMFalse
		End If
		
		' Assign the details to the interface.
		
		For lRow As Integer = m_vCancelArray.GetLowerBound(1) To m_vCancelArray.GetUpperBound(1)
			
			' Assign details to other the columns
			sPolicyClaimNumber = m_sPolicyAndClaimRef
			'                If sPolicyClaimNumber <> "" Then
			'                    sPolicyClaimNumber = sPolicyClaimNumber & "/ " & ToSafeString(m_vCancelArray(ACCancelPaymentPolicyClaimNo, lRow&))
			'                Else
			'                    sPolicyClaimNumber = ToSafeString(m_vCancelArray(ACCancelPaymentPolicyClaimNo, lRow&))
			'                End If
			
			oListItem = lvwCancelPayment.Items.Add(sPolicyClaimNumber.Trim())
			
            dAmount = gPMFunctions.ToSafeDouble(m_vCancelArray(ACCancelPaymentAmount, lRow))
			ListViewHelper.GetListViewSubItem(oListItem, kCancelPaymentColHIndexAmount).Text = dAmount
			
			sDocRef = gPMFunctions.ToSafeString(m_vCancelArray(ACCancelPaymentDocumentRef, lRow))
			ListViewHelper.GetListViewSubItem(oListItem, kCancelPaymentColHIndexDocumentRef).Text = sDocRef.Trim()
			
			sDocType = gPMFunctions.ToSafeString(m_vCancelArray(ACCancelPaymentDocumentType, lRow))
			ListViewHelper.GetListViewSubItem(oListItem, kCancelPaymentColHIndexDocumentType).Text = sDocType.Trim()
            dtTransactionDate = gPMFunctions.ToSafeDate(m_vCancelArray(ACCancelPaymentTransactionDate, lRow))
            ListViewHelper.GetListViewSubItem(oListItem, kCancelPaymentColHIndexTransactionDate).Text = String.Format("{0: dd/MM/yyyy} ", dtTransactionDate)

            oListItem.Tag = lRow

			
		Next lRow
		
		
        Catch ex As Exception
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="FillCancelGrid", r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
		
        Finally
'		Return result
'		Resume 
		
'		Return result
        End Try
        Return result
	End Function
	
	Public Function FillProperties() As Integer
		
		Dim result As Integer = 0
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'MsgBox (g_sUsername)
		txtClientCode.Text = m_sClientCode
		txtAmount.Text = CStr(m_crAmount)
		txtMediaRef.Text = m_sMediaRef
		txtBankAccNo.Text = m_sBankAccountNo
		txtPolicyHolder.Text = m_sPolicyHolder
        txtPaymentDate.Text = String.Format("{0: dd/MM/yyyy} ", m_dtPolicyDate)
		txtMediaType.Text = m_sMediaType
		txtBankSortCode.Text = m_sBankSortCode
		txtDocRef.Text = m_sDocumentRef
		
		
		Catch ex As Exception
		
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="FillProperties", r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
		
		Finally
'		Return result
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
	
	Private Sub cboCancelledReason_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCancelledReason.Click
		cmdOK.Enabled = Not (cboCancelledReason.ItemId < 1)
	End Sub
	


	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Click event of the Cancel button.
		
		Try
		
		If MessageBox.Show("Do you really wish to cancel?", "Cancel Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Me.Hide()
		End If
		

		
		Catch ex As Exception
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
		
		Finally

		End Try
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
        Dim lTransDetailID, lCashListItemID As Long
        Dim lReverseReasonID As Integer
		Dim lEventCnt As Integer
		Dim crPaymentAmt As Decimal
		Dim lRevTransDetailID, lRow As Integer
		Dim crTransAmt, crPayAmt As Decimal
		
		Try
		
		
		cmdOK.Enabled = False
		cmdCancel.Enabled = False
		
		If cboCancelledReason.ListIndex < 1 Then
			
			MessageBox.Show("Their should be atleast one Cancelled Reason", "Cancel Payment", MessageBoxButtons.OK)
			Exit Sub
		End If
		
		If MessageBox.Show("Are you sure that you want to reverse any allocation and cancel the payment?", "Cancel Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' If user has Payment Authority
			If m_iHasPaymentAuthority = gPMConstants.PMEReturnCode.PMTrue Then
				
				crTransAmt = m_crAmount
				If crTransAmt < 0 Then
					crTransAmt = -(m_crAmount)
				End If
				
				' Do Conversion of Currency

				m_lReturn = g_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=m_lTransCurrencyId, v_crCurrencyAmountFrom:=crTransAmt, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=m_lPaymentCurrency, r_crCurrencyAmountTo:=crPayAmt)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'Raise Error.
					gPMFunctions.RaiseError(v_sSource:="cmdOK_Click", v_sDescription:="Currency Conversion Failed")
					Exit Sub
				End If
				
				' User Authority allow Payment Cancel Amount
				crPaymentAmt = m_crPaymentAmount
				
				' Checks Transaction Converted amount with
				' User Authority allow Payment Amount
				If crPayAmt > crPaymentAmt Then
					MessageBox.Show("User has exceeded the maximum Payment Reversal Amount " &  _
					                Strings.Chr(13) & Strings.Chr(10) & "Please refer to System Administrator", "Cancel Payment", MessageBoxButtons.OK)
					Exit Sub
				End If
				
			End If
			' else he is allowed to cancel any amount
			
			' Payment Reversal call
			m_lReturn = PaymentReversal()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Document is already Reversed", "Cancel Payment", MessageBoxButtons.OK)
				Exit Sub
			End If
			
			lTransDetailID = gPMFunctions.ToSafeLong(TransDetailID)
			lReverseReasonID = cboCancelledReason.ItemId
			lCashListItemID = gPMFunctions.ToSafeLong(CInt(CashListItemID))
			
			'Update CashListItem

            m_lReturn = g_oBusiness.SetCashListItemFlags(v_lCashlistitem_id:=lCashListItemID, v_dtReversed_date:=DateTime.Now, v_iCashlistitem_reverse_pmuser_id:=g_iUserID, v_lCashlistitem_reverse_reason_id:=lReverseReasonID, v_lcashlistitem_reversal_transdetail_id:=lTransDetailID)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Raise Error
				gPMFunctions.RaiseError("cmd_OK", "Failed to Update CashListItem")
				Exit Sub
			End If
			
			'Creating Event
			If m_vInsuranceFileCnt = 0 Then

				m_vInsuranceFileCnt = Nothing
			End If
			m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=m_lPartyCnt, v_lEventTypeId:=m_lEventTypeId, v_dtEventDate:=DateTime.Today, v_vInsuranceFileCnt:=m_vInsuranceFileCnt, v_vClaimCnt:=m_lClaimId, v_vDescription:="Cancel Payment" & " - " & m_sDocumentRef), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Raise Error
				gPMFunctions.RaiseError("cmd_OK", "Failed to Create Event")
				Exit Sub
			End If
			
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			cmdOK.Enabled = True
			cmdCancel.Enabled = True
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

			Me.Hide()
			
		End If
		

		
		Catch ex As Exception
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
		
		Finally
		cmdOK.Enabled = True
		cmdCancel.Enabled = True
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		Try
		
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Initialise the error number value.
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
		
		' Create an instance of the general interface object.
		m_oGeneral = New iACTPaymentMaintenance.General()
		
		' Set the interface status to cancelled. This is done
		' so that any interface termination will be noted
		' as cancelled except in the event of accepting
		' the interface.
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        'developer guide no. 38
        Me.cboCancelledReason.FirstItem = ""

		
		Catch ex As Exception
		' Error Section
		
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
		
		Finally
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
	End Sub
	
	

	Private Sub frmCancelPayment_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		' Forms load event.
		
		Try
		
		
		' Check if we have had an error so far.
		' Possibly creating the business object.
		If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
			' We have already encountered an error,
			' so we MUST exit now.
			Exit Sub
		End If
		
		m_lReturn = FillProperties()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			Exit Sub
		End If
		
		cboCancelledReason.FirstItem = "Select Cancel Reason"
		cmdOK.Enabled = False
		

		
		Catch ex As Exception
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
		
		Finally
		

		End Try
	End Sub
	
	Private Function PaymentReversal() As Integer
		
		Dim result As Integer = 0
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = ReverseCashListItem()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMError
                Return result
		End If
		

		
		Catch ex As Exception
		
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="PaymentReversal", r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
		
		Finally


		

		End Try
		Return result
	End Function
	
	Private Function ReverseCashListItem() As Integer
		Dim result As Integer = 0
		Dim sTransDetailID As Integer

        Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'PN: 45966
		Dim temp_g_oACTDocumentReversal As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_g_oACTDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:="ClientManager")
		g_oACTDocumentReversal = temp_g_oACTDocumentReversal
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'Raise Error.
			gPMFunctions.RaiseError(v_sSource:="Initialise", v_sDescription:="Failed to initilise bACTDocumentReversal.Business")
                Return result
		End If
		
		sTransDetailID = gPMFunctions.ToSafeLong(TransDetailID)

		g_oACTDocumentReversal.TransDetailID = sTransDetailID

        g_oACTDocumentReversal.IsCashlistItemReversal = True
            'g_oACTDocumentReversal.CallingAppName = "iACTPaymentMaintenance"

            m_lReturn = g_oACTDocumentReversal.Start
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			result = gPMConstants.PMEReturnCode.PMError
                Return result
		End If
		

		
		Catch ex As Exception
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ReverseCashListItem", r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
		
		Finally


		

		End Try
		Return result
	End Function
	










    'developer guide no. 119
    'Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = DBNull.Value, Optional ByVal v_vInsuranceFileCnt As Object = DBNull.Value, Optional ByVal v_vClaimCnt As Object = DBNull.Value, Optional ByVal v_vDocumentCnt As Object = DBNull.Value, Optional ByVal v_vOldAddressCnt As Object = DBNull.Value, Optional ByVal v_vNewAddressCnt As Object = DBNull.Value, Optional ByVal v_vCampaignId As Object = DBNull.Value, Optional ByVal v_vDocumentTypeId As Object = DBNull.Value, Optional ByVal v_vReportTypeId As Object = DBNull.Value, Optional ByVal v_vDescription As Object = DBNull.Value) As Integer
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateEvent"
        Dim lReturn As Integer
        Dim oEvent As bSIREvent.Business
        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If oEvent Is Nothing Then
            oEvent = New bSIREvent.Business()
            'g_oBusiness is not an dpmdao.database type object and causing error
            'm_lReturn = oEvent.Initialise(sUsername:=g_sUsername.Value, sPassword:="", iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=g_oBusiness)
            m_lReturn = oEvent.Initialise(sUsername:=g_sUsername.Value, sPassword:="", iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=0, sCallingAppName:=ACApp)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oEvent.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return result
            End If
        End If

        m_lReturn = oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=g_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost

        iPMFunc.LogError(v_sUsername:=g_sUsername.Value, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    'developer guide no.191
    'Private Sub cboCancelledReason_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCancelledReason.KeyDown
    Private Sub cboCancelledReason_KeyDown(ByVal Sender As System.Object, ByVal e As PMLookupControl.cboPMLookup.KeyDownEventArgs) Handles cboCancelledReason.KeyDown
        'Dim KeyCode As Integer = EventArgs.KeyCode
        'Dim Shift As Integer = EventArgs.KeyData \ &H10000
        Dim KeyCode As Integer = e.KeyCode
        Dim Shift As Integer = e.KeyCode \ &H10000
        cmdOK.Enabled = Not (cboCancelledReason.ItemId < 1)
    End Sub
End Class
