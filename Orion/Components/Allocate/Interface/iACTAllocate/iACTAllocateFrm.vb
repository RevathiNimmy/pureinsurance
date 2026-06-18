Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 06 May 1997
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	Private Const ACSecondaryImage As String = "SecondaryImage"
	Private Const ACPrimaryImage As String = "PrimaryImage"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
    'developer guide no.88
    Private m_oInterface As Object
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTypeOfBusiness As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lTransdetailID As Integer
	Private m_lAccountID As Integer
	Private m_lSaveAccountID As Integer
	Private m_sDocumentRef As String = ""
	Private m_sInsuranceRef As String = ""
	Private m_vTransdetailIDs( ,  ) As Object
	Private m_lAllocationTransType As Integer
	Private m_lAllocationID As Integer
	
	Private m_lMarkedItems As Integer
	Private m_cTotalDebitsMarked As Decimal
	Private m_cTotalCreditsMarked As Decimal
    Private Const vbFormCode As Integer = 0
	Private m_iAllocationCompany_id As Integer
	Private m_iAllocationCurrency_id As Integer
	Private m_bIsBatch As Boolean
	Private m_lWriteOffTransdetail_id As Integer
	Private m_lWriteOffReason_id As Integer
	Private m_cWriteOffAmount As Decimal
	' What solution are we running as part of?
	Private m_iSolutionConfig As Integer
	
	' Enable multi-currency?
	Private m_bEnableMultiCurrency As Boolean
	Private m_iCompanyId As Integer
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTAllocate.General

	Private m_oFindAccount As iACTFindAccount.Interface_Renamed

	Private m_oACTPeriod As bACTPeriod.Form
	Private m_oCurrencyConvert As Object
	
	' Stores the return value for a function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the business object.
	Public m_vSearchData( ,  ) As Object
	
	
	' PRIVATE Data Members (End)
	
	Private m_bOutstandingOnly As Boolean
	
	' Allow stopped accounts on Find Account
	Private m_bAllowStopped As Boolean
	
    Private m_vSourceArray As Object
	
	Private m_bFirstTime As Boolean
	Private m_bPartMatching As Boolean
	
	Private m_bMultiTreeAccounting As Boolean 'MKW090503 PN1914
	
	
	' PUBLIC Property Procedures (Begin)
	
	' CF180299
	
	Public Property IsBatch() As Boolean
		Get
			Return m_bIsBatch
		End Get
		Set(ByVal Value As Boolean)
			m_bIsBatch = Value
		End Set
	End Property
	
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
	
	Public WriteOnly Property TypeOfBusiness() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTypeOfBusiness = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property ControllingInterface() As Object
        Set(ByVal Value As Object)
            ' Set the controlling interface for this form
            m_oInterface = Value

        End Set
    End Property

    Public ReadOnly Property TransdetailID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lTransdetailID

        End Get
    End Property


    Public Property AccountID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lAccountID = Value
            txtAccountCode.Tag = CStr(Value)

        End Set
    End Property


    Public Property DocumentRef() As String
        Get

            ' Return the objects parameter value.
            Return m_sDocumentRef

        End Get
        Set(ByVal Value As String)

            ' Set the object parameter value.
            m_sDocumentRef = Value

        End Set
    End Property
    'eck220800

    Public Property InsuranceRef() As String
        Get

            ' Return the objects parameter value.
            Return m_sInsuranceRef

        End Get
        Set(ByVal Value As String)

            ' Set the object parameter value.

            m_sInsuranceRef = Value

        End Set
    End Property


    Public ReadOnly Property TransdetailIDs() As Object
        Get

            Return VB6.CopyArray(m_vTransdetailIDs)

        End Get
    End Property



    Public Property AllocationTransType() As Integer
        Get

            Return m_lAllocationTransType

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationTransType = Value

        End Set
    End Property


    Public Property AllocationID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAllocationID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lAllocationID = Value

        End Set
    End Property


    Public WriteOnly Property OutstandingOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bOutstandingOnly = Value
        End Set
    End Property
    'eck040500
    Public WriteOnly Property SourceArray() As Object()
        Set(ByVal Value() As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property


    ' ***************************************************************** '
    '
    ' Name: GetAccountID
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountID(ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the short code filter
            sShortCode = txtAccountCode.Text

            ' Call business and get the account id

            m_lReturn = g_oBusiness.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lAccountID)


            If r_lAccountID <> 0 Then
                txtAccountCode.Tag = CStr(r_lAccountID)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim iCompanyID As Integer
        Dim sDocumentRef As String = ""
        Dim iCurrencyID As Integer
        Dim cCurrencyAmount As Decimal
        Dim iTolerance, iDocTypeGroupId, iDocumentTypeID As Integer
        Dim lPeriodID As Integer
        Dim dtDateFrom, dtDateTo As Date
        'Dim sProject As String
        'Dim sContract As String
        'Dim sProduct As String
        'Dim sDepartment As String
        'Dim sAgent As String
        'Dim sClient As String
        Dim sInsuranceRef, sOperatorName, sPurchaseInvoiceNo, sPurchaseOrderNo, sDepartment, sSpare As String

        Dim lLedgerID As Integer
        Dim sLedgerTypeCode As String = ""

        Dim iOutstandingOnly As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            DisableInterface(bDisable:=True)

            ' {* USER DEFINED CODE (Begin) *}
            m_vSearchData = Nothing

            iCompanyID = g_iCompanyID

            ' 05082002 CMG/PB Only retrieve data when the Sub-Branch is chosen
            If cboSubBranch.SelectedIndex >= 0 Or Not (m_bMultiTreeAccounting) Then
                ' The user has typed in a code so we need the account_id
                m_lAccountID = 0
                If txtAccountCode.Text.Trim() <> "" Then

                    If Strings.Len(Convert.ToString(txtAccountCode.Tag)) = 0 Then
                        m_lReturn = GetAccountID(r_lAccountID:=m_lAccountID)

                        If m_lAccountID = 0 Then
                            m_lReturn = PopulateAccountCode()
                            If Strings.Len(Convert.ToString(txtAccountCode.Tag)) > 0 Then
                                m_lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
                            End If
                        End If

                    Else
                        m_lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
                    End If

                End If

                'We cancelled out, did we?
                If m_lAccountID = 0 Then

                    'This bit is in case we clicked Find Now with nothing in the account code, which is ok.
                    'If (Trim$(uctAccountCode.Text) <> "") Then
                    If txtAccountCode.Text.Trim() <> "" Then

                        ' empty the list
                        m_lReturn = ClearInterface(v_bSilent:=True)

                        DisplayStatusFound()

                        DisableInterface(bDisable:=False)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'Check ledger type is not insurer or agent.

                m_lReturn = g_oBusiness.GetLedgerForAccount(v_lAccountID:=m_lAccountID, r_lLedgerID:=lLedgerID, r_sLedgerTypeCode:=sLedgerTypeCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get ledger details for account from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                End If

                If sLedgerTypeCode = "I" Or sLedgerTypeCode = "A" Then
                    ClearInterface(v_bSilent:=True)
                    MessageBox.Show("Insurers and Agent cannot be processed by Batch Allocation", "Batch Allocation - Ledger Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If


                m_lReturn = DisplayAccountDetails()

                ' If we only want the balance, then exit now


                iCurrencyID = cmbCurrency.ItemId

                iDocTypeGroupId = VB6.GetItemData(cmbDocTypeGroup, cmbDocTypeGroup.SelectedIndex)
                iDocumentTypeID = VB6.GetItemData(cmbDocumentType, cmbDocumentType.SelectedIndex)
                lPeriodID = VB6.GetItemData(cmbPeriod, cmbPeriod.SelectedIndex)

                If Information.IsDate(txtDateFrom.Text) Then
                    dtDateFrom = CDate(txtDateFrom.Text)
                Else
                    dtDateFrom = #12/30/1899#
                End If

                If Information.IsDate(txtDateTo.Text) Then
                    dtDateTo = CDate(txtDateTo.Text)
                Else
                    dtDateTo = #12/30/1899#
                End If


                If cboPMUser.UserID <> 0 Then
                    sOperatorName = cboPMUser.ItemUsername
                Else
                    sOperatorName = ""
                End If

                sDepartment = cboDepartment.ItemCaption
                If sDepartment.Trim().ToLower() = "(all)" Then
                    sDepartment = ""
                End If

                ' Grab the "outstanding only" value
                iOutstandingOnly = 1

                m_lReturn = g_oBusiness.SelectTransQuery(r_lNumberOfRecords:=0, r_vResultArray:=m_vSearchData, v_iCompanyID:=iCompanyID, v_vAccountID:=m_lAccountID, v_vDocumentRef:=sDocumentRef, v_vCurrencyId:=iCurrencyID, v_vCurrencyAmount:=cCurrencyAmount, v_vTolerance:=iTolerance, v_vDocTypeGroupId:=iDocTypeGroupId, v_vDocumentTypeID:=iDocumentTypeID, v_vPeriodID:=lPeriodID, v_vDateFrom:=dtDateFrom, v_vDateTo:=dtDateTo, v_vInsuranceRef:=sInsuranceRef, v_vusername:=sOperatorName, v_vPurchaseOrderNo:=sPurchaseOrderNo, v_vPurchaseInvoiceNo:=sPurchaseInvoiceNo, v_vDepartment:=sDepartment, v_vSpare:=sSpare, v_bMultiTreeAccounting:=m_bMultiTreeAccounting)
            Else
                'sub branch not chosen
                m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer
        Dim result As Integer = 0
        Dim oList, oListDebits, oListCredits As ListView
        Dim oListItem As ListViewItem
        Dim sLookupDesc As String = ""
        Dim sFormattedCurrency, sFormattedOSCurrency As String
        Dim cOSCurrencyAmount, cMarkedCurrencyAmount As Decimal
        Dim lCurrencyID, lItems As Integer
        Dim iBaseCurrency As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDebits.Items.Clear()
            lvwSearchCredits.Items.Clear()

            m_lMarkedItems = 0
            m_cTotalDebitsMarked = 0
            m_cTotalCreditsMarked = 0
            m_iAllocationCompany_id = 0
            m_iAllocationCurrency_id = 0
            ' Check that search details are valid before
            ' continuing.
            'Populate The List Views
            If Information.IsArray(m_vSearchData) Then


                m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwSearchDebits)
                oListDebits = lvwSearchDebits

                m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwSearchDebits)
                oListCredits = lvwSearchCredits

                lItems = m_vSearchData.GetUpperBound(1)

                ' Assign the details to the interface.
                For lRow As Integer = m_vSearchData.GetLowerBound(1) To lItems

                    ' {* USER DEFINED CODE (Begin) *}
                    If CDec(m_vSearchData(ACIBaseAmount, lRow)) > 0 Then
                        oList = oListDebits
                    Else
                        oList = oListCredits
                    End If
                    If CInt(m_vSearchData(ACIMarkedStatus, lRow)) = gPMConstants.PMEReturnCode.PMTrue Then
                        ' Use a check mark

                        oListItem = oList.Items.Add(CStr(m_vSearchData(ACISourceID, lRow)).Trim(), ACIconCheck)

                        m_iAllocationCompany_id = CInt(m_vSearchData(ACISourceID, lRow))
                        m_iAllocationCurrency_id = CInt(m_vSearchData(ACICurrencyId, lRow))
                        m_lMarkedItems += 1

                    Else
                        ' Dont use any icon

                        oListItem = oList.Items.Add(CStr(m_vSearchData(ACISourceID, lRow)).Trim(), ACIconBlank - 1)
                    End If

                    ' Get Currency ID
                    lCurrencyID = CInt(m_vSearchData(ACICurrencyId, lRow))

                    With oListItem
                        ' Document Ref
                        ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text = CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim()
                        ' Document Date
                        ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vSearchData(ACIDocumentDate, lRow)))
                        ' Accounting Date
                        ListViewHelper.GetListViewSubItem(oListItem, ACListAccountingDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vSearchData(ACIAccountingDate, lRow)))

                        'CurrencyAmount

                        m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=m_vSearchData(ACISourceID, lRow), r_iBaseCurrencyID:=iBaseCurrency)


                        m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=iBaseCurrency, vCurrencyAmount:=m_vSearchData(ACIBaseAmount, lRow), vFormattedCurrency:=sFormattedCurrency, vConversionDate:=m_vSearchData(ACIAccountingDate, lRow))

                        ListViewHelper.GetListViewSubItem(oListItem, ACListCurrencyAmount).Text = sFormattedCurrency.Trim()

                        'Primary Settled

                        If Not CBool(m_vSearchData(ACIPrimarySettled, lRow)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, ACListPrimarySettled).Text = g_sNo
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, ACListPrimarySettled).Text = g_sYes
                        End If
                        'OS Amount
                        cOSCurrencyAmount = CDec(m_vSearchData(ACICurrencyAmount, lRow)) - CDec(m_vSearchData(ACIMatchAmount, lRow)) ' matched is negative

                        If m_bEnableMultiCurrency Then

                            m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=lCurrencyID, vCurrencyAmount:=cOSCurrencyAmount, vFormattedCurrency:=sFormattedOSCurrency, vConversionDate:=m_vSearchData(ACIAccountingDate, lRow))
                        Else
                            sFormattedOSCurrency = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cOSCurrencyAmount)) & " GBP"
                        End If
                        ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text = sFormattedOSCurrency.Trim()
                        'Marked Amount
                        cMarkedCurrencyAmount = CDec(m_vSearchData(ACIMarkedAmount, lRow))

                        m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=lCurrencyID, vCurrencyAmount:=cMarkedCurrencyAmount, vFormattedCurrency:=sFormattedOSCurrency, vConversionDate:=m_vSearchData(ACIAccountingDate, lRow))
                        ListViewHelper.GetListViewSubItem(oListItem, ACListMarkedAmount).Text = sFormattedOSCurrency
                        'Matched Date
                        If CStr(m_vSearchData(ACIMatchDate, lRow)) = CStr(0) Then
                            ListViewHelper.GetListViewSubItem(oListItem, ACListMatchDate).Text = "                    "
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, ACListMatchDate).Text = CStr(m_vSearchData(ACIMatchDate, lRow)).Trim()
                        End If
                        ' Document Type
                        m_lReturn = GetLookupDesc(lLookupRow:=ACLDocumentType, lLookupID:=CInt(m_vSearchData(ACIDocumentTypeId, lRow)), sLookupDesc:=sLookupDesc)
                        ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentTypeId).Text = sLookupDesc
                        'Insurance Ref
                        ListViewHelper.GetListViewSubItem(oListItem, ACListInsuranceRef).Text = CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()
                        'Operator Name
                        ListViewHelper.GetListViewSubItem(oListItem, ACListOperatorName).Text = CStr(m_vSearchData(ACIOperatorName, lRow)).Trim()
                        'Period
                        ListViewHelper.GetListViewSubItem(oListItem, ACListPeriodName).Text = CStr(m_vSearchData(ACIPeriodName, lRow)).Trim()

                        ' Document Type Group
                        m_lReturn = GetLookupDesc(lLookupRow:=ACLDocTypeGroup, lLookupID:=CInt(m_vSearchData(ACIDocTypeGroupId, lRow)), sLookupDesc:=sLookupDesc)
                        ListViewHelper.GetListViewSubItem(oListItem, ACListDocTypeGroupId).Text = sLookupDesc
                        ListViewHelper.GetListViewSubItem(oListItem, ACListSpare).Text = CStr(m_vSearchData(ACISpare, lRow)).Trim()

                        ' Don't display the following uder the SFORB solution
                        If m_iSolutionConfig <> gACTLibrary.ACTOrionSolutionSFORB Then
                            ListViewHelper.GetListViewSubItem(oListItem, ACListPurchaseInvoiceNo).Text = CStr(m_vSearchData(ACIPurchaseOrderNo, lRow)).Trim()
                            ListViewHelper.GetListViewSubItem(oListItem, ACListPurchaseOrderNo).Text = CStr(m_vSearchData(ACIPurchaseInvoiceNo, lRow)).Trim()
                            ListViewHelper.GetListViewSubItem(oListItem, ACListDepartment).Text = CStr(m_vSearchData(ACIDepartment, lRow)).Trim()
                        End If
                        'Department
                        ListViewHelper.GetListViewSubItem(oListItem, ACListDepartment).Text = ListViewHelper.GetListViewSubItem(oListItem, ACListDepartment).Text

                    End With

                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)
                    '            End If
                Next lRow
                If lvwSearchDebits.Items.Count > 0 Or lvwSearchCredits.Items.Count > 0 Then
                    'm_lReturn = ListViewBatchEnd()
                    m_lReturn = ListViewFunc.ListViewBatchEnd()
                    DisplayStatusFound()
                End If

            End If

            m_lReturn = UpdateTotalMarked()

            ' Enable the interface now that the search
            ' has completed.
            DisableInterface(bDisable:=False)

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer
        Dim sFormattedBase As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDebits.Items.Item(lvwSearchDebits.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lTransdetailID = CInt(m_vSearchData(ACITransDetailId, lSelectedItem))
            '    m_lAccountID = CLng(m_vSearchData(ACIAccountId, lSelectedItem&))
            m_sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()

            ReDim m_vTransdetailIDs(0, 0)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sLookupDesc As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all of the lookup details.
            ' See Interface SetProcessModes

            ' {* USER DEFINED CODE (Begin) *}

            cmbDocTypeGroup.Items.Clear()
            m_lReturn = GetLookupDetails(lLookupRow:=ACLDocTypeGroup, ctlLookup:=cmbDocTypeGroup, vAllTypes:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmbDocumentType.Items.Clear()
            m_lReturn = GetLookupDetails(lLookupRow:=ACLDocumentType, ctlLookup:=cmbDocumentType, vAllTypes:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            cmbCurrency.FirstItem = sLookupDesc
            cmbCurrency.ListIndex = 0

            m_lReturn = GetPeriodLookup()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}
            If m_lAccountID > 0 Then
                ' Get the matching account details
                m_lReturn = DisplayAccountDetails()
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim vDefault As Object
        Dim sAppName, sSection, sResult, sBalanceChk As String
        Dim vResultArray(,) As Object
        'eck PN4594
        Dim vMultiStructureTree As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Get what solution we're part of

            m_lReturn = g_oBusiness.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(gACTLibrary.ACTOrionSolutionMBP)
            End If

            m_iSolutionConfig = CInt(sResult)

            'MKW090503 PN1914 START
            'Get if multi tree accounting is enabled.
            '    sResult$ = ""
            '    m_lReturn& = getProductOptionValue(v_vOptionNumber:=SIROPTMultiTreeAccounting, _
            ''                        v_vBranch:=1, _
            ''                        r_vUnderwriting:=sResult)
            '    If m_lReturn <> PMTrue Then
            '        m_bMultiTreeAccounting = False
            '    End If


            'eck 141103 PN4594
            'Get if multi tree accounting is enabled.

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vMultiStructureTree)


            m_bMultiTreeAccounting = vMultiStructureTree.Trim() = "1"

            'DD 09/07/2003: Corrected display of Sub-Branch
            If vMultiStructureTree.Trim() = "1" Then
                '       m_bMultiTreeAccounting = True  'eck 141103 PN4594
                cboSubBranch.Visible = False
                cboSubBranch.SelectedIndex = -1
                lblSubBranch.Visible = False
            Else
                cboSubBranch.Visible = True
                lblSubBranch.Visible = True
            End If
            'MKW090503 PN1914 END

            ' Get if we want multi-currency or not
            sResult = ""
            sAppName = ""

            m_lReturn = g_oBusiness.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionMultiCurrency, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Then
                sResult = "1"
            End If

            ' CTAF 130300 - Use registry setting to decide if have any check boxes
            '               enabled by default
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACTFindTransBalanceOption, r_sSettingValue:=sBalanceChk)

            'eck220102
            If m_lAccountID <> 0 Then
                m_bOutstandingOnly = True
            End If

            m_bEnableMultiCurrency = CBool(sResult)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            'EK Not used any longer
            '    Select Case (m_lNavigate&)
            '        Case PMNavigateEnabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = True
            '
            '        Case PMNavigateDisabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = False
            '
            '        Case Else
            '            cmdNavigate.Visible = False
            '
            '    End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'MKW 210303 PN2447 Move Code from Latter in function (as data is need in DisplayLookupDetails)
            'CMG/PB 05082002 Use period business object
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oACTPeriod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oACTPeriod, "bACTPeriod.Form", vInstanceManager:="ClientManager")
            m_oACTPeriod = temp_m_oACTPeriod


            'Only do if multibranch accounting enabled MKW090503 PN1914
            If m_bMultiTreeAccounting Then

                ' CMG/PB 05082002 Added population of Sub-branch Combo

                m_lReturn = m_oACTPeriod.GetSubBranches(vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Display message.
                    MessageBox.Show("Failed to get Sub-Branches", "Sub-Branches failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultArray) Then

                    For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        Dim cboSubBranch_NewIndex As Integer = -1

                        cboSubBranch_NewIndex = cboSubBranch.Items.Add(CStr(vResultArray(3, lRow))) 'MKW 210303 PN2447 Changed index to 3 instead of 4

                        VB6.SetItemData(cboSubBranch, cboSubBranch_NewIndex, CInt(vResultArray(0, lRow)))
                    Next lRow

                    'If there is only one sub-branch then choose it and disable the control

                    If vResultArray.GetUpperBound(1) = 0 Then
                        cboSubBranch.SelectedIndex = 0 'MKW 210303 PN2447 Changed listindex to 0 instead of 1 (as zero based and require first).
                        cboSubBranch.Enabled = False
                    End If
                End If
                'MKW 210303 PN2447 END
            End If

            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation Then
                m_lReturn = GetAllocationDetails()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' Set the column widths for the debit search list.
            lvwSearchDebits.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(750))
            lvwSearchDebits.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(600))
            lvwSearchDebits.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(8).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwSearchDebits.Columns.Item(9).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(10).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(11).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(12).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(13).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(15).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(16).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDebits.Columns.Item(17).Width = CInt(0)
            lvwSearchDebits.Columns.Item(ACListCurrencyAmount).TextAlign = HorizontalAlignment.Right
            lvwSearchDebits.Columns.Item(ACListOSCurrencyAmount).TextAlign = HorizontalAlignment.Right

            ' Set the column widths for the credit search list.
            lvwSearchCredits.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(750))
            lvwSearchCredits.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(600))
            lvwSearchCredits.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(8).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwSearchCredits.Columns.Item(9).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(10).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(11).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(12).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(13).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(15).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(16).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchCredits.Columns.Item(17).Width = CInt(0)
            lvwSearchCredits.Columns.Item(ACListCurrencyAmount).TextAlign = HorizontalAlignment.Right
            lvwSearchCredits.Columns.Item(ACListOSCurrencyAmount).TextAlign = HorizontalAlignment.Right



            g_sYes = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACYes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            g_sNo = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            txtAccountCode.Enabled = True
            txtAccountCode.ReadOnly = False
            m_bAllowStopped = True
            cmdNewSearch.Enabled = True


            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchDebits.Handle.ToInt32(), v_vShowRowSelect:=True)

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchCredits.Handle.ToInt32(), v_vShowRowSelect:=True)

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
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(Optional ByRef v_bSilent As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim bSilent As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.

            ' Default to not silent (ie, ask the user) if missing parameter
            ' This is dont for backwards compatability
            If Not True Then
                bSilent = False
            Else
                bSilent = v_bSilent
            End If

            ' Display the message if we're not in silent mode
            If Not bSilent Then


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If

            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search debit list details.
            lvwSearchDebits.Items.Clear()

            ' Clear the search credit list details.
            lvwSearchCredits.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            txtAccountCode.Text = ""
            txtAccountCode.Tag = ""

            cmbDocTypeGroup.SelectedIndex = 0
            cmbDocumentType.SelectedIndex = 0
            cmbPeriod.SelectedIndex = 0
            txtDateFrom.Text = ""
            txtDateTo.Text = ""
            cmbCurrency.ListIndex = 0
            cboPMUser.ListIndex = 0
            cboDepartment.ListIndex = 0
            'Developer Guide No.51

            panAccountName.Name = ""

            panContactName.Name = ""

            panPhoneAreaCode.Name = ""

            panPhoneNumber.Name = ""

            panPhoneExtension.Name = ""

            panAccountBalance.Name = ""

            panStatus.Name = ""

            'uctAccountCode.AccountID = 0

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMain, 0)

            ' Set focus to the search details.
            If txtAccountCode.Enabled Then
                txtAccountCode.Focus()
            End If

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            DisableInterface(bDisable:=True)


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears the interface display, to clear results after changing parameters
    '
    ' ***************************************************************** '
    Private Function ClearDisplay() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vSearchData = Nothing

            lvwSearchDebits.Items.Clear()

            lvwSearchCredits.Items.Clear()

            stbStatus.Text = ""


            panDebitsMarked.Name = ""

            panCreditsMarked.Name = ""

            panAccountName.Name = ""

            panContactName.Name = ""

            panPhoneAreaCode.Name = ""

            panPhoneNumber.Name = ""

            panPhoneExtension.Name = ""

            panAccountBalance.Name = ""

            panStatus.Name = ""

            cmdOK.Enabled = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface display", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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
            ReDim m_ctlTabFirstLast(1, ACTabTitleCount - 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}
            m_ctlTabFirstLast(ACControlStart, 0) = txtAccountCode
            m_ctlTabFirstLast(ACControlEnd, 0) = panCreditsMarked

            m_ctlTabFirstLast(ACControlStart, 1) = cmbDocTypeGroup
            m_ctlTabFirstLast(ACControlEnd, 1) = txtDateTo




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
    'eck090500
    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    'UPGRADE_NOTE: (7001) The following declaration (ValidSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidSource(ByVal vSource As Object) As Boolean
    'Dim result As Boolean = False
    'If Not Information.IsArray(m_vSourceArray) Then
    'Return True
    'End If
    'For 'i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

    'If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
    'result = True
    'End If
    'Next i
    'Return result
    'End Function

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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

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


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdAllocate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllocateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdMark.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMarkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSubBranchLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            For i As Integer = 0 To ACTabTitleCount - 1

                SSTabHelper.SetTabCaption(tabMain, i, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Next i

            For i As Integer = 0 To ACListTitleCount - 1


                lvwSearchDebits.Columns.Item(i).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Next i

            For i As Integer = 0 To ACListTitleCount - 1


                lvwSearchCredits.Columns.Item(i).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Next i
            ' {* USER DEFINED CODE (Begin) *}


            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblContactName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblAccountBalance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountBalance, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblDocTypeGroup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocTypeGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblDocumentType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDocumentType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblPeriod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblDateFrom.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateFrom, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblDateTo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            lblOperatorID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOperatorName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            lblDepartment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDepartment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrimaryForAllocation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            ElseIf m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSecondaryForAllocation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            End If


            ' Set the icon to S4B if part of s4b...
            If m_iSolutionConfig = gACTLibrary.ACTOrionSolutionSFORB Then
                'ImgImage.Picture = imgSFORB.Picture
            End If

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
    ' Name: DisplayAccountDetails
    '
    ' Description: Get and Display selected Account details
    '
    ' ***************************************************************** '
    Private Function DisplayAccountDetails() As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim sContactName As New FixedLengthString(60)
        Dim sPhoneAreaCode As New FixedLengthString(10)
        Dim sPhoneNumber As New FixedLengthString(15)
        Dim sPhoneExtension As New FixedLengthString(6)
        Dim dAccountBalance As Double
        Dim sFormattedBalance, sStatusCode, sAccountName As String
        Dim dtAccountingDate As Date
        Dim lAccountCurrencyId As Integer
        Dim iBaseCurrency As Integer
        Dim lAccountCompanyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Strings.Len(Convert.ToString(txtAccountCode.Tag)) > 0 Then
                lAccountID = CInt(Convert.ToString(txtAccountCode.Tag))
            Else
                lAccountID = m_lAccountID
            End If

            dtAccountingDate = #12/31/9999#

            ' Get the Account details


            m_lReturn = g_oBusiness.GetAccountDetails(r_lAccountID:=lAccountID, sAccountName:=sAccountName, sContactName:=sContactName.Value, sPhoneAreaCode:=sPhoneAreaCode.Value, sPhoneNumber:=sPhoneNumber.Value, sPhoneExtension:=sPhoneExtension.Value, r_vdAccountBalance:=dAccountBalance, r_sAccountCode:=sStatusCode, r_vlAccountCurrencyId:=lAccountCurrencyId, v_dtAccountingDate:=dtAccountingDate, r_lCompanyID:=lAccountCompanyID)

            If lAccountID <> 0 Then

                ' Format in Base Currency
                If lAccountCurrencyId <> 0 Then

                    m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=lAccountCurrencyId, vCurrencyAmount:=dAccountBalance, vFormattedCurrency:=sFormattedBalance, vConversionDate:=DateTime.Today)


                Else

                    m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=lAccountCompanyID, r_iBaseCurrencyID:=iBaseCurrency)


                    m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=iBaseCurrency, vCurrencyAmount:=dAccountBalance, vFormattedCurrency:=sFormattedBalance, vConversionDate:=DateTime.Today)
                End If
                ' Setup contents of panels

                panAccountName.Name = sAccountName.Trim()

                panContactName.Name = sContactName.Value.Trim()

                panPhoneAreaCode.Name = " " & sPhoneAreaCode.Value.Trim()

                panPhoneNumber.Name = " " & sPhoneNumber.Value.Trim()

                panPhoneExtension.Name = " " & sPhoneExtension.Value.Trim()

                panAccountBalance.Name = " " & sFormattedBalance.Trim()

                sStatusCode = sStatusCode.Trim()

                panStatus.Name = sStatusCode.Substring(0, 1).ToUpper() & sStatusCode.Substring(sStatusCode.Length - (sStatusCode.Length - 1)).ToLower()

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the account details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAccountDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: FindAccountTransactions
    '
    ' Description: Start another instance of Find Transaction passing
    ' the selected transaction's account ID
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (FindAccountTransactions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function FindAccountTransactions() As Integer
    '
    'Dim result As Integer = 0
    'Dim oAllocate As ClassInterface
    'Dim lSelectedItem, lAccountID As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oAllocate = New ClassInterface()
    '
    'm_lReturn = CType(oAllocate, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'm_lReturn = oAllocate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
    '
    ' Get the Account ID of the selected item

    'lSelectedItem = Convert.ToString(lvwSearchDebits.Items.Item(lvwSearchDebits.FocusedItem.Index).Tag)
    'lAccountID = CInt(m_vSearchData(ACIAccountId, lSelectedItem))
    '
    ' Pass the Account ID to the new find instance
    'oAllocate.AccountID = lAccountID
    '
    'm_lReturn = oAllocate.Start()
    '
    'oAllocate = Nothing
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find account transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="FindAccountTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessMark
    '
    ' Description: Marks selected transaction
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function ProcessMark(Optional ByRef vMarkAmount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oList As ListView
        Dim lTransDetailID As Integer
        Dim iCurrencyID As Integer
        Dim lRow As Integer

        Dim cAmount As Decimal
        Dim sFormattedAmount As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            Select Case SSTabHelper.GetSelectedIndex(TabResults)
                Case 0
                    oList = lvwSearchDebits
                Case 1
                    oList = lvwSearchCredits
            End Select

            For iLoop1 As Integer = 1 To oList.Items.Count

                oListItem = oList.Items.Item(iLoop1 - 1)

                ' If its selected then...
                If oListItem.Selected Then

                    ' Get the row

                    lRow = Convert.ToString(oListItem.Tag)


                    If Not Information.IsNothing(vMarkAmount) Then
                        'Mark partial amount

                        cAmount = CDec(vMarkAmount)
                    Else
                        cAmount = CDec(m_vSearchData(ACICurrencyAmount, lRow)) - CDec(m_vSearchData(ACIMatchAmount, lRow))
                    End If


                    lTransDetailID = CInt(m_vSearchData(ACITransDetailId, lRow))
                    iCurrencyID = CInt(m_vSearchData(ACICurrencyId, lRow))

                    ' if its checked then unmark it
                    If m_vSearchData(ACIMarkedStatus, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = g_oBusiness.UnMarkTransaction(v_lTransDetailID:=lTransDetailID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return result
                        End If
                        ' No icons

                        'Developer Guide no.49
                        oListItem.ImageKey = ACIconBlank

                        'Developer Guide No.126
                        'oListItem.Icon = ACIconBlank

                        m_lMarkedItems -= 1
                        If m_lMarkedItems = 0 Then
                            m_iAllocationCompany_id = 0
                            m_iAllocationCurrency_id = 0
                        End If
                        m_vSearchData(ACIMarkedStatus, lRow) = gPMConstants.PMEReturnCode.PMFalse
                        m_vSearchData(ACIMarkedAmount, lRow) = 0

                        m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=iCurrencyID, vCurrencyAmount:=0, vFormattedCurrency:=sFormattedAmount, vConversionDate:=m_vSearchData(ACIAccountingDate, lRow))
                        ListViewHelper.GetListViewSubItem(oListItem, ACListMarkedAmount).Text = sFormattedAmount
                    Else
                        If m_iAllocationCurrency_id <> 0 Then
                            If m_iAllocationCurrency_id <> CInt(m_vSearchData(ACICurrencyId, lRow)) Then
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                MessageBox.Show("Cannot allocate transactions for more than one currency", "Error")
                                Return result
                            End If
                        End If

                        If m_iAllocationCompany_id <> 0 Then
                            If m_iAllocationCompany_id <> CInt(m_vSearchData(ACISourceID, lRow)) Then
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                MessageBox.Show("Cannot allocate transactions for more than one Company", "Error")
                                Return result
                            End If
                        End If

                        m_lReturn = g_oBusiness.MarkTransaction(v_lTransactionID:=lTransDetailID, v_iCurrencyID:=iCurrencyID, v_lCompanyID:=CInt(m_vSearchData(ACISourceID, lRow)), v_cPayment:=cAmount)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Set the mouse pointer back to normal
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return result
                        End If
                        'Set the icons to the check

                        'Developer Guide no.49
                        oListItem.ImageKey = ACIconCheck

                        'Developer Guide No.126
                        'oListItem.Icon = ACIconCheck
                        m_lMarkedItems += 1
                        m_iAllocationCompany_id = CInt(m_vSearchData(ACISourceID, lRow))
                        m_iAllocationCurrency_id = CInt(m_vSearchData(ACICurrencyId, lRow))

                        ' Update the status
                        m_vSearchData(ACIMarkedStatus, lRow) = gPMConstants.PMEReturnCode.PMTrue
                        m_vSearchData(ACIMarkedAmount, lRow) = cAmount

                        m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=iCurrencyID, vCurrencyAmount:=cAmount, vFormattedCurrency:=sFormattedAmount, vConversionDate:=m_vSearchData(ACIAccountingDate, lRow))
                        ListViewHelper.GetListViewSubItem(oListItem, ACListMarkedAmount).Text = sFormattedAmount
                        oList.Refresh()
                    End If
                End If

            Next iLoop1

            ' Update Debits marked for payment
            UpdateTotalMarked()




            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMark Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMark", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateTotalMarked
    '
    ' Description: Updates the total marked value
    '
    '
    '
    ' ***************************************************************** '
    Private Function UpdateTotalMarked() As Integer

        Dim result As Integer = 0
        Dim cDebitTotal, cCreditTotal As Decimal
        Dim sFormattedAmount As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cDebitTotal = 0
            cCreditTotal = 0

            ' Exit if no data
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Get the currency sign
            'iCurrencyID = m_iAllocationCurrency_id

            'm_lReturn& = g_oBusiness.GetSymbolForCurrency(v_iCurrencyID:=iCurrencyID, _
            'r_sSymbol:=sSymbol$)
            'If (m_lReturn& <> PMTrue) Then
            '    sSymbol = "Pounds Sterling"
            'End If

            ' Get the total
            For lLoop1 As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CInt(m_vSearchData(ACIMarkedStatus, lLoop1)) = gPMConstants.PMEReturnCode.PMTrue Then
                    If (CDec(m_vSearchData(ACIMarkedAmount, lLoop1))) > 0 Then
                        cDebitTotal += CDec(m_vSearchData(ACIMarkedAmount, lLoop1))
                    Else
                        cCreditTotal += CDec(m_vSearchData(ACIMarkedAmount, lLoop1))
                    End If
                End If
            Next lLoop1

            'DD 09/07/2003: Changed to match Account Balance

            m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=m_iAllocationCurrency_id, vCurrencyAmount:=cDebitTotal, vFormattedCurrency:=sFormattedAmount, vConversionDate:=DateTime.Today)

            panDebitsMarked.Name = sFormattedAmount


            m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=m_iAllocationCurrency_id, vCurrencyAmount:=cCreditTotal, vFormattedCurrency:=sFormattedAmount, vConversionDate:=DateTime.Today)

            panCreditsMarked.Name = sFormattedAmount

            'panDebitsMarked.Caption = sSymbol$ & Trim$(FormatField(iFormatType:=PMFormatCurrency, vFieldValue:=cDebitTotal))
            'panCreditsMarked.Caption = "-" & sSymbol$ & Trim$(FormatField(iFormatType:=PMFormatCurrency, vFieldValue:=cCreditTotal))


            ' Store the total marked
            m_cTotalDebitsMarked = cDebitTotal
            m_cTotalCreditsMarked = cCreditTotal

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTotalMarked Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTotalMarked", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: IsValidWriteOff
    '
    ' Description: Process WriteOff
    '
    ' ***************************************************************** '
    Private Function IsValidWriteOff(ByRef r_bValidWriteOff As Boolean, ByRef r_cWriteOffLimit As Decimal, ByRef r_sWriteOffCurrency As String) As Integer
        Dim result As Integer = 0
    
        Dim oUserAuthorities As bACTUserAuthorities.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Initialise Variables.
            r_bValidWriteOff = False
            r_cWriteOffLimit = 0
            r_sWriteOffCurrency = ""

            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:="ClientManager")
            oUserAuthorities = temp_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oUserAuthorities.ValidateAmounts(v_iCurrencyID:=m_iAllocationCurrency_id, v_cAmount:=Math.Abs(m_cTotalDebitsMarked + m_cTotalCreditsMarked), v_lCompanyID:=m_iAllocationCompany_id, r_vWriteOffValid:=r_bValidWriteOff, r_cAuthorityAmount:=r_cWriteOffLimit, r_sCurrency:=r_sWriteOffCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in IsValidWriteOff", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValidWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessWriteOff
    '
    ' Description: Process WriteOff
    '
    '
    ' ***************************************************************** '
    Private Function ProcessWriteOff() As Integer
        Dim result As Integer = 0
        Dim oWriteOff As frmWriteOff
        Dim sMsg As String = ""
        Dim r_bValidWriteOff As Boolean
        Dim r_cWriteOffLimit As Decimal
        Dim r_sWriteOffCurrency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = IsValidWriteOff(r_bValidWriteOff:=r_bValidWriteOff, r_cWriteOffLimit:=r_cWriteOffLimit, r_sWriteOffCurrency:=r_sWriteOffCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Failed to get write off amounts", "Error")
                Return result
            End If

            If Not r_bValidWriteOff Then
                sMsg = "Your write off limit does not allow you to write off the difference." & Environment.NewLine & _
                       "Difference : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cTotalDebitsMarked + m_cTotalCreditsMarked)) & Environment.NewLine & _
                       "Your write off limit : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(r_cWriteOffLimit)) & " " & r_sWriteOffCurrency

                MessageBox.Show(sMsg, "Error", MessageBoxButtons.OK)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Load the jolly write off form

            oWriteOff = New frmWriteOff()

            oWriteOff.DebitAmount = CStr(m_cTotalDebitsMarked)
            oWriteOff.CreditAmount = CStr(m_cTotalCreditsMarked)

            ' Initialise it
            m_lReturn = CType(oWriteOff, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Display it
            oWriteOff.ShowDialog()

            ' Set it back to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get values here

            m_lWriteOffReason_id = oWriteOff.WriteOffReasonID

            If oWriteOff.Status <> gPMConstants.PMEReturnCode.PMOK Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove from memory
            oWriteOff = Nothing

            ' Set it back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process write off", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'eck240102End


    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Sub DisableInterface(ByRef bDisable As Boolean)

        Try

            ' Add commands here eg.
            cmdFindNow.Enabled = Not bDisable
            cmdNewSearch.Enabled = Not bDisable

            If m_bIsBatch Then
                cmdNewSearch.Enabled = False
            End If

            ' Always disable these if the results listview is empty
            If lvwSearchDebits.Items.Count = 0 Then
                bDisable = True
            End If
            If lvwSearchCredits.Items.Count = 0 Then
                bDisable = True
            End If

            cmdOK.Enabled = Not bDisable

            '    cmdFindAccTrans.Enabled = Not bDisable
            '    cmdFindDocTrans.Enabled = Not bDisable

            '    cmdEdit.Enabled = Not bDisable

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString))
            End If

            ' Display the status message.
            stbStatus.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.

            If txtAccountCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            If cmbDocTypeGroup.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbDocTypeGroup, cmbDocTypeGroup.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If cmbDocumentType.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbDocumentType, cmbDocumentType.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If cmbPeriod.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbPeriod, cmbPeriod.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtDateFrom.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDateTo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            If cboPMUser.UserID <> 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If



            If cboDepartment.ListIndex <> 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub CheckMandatoryEnable()

        ' Check mandatory and enable the Find Now button accordingly
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)

            ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(975)

            tabMain.Width = Me.Width - VB6.TwipsToPixelsX(1560)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            TabResults.Width = Me.Width - VB6.TwipsToPixelsX(375)
            TabResults.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(TabResults.Top) - 1210)
            lvwSearchCredits.Width = TabResults.Width - VB6.TwipsToPixelsX(500)
            lvwSearchDebits.Width = TabResults.Width - VB6.TwipsToPixelsX(500)
            lvwSearchCredits.Height = TabResults.Height - VB6.TwipsToPixelsY(750)
            lvwSearchDebits.Height = TabResults.Height - VB6.TwipsToPixelsY(750)

            cmdAllocate.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            cmdMark.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdWriteOff.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: FindNow
    '
    ' Description: Get the interface details from the query
    '
    ' ***************************************************************** '
    Private Sub FindNow()

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDebits.Items.Count > 1 Then
                If lvwSearchDebits.Visible Then
                    ' Set the focus.
                    lvwSearchDebits.Focus()
                    'Developer Guide No.185
                    lvwSearchDebits_ItemClick(lvwSearchDebits.SelectedItems.Item(0))
                End If
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the FindNow command", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: GetPeriodLookup
    '
    ' Description: Gets a period details and populates a combo
    '
    ' ***************************************************************** '
    Private Function GetPeriodLookup() As Integer

        Dim result As Integer = 0
        Dim lPeriodID As Integer
        Dim iCompanyID As Integer
        Dim sYearName, sPeriodName As String
        Dim dtPeriodEndDate As Date
        Dim sLookupDesc As String = ""
        Dim lTodayPeriodID As Integer
        Dim iTodayPeriodListIndex As Integer
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            Dim cmbPeriod_NewIndex As Integer = -1
            cmbPeriod_NewIndex = cmbPeriod.Items.Add(sLookupDesc)
            VB6.SetItemData(cmbPeriod, cmbPeriod_NewIndex, -1)

            If cboSubBranch.SelectedIndex >= 0 Or Not (m_bMultiTreeAccounting) Then

                If m_bMultiTreeAccounting Then
                    lSubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                    'MKW 210303 PN2447 Changed v_vSubBranchID to vSubBranchID

                    m_lReturn = m_oACTPeriod.GetPeriodForDate(dtDateInPeriod:=DateTime.Today, lPeriodID:=lTodayPeriodID, vYearName:=sYearName, vSubBranchID:=lSubBranchID)
                Else

                    m_lReturn = m_oACTPeriod.GetPeriodForDate(dtDateInPeriod:=DateTime.Today, lPeriodID:=lTodayPeriodID, vYearName:=sYearName)
                End If


                m_lReturn = m_oACTPeriod.GetDetails(vYearName:=sYearName)

                While m_lReturn = gPMConstants.PMEReturnCode.PMTrue


                    m_lReturn = m_oACTPeriod.GetNext(vPeriodID:=lPeriodID, vCompanyID:=iCompanyID, vYearName:=sYearName, vPeriodName:=sPeriodName, vPeriodEndDate:=dtPeriodEndDate)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        sLookupDesc = sYearName.Trim() & ": " & sPeriodName
                        cmbPeriod_NewIndex = cmbPeriod.Items.Add(sLookupDesc)
                        VB6.SetItemData(cmbPeriod, cmbPeriod_NewIndex, lPeriodID)
                        If lPeriodID = lTodayPeriodID Then
                            iTodayPeriodListIndex = cmbPeriod_NewIndex
                        End If

                    End If
                End While

                'developer guide no.28
                cmbPeriod.SelectedIndex = 0
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function GetAllocationDetails() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the object if needed



        Return g_oBusiness.GetAllocationDetails(v_lAllocationId:=m_lAllocationID)



        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Allocation Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' PRIVATE Methods (End)

    Private Sub cboDepartment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDepartment.Click
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    Private Sub cboPMUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMUser.Click
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopulateAccountCode
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function PopulateAccountCode(Optional ByRef r_lAccountID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sAccountCode As String = ""
        Dim lAccountID As Integer
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of Find Account if needed
            If m_oFindAccount Is Nothing Then

                Dim temp_m_oFindAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindAccount = temp_m_oFindAccount
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iACTFindAccount.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            ' Get the account code
            sAccountCode = txtAccountCode.Text


            ReDim vKeyArray(1, 2)

            ' Set the keys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyAllowStoppedAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_bAllowStopped


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = sAccountCode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyDisallowInsurerAndAgentAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = True


            m_lReturn = m_oFindAccount.SetKeys(vKeyArray)


            m_lReturn = m_oFindAccount.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of iACTFindAccount.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            ' If they didn't cancel then store the new data

            If m_oFindAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                sAccountCode = m_oFindAccount.ShortCode

                lAccountID = m_oFindAccount.AccountID
                txtAccountCode.Text = sAccountCode
                txtAccountCode.Tag = CStr(lAccountID)

                If Not False Then
                    r_lAccountID = lAccountID
                End If

            End If

            ' Remove the instance as it's not working a second time...

            m_oFindAccount.Dispose()
            m_oFindAccount = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateAccountCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private isInitializingComponent As Boolean
    Private Sub cboSubBranch_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSubBranch.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'Refresh the form
        GetPeriodLookup()

        ClearDisplay()
    End Sub

    Private Sub cmbCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbCurrency.Click
        ClearDisplay()
    End Sub

    Private Sub cmdAccountCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountCode.Click

        m_lReturn = PopulateAccountCode()

    End Sub


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenhelpID)

    End Sub



    Private Sub cmdMark_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMark.Click

        m_lReturn = ProcessMark()

    End Sub

    Private Sub cmdWriteOff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWriteOff.Click
        Dim lRow As Integer
        Select Case SSTabHelper.GetSelectedIndex(tabMain)
            Case 0

                lRow = Convert.ToString(lvwSearchDebits.Items.Item(lvwSearchDebits.FocusedItem.Index).Tag)
            Case 1

                lRow = Convert.ToString(lvwSearchDebits.Items.Item(lvwSearchCredits.FocusedItem.Index).Tag)
        End Select

        m_lWriteOffTransdetail_id = CInt(m_vSearchData(ACITransDetailId, lRow))

        m_lReturn = ProcessWriteOff()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to Process the Write Off Form", "Error")

            'Reset the WriteOffTransDetail_id variable
            m_lWriteOffTransdetail_id = 0

            Exit Sub
        End If

        m_cWriteOffAmount = m_cTotalDebitsMarked + m_cTotalCreditsMarked

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Dim iStartTab As Integer = 0

            ' Show the start tab
            SSTabHelper.SetSelectedIndex(tabMain, iStartTab)


        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the object parameters to default values
            '    cmbDocTypeGroup.ListIndex = -1
            '    cmbDocumentType.ListIndex = -1
            '    cmbCurrency.ListIndex = -1

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTAllocate.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:="ClientManager")
            m_oCurrencyConvert = temp_m_oCurrencyConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Check if there is sufficient search criteria

            ' {* USER DEFINED CODE (Begin) *}

            m_bFirstTime = True

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim sBalanceChk As String = ""

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide No.7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            If Not (m_oFindAccount Is Nothing) Then

                m_oFindAccount.Dispose()
                m_oFindAccount = Nothing
            End If

            sBalanceChk = "1"

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACTFindTransBalanceOption, v_sSettingValue:=sBalanceChk)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Doesnt really matter if it doesnt save the setting. It's not critical.
            End If

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

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Forms unload event.

        Try

            ' Terminate the general object.
		m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            If Not (m_oCurrencyConvert Is Nothing) Then

                m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
            End If

            'Inform the Interface
            m_lReturn = m_oInterface.OnForm_Unload()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Destroy the interface
            m_oInterface = Nothing

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown, iShiftDown, iNewTab As Integer
        Dim bProcessed As Boolean = False
        Dim bTabChanged As Boolean = False

        Const ACShiftMask As Integer = 1
        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iShiftDown = (Shift And ACShiftMask) > 0
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMain
                iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            iNewTab = 0
                            ' New tab must be visible
                            Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                                iNewTab += 1
                            Loop
                        Else
                            Do
                                ' If we are on the first tab.
                                If iNewTab = 0 Then
                                    ' Display the last tab.
                                    iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                                Else
                                    ' Display the previous tab.
                                    iNewTab -= 1
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                        End If
                        bProcessed = True
                        bTabChanged = True

                    Case Keys.PageDown
                        ' Page Down key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                            ' New tab must be visible
                            Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                                iNewTab -= 1
                            Loop
                        Else
                            Do
                                ' If we are on the last tab.
                                If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
                                    ' Display the first tab.
                                    iNewTab = 0
                                Else
                                    ' Display the next tab.
                                    iNewTab += 1
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                        End If
                        bProcessed = True
                        bTabChanged = True

                    Case Keys.Home
                        ' Home key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus to the first control on the tab.
                            If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, iNewTab).Focus()
                            End If
                            bProcessed = True
                        End If

                    Case Keys.End
                        ' End key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus to the last control on the tab.
                            If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, iNewTab).Focus()
                            End If
                            bProcessed = True
                        End If
                End Select
                ' Change tabs?
                If bTabChanged Then
                    SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
                End If
            End With

            ' If the key was processed
            If bProcessed Then
                KeyCode = 0
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub frmInterface_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown, iShiftDown, iNewTab As Integer
        Dim bProcessed As Boolean = False
        Dim bTabChanged As Boolean = False

        Const ACShiftMask As Integer = 1
        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iShiftDown = (Shift And ACShiftMask) > 0
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMain
                iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.Tab
                        ' Tab key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            Do
                                ' Check if the shift key has also been pressed.
                                If iShiftDown Then
                                    ' If we are on the first tab.
                                    If iNewTab = 0 Then
                                        ' Display the last tab.
                                        iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
                                    Else
                                        ' Display the previous tab.
                                        iNewTab -= 1
                                    End If
                                Else
                                    ' Check we are not on the last tab.
                                    If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
                                        ' Display the first tab.
                                        iNewTab = 0
                                    Else
                                        ' Display the next tab.
                                        iNewTab += 1
                                    End If
                                End If
                                ' New tab must be visible
                            Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
                            bProcessed = True
                            bTabChanged = True
                        End If
                End Select
                ' Change tabs?
                If bTabChanged Then
                    SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
                End If
            End With

            ' If the key was processed
            If bProcessed Then
                KeyCode = 0
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = ResizeInterface()
    End Sub

    Private Sub lvwSearchCredits_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchCredits.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchCredits.Columns(eventArgs.Column)

        Try

            If lvwSearchCredits.Items.Count > 0 Then

                lvwSearchCredits.Items.Item(0).EnsureVisible()
                ' Column click event for the search details
                ' Defer to the common interface
                OnColumnClick(lvwSearchCredits, ColumnHeader)
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort columns", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchCredits_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub lvwSearchCredits_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchCredits.DoubleClick
        ProcessMark()
    End Sub

    Private Sub lvwSearchCredits_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchCredits.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim sReturn As String = ""
        Dim cReturn As Decimal
        Dim sOutstanding As String = ""
        Dim cOutstanding As Decimal
   
        Dim bOK As Boolean

        Try
            If Button = MouseButtonConstants.RightButton Then

                'Developer Guide No.49
                If lvwSearchCredits.SelectedItems.Item(0).ImageKey = ACIconBlank Then
                    bOK = False
                    'Developer Guide No.52
                    sOutstanding = lvwSearchCredits.FocusedItem.SubItems(ACListOSCurrencyAmount).Text
                    sOutstanding = sOutstanding.Substring(0, sOutstanding.IndexOf(" "c) + 1)
                    cOutstanding = CDec(sOutstanding)
                    While Not bOK
                        'Developer Guide No.52
                        sReturn = lvwSearchCredits.FocusedItem.SubItems(ACListOSCurrencyAmount).Text
                        sReturn = Interaction.InputBox("Enter Mark Amount", "Mark Input", sReturn)
                        If sReturn.IndexOf(" "c) >= 0 Then
                            sReturn = sReturn.Substring(0, sReturn.IndexOf(" "c) + 1)
                        End If
                        If sReturn <> "" Then
                            cReturn = CDec(sReturn)
                            'eck030502 oustanding must be < 0
                            '                   If cOutstanding > 0 And cReturn >= cOutstanding Then
                            'TF250903 - PN6939 - Improve validation
                            'If cOutstanding < 0 And cReturn >= cOutstanding Then
                            If cOutstanding < 0 And cReturn >= cOutstanding And cReturn < 0 Then
                                bOK = True
                                m_lReturn = ProcessMark(vMarkAmount:=cReturn)
                            End If
                        Else
                            bOK = True

                        End If
                    End While
                End If
            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Private Sub lvwSearchDebits_ItemClick(ByVal Item As ListViewItem)

      
        ' Item click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDebits.Items.Count = 0 Then
                Exit Sub
            End If


            If m_bFirstTime Then
                m_bFirstTime = False

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDebits_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged

        Try


            With tabMain
                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMain) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    If m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMain)).Enabled Then
                        m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMain)).Focus()
                    End If
                End If

            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process tabMain_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="tabMain_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            tabMainPreviousTab = tabMain.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

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

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

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

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Find Now button.

        ' Set focus to find now
        cmdFindNow.Focus()

        ' Perform the search
        FindNow()

        ' Set focus back to a control
        Select Case SSTabHelper.GetSelectedIndex(tabMain)
            Case 0

                If txtAccountCode.Enabled Then
                    txtAccountCode.Focus()
                End If
            Case 1
                cmbDocTypeGroup.Focus()
            Case Else
                ' If something weird happened then set to the first tab
                SSTabHelper.SetSelectedIndex(tabMain, 0)
                txtAccountCode.Focus()
        End Select

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub cmdAllocate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAllocate.Click
      
        Dim iCompanyID, iCurrencyID, iDocTypeGroupId, iDocumentTypeID As Integer
        Dim lPeriodID As Integer
        Dim dtDateFrom, dtDateTo As Date
        Dim sOperatorName, sDepartment As String

        Dim r_bValidWriteOff As Boolean
        Dim r_cWriteOffLimit As Decimal
        Dim r_sWriteOffCurrency As String = ""

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If Not Information.IsArray(m_vSearchData) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            If m_cTotalDebitsMarked = 0 And m_cTotalCreditsMarked = 0 Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show("Please select items to Allocate", Application.ProductName)
                Exit Sub
            End If

            If m_lWriteOffTransdetail_id = 0 Then
                If Math.Round(m_cTotalDebitsMarked, 2) + Math.Round(m_cTotalCreditsMarked, 2) <> 0 Then
                    m_lReturn = m_lReturn = IsValidWriteOff(r_bValidWriteOff:=r_bValidWriteOff, r_cWriteOffLimit:=r_cWriteOffLimit, r_sWriteOffCurrency:=r_sWriteOffCurrency)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        MessageBox.Show("Unable to Write Off - Allocation Debits and Credits do not match please adjust", Application.ProductName)
                        Exit Sub
                    End If
                    If Not r_bValidWriteOff Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        MessageBox.Show("Allocation Debits and Credits do not match and exceeed write off limit", Application.ProductName)
                        Exit Sub
                    Else
                        If Math.Round(m_cTotalDebitsMarked, 2) > (Math.Round(m_cTotalCreditsMarked, 2) * -1) Then
                            SSTabHelper.SetSelectedIndex(TabResults, 0)
                            lvwSearchDebits.Focus()
                            MessageBox.Show("Allocation Debits exceed Credits  - Choose a debit transaction to writeoff", Application.ProductName)
                        Else
                            SSTabHelper.SetSelectedIndex(TabResults, 1)
                            lvwSearchCredits.Focus()
                            MessageBox.Show("Allocation Credits exceed Debits - Choose a credit transaction to writeoff", Application.ProductName)
                        End If
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If
            End If


            g_oBusiness.CompanyID = m_iAllocationCompany_id

            g_oBusiness.CurrencyID = m_iAllocationCurrency_id

            g_oBusiness.AccountID = m_lAccountID

            g_oBusiness.WriteOffTransdetailID = m_lWriteOffTransdetail_id

            g_oBusiness.WriteOffReasonID = m_lWriteOffReason_id

            g_oBusiness.WriteOffAmount = m_cWriteOffAmount
            '    m_lReturn = g_oBusiness.Allocate()

            iCompanyID = g_iCompanyID
            iCurrencyID = cmbCurrency.ItemId

            iDocTypeGroupId = VB6.GetItemData(cmbDocTypeGroup, cmbDocTypeGroup.SelectedIndex)
            iDocumentTypeID = VB6.GetItemData(cmbDocumentType, cmbDocumentType.SelectedIndex)
            lPeriodID = VB6.GetItemData(cmbPeriod, cmbPeriod.SelectedIndex)

            If Information.IsDate(txtDateFrom.Text) Then
                dtDateFrom = CDate(txtDateFrom.Text)
            Else
                dtDateFrom = #12/30/1899#
            End If

            If Information.IsDate(txtDateTo.Text) Then
                dtDateTo = CDate(txtDateTo.Text)
            Else
                dtDateTo = #12/30/1899#
            End If


            If cboPMUser.UserID <> 0 Then
                sOperatorName = cboPMUser.ItemUsername
            Else
                sOperatorName = ""
            End If

            sDepartment = cboDepartment.ItemCaption
            If sDepartment.Trim().ToLower() = "(all)" Then
                sDepartment = ""
            End If
            'eck PN4594  pass allocating company

            m_lReturn = g_oBusiness.Allocate(v_lCompanyID:=m_iAllocationCompany_id, v_lAccountID:=m_lAccountID, v_CurrencyID:=iCurrencyID, v_iDocTypeGroup:=iDocTypeGroupId, v_iDocumentTypeID:=iDocumentTypeID, v_lPeriodID:=lPeriodID, v_dtDateFrom:=dtDateFrom, v_dtDateTo:=dtDateTo, v_sOperatorName:=sOperatorName, v_sDepartment:=sDepartment, v_bMultiTreeAccounting:=m_bMultiTreeAccounting)

            m_lWriteOffTransdetail_id = 0
            m_lWriteOffReason_id = 0

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to Allocate Transactions", "Error")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            Else
                MessageBox.Show("Allocated Sucessfully", "Message")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If


            FindNow()

        Catch excep As System.Exception



            ' Error Section.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Allocate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAllocate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDebits_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDebits.DoubleClick

        ' Double click event for the search details.

        'On Error GoTo Err_lvwSearchDebitsDblClick

        ' Check if there are any items available.
        'If (lvwSearchDebits.ListItems.Count = 0) Then
        '    Exit Sub
        'End If

        'If lvwSearchDebits.SelectedItem.Selected = False Then
        '    Exit Sub 'was selected but then deselected as invalid selection
        'End If

        ' Set the interface status.
        'm_lStatus& = PMOK

        ' Process the next set of actions.
        'm_lReturn& = m_oGeneral.ProcessCommand()

        ' Check the return value.
        'If (m_lReturn& = PMTrue) Then
        ' Everything OK, so we can hide the interface.
        '    Me.Hide
        'End If

        'DD 09/07/2003: Changed to perform mark/unmark
        ProcessMark()

        Exit Sub



        ' Error Section.

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDebits_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub lvwSearchDebits_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDebits.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

            ' Allow find based on a selected trans
            '    cmdFindAccTrans.Enabled = True
            '    cmdFindDocTrans.Enabled = True

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDebits_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub lvwSearchDebits_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDebits.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDebits_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDebits_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDebits.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDebits.Columns(eventArgs.Column)

        Try

            If lvwSearchDebits.Items.Count > 0 Then

                lvwSearchDebits.Items.Item(0).EnsureVisible()
                ' Column click event for the search details
                ' Defer to the common interface
                OnColumnClick(lvwSearchDebits, ColumnHeader)
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort columns", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDebits_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDebits_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDebits.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim sReturn As String = ""
        Dim cReturn As Decimal
        Dim sOutstanding As String = ""
        Dim cOutstanding As Decimal
      
        Dim bOK As Boolean

        Try
            If Button = MouseButtonConstants.RightButton Then

                'Developer Guide No.49
                If lvwSearchDebits.SelectedItems.Item(0).ImageKey = ACIconBlank Then
                    bOK = False
                    'Developer Guide no.52
                    sOutstanding = lvwSearchDebits.FocusedItem.SubItems(ACListOSCurrencyAmount).Text
                    sOutstanding = sOutstanding.Substring(0, sOutstanding.IndexOf(" "c) + 1)
                    cOutstanding = CDec(sOutstanding)
                    While Not bOK
                        'Developer Guide no.52
                        sReturn = lvwSearchDebits.FocusedItem.SubItems(ACListOSCurrencyAmount).Text
                        sReturn = Interaction.InputBox("Enter Mark Amount", "Mark Input", sReturn)
                        If sReturn.IndexOf(" "c) >= 0 Then
                            sReturn = sReturn.Substring(0, sReturn.IndexOf(" "c) + 1)
                        End If
                        If sReturn <> "" Then
                            cReturn = CDec(sReturn)
                            'TF250903 - PN6939 - Improve validation
                            'If cOutstanding > 0 And cReturn <= cOutstanding Then
                            If cOutstanding > 0 And cReturn <= cOutstanding And cReturn > 0 Then
                                bOK = True
                                m_lReturn = ProcessMark(vMarkAmount:=cReturn)
                            End If
                        Else
                            bOK = True

                        End If
                    End While
                End If
            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub txtAccountCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If



        txtAccountCode.Tag = ""

        CheckMandatoryEnable()

        ClearDisplay()
        '    End If

    End Sub

    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter

        If Strings.Len(txtAccountCode.Text) > 0 Then
            ' CF100100 - Fixed problem
            If (Convert.ToString(txtAccountCode.Tag)) <> "" Then
                m_lSaveAccountID = CInt(Convert.ToString(txtAccountCode.Tag))

                txtAccountCode.SelectionStart = 0
                txtAccountCode.SelectionLength = Strings.Len(txtAccountCode.Text)
            End If
        End If

    End Sub
    Private Sub cmbDocTypeGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocTypeGroup.SelectedIndexChanged
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    Private Sub cmbDocumentType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbDocumentType.SelectedIndexChanged
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    Private Sub cmbPeriod_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbPeriod.SelectedIndexChanged
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    Private Sub txtAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Leave
        '    If txtAccountCode.Text <> "" Then
        '        m_lReturn& = GetAccountFromShort(v_sShortCode:=(Trim$(txtAccountCode.Text)), r_lAccountID:=m_lAccountID)
        '     End If

    End Sub

    Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDateFrom)
    End Sub

    Private Sub txtDateFrom_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub

    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDateTo)
    End Sub

    Private Sub txtDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()

        ClearDisplay()
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (txtCurrencyAmount_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtCurrencyAmount_Change()
    'CheckMandatoryEnable()
    'End Sub


    ' PRIVATE Events (End)
End Class
