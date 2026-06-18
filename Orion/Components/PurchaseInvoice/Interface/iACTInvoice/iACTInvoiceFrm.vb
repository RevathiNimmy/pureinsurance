Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 29/10/1998
	'
	' Description: Main interface.
	'
	' Edit History:
	' ******************************************************************'
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	'********************************************************************
	'Maximum number of rows for Spread Control
	Private Const ACSpreadMaxCol As Integer = 3
	
	Private Const ACEnabledTextColor As Integer = &H80000005
	Private Const ACDisabledTextColor As Integer = &H8000000F
	
	'********************************************************************
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	'********************************************************************
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	' {* USER DEFINED CODE (Begin) *}
	
	'Invoice Details
	Private m_lInvoiceID As Integer
	Private m_sInvoiceNumber As New FixedLengthString(40)
	Private m_dInvoiceValue As Double
	Private m_dtInvoiceDate As Date
	Private m_sOrderNo As New FixedLengthString(40)
	Private m_sDescription As New FixedLengthString(255)
	Private m_lSupplierID As Integer
    Private m_sCode As New FixedLengthString(30)
	Private m_sReference As New FixedLengthString(40)
	
	'DC020806 added new transdetail type id
	Private m_iTransdetailTypeId As Integer
	
	' CTAF 310800
	Private m_lBranchID As Integer
	
	' CF120199
	Private m_lNominalID As Integer
	Private m_sNominalCode As String = ""
	'PN5508
	Private m_lCurrencyId As Integer
	'Tomo220199
	Private m_lSupplierIdRef As Integer
	
	'Invoice Item Array
	Private m_vInvoiceItem As Object
	
	'Flag to indicate if the invoice item data has changed
	Private m_bInvoiceItemChange As Boolean
	
	'Flag to indicate that the invoice items are being set up
	Private m_bSpreadSetup As Boolean
	
	'Next available row for data entry in Spread
	Private m_lSpreadRows As Integer
	
	'Document Inputs
	Private m_vDocumentInputs() As Object
	
	' CTAF 100400
	Private m_vItems( ,  ) As Object
	'Datasure modified to return calculated tax
	Private Const ACArrayDescription As Integer = 0
	Private Const ACArrayNominalID As Integer = 1
	Private Const ACArrayAmount As Integer = 2
	Private Const ACArrayDepartmentID As Integer = 3
	Private Const ACArrayDeptAmount As Integer = 4
	Private Const ACArrayDepartment As Integer = 5
	Private Const ACArrayNominal As Integer = 6
	Private Const ACArrayCurrencyID As Integer = 7
	Private Const ACArrayInvoiceItemNo As Integer = 8
	Private Const ACArrayInvoiceID As Integer = 9
	Private Const ACArrayTaxAmount As Integer = 10 'Datasure replaced tax rate
	Private Const ACArrayHasTax As Integer = 11
	Private Const ACArrayTaxGroupId As Integer = 12
	Private Const ACArrayTaxArray As Integer = 13
	' Change this to the max value for the array ubound
	Private Const ACArrayMax As Integer = 13
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTInvoice.General
	
	' Business objects. ([Invoice], [Invoice_item], [Document])
	Private m_oBusiness As Object
	Private m_oInvoiceItem As Object

	Private m_oDocument As bACTDocument.Form
	'DC030806

	Private m_oImportSiriusTrans As bACTImportSiriusTrans.Business

	Private m_oCommissionPost As bACTCommissionPost.Business
	
	'eck PN5508
	Private m_oCurrencyConvert As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	' Stores the details from the business object.
	
	' CTAF 310800 - InvoiceType
	Private m_sInvoiceType As String = ""
	' CTAF 310800 - Branches
	Private m_vBranches( ,  ) As Object
	
	'DC020806
    Private m_sTaxGroupCode As String = ""
    Private Const vbFormCode As Integer = 0
    Dim itm As Integer
	
	Public Property InvoiceType() As String
		Get
			Return m_sInvoiceType
		End Get
		Set(ByVal Value As String)
			m_sInvoiceType = Value
		End Set
	End Property
	
	Public ReadOnly Property DocumentInputs() As Object
		Get
			
			Return VB6.CopyArray(m_vDocumentInputs)
			
		End Get
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	Public Property InvoiceID() As Integer
		Get
			
			Return m_lInvoiceID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lInvoiceID = Value
			
		End Set
	End Property
	
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	Public ReadOnly Property StepStatus() As String
		Get
			
			' Standard Property.
			
			' Return the Steps Status
			Return m_sStepStatus.Value
			
		End Get
	End Property
	
	' ***************************************************************** '
	'
	' Name: RefreshList
	'
	' Description: Refreshes the invoice item list
	'
	' History: 10/04/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function RefreshList(Optional ByRef r_vInvoiceTotal As Double = 0) As Integer
		
		Dim result As Integer = 0
		Dim lstItem As ListViewItem
		Dim sText As String = ""
		Dim dItemsTotal, dSubTotal As Double
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			lvwItems.Items.Clear()
			
			cmdEdit.Enabled = True
			cmdDelete.Enabled = True
			
			dItemsTotal = 0
			
			If Not Information.IsArray(m_vItems) Then
				txtTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(dItemsTotal))
				Return result
			End If
			

			If m_vItems Is Nothing Then
				txtTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(dItemsTotal))
				Return result
			End If
			
			' Loop the array and display
			For iLoop1 As Integer = m_vItems.GetLowerBound(1) To m_vItems.GetUpperBound(1)
				
				' Check it's not been deleted
				If CStr(m_vItems(ACArrayNominal, iLoop1)) <> "-1" Then
					
					' Description
					sText = CStr(m_vItems(ACArrayDescription, iLoop1))
					lstItem = lvwItems.Items.Add("I" & iLoop1, sText.Trim(), "")
					lstItem.Tag = CStr(iLoop1)
					
					' Nominal
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vItems(ACArrayNominal, iLoop1)).Trim()
					
					' Amount
					ListViewHelper.GetListViewSubItem(lstItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vItems(ACArrayAmount, iLoop1)))
					
					' Add to the total
					' m_vItems(ACArrayAmount, iLoop1%)
					'Datasure Don't calculate the tax here or use tax rate
					'            If (m_vItems(ACArrayHasTax, iLoop1%) = 1) Then
					'                dSubTotal = m_vItems(ACArrayAmount, iLoop1%)
					'                dSubTotal = dSubTotal + (dSubTotal / 100) * m_vItems(ACArrayTaxRate, iLoop1%)
					'            Else
					'                dSubTotal = m_vItems(ACArrayAmount, iLoop1%)
					'            End If
					' VAT it
					'            If (CInt(m_vItems(ACArrayHasTax, iLoop1%)) = 1) Then
					'
					'                lstItem.SubItems(3) = FormatField(iFormatType:=PMFormatCurrency, _
					''                                                  vFieldValue:=m_vItems(ACArrayAmount, iLoop1%) + ((m_vItems(ACArrayAmount, iLoop1%) / 100) * m_vTaxRate))
					'
					'            Else
					'
					'                lstItem.SubItems(3) = FormatField(iFormatType:=PMFormatCurrency, _
					''                                                  vFieldValue:=m_vItems(ACArrayAmount, iLoop1%))
					'
					'            End If
					
					
					dSubTotal = CDbl(m_vItems(ACArrayAmount, iLoop1)) + CDbl(m_vItems(ACArrayTaxAmount, iLoop1))
					ListViewHelper.GetListViewSubItem(lstItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(dSubTotal))
					'datasure end
					
					dItemsTotal += dSubTotal
					
					' Department
					If CDbl(m_vItems(ACArrayDeptAmount, iLoop1)) <> 0 Then
						
						ListViewHelper.GetListViewSubItem(lstItem, 4).Text = CStr(m_vItems(ACArrayDepartment, iLoop1)).Trim()
						
						' Department Amount
						ListViewHelper.GetListViewSubItem(lstItem, 5).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vItems(ACArrayDeptAmount, iLoop1)))
						
					Else
						
						ListViewHelper.GetListViewSubItem(lstItem, 4).Text = "(none)"
						
					End If
					
				End If
				
			Next iLoop1
			
			ListView6Autosize(lvwList:=lvwItems, bSizeHeaders:=True)
			
			' Set the total
			txtTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(dItemsTotal))
			
			' Pass back the total if it's required

			If Not Information.IsNothing(r_vInvoiceTotal) Then
				r_vInvoiceTotal = dItemsTotal
			End If
			
			lvwItems.Refresh()
			
			If lvwItems.Items.Count = 0 Or m_iTask = gPMConstants.PMEComponentAction.pmview Then
				cmdEdit.Enabled = False
				cmdDelete.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ProcessDetailsForm
	'
	' Description: Displays the details form for adding/editing
	'
	' History: 10/04/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function ProcessDetailsForm(ByVal v_iTask As Integer) As Integer
		
		Dim result As Integer = 0
		Dim oDetails As frmDetails
		Dim iIndex As Integer
		
		Try 
			
            result = gPMConstants.PMEReturnCode.PMTrue


            oDetails = New frmDetails()

            ' Set the task
            oDetails.Task = v_iTask
            ' Set the tax rate

            'Datasure need to pass currency
            oDetails.CurrencyID = m_lCurrencyId

            ' If edit, then get and set the property
            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                ' Get the current position in the array

                'Modified,save the focused item index so we used it later
                'start
                If IsNothing(lvwItems.FocusedItem) Then

                    lvwItems.FocusedItem = lvwItems.Items.Item(itm)
                End If
                'end
                iIndex = Convert.ToString(lvwItems.FocusedItem.Tag)
                'Modified,save the focused item index so we used it later
                itm = iIndex
                ' Populate the properties
                oDetails.Description = CStr(m_vItems(ACArrayDescription, iIndex))

                If (CDbl(m_vItems(ACArrayNominalID, iIndex)) = 0) And (CStr(m_vItems(ACArrayNominal, iIndex)) <> "") Then
                    oDetails.NominalAccount = CStr(m_vItems(ACArrayNominal, iIndex))
                Else
                    oDetails.NominalAccountID = CInt(m_vItems(ACArrayNominalID, iIndex))
                End If


                'Developer Guide No. 24
                oDetails.Amount = m_vItems(ACArrayAmount, iIndex)
                oDetails.DepartmentID = CInt(m_vItems(ACArrayDepartmentID, iIndex))

                'Developer Guide No. 24
                oDetails.DeptAmount = m_vItems(ACArrayDeptAmount, iIndex)
                oDetails.HasVAT = CBool(m_vItems(ACArrayHasTax, iIndex))
                'Datasure
                oDetails.TaxAmount = CDec(m_vItems(ACArrayTaxAmount, iIndex))
                oDetails.TaxGroupId = CInt(m_vItems(ACArrayTaxGroupId, iIndex))

            ElseIf (v_iTask = gPMConstants.PMEComponentAction.PMAdd) Then

                ' Default the nominal code
                If uctNominalAccount.AccountId <> 0 Then
                    oDetails.NominalAccountID = uctNominalAccount.AccountId
                End If


            End If

            ' Display the form
            VB6.ShowForm(oDetails, FormShowConstants.Modal, Me)

            If oDetails.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            ' Make sure we updated
            m_bInvoiceItemChange = True

            If Not Information.IsArray(m_vItems) Then

                ReDim m_vItems(ACArrayMax, 0)
            Else
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    ReDim Preserve m_vItems(ACArrayMax, m_vItems.GetUpperBound(1) + 1)
                End If
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                iIndex = m_vItems.GetUpperBound(1)
            End If

            ' Get the properties
            If (v_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (v_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                m_vItems(ACArrayDescription, iIndex) = oDetails.Description
                m_vItems(ACArrayNominalID, iIndex) = oDetails.NominalAccountID
                m_vItems(ACArrayNominal, iIndex) = oDetails.NominalAccount

                m_vItems(ACArrayAmount, iIndex) = oDetails.Amount
                m_vItems(ACArrayDepartment, iIndex) = oDetails.Department

                m_vItems(ACArrayDeptAmount, iIndex) = oDetails.DeptAmount
                m_vItems(ACArrayDepartmentID, iIndex) = oDetails.DepartmentID
                If oDetails.HasVAT Then
                    m_vItems(ACArrayHasTax, iIndex) = 1
                Else
                    m_vItems(ACArrayHasTax, iIndex) = 0
                End If
                'Datasure
                m_vItems(ACArrayTaxAmount, iIndex) = oDetails.TaxAmount
                m_vItems(ACArrayTaxGroupId, iIndex) = oDetails.TaxGroupId
                m_vItems(ACArrayTaxArray, iIndex) = oDetails.TaxArray

                'DC020806
                m_sTaxGroupCode = oDetails.TaxGroupCode

            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' Set the invoice item number
                m_vItems(ACArrayInvoiceItemNo, iIndex) = lvwItems.Items.Count + 1
            End If

            m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)

            oDetails = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDetailsForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDetailsForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	Private Sub cboBranches_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranches.SelectedIndexChanged
		
		cboCurrency.CompanyId = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)
		cboCurrency.RefreshList()
		
		cboCurrency.CurrencyId = g_iCurrencyID 'PN 18639
		
	End Sub
	
	Private Sub cboBranches_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranches.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboBranches)
		
	End Sub
	
	Private Sub cboBranches_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranches.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboBranches)
		
	End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		m_lReturn = CType(ProcessDetailsForm(v_iTask:=gPMConstants.PMEComponentAction.PMadd), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		'PN10332 eck 240204
		
		Dim iDelete As DialogResult = MessageBox.Show("Are you sure you wish to delete ?", "Confirm Delete ", MessageBoxButtons.YesNo)
		If iDelete = System.Windows.Forms.DialogResult.No Then
			Exit Sub
		End If
		'PN10332End
		
		' Get the current one
        If Not lvwItems.FocusedItem Is Nothing Then
            Dim iIndex As Integer = Convert.ToString(lvwItems.FocusedItem.Tag)

            ' Mark it as deleted
            m_vItems(ACArrayNominal, iIndex) = "-1"
        End If


       

        ' Refresh the list
        m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)

    End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		m_lReturn = CType(ProcessDetailsForm(v_iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		' Fire up the help screen
        'Developer Guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

	End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_0.Click
		Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
		
		Try 
			
			' Do not change tab if Spread has not been enabled
			If Not SSTabHelper.GetTabEnabled(tabMainTab, 1) Then
				Exit Sub
			End If
			
			' Change to the next tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
				SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
			End If
			
			' Set focus to the first control on the tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
				m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
			End If
		
		Catch 
			
			
			
			' Error Section
			
			Exit Sub
		End Try
		
		
	End Sub
	
	' *********************************************************************************
	'
	' Name: SetDocumentInputs
	' History: CF220199 - Changed signs on transactions
	'          RM090299 - Changed to allow only one entry for the purchase
	'                   - keeping analysis per item in the nominal
	' *********************************************************************************
    Private Function SetDocumentInputs() As Integer
        Dim result As Integer = 0
      
        Dim vDocumentTransactions, vDocumentTransactionDet As Object
        Dim lPtr As Integer


        Dim oAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Dim dSubTotal As Double

        Dim vAmount As Object

        Dim dVATAmount As Object
        Dim lNominalID, lVatAccountID As Integer

        'PN5508
        Dim cBaseValue, cBaseVATAmount As Decimal
        Dim vdCurrencyAmountUnrounded, vdBaseAmountUnrounded, vdCurrencyBaseXrate As Object
        'PN5503end
        'Datasure
        Dim vTaxesToPost, vTaxArray(,) As Object
        Dim sTaxCode As String = ""
        'AR20070216 - PN33165
        Dim lTransactionIndex As Integer
        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sReference, sRangeCode As String
        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the AutoNumber object
            Dim temp_oAutoNumber As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAutoNumber, "bACTAutoNumber.Business", vInstanceManager:="ClientManager")
            oAutoNumber = temp_oAutoNumber

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 310800 - Either PIN or PCN

            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN

                    'EK 220200 Get Correct Number Range
                    'Get the number range for Document Reference

                    m_lReturn = oAutoNumber.GetNumberRange(gACTLibrary.ACTAutoNumberGroupCodeDocumentRef13, gACTLibrary.ACTAutoNumberRangeCodePin, lNumberRangeID)
                    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePin
                Case ACInvoiceTypePCN

                    'Get the number range for Document Reference

                    m_lReturn = oAutoNumber.GetNumberRange(gACTLibrary.ACTAutoNumberGroupCodeDocumentRef25, gACTLibrary.ACTAutoNumberRangeCodePcn, lNumberRangeID)
                    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePcn

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Auto number
            ' CTAF 310800 - Also pass in the branch id
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=g_oObjectManager.UserID, v_iCompanyID:=m_lBranchID, r_sDocumentRef:=sReference, v_sRangeCode:=sRangeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC020806 get transdetail type id for Tax

            m_lReturn = m_oBusiness.GetTransdetailTypeID("TAX", m_iTransdetailTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Document Array
            'MKW160103 - Changed Upper Limit to ACTDocumentCompany (7) instead of
            ' ACTDocumentStatus (6)
            'ReDim m_vDocumentInputs(ACTDocumentStatus) As Variant

            ReDim m_vDocumentInputs(ACTBatchConst.ACTDocumentCompany)

            'MKW160103 - Store selected Branch/Company ID to variable for submittion.
            m_vDocumentInputs(ACTBatchConst.ACTDocumentCompany) = m_lBranchID

            'Document Transactions array

            ReDim vDocumentTransactions(0)

            'Document Transaction Properties
            'DC160904 PN13880 extended array for departmentid / cost centre
            'ReDim vDocumentTransactionDet(ACTTransDetailRefUnits) As Variant
            'DC020806
            'ReDim vDocumentTransactionDet(ACTTransDetailDepartmentId) As Variant

            ReDim vDocumentTransactionDet(ACTBatchConst.ACTTransdetailTypeId)

            'Get the document detail for this invoice
            m_vDocumentInputs(ACTBatchConst.ACTDocumentStatus) = gACTLibrary.ACTPostStatusPosted
            'eck070900 Document type will depend upon the Invoice type
            '    m_vDocumentInputs(ACTDocumentType) = ACTDocTypePurchaseInvoice
            'EK 220200 Add prefix
            ' CTAF 310800 - PCN or PIN ?
            'eck070900 added document selection
            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN
                    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    m_vDocumentInputs(ACTBatchConst.ACTDocumentRef) = gACTLibrary.ACTAutoNumberRangeCodePin & sReference
                    m_vDocumentInputs(ACTBatchConst.ACTDocumentType) = gACTLibrary.ACTDocTypePurchaseInvoice
                Case ACInvoiceTypePCN
                    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    m_vDocumentInputs(ACTBatchConst.ACTDocumentRef) = gACTLibrary.ACTAutoNumberRangeCodePcn & sReference
                    m_vDocumentInputs(ACTBatchConst.ACTDocumentType) = gACTLibrary.ACTDocTypePurchaseCreditNote
            End Select

            m_vDocumentInputs(ACTBatchConst.ACTDocumentDate) = m_dtInvoiceDate
            m_vDocumentInputs(ACTBatchConst.ACTDocumentComments) = m_sDescription.Value 'Description
            m_vDocumentInputs(ACTBatchConst.ACTDocumentAccountingDate) = m_dtInvoiceDate


            vDocumentTransactions(0) = vDocumentTransactionDet


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailCurrencyID) = m_lCurrencyId


            vdCurrencyBaseXrate = 0


            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=m_lCurrencyId, lCompanyID:=m_lBranchID, cBaseAmount:=cBaseValue, cCurrencyAmount:=m_dInvoiceValue, vConversionDate:=m_dtInvoiceDate, vConversionRate:=vdCurrencyBaseXrate, vCCyAmountUnRounded:=vdCurrencyAmountUnrounded, vBaseAmountUnrounded:=vdBaseAmountUnrounded)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                vdCurrencyBaseXrate = 1
                cBaseValue = m_dInvoiceValue
            End If


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailAccountID) = m_lSupplierID

            vDocumentTransactions(0)(ACTBatchConst.ACTDocumentAccountingDate) = m_dtInvoiceDate
            'eck070900 added document selection
            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN
                    'PN5503
                    '           vDocumentTransactions(lPtr&)(ACTTransDetailAmount) = CDbl(m_dInvoiceValue# * -1)


                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailAmount) = cBaseValue * -1
                Case ACInvoiceTypePCN
                    'PN5503
                    '           vDocumentTransactions(lPtr&)(ACTTransDetailAmount) = CDbl(m_dInvoiceValue#)


                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailAmount) = cBaseValue
            End Select


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailComment) = m_sDescription.Value


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailOperatorID) = g_oObjectManager.UserID


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailPurchaseOrderNo) = m_sOrderNo.Value

            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailPurchaseInvoiceNo) = m_sInvoiceNumber.Value
            'PN5508
            '   vDocumentTransactions(lPtr&)(ACTTransDetailCurrencyBaseXrate) = 0



            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailCurrencyBaseXrate) = vdCurrencyBaseXrate

            'eck070900 sign depends upon invoice type
            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN


                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailCurrencyAmount) = m_dInvoiceValue * -1
                Case ACInvoiceTypePCN


                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailCurrencyAmount) = m_dInvoiceValue
            End Select
            'PN5508
            '   vDocumentTransactions(lPtr&)(ACTTransDetailCurrencyBaseXrate) = 0



            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailCurrencyBaseXrate) = vdCurrencyBaseXrate



            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailThirdCurrency) = 0
            'eck070900 sign depends upon invoice type
            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN

                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailThirdCurrencyAmount) = m_dInvoiceValue * -1
                Case ACInvoiceTypePCN

                    vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailThirdCurrencyAmount) = m_dInvoiceValue
            End Select


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailThirdCurrencyBaseXrate) = 0


            vDocumentTransactions(0)(ACTBatchConst.ACTTransDetailRefDate) = m_dtInvoiceDate

            ' CF120199 - Added nominal ledger transactions

            dSubTotal = 0

            ' Add the documents for the nominal ledger
            For lLoop As Integer = m_vItems.GetLowerBound(1) To m_vItems.GetUpperBound(1)

                'PN30711 Datasure Issue
                If CStr(m_vItems(ACArrayNominal, lLoop)) <> "-1" Then

                    lTransactionIndex = vDocumentTransactions.GetUpperBound(0) + 1
                    ReDim Preserve vDocumentTransactions(lTransactionIndex)
                    ReDim vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransdetailTypeId)
                    'vDocumentTransactions(lTransactionIndex) = vDocumentTransactionDet
                    'vDocumentTransactions(lTransactionIndex) = vDocumentTransactionDet

                    ' Get the id for the nominal code

                    m_lReturn = m_oBusiness.GetAccountID(lNominalID, CStr(m_vItems(ACArrayNominal, lLoop)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'posting sign depends upon invoice type
                    Select Case m_sInvoiceType
                        Case ACInvoiceTypePIN

                            vAmount = CDbl(m_vItems(ACArrayAmount, lLoop))
                        Case ACInvoiceTypePCN

                            vAmount = CDbl(m_vItems(ACArrayAmount, lLoop)) * -1
                    End Select



                    m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=m_lCurrencyId, lCompanyID:=m_lBranchID, cBaseAmount:=cBaseValue, cCurrencyAmount:=CDec(vAmount), vConversionDate:=m_dtInvoiceDate, vCCyAmountUnRounded:=vdCurrencyAmountUnrounded, vBaseAmountUnrounded:=vdBaseAmountUnrounded)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        vdCurrencyBaseXrate = 1

                        cBaseValue = CDec(vAmount)
                    End If


                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailCurrencyID) = m_lCurrencyId

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailAccountID) = lNominalID

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTDocumentAccountingDate) = DateTime.Today

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailAmount) = cBaseValue

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailComment) = CStr(m_vItems(ACArrayDescription, lLoop))

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailOperatorID) = g_oObjectManager.UserID

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailPurchaseOrderNo) = m_sOrderNo.Value

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailPurchaseInvoiceNo) = m_sInvoiceNumber.Value


                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailCurrencyAmount) = vAmount


                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailCurrencyBaseXrate) = vdCurrencyBaseXrate

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailThirdCurrency) = 0


                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailThirdCurrencyAmount) = vAmount

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailThirdCurrencyBaseXrate) = 1

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailRefDate) = m_dtInvoiceDate


                    dSubTotal += CDbl(vAmount)
                    'DC160904 PN13880 added department/cost centre

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailDepartment) = CStr(m_vItems(ACArrayDepartment, lLoop))

                    vDocumentTransactions(lTransactionIndex)(ACTBatchConst.ACTTransDetailDepartmentId) = CStr(m_vItems(ACArrayDepartmentID, lLoop))
                    'Accumulate Taxes

                    vTaxArray = m_vItems(ACArrayTaxArray, lLoop)
                    If Information.IsArray(vTaxArray) Then
                        If Not Information.IsArray(vTaxesToPost) Then


                            vTaxesToPost = vTaxArray
                        Else

                            For iTaxPtr As Integer = 0 To vTaxArray.GetUpperBound(1)

                                For iTaxPtr2 As Integer = 0 To vTaxesToPost.GetUpperBound(1)


                                    If vTaxesToPost(0, iTaxPtr2).Equals(vTaxArray(0, iTaxPtr)) Then



                                        vTaxesToPost(1, iTaxPtr2) = CDec(vTaxesToPost(1, iTaxPtr2)) + CDec(vTaxArray(1, iTaxPtr))

                                        vTaxArray(1, iTaxPtr) = 0
                                        Exit For
                                    End If
                                Next iTaxPtr2

                                If (CDbl(vTaxArray(1, iTaxPtr))) <> 0 Then

                                    ReDim Preserve vTaxesToPost(1, vTaxesToPost.GetUpperBound(1) + 1)



                                    vTaxesToPost(0, vTaxesToPost.GetUpperBound(1)) = vTaxArray(0, iTaxPtr)



                                    vTaxesToPost(1, vTaxesToPost.GetUpperBound(1)) = vTaxArray(1, iTaxPtr)
                                End If

                            Next iTaxPtr
                        End If
                    End If
                    ''PN30711 Datasure Issue
                End If
            Next lLoop

            'eck070900 calculation depends upon invoice type
            Select Case m_sInvoiceType
                Case ACInvoiceTypePIN

                    dVATAmount = m_dInvoiceValue - dSubTotal
                Case ACInvoiceTypePCN

                    dVATAmount = (m_dInvoiceValue * -1) - dSubTotal
            End Select
            'Modified for Datasure
            ' Post the difference to the Tax Accounts
            If Information.IsArray(vTaxesToPost) Then

                lPtr = vDocumentTransactions.GetUpperBound(0) + 1

                For iTaxPtr As Integer = 0 To vTaxesToPost.GetUpperBound(1)
                    'ReDim Preserve vDocumentTransactions(1, vDocumentTransactions.GetUpperBound(0)) 'MYTEST
                    ReDim Preserve vDocumentTransactions(lPtr)
                    ReDim vDocumentTransactions(lPtr)(ACTBatchConst.ACTTransdetailTypeId)

                    'vDocumentTransactions(lPtr) = vDocumentTransactionDet
                    'DC020806 cahnge the tax code used
                    sTaxCode = "TAX" & m_sTaxGroupCode.TrimEnd() & "IN"



                    dVATAmount = CDec(vTaxesToPost(1, iTaxPtr))

                    'DC020806 change sign if Credit Note
                    Select Case m_sInvoiceType
                        Case ACInvoiceTypePCN


                            dVATAmount = CDbl(dVATAmount) * -1
                    End Select

                    ' Get the account_id for the the Tax account

                    m_lReturn = m_oBusiness.GetAccountID(lVatAccountID, sTaxCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If



                    m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=m_lCurrencyId, lCompanyID:=m_lBranchID, cBaseAmount:=cBaseVATAmount, cCurrencyAmount:=CDec(dVATAmount), vConversionDate:=m_dtInvoiceDate, vCCyAmountUnRounded:=vdCurrencyAmountUnrounded, vBaseAmountUnrounded:=vdBaseAmountUnrounded)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        vdCurrencyBaseXrate = 1

                        cBaseVATAmount = CDec(dVATAmount)
                    End If
                    vDocumentTransactions(lPtr)(ACTTransDetailCurrencyID) = m_lCurrencyId
                    vDocumentTransactions(lPtr)(ACTTransDetailAccountID) = lVatAccountID
                    vDocumentTransactions(lPtr)(ACTDocumentAccountingDate) = DateTime.Today
                    vDocumentTransactions(lPtr)(ACTTransDetailAmount) = cBaseVATAmount
                    vDocumentTransactions(lPtr)(ACTTransDetailComment) = "TAX"
                    vDocumentTransactions(lPtr)(ACTTransDetailOperatorID) = g_oObjectManager.UserID
                    vDocumentTransactions(lPtr)(ACTTransDetailPurchaseOrderNo) = m_sOrderNo.Value
                    vDocumentTransactions(lPtr)(ACTTransDetailPurchaseInvoiceNo) = m_sInvoiceNumber.Value
                    vDocumentTransactions(lPtr)(ACTTransDetailCurrencyAmount) = dVATAmount
                    vDocumentTransactions(lPtr)(ACTTransDetailCurrencyBaseXrate) = 1
                    vDocumentTransactions(lPtr)(ACTTransDetailThirdCurrency) = 0
                    vDocumentTransactions(lPtr)(ACTTransDetailThirdCurrencyAmount) = dVATAmount
                    vDocumentTransactions(lPtr)(ACTTransDetailThirdCurrencyBaseXrate) = 1
                    vDocumentTransactions(lPtr)(ACTTransDetailRefDate) = m_dtInvoiceDate

                    'DC020806 added new transdetail type id
                    vDocumentTransactions(lPtr)(ACTTransdetailTypeId) = m_iTransdetailTypeId
                    vDocumentTransactions(lPtr)(ACTTransDetailSpare) = "TAX"
                    lPtr += 1
                Next iTaxPtr
            End If

            'Place the Document Transaction Details in the Document Inputs array

            m_vDocumentInputs(ACTBatchConst.ACTDocumentTransactions) = vDocumentTransactions

            'Destroy AutoNumber object

            oAutoNumber.Dispose()
            oAutoNumber = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetDocumentInputs", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDocumentInputs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (SetProperties) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function SetProperties(ByVal v_sEntityCode As String, ByRef r_sEntityName As String) As Integer
		'
		'Dim result As Integer = 0
		'Dim iPtr As Integer
		'Dim sTextValue As String = ""
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'If v_sEntityCode = "" Then
				'r_sEntityName = CStr(0)
			'End If
			'
			'Return result
		'
		'Catch 
			'
			'
			'
			'Return result
		'End Try
	'End Function
	
	
	Private Sub SpreadEnable()
		
		'Enable or Disable 2nd Tab depending on the contents of invoice no.
		
		Dim bTab2Status As Boolean
		
		Try 
			
			bTab2Status = (txtInvNum.Text <> "")
			
			If SSTabHelper.GetTabEnabled(tabMainTab, 1) <> bTab2Status Then
				SSTabHelper.SetTabEnabled(tabMainTab, 1, bTab2Status)
			End If
		
		Catch excep As System.Exception
			
			
			
			'Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process SpreadEnable", vApp:=ACApp, vClass:=ACClass, vMethod:="SpreadEnable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************* '
			' Enter your code here to assign all of the controls to
			' PMFormControl
			'
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInvNum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInvDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInvVal, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=2)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPONum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInvDes, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctSupplierAccount, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctNominalAccount, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' CTAF 310800
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBranches, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'Get the invoice details

			m_lReturn = m_oBusiness.GetDetails(vInvoiceID:=m_lInvoiceID)
			
			'Get then invoice item details if the invoice details were retrieved
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

				m_lReturn = m_oInvoiceItem.GetDetails(vInvoiceID:=m_lInvoiceID)
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
		Dim sAccountName As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'Getthe invoice details
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInvNum, vControlValue:=m_sInvoiceNumber.Value)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInvDate, vControlValue:=m_dtInvoiceDate)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPONum, vControlValue:=m_sOrderNo.Value)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInvDes, vControlValue:=m_sDescription.Value)
			
			uctSupplierAccount.AccountId = m_lSupplierID
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRef, vControlValue:=m_sReference.Value)
			
			For lLoop As Integer = 1 To cboBranches.Items.Count
				If VB6.GetItemData(cboBranches, lLoop - 1) = m_lBranchID Then
                    cboBranches.SelectedIndex = lLoop - 1
                End If
			Next 
			cboCurrency.CompanyId = m_lBranchID
			cboCurrency.CurrencyId = m_lCurrencyId
			
			'Get the Nominal Account ID
			m_sCode.Value = m_sCode.Value.Trim()
			uctNominalAccount.Text = m_sCode.Value

			m_lReturn = m_oBusiness.GetAccountID(v_sShortCode:=m_sCode.Value, r_lSupplierID:=m_lNominalID)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			uctNominalAccount.AccountId = m_lNominalID
			m_sNominalCode = m_sCode.Value
			
			'And this
			m_lSupplierIdRef = m_lSupplierID
			
			' CTAF 170400
			m_lReturn = CType(RefreshList(r_vInvoiceTotal:=m_dInvoiceValue), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Display the total
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInvVal, vControlValue:=m_dInvoiceValue)
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToBusiness
	'
	' Description: Updates all business members from the interface
	'              details.
	'
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		
		Try 
			
			Dim lInvoiceRow As Integer 'AR20070216 - PN33165 Cater for deleted invoice rows
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the business data ID to one because we are only
			' dealing with one record item only.
			lBusinessDataID = 1
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMadd
					' Inform the business object with a new data item.
					
					' {* USER DEFINED CODE (Begin) *}
					
					'Get new id for the [Invoice] Table

					m_lInvoiceID = m_oBusiness.GetNewID()
					
					If m_lInvoiceID = gPMConstants.PMEReturnCode.PMFalse Then
						
					End If
					
					'Create New Invoice in [Invoice]

					m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vInvoiceID:=m_lInvoiceID, vInvoiceNumber:=m_sInvoiceNumber.Value, vInvoiceDate:=m_dtInvoiceDate, vOrderNo:=m_sOrderNo.Value, vDescription:=m_sDescription.Value, vSupplierID:=m_lSupplierID, vCode:=m_sCode.Value, vReference:=m_sReference.Value, vCompanyID:=m_lBranchID)
					
					'Create the invoice item details in [Invoice_item]
					If m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
						'Add all the invoice items entered in the Spread Control
						For lPtr As Integer = 0 To m_vItems.GetUpperBound(1)
							' Check it's not been deleted
							'PN30711 Datasure Issue
							If CStr(m_vItems(ACArrayNominal, lPtr)) <> "-1" Then

								m_lReturn = m_oInvoiceItem.EditAdd(lRow:=lInvoiceRow, vInvoiceID:=m_lInvoiceID, vInvoiceItemNo:=lInvoiceRow + 1, vDescription:=m_vItems(ACArrayDescription, lPtr), vNominalCode:=m_vItems(ACArrayNominal, lPtr), vValue:=m_vItems(ACArrayAmount, lPtr), vCurrencyID:=m_lCurrencyId, vDepartmentID:=m_vItems(ACArrayDepartmentID, lPtr), vDeptAmount:=m_vItems(ACArrayDeptAmount, lPtr), vVATRate:=m_vItems(ACArrayTaxAmount, lPtr), vHasVat:=m_vItems(ACArrayHasTax, lPtr))
								
								If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									Exit For
								End If
								
								lInvoiceRow += 1
								
								'PN30711 Datasure Issue
							End If
						Next lPtr
					End If
					
					'Setup the document inputs for Document Entry
					If m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
						m_lReturn = CType(SetDocumentInputs(), gPMConstants.PMEReturnCode)
					End If
					
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					
					' {* USER DEFINED CODE (Begin) *}
					
					'Update the Invoice details

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vInvoiceID:=m_lInvoiceID, vInvoiceNumber:=m_sInvoiceNumber.Value, vInvoiceDate:=m_dtInvoiceDate, vOrderNo:=m_sOrderNo.Value, vDescription:=m_sDescription.Value, vSupplierID:=m_lSupplierID, vCode:=m_sCode.Value, vReference:=m_sReference.Value, vCompanyID:=m_lBranchID)
					
					'If Update on invoice object succeeded AND the invoice item
					'details were changed then delete all item detail and add
					'changed data
					
					If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And m_bInvoiceItemChange Then
						
						'Clear Invoice Items from table

						m_lReturn = m_oInvoiceItem.DeleteInvoiceItems(m_lInvoiceID)
						
						If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
							
							'Get the invoice item details for this edit
							For lPtr As Integer = 0 To m_vItems.GetUpperBound(1)
								
								'AR20070216 - PN33165 Cater for deleted invoice lines
								If CStr(m_vItems(ACArrayNominal, lPtr)) <> "-1" Then
									

									m_lReturn = m_oInvoiceItem.EditAdd(lRow:=lInvoiceRow, vInvoiceID:=m_lInvoiceID, vInvoiceItemNo:=lInvoiceRow + 1, vDescription:=m_vItems(ACArrayDescription, lPtr), vNominalCode:=m_vItems(ACArrayNominal, lPtr), vValue:=m_vItems(ACArrayAmount, lPtr), vCurrencyID:=m_lCurrencyId, vDepartmentID:=m_vItems(ACArrayDepartmentID, lPtr), vDeptAmount:=m_vItems(ACArrayDeptAmount, lPtr), vVATRate:=0, vHasVat:=m_vItems(ACArrayHasTax, lPtr))
									
									If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
										Exit For
									End If
									
									lInvoiceRow += 1
									
								End If
								
							Next lPtr
							
						End If
						
					End If
					
					' {* USER DEFINED CODE (End) *}
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to retreive all of the lookup
			' descriptions for a given lookup type.
			' The GetLookupDetails function will allow you to do this.
			'
			' Example:-
			'
			'    m_lReturn& = GetLookupDetails( _
			''        sLookupTable:=PMLookupCodeName, _
			''        ctlLookup:=cmbCodeName)
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetStatus (Standard Method)
	'
	' Description: Set the Process, Map and Step status.
	' Note:        A Property Get is provided for the Step Status only
	'              as this is the only one which this component can
	'              alter directly.
	' ***************************************************************** '
	Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the current Status settings.
			m_sProcessStatus.Value = sProcessStatus.Trim()
			m_sMapStatus.Value = sMapStatus.Trim()
			m_sStepStatus.Value = sStepStatus.Trim()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Dim iIndex As Integer
		
		Dim vInvoiceItemNo, vDescription, vNominalCode, vValue As Object
		Dim vCurrencyID As Integer
		Dim vDepartmentID, vDeptAmount, vDepartment As Object
		Dim vHasTax As Boolean

		'DC020806
		Dim vTaxAmount As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Get the invoice detail

			m_lReturn = m_oBusiness.GetNext(vInvoiceID:=m_lInvoiceID, vInvoiceNumber:=m_sInvoiceNumber.Value, vInvoiceDate:=m_dtInvoiceDate, vOrderNo:=m_sOrderNo.Value, vDescription:=m_sDescription.Value, vSupplierID:=m_lSupplierID, vCode:=m_sCode.Value, vReference:=m_sReference.Value, vCompanyID:=m_lBranchID)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				' Set the Invoice Item array to the array retrieved
				' from the invoice item table


				m_vInvoiceItem = m_oInvoiceItem.InvoiceItem
			End If
			
			iIndex = 0
			
			ReDim m_vItems(ACArrayMax, iIndex)
			
			'm_lReturn& = m_oInvoiceItem.GetDetails(vInvoiceID:=m_lInvoiceID)
			'If (m_lReturn& <> PMTrue) Then
			'    BusinessToData = PMFalse
			'    Exit Function
			'End If
			
			' CTAF 080500 - Get the details for the items
			Do 
				
				'DC020806 set taxrate

				m_lReturn = m_oInvoiceItem.GetNext(vInvoiceID:=m_lInvoiceID, vInvoiceItemNo:=vInvoiceItemNo, vDescription:=vDescription, vNominalCode:=vNominalCode, vValue:=vValue, vCurrencyID:=vCurrencyID, vDepartmentID:=vDepartmentID, vDeptAmount:=vDeptAmount, vVATRate:=vTaxAmount, vHasVat:=vHasTax)
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					
					'Set the invoice currency to the currency of the first invoice item.
					If iIndex = 0 Then
						m_lCurrencyId = vCurrencyID
					End If
					
					' Set the values in the array

					m_vItems(ACArrayInvoiceItemNo, iIndex) = vInvoiceItemNo

					m_vItems(ACArrayDescription, iIndex) = vDescription

					m_vItems(ACArrayNominal, iIndex) = vNominalCode

					m_vItems(ACArrayAmount, iIndex) = vValue

					m_vItems(ACArrayDepartmentID, iIndex) = vDepartmentID

					m_vItems(ACArrayDeptAmount, iIndex) = vDeptAmount
					
					If vHasTax Then

						m_vItems(ACArrayTaxAmount, iIndex) = vTaxAmount
						m_vItems(ACArrayHasTax, iIndex) = 1
					Else
						m_vItems(ACArrayHasTax, iIndex) = 0
					End If
					
					' Get the department if it has one

					If CDbl(vDepartmentID) <> 0 Then
						

						m_lReturn = m_oInvoiceItem.GetDepartment(v_iDepartmentID:=CInt(m_vItems(ACArrayDepartmentID, iIndex)), r_vDepartment:=vDepartment)
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							result = gPMConstants.PMEReturnCode.PMFalse
							' Log Error.
							iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get department name", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
							Return result
						Else

							m_vItems(ACArrayDepartment, iIndex) = vDepartment
						End If
						
					End If
					
					iIndex += 1
					ReDim Preserve m_vItems(ACArrayMax, iIndex)
					
				End If
				
			Loop While (m_lReturn = gPMConstants.PMEReturnCode.PMTrue)
			
			' Remove the last blank entry in the array
			ReDim Preserve m_vItems(ACArrayMax, iIndex - 1)
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			
			'Place the invoice details

            'Developer Guide No. 236
            m_sInvoiceNumber.Value = m_oFormFields.UnformatControl(txtInvNum)


            m_dInvoiceValue = CDbl(m_oFormFields.UnformatControl(txtInvVal))

			m_dtInvoiceDate = CDate(m_oFormFields.UnformatControl(txtInvDate))

            'Developer Guide No. 236
            m_sOrderNo.Value = m_oFormFields.UnformatControl(txtPONum)

            'Developer Guide No. 236
            m_sDescription.Value = m_oFormFields.UnformatControl(txtInvDes)
			m_sCode.Value = uctNominalAccount.Text

            'Developer Guide No. 236
            m_sReference.Value = m_oFormFields.UnformatControl(txtRef)
			m_lBranchID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)
			'Datasure
			'Invoice Item details are in m_vInvoiceItem Place by Spread Validate
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: PopulateBranches
	'
	' Description:
	'
	' History: 31/08/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function PopulateBranches() As Integer
		Dim result As Integer = 0
		
		Dim oUser As bPMUser.Business
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			cboBranches.Items.Clear()
			
			' Get an instance of bPMUser
			Dim temp_oUser As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oUser = temp_oUser
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateBranches", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			

			m_lReturn = oUser.GetUserSources(r_vSourceArray:=m_vBranches, v_vUserID:=g_oObjectManager.UserID)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get sources for User ID of " & g_oObjectManager.UserID, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateBranches", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				' Terminate it and remove

                oUser.Dispose()
                oUser = Nothing
				Return result
			End If
			
			' Terminate it and remove

            oUser.Dispose()
            oUser = Nothing
			
			' Check we have some branches. It's not a crime if they don't, but
			' they need to set some up.
			If Not Information.IsArray(m_vBranches) Then
				MessageBox.Show("There are no branches set up. Please click Cancel and do this first.", "Branches", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return result
			End If
			
			For iLoop1 As Integer = m_vBranches.GetLowerBound(1) To m_vBranches.GetUpperBound(1)
				Dim cboBranches_NewIndex As Integer = -1
                'developer guide no.29
                cboBranches_NewIndex = cboBranches.Items.Add(New VB6.ListBoxItem(Trim(m_vBranches(2, iLoop1)), CInt(m_vBranches(0, iLoop1))))


            Next iLoop1
			
			' Default to the first branch
			'    cboBranches.ListIndex = 0
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
            'Set up Tab
            tabMainTab.TabPages(0).Text = "Invoice"
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			SSTabHelper.SetTabEnabled(tabMainTab, 1, False)
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Populate the Branches
			m_lReturn = CType(PopulateBranches(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If cboBranches.SelectedIndex >= 0 Then
				cboCurrency.CompanyId = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)
			Else
				cboCurrency.CompanyId = g_iSourceID
			End If
			cboCurrency.CurrencyId = g_iCurrencyID 'PN16993
			If m_iTask <> gPMConstants.PMEComponentAction.PMadd Then
				txtInvVal.Enabled = False
				cboBranches.Enabled = False
				uctNominalAccount.Enabled = False
				uctSupplierAccount.Enabled = False
			End If
			
			cmdAdd.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdOK.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdEdit.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdDelete.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			uctNominalAccount.Visible = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			lblNominalAccount.Visible = (m_iTask = gPMConstants.PMEComponentAction.PMadd)

            If lvwItems.Items.Count = 0 Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

            'Developer Guide No. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            'Developer Guide No. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            'Developer Guide No. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            'Developer Guide No. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			

            'Developer Guide No. 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to display all language specific
			' captions.
			' The GetResData function will allow you to do this.
			'
			' Example:-
			'
			'    lblDesc.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACDesc, _
			''        iDataType:=PMResString)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			
			' The .RC caption file is gone. no more. dead. vanished. ARGH!!!
			
			' CTAF 310800 - Either PIN or PCN
			
			Select Case m_sInvoiceType
				Case ACInvoiceTypePIN
					
					Me.Text = "Purchase Invoice"
					
				Case ACInvoiceTypePCN
					
					Me.Text = "Purchase Invoice Credit Note"
					
			End Select
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'
	' ***************************************************************** '
	Private Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMadd
					' Get all of the lookup values.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Get all of the lookup values with the correct
					' effective date.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.pmview
					' Get lookup values for viewing only.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
		'
		'Dim result As Integer = 0
		'Dim lRow As Integer
		'Dim bFoundMatch As Boolean
		'
		' Lookup value contants.
		'Const ACValueTableName As Integer = 0
		'Const ACValueID As Integer = 1
		'Const ACValueStartPos As Integer = 2
		'Const ACValueNumber As Integer = 3
		'
		' Lookup detail contants.
		'Const ACDetailKey As Integer = 0
		'Const ACDetailDesc As Integer = 1
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Get the lookup values.
			'
			'bFoundMatch = False
			'
			'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
				' Check for a match of the table name.
				'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
					' Found a match
					'bFoundMatch = True
					'Exit For
				'End If
			'Next lRow
			'
			' Check if there has been a table match.
			'If Not bFoundMatch Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
				'
				'Return result
			'End If
			'
			' Using the lookup values, populate the control with
			' the details from the lookup details array.
			'
			'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
				' Add the details to the control.

				'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


				'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
				'
				'SP150998 - compare long value not string
				' Check if this is the selected index.
				'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
					'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


						'ctlLookup.ListIndex = ctlLookup.NewIndex
					'End If
				'End If
				'
			'Next lCntr
			'
			' Check if the selected index is blank. If so,
			' we set the controls index to zero.
			'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

				'ctlLookup.ListIndex = 0
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	' PRIVATE Methods (End)
	
	
	'Private Sub cmdBuild_Click()
	'
	'Dim oFindAccount As Object
	'Dim iStatus As Integer
	'
	'    On Error GoTo Err_cmdBuild_Click
	'
	'    Set oFindAccount = CreateObject("iACTFindAccount.Interface")
	'
	'    m_lReturn = oFindAccount.Initialise()
	'
	'    If (m_lReturn = PMTrue) Then
	'
	'        oFindAccount.ShortCode = txtSuppAcc
	'
	'        'Set process modes
	'        m_lReturn = oFindAccount.SetProcessModes( _
	''            vNavigate:=PMNavigateDisabled, _
	''            vProcessMode:=PMProcessModeGeneric, _
	''            vTransactionType:=PMTypeOfBusinessGeneric, _
	''            vEffectiveDate:=Now)
	'
	'        'Start
	'        If (m_lReturn = PMTrue) Then
	'            m_lReturn = oFindAccount.Start()
	'        End If
	'
	'        'If an account was selected then get it
	'        If (m_lReturn = PMTrue) Then
	'            If (oFindAccount.AccountId <> 0) Then
	'                m_lSupplierID = oFindAccount.AccountId
	'                txtSuppAcc = oFindAccount.ShortCode
	'            End If
	'        End If
	'
	'    End If
	'
	'    m_lReturn = oFindAccount.Terminate()
	'
	'    Set oFindAccount = Nothing
	'
	'    Exit Sub
	'
	'Err_cmdBuild_Click:
	'
	'    ' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to process cmdBuild_Click", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="cmdBuild_Click", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'End Sub
	
	
	Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrevious.Click
		Try 
			
			' Do not change tab if Spread has not been enabled
			If Not SSTabHelper.GetTabEnabled(tabMainTab, 0) Then
				Exit Sub
			End If
			
			' Change to the next tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) >= 1 Then
				SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
			End If
		
		Catch 
			
			
			
			' Error Section
			
			Exit Sub
		End Try
		
	End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		Dim sMessage, sTitle As String
		'DC040806
		
		' Forms initialise event.
		
		Try 
			
			iPMFunc.ShowFormInTaskBar_Attach()
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the main business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTInvoice.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Get an instance of the invoice item object
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				Dim temp_m_oInvoiceItem As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oInvoiceItem, "bACTInvoiceItem.Business", vInstanceManager:="ClientManager")
				m_oInvoiceItem = temp_m_oInvoiceItem
				
			End If
			
			' Get an instance of the document object for Add
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				Dim temp_m_oDocument As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocument, "bACTDocument.Form", vInstanceManager:="ClientManager")
				m_oDocument = temp_m_oDocument
			End If
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
				
			End If
			'eck PN5508
			' Get an instance of the currency conversion object for Add
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				Dim temp_m_oCurrencyConvert As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:="ClientManager")
				m_oCurrencyConvert = temp_m_oCurrencyConvert
			End If
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
				
			End If
			
			
			'DC030806
			' Get an instance of the document object for Add
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				Dim temp_m_oImportSiriusTrans As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oImportSiriusTrans, "bACTImportSiriusTrans.Business", vInstanceManager:="ClientManager")
				m_oImportSiriusTrans = temp_m_oImportSiriusTrans
			End If
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
				
			End If
			
			' Get an instance of the document object for Add
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				Dim temp_m_oCommissionPost As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oCommissionPost, "bACTCommissionPost.Business", vInstanceManager:="ClientManager")
				m_oCommissionPost = temp_m_oCommissionPost
			End If
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
				
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iACTInvoice.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness, oInvoiceItem:=m_oInvoiceItem, oDocument:=m_oDocument, oImportSiriusTrans:=m_oImportSiriusTrans, oCommissionPost:=m_oCommissionPost), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
        Try
            'Developer Guide No. 38
            Me.cboCurrency.FirstItem = ""
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines objects.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oInvoiceItem.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            End If

            If Task = gPMConstants.PMEComponentAction.PMAdd Then

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = m_oDocument.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
                End If

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the status for the business object.

            m_lReturn = m_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oInvoiceItem.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            'Get the List of Supplier Accounts using the business object

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'SpreadSheet Setup complete
            m_bSpreadSetup = False

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub

        End Try
		
	End Sub
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

			If UnloadMode <> vbFormCode Then
				' Process the next set of actions depending
				' upon the interface task etc.
				m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Do not procced with the interface termination.
					Cancel = 1
                    eventArgs.cancel = True
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Exit Sub
				End If
			End If
			
			' Terminate the general object.
            m_oGeneral.Dispose()
			
			' Check for errors.
			
			
			' Destroy the instance of the general object
			' from memory.
			m_oGeneral = Nothing
			
			'eck PN5508
			If Not (m_oCurrencyConvert Is Nothing) Then

                m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
			End If
			m_oCurrencyConvert = Nothing
			
			
			' Terminate the business object

		m_oBusiness.Dispose()
			
			' Check for errors.
			
			
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			' Terminate the form control object.
		m_oFormFields.Dispose()
			
			' Check for errors.
			
			
			' Destroy the instance of the form control object
			' from memory.
			m_oFormFields = Nothing
			
			' Reset the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMainTab
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the
							' first tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
								' Display the previous tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
							End If
						End If
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the last tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
						Else
							' Check we are not on the
							' last tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
								' Display the next tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
							End If
						End If
						
					Case Keys.Home
						' Home key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
				End Select
			End With

            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to FormKeyDown", vApp:=ACApp, vClass:=ACClass, vMethod:="FormKeyDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwItems_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwItems.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.74
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

		' Check if the user clicked on anything
		If Not (lvwItems.GetItemAt(x, y) Is Nothing) Then
			' Yep, so enable edit and delete
			cmdAdd.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdEdit.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdDelete.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
		Else
			' Nope, so disable edit and delete
			cmdAdd.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMadd)
			cmdEdit.Enabled = False
			cmdDelete.Enabled = False
		End If
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			If Task = gPMConstants.PMEComponentAction.pmview Then
				Exit Sub
			End If
			
			With tabMainTab
				
				' Set the default button.
				If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
					VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
				Else
					VB6.SetDefault(cmdOK, True)
				End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				Application.DoEvents()
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) = m_ctlTabFirstLast.GetUpperBound(1) Then
					If m_ctlTabFirstLast(ACControlStart, 0).Visible Then
						m_ctlTabFirstLast(ACControlStart, 0).Focus()
					End If
				End If
				
				'Enable/Disable invoice number and value as we move between tabs
				If SSTabHelper.GetSelectedIndex(tabMainTab) = 1 Then
					txtInvNum.Enabled = False
					txtInvNum.BackColor = ColorTranslator.FromOle(ACDisabledTextColor)
					
					txtInvVal.Enabled = False
					txtInvVal.BackColor = ColorTranslator.FromOle(ACDisabledTextColor)
					
				Else
					If Task <> gPMConstants.PMEComponentAction.pmview Then
						txtInvNum.Enabled = True
						txtInvVal.Enabled = True
					End If
					
					txtInvNum.BackColor = ColorTranslator.FromOle(ACEnabledTextColor)
					txtInvVal.BackColor = ColorTranslator.FromOle(ACEnabledTextColor)
					
				End If
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) < m_ctlTabFirstLast.GetUpperBound(1) Then
					If m_ctlTabFirstLast(ACControlStart, 0).Visible Then
						m_ctlTabFirstLast(ACControlStart, 0).Focus()
					End If
				End If
				
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process tabMainTabClick", vApp:=ACApp, vClass:=ACClass, vMethod:="tabMainTabClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' ***************************************************************** '
	Private Function SetFirstLastControls() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 1)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			'
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtInvNum
			m_ctlTabFirstLast(ACControlEnd, 0) = txtRef
			
			m_ctlTabFirstLast(ACControlStart, 1) = lvwItems
			m_ctlTabFirstLast(ACControlEnd, 1) = cmdDelete
			
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim sSupplierID As String = ""
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			
			' Check mandatory controls have been entered into.
			m_lReturn = m_oFormFields.CheckMandatoryControls()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'MKW050204 PN10287 Remove so that mandatory fields must be filled in.
				' Set back to the first tab
				'tabMainTab.Tab = 1
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			'IJB 18/11/02 Ensure Supplier account entered
			If uctSupplierAccount.AccountId = 0 Then
				MessageBox.Show(" Please Enter a valid Supplier Account", "Supplier Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			'eck041001
			If uctNominalAccount.AccountId = 0 Then
				MessageBox.Show(" Please Enter a valid Nominal Account", "Nominal Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			'eck030901
			If txtTotal.Text = "" Then
				MessageBox.Show("Please enter Invoice details", "Invoice Details", MessageBoxButtons.OK, MessageBoxIcon.Error) '''PN68899
				'Set to the second tab
				SSTabHelper.SetSelectedIndex(tabMainTab, 1)
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			' Check that the invoice items add up to the total entered
			If CDec(txtInvVal.Text) <> CDec(txtTotal.Text) Then
				MessageBox.Show("The sum of the items entered does not match the invoice value.", "Error - Total", MessageBoxButtons.OK, MessageBoxIcon.Information)
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				Exit Sub
			End If
			
			'Get the account id from the entered short code
			If m_lSupplierID = 0 Then
				'm_lReturn = m_oBusiness.GetAccountID(m_lSupplierID, CStr(txtSuppAcc))
				m_lSupplierID = uctSupplierAccount.AccountId
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("The entered supplier account code does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
					m_lStatus = gPMConstants.PMEReturnCode.PMCancel
					uctSupplierAccount.Focus()
					Exit Sub
				End If
				
			End If
			
			m_lCurrencyId = cboCurrency.CurrencyId
			
			'IJB 18/11/02 Check for duplicate invoice numbers
			'DD 10/07/2003: Only do this if Adding
			If Task = gPMConstants.PMEComponentAction.PMadd Then
				sSupplierID = CStr(uctSupplierAccount.AccountId)

				If (m_oBusiness.CheckIfInvoiceNumberExists(sSupplierID, txtInvNum.Text.Trim())) = gPMConstants.PMEReturnCode.PMTrue Then
					If StringsHelper.ToDoubleSafe(sSupplierID) <> 0 Then
						MessageBox.Show("Duplicate invoice number entered for this account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
						m_lStatus = gPMConstants.PMEReturnCode.PMCancel
						Exit Sub
					End If
				Else
					m_lStatus = gPMConstants.PMEReturnCode.PMCancel
					Exit Sub
				End If
			End If
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Process the next set of actions depending
			' upon the interface task etc.
			If MessageBox.Show("Are you sure the Invoice is complete? You will not be able to alter it after posting.", "Add Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then
				m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
				
				
				' Check the return value.
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					' Everything OK, so we can hide the interface.
					Me.Hide()
				End If
			End If
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub txtInvDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvDate.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtInvDate)
		
	End Sub
	
	Private Sub txtInvDate_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtInvDate.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		' If the user presses Ctrl+T then insert todays date
		If (Shift And ShiftConstants.CtrlMask) And (eventArgs.KeyCode = Keys.T) Then
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInvDate, vControlValue:=DateTime.Today)
			
			' Call the got focus so form control can format it etc...
			txtInvDate_Enter(txtInvDate, New EventArgs())
			
		End If
		
	End Sub
	
	Private Sub txtInvDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvDate.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtInvDate)
		
	End Sub
	
	Private Sub txtInvDes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvDes.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtInvDes)
		
	End Sub
	
	Private Sub txtInvDes_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvDes.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtInvDes)
		
	End Sub
	Private isInitializingComponent As Boolean
	Private Sub txtInvNum_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvNum.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		SpreadEnable()
		
	End Sub
	Private Sub txtInvNum_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvNum.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtInvNum)
		
	End Sub
	Private Sub txtInvNum_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvNum.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtInvNum)
		
	End Sub
	
	Private Sub txtInvVal_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvVal.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtInvVal)
		
	End Sub
	
	Private Sub txtInvVal_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvVal.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtInvVal)
		'Formatting the field... PN 18421
		txtInvVal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtInvVal.Text).Trim()
		
	End Sub
	
	' ***************************************************************** '
	' Name: SelectText
	'
	' Description: Hightlights any text within the control passed.
	'
	' ***************************************************************** '
	Public Sub SelectText(ByRef ctlControl As Control)
		
		Try 
			
			' Set the controls properties.
			With ctlControl

                'Developer Guide No. 17 (No Solution)
                '.SelStart = 0

                'Developer Guide No. 17 (No Solution)
                '.SelLength = Strings.Len(ctlControl.ToString())
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select the text", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub txtRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRef.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtRef)
		
	End Sub
	
	Private Sub txtRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRef.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtRef)
		
	End Sub
	
	Private Sub txtPONum_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPONum.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtPONum)
		
	End Sub
	
	Private Sub txtPONum_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPONum.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtPONum)
		
	End Sub
	
	Private Sub uctSupplierAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctSupplierAccount.Click
		
		' If the supplier has a nominal account id then use it
		'Tomo220199
		'If it's changed
		If uctSupplierAccount.AccountId <> m_lSupplierIdRef Then
			If uctSupplierAccount.NominalAccountID <> 0 Then
				uctNominalAccount.AccountId = uctSupplierAccount.NominalAccountID
			End If
			m_lSupplierIdRef = uctSupplierAccount.AccountId
		End If
		
	End Sub
	
    Private Sub uctSupplierAccount_LostFocus() Handles uctSupplierAccount.AccountLookup

        m_lReturn = uctSupplierAccount.AccountId

        'Tomo220199
        'If it's changed...
        If uctSupplierAccount.AccountId <> m_lSupplierIdRef Then

            ' Is a nominal a/c associated with the supplier
            If uctSupplierAccount.NominalAccountID <> 0 Then
                ' Fill in the nominal a/c
                uctNominalAccount.AccountId = uctSupplierAccount.NominalAccountID
                '    Else
                '        ' Clear the nominal account if none associated with supplier a/c
                '        uctNominalAccount.AccountId = 0
                '        uctNominalAccount.Text = ""
            End If

            m_lSupplierIdRef = uctSupplierAccount.AccountId

        End If

    End Sub

    Private Sub _tabMainTab_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _tabMainTab_TabPage0.Click

    End Sub

    Private Sub lvwItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwItems.SelectedIndexChanged

    End Sub

End Class
