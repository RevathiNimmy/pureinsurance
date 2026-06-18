Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129 
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    ' Date: 10/10/2000
    ' Description: Main interface.
    ' Edit History:
    '       MKR 08/02/2005 PN-18578 Cleared the value of m_cTotalSelected during new search
    ' ***************************************************************** '
    'Replace iPMFunc.GetResData with GetResData in the whole document
    'developer guide no.243
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' What solution are we running as part of?
    Private m_iSolutionConfig As Integer

    ' Instance of Find Transaction
    Private m_oFindTransaction As Object
    Private m_lAccountId As Integer
    Private m_sAccountCode As String = ""
    Private m_vTransactionArray As Object
    Private m_vSourceArray As Object
    Private m_iDrillCompany As Integer
    Private m_lPremiumFinanceCnt As Integer
    Private m_bAllowStopped As Boolean
    Private m_bIsAgent As Boolean

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBFinanceTransactions.General

    Private m_oFindAccount As Object

    ' Declare an instance of the nav starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Control array to store the first and last text box controls for each tab
    Private m_ctlTabFirstLast(,) As Control
    'At least one transaction has been selected
    Private m_bNoneSelected As Boolean
    'Insurance Ref as dictated by first selected ransaction
    Private m_lInsuranceFileCnt As Integer
    Private m_dAgentCommissionAmount As Double
    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    ' Stores the total Selected value
    Private m_cTotalSelected As Decimal
    Private m_sUnderwriting As String = ""
    Private m_lLeadAgentCnt As Integer

    'Thinh Nguyen 01/02/2004
    Private m_lPlanInsuranceFolderCnt As Integer
    Private m_bIsSingleInstalmentPlan As Boolean
    'SJ 25/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 25/02/2004 - end

    ' Stores the selected items from the list view.
    Private m_vSelectedItems() As Object

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


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Standard Property.
    ' Set the interface exit status.
    'm_lStatus = Value
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
    Public WriteOnly Property AccountCode() As String
        Set(ByVal Value As String)
            ' Set the object parameter value.
            m_sAccountCode = Value
        End Set
    End Property
    Public Property AccountID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lAccountId
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lAccountId = Value
        End Set
    End Property
    Public Property TransactionArray() As Object
        Get
            ' Return the Transaction Array
            Return m_vTransactionArray
        End Get
        Set(ByVal Value As Object)
            ' Set the valid sources for the user


            m_vTransactionArray = Value
        End Set
    End Property
    Public Property IsAgent() As Boolean
        Get
            ' Return the objects parameter value.
            Return m_bIsAgent
        End Get
        Set(ByVal Value As Boolean)
            ' Return the objects parameter value.
            m_bIsAgent = Value
        End Set
    End Property
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property
    Public Property DrillCompany() As Integer
        Get
            ' Return the objects parameter value.
            Return m_iDrillCompany
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_iDrillCompany = Value
        End Set
    End Property
    Public ReadOnly Property NavigatorTitle() As String
        Get
            ' Return the objects parameter value.
            Return m_sNavigatorTitle
        End Get
    End Property
    Public ReadOnly Property StepStatus() As String
        Get
            ' Standard Property.
            ' Return the Steps Status
            Return m_sStepStatus.Value
        End Get
    End Property
    Public Property PremiumFinanceCnt() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lPremiumFinanceCnt
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lPremiumFinanceCnt = Value
        End Set
    End Property
    Public ReadOnly Property InsuranceFileCnt() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lInsuranceFileCnt
        End Get
    End Property

    Public ReadOnly Property AgentCommissionAmount() As Double
        Get
            ' Return the objects parameter value.
            Return m_dAgentCommissionAmount
        End Get
    End Property

    Public WriteOnly Property PlanInsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPlanInsuranceFolderCnt = Value
        End Set
    End Property
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsSingleInstalmentPlan = Value
        End Set
    End Property


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

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim vAccountID As Decimal
        Dim dtDateTo As Object
        Dim iCompanyID As Integer
        Dim lRow, lFinancePlanCnt As Integer
        Dim vUserSource As Object = Nothing

        Dim vInsuranceFolderDetails(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayStatusFound()
                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}



            ' Storing the previously selected items in an array (by using Transdetail ID) - PN 18578
            m_vSelectedItems = Nothing
            If lvwSearchDetails.Items.Count > 0 Then
                For iLoop As Integer = 1 To lvwSearchDetails.Items.Count

                    lRow = Convert.ToString(lvwSearchDetails.Items.Item(iLoop - 1).Tag)

                    'developer guide no. 49
                    If lvwSearchDetails.Items(iLoop - 1).ImageKey = MainModule.ACIconCheck Then
                        If Not Information.IsArray(m_vSelectedItems) Then
                            ReDim m_vSelectedItems(0)
                        Else
                            ReDim Preserve m_vSelectedItems(m_vSelectedItems.GetUpperBound(0) + 1)
                        End If
                        m_vSelectedItems(m_vSelectedItems.GetUpperBound(0)) = gPMFunctions.ToSafeInteger(m_vSearchData(ACITransDetailId, lRow))
                    End If
                Next
            End If
            'PN 18578 - End

            If pnlAccountCode.Text.Trim() <> "" Then
                vAccountID = m_lAccountId
            Else
                vAccountID = 0
            End If

            If Trim(txtDateTo.Text) <> "" AndAlso Information.IsDate(txtDateTo.Text) Then

                dtDateTo = CDate(txtDateTo.Text)
            Else

                dtDateTo = 0
            End If

            m_vSearchData = Nothing
            iCompanyID = g_iSourceID



            ' get the insurance folder details

            m_lReturn = g_oBusiness.GetInsuranceFolderDetails(v_lInsuranceFolderCnt:=m_lPlanInsuranceFolderCnt, r_vResults:=vInsuranceFolderDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vInsuranceFolderDetails) Then

                txtPolicyNo.Text = CStr(vInsuranceFolderDetails(4, 0)).Trim()
            End If

            If txtPolicyNo.Text <> "" Then
                'get account id for lead agent on this policy
                If GetAccountID(v_sInsuranceRef:=txtPolicyNo.Text.Trim()) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    DisplayStatusFound()
                    Return result
                End If
            End If
            If m_bIsSingleInstalmentPlan Then
                txtPolicyNo.Text = ""
            End If


            If cboTransPolicy.SelectedIndex = 0 Then
                lFinancePlanCnt = m_lPremiumFinanceCnt
            Else
                lFinancePlanCnt = 0
            End If


            m_lReturn = g_oPremiumFinance.GetValidSources(r_vUserSource:=vUserSource)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'developer guide no. 8
            g_oBusiness.RestrictByCompany = True

            'developer guide no. 8
            g_oBusiness.IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan


            'developer guide no. 8
            m_lReturn = g_oBusiness.SelectTransQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_iCompanyID:=iCompanyID, v_vAccountID:=m_lAccountId, v_vDateTo:=dtDateTo, v_vOutstandingOnly:=1, v_vInsuranceRef:=txtPolicyNo.Text, v_vIsNewPF:=1, v_lFinancePlanCnt:=lFinancePlanCnt, v_vSourceArray:=vUserSource, v_bIncludeExcludedTaxRows:=False)

            'Reset selected flag
            m_bNoneSelected = True

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No found search details

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
        Dim oListItem As ListViewItem
        Dim lCurrencyID As Integer
        Dim sFormattedCurrency As String = String.Empty
        Dim sFormattedOSCurrency As String = String.Empty
        Dim cOsAmount As Decimal
        Dim vConversionDate As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            m_cTotalSelected = 0

            ' Check that search details are valid before continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If
            cmdSelectAll.Enabled = True

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If (Trim$(m_vSearchData(13, lRow)) = "TAX") And (m_vSearchData(68, lRow) = "0") Then
                    'Skip this tax row
                Else
                    ' Dont use any icon

                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACISourceID, lRow)).Trim(), ACIconBlank)

                    'Future proof code
                    If m_vSearchData.GetUpperBound(0) < ACIAlternateReference Then
                        'ListViewHelper.GetListViewSubItem(oListItem, ACListInsuranceRef).Text = CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()
                        oListItem.SubItems.Add(ACListInsuranceRef).Text = CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()
                    Else
                        If m_bIsUnderwritingBranch And CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim() <> "" Then
                            ' ListViewHelper.GetListViewSubItem(oListItem, ACListInsuranceRef).Text = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                            oListItem.SubItems.Add(ACListInsuranceRef).Text = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                        Else
                            ' ListViewHelper.GetListViewSubItem(oListItem, ACListInsuranceRef).Text = CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()
                            oListItem.SubItems.Add(ACListInsuranceRef).Text = CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim()
                        End If
                    End If
                    'ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text = CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim()
                    'ListViewHelper.GetListViewSubItem(oListItem, ACListAccountingDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACIAccountingDate, lRow))
                    'ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACIDocumentDate, lRow))
                    'ListViewHelper.GetListViewSubItem(oListItem, ACListPeriodName).Text = CStr(m_vSearchData(ACIPeriodName, lRow)).Trim()
                    oListItem.SubItems.Add(ACListDocumentRef).Text = CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim()
                    oListItem.SubItems.Add(ACListAccountingDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACIAccountingDate, lRow))
                    oListItem.SubItems.Add(ACListDocumentDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACIDocumentDate, lRow))
                    oListItem.SubItems.Add(ACListPeriodName).Text = CStr(m_vSearchData(ACIPeriodName, lRow)).Trim()

                    lCurrencyID = gPMFunctions.ToSafeInteger(m_vSearchData(ACICurrencyID, lRow))

                    cOsAmount = Conversion.Val(CStr(m_vSearchData(ACIOutstandingBaseAmount, lRow)))



                    'developer guide no. 8
                    m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAmountCurrencyID, lRow), vCurrencyAmount:=m_vSearchData(ACIBaseAmount, lRow), vFormattedCurrency:=sFormattedCurrency, vConversionDate:=vConversionDate)


                    'developer guide no. 8
                    m_lReturn = g_oBusiness.FormatCurrency(vCurrencyID:=m_vSearchData(ACIAmountCurrencyID, lRow), vCurrencyAmount:=cOsAmount, vFormattedCurrency:=sFormattedOSCurrency, vConversionDate:=vConversionDate)

                    'ListViewHelper.GetListViewSubItem(oListItem, ACListCurrencyAmount).Text = sFormattedCurrency.Trim()
                    'ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text = sFormattedOSCurrency.Trim()

                    'ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentTypeId).Text = CStr(m_vSearchData(ACIDocumentType, lRow)) 'Added by Nitesh for (PN-72036)Dt-24-05-2010' sLookupDesc
                    oListItem.SubItems.Add(ACListCurrencyAmount).Text = sFormattedCurrency.Trim()
                    oListItem.SubItems.Add(ACListOSCurrencyAmount).Text = sFormattedOSCurrency.Trim()

                    oListItem.SubItems.Add(ACListDocumentTypeId).Text = CStr(m_vSearchData(ACIDocumentType, lRow))

                    oListItem.Tag = CStr(lRow)

                    'Checking if the previously selected items still exists in the SearchData - PN 18578
                    If Information.IsArray(m_vSelectedItems) Then
                        For Each m_vSelectedItems_item As Object In m_vSelectedItems
                            If CStr(m_vSelectedItems_item) = CStr(m_vSearchData(ACITransDetailId, lRow)) Then

                                'developer guide no. 49
                                oListItem.ImageKey = MainModule.ACIconCheck

                                'developer guide no. 49
                                m_cTotalSelected += cOsAmount
                                Exit For
                            End If
                        Next m_vSelectedItems_item
                    End If
                End If
            Next

            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If
            ' Select the first item.
            lvwSearchDetails.Items.Item(0).Selected = True

            ' Size the columns
            ListView6Autosize(lvwList:=lvwSearchDetails, bSizeHeaders:=True)

            ' Show the list view
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewBatchEnd()
            txtTotalSelected.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTotalSelected).Trim()

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' DD 24/06/2003: Added Insurance File Cnt for Premium Finance
    ' DD 20/05/2004: Ensured the Outstanding Amount is passed through
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim vAddedArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vTransactionArray) Then
                m_vTransactionArray = Nothing
            End If

            For iLoop As Integer = 1 To lvwSearchDetails.Items.Count

                lRow = Convert.ToString(lvwSearchDetails.Items.Item(iLoop - 1).Tag)


                'Developer guide No 162
                If lvwSearchDetails.Items(iLoop - 1).ImageKey = MainModule.ACIconCheck Then
                    If Not Information.IsArray(m_vTransactionArray) Then
                        ReDim m_vTransactionArray(6, 0)
                    Else

                        ReDim Preserve m_vTransactionArray(6, m_vTransactionArray.GetUpperBound(1) + 1)
                    End If


                    m_vTransactionArray(0, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACITransDetailId, lRow)


                    m_vTransactionArray(1, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACIInsuranceRef, lRow)


                    m_vTransactionArray(2, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACIOutstandingBaseAmount, lRow)


                    'm_vTransactionArray(3, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACIDocInsuranceFileCnt, lRow)
                    m_vTransactionArray(3, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACISpare, lRow)


                    m_vTransactionArray(4, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACIDocumentTypeId, lRow)


                    'm_vTransactionArray(m_vSearchData(ACISpare, lRow), 5, m_vTransactionArray.GetUpperBound(1))
                    m_vTransactionArray(5, m_vTransactionArray.GetUpperBound(1)) = m_vSearchData(ACIDocInsuranceFileCnt, lRow)
                End If
            Next iLoop

            'PSL 25/02/2003 Added code so it automatically adds all the
            'transactions for this Selected Insurance File cnt.
            'MKW150104 PN9674 For Underwriting Only.


            For iLoop As Integer = 0 To m_vTransactionArray.GetUpperBound(1)
                For lRow = 0 To m_vSearchData.GetUpperBound(1)
                    'If this row in the grid is the same insurance file as the selected one
                    If m_lInsuranceFileCnt = gPMFunctions.ToSafeDouble(m_vSearchData(ACIDocInsuranceFileCnt, lRow)) Then
                        'check it is not the same row
                        If InArray(m_vSearchData(ACITransDetailId, lRow), m_vTransactionArray) = gPMConstants.PMEReturnCode.PMFalse _
                        And ((Trim$(m_vSearchData(ACISpare, lRow)) = "TAX") And (m_vSearchData(ACIMinimumCashListID, lRow) = "0")) <> True Then
                            'PN5205 - Exclude Commission transactions
                            If CStr(m_vSearchData(ACISpare, lRow)).Trim().ToUpper() <> "COMM" Then
                                'Its the same policy, but not the same row so add it to the list of additions
                                If Not Information.IsArray(vAddedArray) Then
                                    ReDim vAddedArray(3, 0)
                                Else

                                    ReDim Preserve vAddedArray(3, vAddedArray.GetUpperBound(1) + 1)
                                End If


                                vAddedArray(0, vAddedArray.GetUpperBound(1)) = m_vSearchData(ACITransDetailId, lRow)


                                vAddedArray(1, vAddedArray.GetUpperBound(1)) = m_vSearchData(ACIInsuranceRef, lRow)


                                vAddedArray(2, vAddedArray.GetUpperBound(1)) = m_vSearchData(ACIOutstandingBaseAmount, lRow)


                                vAddedArray(3, vAddedArray.GetUpperBound(1)) = m_vSearchData(ACIDocInsuranceFileCnt, lRow)
                            End If
                        End If
                    End If
                Next lRow
            Next iLoop


            'put the list of additions into the array to be passed onto quote
            If Information.IsArray(vAddedArray) Then

                For lRow = 0 To vAddedArray.GetUpperBound(1)
                    If Not Information.IsArray(m_vTransactionArray) Then
                        ReDim m_vTransactionArray(2, 0)
                    Else
                        'Tracy Richards 26/05/03 - Changed first dim from 2 to 3

                        ReDim Preserve m_vTransactionArray(3, m_vTransactionArray.GetUpperBound(1) + 1)
                    End If



                    m_vTransactionArray(0, m_vTransactionArray.GetUpperBound(1)) = vAddedArray(0, lRow)



                    m_vTransactionArray(1, m_vTransactionArray.GetUpperBound(1)) = vAddedArray(1, lRow)



                    m_vTransactionArray(2, m_vTransactionArray.GetUpperBound(1)) = vAddedArray(2, lRow)



                    m_vTransactionArray(3, m_vTransactionArray.GetUpperBound(1)) = vAddedArray(3, lRow)
                Next
            End If
            Return result

        Catch excep As System.Exception


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
        Try


            ' Get the lookup values.

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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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
        Dim sSection As String = String.Empty
        Dim sResult As String = String.Empty
        Dim sAppName As String = String.Empty
        Dim vDefault As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Get what solution we're part of

            'developer guide no. 8
            m_lReturn = g_oSirConfig.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=ACTConst.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(ACTConst.ACTOrionSolutionMBP)
            End If

            m_iSolutionConfig = CInt(sResult)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

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

            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwSearchDetails.FullRowSelect = True
            lvwSearchDetails.Handle.ToInt32()
            ' {* USER DEFINED CODE (Begin) *}
            cmdSelectAll.Enabled = False
            '
            ' Size the columns to the size of the headers
            'developer guide no. 170
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwSearchDetails), gPMConstants.PMEReturnCode)

            ' Default the date
            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)
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
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If
            cmdSelectAll.Enabled = False

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to clear all of the interface details
            ' for a new search.
            '
            ' Example:-
            '
            '    txtName.Text = ""
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)
            txtTotalSelected.Text = "0.00"
            txtPolicyNo.Text = ""

            m_cTotalSelected = 0

            ' Set focus to the search details.

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdSelect.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

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

            For iLoop1 As Integer = ACListTitle1 To ACListTitle9



                lvwSearchDetails.Columns.Item(iLoop1 - ACListTitle1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=iLoop1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Next iLoop1

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
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdSelect.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

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

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

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

            If pnlAccountCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDateTo.Text.Trim() <> "" Then
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
    ' PRIVATE Methods (End)

    '*************************************************************************
    ' Name:         ProcessSelect
    ' Description:  Processes the users selection
    ' History:
    '*************************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (ProcessSelect) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ProcessSelect(ByRef bSelectAll As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim lRow As Integer
    'Dim sOSAmount, sMessage As String
    'Dim cOsAmount As Decimal
    'Dim bMixedCurrenciesSelected As Boolean
    'Dim iFirstRow, iLastRow As Integer
    '
    'Static lFirstCurrencyID As Integer
    '
    'Try 
    '
    'Initialise variables
    'result = gPMConstants.PMEReturnCode.PMTrue
    'bMixedCurrenciesSelected = False
    '
    ' Set the mouse pointer to busy
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    '
    'First Loop through the items. See if those selected are from
    'multiple Insurance Policies (using Insurance Ref)
    'If bSelectAll Then
    'm_cTotalSelected = 0
    'End If
    '
    'iFirstRow = 1
    'iLastRow = lvwSearchDetails.Items.Count
    '
    'For 'iCurrentRow As Integer = iFirstRow To iLastRow
    'change the individual item
    'Get the Object for this item
    'If lvwSearchDetails.Items.Item(iCurrentRow - 1).Selected Or bSelectAll Then
    'oListItem = lvwSearchDetails.Items.Item(iCurrentRow - 1)
    'With oListItem
    'sOSAmount = ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text
    '
    'cOsAmount = gPMFunctions.ConvertCurrencyStringToValue(sOSAmount)

    'If .SmallIcon = ACIconCheck And Not bSelectAll Then
    ' Set the icons to off

    '.SmallIcon = 0

    '.Icon = 0
    'm_cTotalSelected -= cOsAmount
    'Else
    ' Set the icons to checked

    '.SmallIcon = ACIconCheck

    '.Icon = ACIconCheck
    'm_cTotalSelected += cOsAmount
    '
    'If this is the first to be selected, set the
    'Currency ID. Subsequent Transactions must
    'have the same base currency
    'If lFirstCurrencyID = 0 Then
    'lFirstCurrencyID = CInt(m_vSearchData(ACIAmountCurrencyID, lRow))
    'Else
    'If CDbl(m_vSearchData(ACIAmountCurrencyID, lRow)) <> lFirstCurrencyID Then
    'bMixedCurrenciesSelected = True
    'Exit For
    'End If
    'End If
    'End If
    'End With
    'End If
    'Next iCurrentRow
    '
    'If bMixedCurrenciesSelected Then
    'Get the string from the resource file

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACDiffCurrencies, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    'MessageBox.Show(sMessage, "More than one Branch currency chosen", MessageBoxButtons.OK, MessageBoxIcon.Error)
    'End If
    '
    'Reset selected Flag
    'm_bNoneSelected = (m_cTotalSelected = 0)
    '
    'Enable / Disable OK key based on if any items are selected
    'cmdOK.Enabled = Not m_bNoneSelected
    '
    'Show that sum of all selected transactions in title
    'txtTotalSelected.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTotalSelected).Trim()
    '
    ' Set the mouse pointer back to normal
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    ' Let's the listview refresh (this is a Microsoft Bug in Listview 6)
    'ListViewBatchStart(lvwSearchDetails)
    'lvwSearchDetails.Visible = False
    'lvwSearchDetails.Visible = True
    'ListViewBatchEnd()
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    ' Log Error Message
    'iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ProcessSelect Failed", ACApp, ACClass, "ProcessSelect", Information.Err().Number, excep.Message)
    'Return result
    'End Try
    'End Function

    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click
        m_lReturn = CType(ProcessSelectUnderwriting(True), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
        m_lReturn = CType(ProcessSelectUnderwriting(False), gPMConstants.PMEReturnCode)
    End Sub


    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                ' Tell the resizer control about the controls on the form
                With uctPMResizer

                    .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdSelect", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdFindNow", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdNewSearch", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("imgImage", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lvwSearchDetails", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblPolicyNo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblAccountCode", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalSelectedLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtTotalSelected", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("pnlAccountCode", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtPolicyNo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdSelectAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("stbStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTransPolicy", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cboTransPolicy", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .FormMinHeight = 5685
                    .FormMinWidth = 9675

                End With

                Exit Sub

            Catch excep As System.Exception



                ' Error Section
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBFinanceTransactions.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.

            m_lReturn = m_oGeneral.Initialise(Me, g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Thinh Nguyen 19/02/2002 (start)

            If g_oPremiumFinance.GetHiddenOption(r_sValue:=m_sUnderwriting) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            m_sUnderwriting = m_sUnderwriting.ToUpper()
            'Thinh Nguyen 19/02/2002 (end)

            'SJ 25/02/2004 - start
            m_lReturn = CType(CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                Exit Sub
            End If
            'SJ 25/02/2004 - end

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
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check if the search contains more or equal
            ' to the miniumum search length.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}
            pnlAccountCode.Text = m_sAccountCode
            pnlAccountCode.Tag = CStr(m_lAccountId)

            If m_sTransactionType = "NB" Then
                cboTransPolicy.SelectedIndex = 1
                cboTransPolicy.Enabled = False
            Else
                cboTransPolicy.SelectedIndex = 0
            End If

            If pnlAccountCode.Text.Trim().Length < ACMinSearchLength Then
                ' Because of the search length, we can't
                ' continue with the search.

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            ' DD 04/02/2002 - Only auto-search for non-Agents.
            ' Agents can have huge amounts of transactions so give
            ' the user an opportunity to filter by Policy No first.
            'Thinh Nguyen 20/02/2002 - only do this if its not underwriting
            'Start Girija - PN 57282

            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            'End Girija - PN 57282
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            Me.Refresh()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the find transaction if needed
            If Not (m_oFindTransaction Is Nothing) Then
                ' Terminate the instance

                m_oFindTransaction.Dispose()
                ' Remove the instance
                m_oFindTransaction = Nothing
            End If


            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate it
            If Not (m_oNavStart Is Nothing) Then
                m_oNavStart.Dispose()
                ' Remove the instance
                m_oNavStart = Nothing
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
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70

        If lvwSearchDetails.GetItemAt(eventArgs.X, eventArgs.Y) Is Nothing Then
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: CheckEnableButtons
    '
    ' Description: Enables the Select button if any items are selected.
    '
    ' History: 08/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckEnableButtons() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check each list item
            For iLoop1 As Integer = 1 To lvwSearchDetails.Items.Count
                ' If it's selected then
                If lvwSearchDetails.Items.Item(iLoop1 - 1).Selected Then
                    ' Enable the Select button
                    cmdSelect.Enabled = True
                    Exit For
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckEnableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckEnableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchDetails_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        ' Check if anything was selected so as to enable the right
        ' button(s)
        m_lReturn = CType(CheckEnableButtons(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose

        ' Re-enable the controls
        lvwSearchDetails.Enabled = True

        cmdFindNow.Enabled = True
        cmdNewSearch.Enabled = True

        cmdOK.Enabled = True
        cmdCancel.Enabled = True
        cmdHelp.Enabled = True
        ' Gets the interface details to be displayed.
        m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                'If (.Tab < cmdNext.Count) Then
                '    cmdNext(.Tab).Default = True
                'Else
                '    cmdOK.Default = True
                'End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim lRow As Integer = 0
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Comment below code to allow Posting SEC Trancasction.
            'If (m_cTotalSelected <= 0 AndAlso m_lPremiumFinanceCnt = 0) OrElse (m_cTotalSelected <= 0 AndAlso m_lPremiumFinanceCnt <> 0 AndAlso (m_vSearchData IsNot Nothing AndAlso gPMFunctions.ToSafeInteger(m_vSearchData(ACIPolicyTypeId, lRow)) = 8)) Then
            '    MsgBox("Unable to proceed. Total Selected amount is negative")
            '    Exit Sub
            'End If

            If lvwSearchDetails.Items.Count = 0 Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Me.Hide()
                Exit Sub
            End If

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            'eck160701
            If Not Information.IsArray(m_vTransactionArray) Then
                MessageBox.Show("Please select transactions for inclusion", Application.ProductName)
                Exit Sub
            End If


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
            If lvwSearchDetails.Items.Count = 0 Then
                Me.Hide()
                Exit Sub
            End If

            ' Process the next set of actions.
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

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            cmdSelectAll.Enabled = False

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)

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

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.
        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Call the edit button function
            If cmdSelect.Enabled Then
                cmdSelect_Click(cmdSelect, New EventArgs())
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave
        Try
            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwSearchDetails
                ' If current sort column header is pressed.

                Select Case ColumnHeader.Index + 1
                    ' Sort the date columns
                    Case 4, 5
                        'developer guide no. 170
                        m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2), gPMConstants.PMEReturnCode)
                        'Document Reference
                    Case 3
                        m_lReturn = CType(ListViewSortByCode(v_iSourceColumn:=2, v_iDirection:=(ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2), gPMConstants.PMEReturnCode)

                    Case Else
                        If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                            ' Set sort order opposite of
                            ' current direction.
                            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                        Else
                            ' Sort by this column (ascending).
                            ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                            ' Turn off sorting so that the list is not sorted twice
                            ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                            ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                            ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                        End If
                End Select
            End With

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Function ListViewSortByCode(ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Try

            'Return success
            result = gPMConstants.PMEReturnCode.PMTrue
            With lvwSearchDetails
                ' Not sorted yet
                ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                ' Sort now
                ListViewHelper.SetSortOrderProperty(lvwSearchDetails, v_iDirection)
                ' Set the sort key
                ListViewHelper.SetSortKeyProperty(lvwSearchDetails, v_iSourceColumn)
                ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
            End With
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ListViewSortByCode Failed", ACApp, ACClass, "ListViewSortByCode", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private isInitializingComponent As Boolean
    Private Sub txtDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub
    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
        ' Hightlight any text.
        iPMFunc.SelectText(txtDateTo)
    End Sub

    '************************************************************
    ' Name : GetAccountIDForPolicy
    '
    ' Desc : get account ID for lead agent on this policy
    '
    ' Hist : Thinh Nguyen 20/02/2002 - created
    '************************************************************
    Private Function GetAccountID(ByVal v_sInsuranceRef As String) As Integer

        Dim result As Integer = 0
        Dim lSalesAccountId, lPurchaseAccountId, lInsurerAccountId, lAgentAccountId, lFeeAccountId, lCommissionAccountId, lDiscountAccountId, lPremiumFinanceAccountId, lSubAGentAccountId, lNominalAccountId, lOtherPartyPayAccountId, lOtherPartyRecAccountId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If g_oPremiumFinance.GetLeadAgent(v_sInsuranceRef:=v_sInsuranceRef, r_lAgentCnt:=m_lLeadAgentCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' if lead agent is zero then this must be a direct policy
            ' m_lAccountID = client
            ' otherwise go and get m_lAccountID for lead agent
            If m_lLeadAgentCnt <> 0 Then

                m_lReturn = g_oOrionLink.GetAccountIDs(r_lSalesAccountID:=lSalesAccountId, r_lPurchaseAccountID:=lPurchaseAccountId, r_lInsurerAccountID:=lInsurerAccountId, r_lAgentAccountID:=lAgentAccountId, r_lFeeAccountID:=lFeeAccountId, r_lCommissionAccountID:=lCommissionAccountId, r_lDiscountAccountID:=lDiscountAccountId, r_lPremiumFinanceAccountID:=lPremiumFinanceAccountId, r_lSubAgentAccountID:=lSubAGentAccountId, r_lNominalAccountID:=lNominalAccountId, r_lOtherPartyPayAccountID:=lOtherPartyPayAccountId, r_lOtherPartyRecAccountID:=lOtherPartyRecAccountId, v_vPartyCnt:=m_lLeadAgentCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lAccountId = lSalesAccountId + lPurchaseAccountId + lInsurerAccountId + lAgentAccountId + lFeeAccountId + lCommissionAccountId + lDiscountAccountId + lPremiumFinanceAccountId + lSubAGentAccountId + lNominalAccountId + lOtherPartyPayAccountId + lOtherPartyRecAccountId
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '************************************************************
    ' Name : InArray
    '
    ' Desc : checks if a row has already been selected
    '
    ' Hist : PSL 25/02/2003 - created
    Private Function InArray(ByRef vSearchValue As Object, ByRef vArray(,) As Object) As Integer

        Try

            Dim lFound As gPMConstants.PMEReturnCode

            lFound = gPMConstants.PMEReturnCode.PMFalse


            For n As Integer = 0 To vArray.GetUpperBound(1)


                If vArray(0, n).Equals(vSearchValue) Then
                    lFound = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next


            Return lFound

        Catch

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    '*************************************************************************
    ' Name:         ProcessSelectUnderwriting
    ' Description:  Selects all the selected transactions
    ' History:      05/09/1999 CTAF - Created.
    '   EK  31-01-00 New Option to Select All transactions
    '   JAS 10-10-02 Only display OK button when something is selected
    '   TR  06-01-03 Only allows selection of transactions with same Insurance
    '                Reference
    '*************************************************************************
    Private Function ProcessSelectUnderwriting(ByVal v_bSelectAll As Boolean) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lRow As Integer
        Dim sOSAmount, sMessage As String
        Dim cOsAmount As Decimal
        Dim bMixedInsurancePoliciesSelected As Boolean

        Try

            'TR - Initialise variables
            result = gPMConstants.PMEReturnCode.PMTrue
            bMixedInsurancePoliciesSelected = False

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'TR - First Loop through the items. See if those selected are from
            'multiple Insurance Policies (using Insurance Ref)



            For iItemNumber As Integer = 1 To lvwSearchDetails.Items.Count
                'TR - Only process this item if it's been selected
                If (v_bSelectAll Or lvwSearchDetails.Items.Item(iItemNumber - 1).Selected) Then
                    If gPMFunctions.ToSafeString(m_vSearchData(ACISpare, lvwSearchDetails.Items.Item(iItemNumber - 1).Tag)).ToUpper().Trim() <> "COMM" Then

                        'TR - Get the Object for this item
                        oListItem = lvwSearchDetails.Items.Item(iItemNumber - 1)
                        'TR - Use this object
                        With oListItem
                            'TR - Get the row number

                            lRow = .Tag
                            'TR - Only process this if it's previous state was de-selected

                            'developer guide no. 49
                            If .ImageKey <> MainModule.ACIconCheck Then
                                'TR - If this is the first to be selected, set the
                                'Insurance file count. Subsequent Transactions must
                                'belong to the same insurance file
                                If m_bNoneSelected Then
                                    'TR - This first selected transaction sets the
                                    'InsuranceRef
                                    m_lInsuranceFileCnt = gPMFunctions.ToSafeInteger(m_vSearchData(ACIDocInsuranceFileCnt, lRow))
                                    If m_lInsuranceFileCnt = 0 Then
                                        'TR - Can't select this one (blank InsuranceFileCnt)
                                        m_bNoneSelected = True
                                    Else
                                        'TR - One has been selected now
                                        m_bNoneSelected = False
                                    End If
                                Else

                                    'TR - Check that the InsuranceFileCnt of this selected
                                    'transaction is the same as that of the first selected
                                    If m_lInsuranceFileCnt <> gPMFunctions.ToSafeDouble(m_vSearchData(ACIDocInsuranceFileCnt, lRow)) Then
                                        'TR - Make sure the message is shown at the end
                                        bMixedInsurancePoliciesSelected = True
                                    End If

                                End If
                            End If
                        End With
                    End If
                End If
            Next iItemNumber

            'TR - Only loop through for a 2nd time if it is OK to select all the
            'items that the user wants to
            m_dAgentCommissionAmount = 0

            If Not bMixedInsurancePoliciesSelected Then

                ' RAW 16/01/2004 : CQ3904 : added
                ' auto select all entries for the same insurance file
                For iItemNumber As Integer = 1 To lvwSearchDetails.Items.Count
                    'Only process this item if has not been selected
                    If Not lvwSearchDetails.Items.Item(iItemNumber - 1).Selected Then
                        ' select it if it is for the same insurance file count

                        lRow = lvwSearchDetails.Items.Item(iItemNumber - 1).Tag
                        If gPMFunctions.ToSafeDouble(m_vSearchData(ACIDocInsuranceFileCnt, lRow)) = m_lInsuranceFileCnt Then
                            If gPMFunctions.ToSafeString(m_vSearchData(ACISpare, lRow)).ToUpper().Trim() = "COMM" Then
                                m_dAgentCommissionAmount += ToSafeDouble(gPMFunctions.ToSafeString(m_vSearchData(ACICurrencyAmount, lRow)))
                            Else
                                lvwSearchDetails.Items.Item(iItemNumber - 1).Selected = True
                            End If

                        End If
                    End If
                Next
                ' RAW 16/01/2004 : CQ3904 : end


                'TR - Loop through all the items and change the icon to selected
                For iItemNumber As Integer = 1 To lvwSearchDetails.Items.Count
                    'TR - Only process this item if it's been selected
                    If (v_bSelectAll Or lvwSearchDetails.Items.Item(iItemNumber - 1).Selected) AndAlso gPMFunctions.ToSafeString(m_vSearchData(ACISpare, lvwSearchDetails.Items.Item(iItemNumber - 1).Tag)).ToUpper().Trim() <> "COMM" Then

                        'TR - Get the Object for this item
                        oListItem = lvwSearchDetails.Items.Item(iItemNumber - 1)
                        'TR - Use this object
                        With oListItem
                            'TR - Get the row number

                            lRow = .Tag
                            'TR - Get the total amount for this transaction
                            sOSAmount = ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text
                            'AAB 5/20/2003 based on Danny Davis advise use the function to get the value
                            'PSL 25/02/2003 The $ sign at the fron stopped currency conversion
                            '                    If Left$(sOSAmount, 1) < "0" Or Left$(sOSAmount, 1) > "9" Then
                            '                        sOSAmount = Right$(sOSAmount, Len(sOSAmount) - 1)
                            '                    End If
                            'cOsAmount = CCur(Left(sOSAmount, InStr(sOSAmount, " ")))
                            cOsAmount = gPMFunctions.ConvertCurrencyStringToValue(sOSAmount)

                            'developer guide no. 49
                            If .ImageKey = MainModule.ACIconCheck Then
                                ' Set the icons to off

                                'developer guide no. 49
                                .ImageKey = MainModule.ACIconBlank

                                'developer guide no. 49
                                .ImageKey = MainModule.ACIconBlank
                                m_cTotalSelected -= cOsAmount
                            Else
                                ' Set the icons to checked

                                'developer guide no. 49
                                .ImageKey = MainModule.ACIconCheck

                                'developer guide no. 49
                                m_cTotalSelected += cOsAmount
                            End If
                        End With
                    End If
                Next iItemNumber
            Else
                'TR - Show a message but not for "Select All"
                If Not v_bSelectAll Then
                    If bMixedInsurancePoliciesSelected Then
                        'TR - Get the string from the resource file

                        sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACDiffInsRefs, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        MessageBox.Show(sMessage, "Select Finance Plan Transactions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            End If

            'TR - Reset selected Flag
            m_bNoneSelected = True
            'TR - Loop through the items again to see if any have been selected (
            'by the icon type)
            For iItemNumber As Integer = 1 To lvwSearchDetails.Items.Count
                oListItem = lvwSearchDetails.Items.Item(iItemNumber - 1)

                'developer guide no. 49
                If oListItem.ImageKey = MainModule.ACIconCheck Then
                    m_bNoneSelected = False
                    Exit For
                End If
            Next iItemNumber

            'TR - Enable / Disable OK key based on if any items are selected
            cmdOK.Enabled = Not m_bNoneSelected

            'TR - Show that sum of all selected transactions in title
            txtTotalSelected.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTotalSelected).Trim()

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ProcessSelectUnderwriting Failed", ACApp, ACClass, "ProcessSelectUnderwriting", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub frmInterface_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If isInitializingComponent Then
            Exit Sub
        End If

        Try
            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)
            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3100)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdSelect.Top = Me.Height - VB6.TwipsToPixelsY(1150)
            cmdSelectAll.Top = Me.Height - VB6.TwipsToPixelsY(1150)

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            ' cmdFindNow.Top = Me.Height - VB6.TwipsToPixelsY(3500)

            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            '  cmdNewSearch.Top = Me.Height - VB6.TwipsToPixelsY(3300)

        Catch




            Exit Sub
        End Try

    End Sub
    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.GotFocus
        If Trim(Len(txtDateTo.Text)) > 0 Then
            txtDateTo.Text = CDate(txtDateTo.Text)
        End If

    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave
        txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, txtDateTo.Text)
    End Sub
End Class