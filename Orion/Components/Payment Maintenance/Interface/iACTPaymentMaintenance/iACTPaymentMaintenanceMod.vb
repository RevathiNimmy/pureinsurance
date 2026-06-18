Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no 129
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "iACTFindPaymentMaintenance"
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	'Constants Payment Maintenance
	Public Const ACPaymentCashListItemID As Integer = 0
	Public Const ACPaymentClientCode As Integer = 15
	Public Const ACPaymentPolicyHolder As Integer = 16
	Public Const ACPaymentPolicyClaimNumber As Integer = 17
	Public Const ACPaymentAmount As Integer = 8
	Public Const ACPaymentPaymentDate As Integer = 21
	Public Const ACPaymentMediaReference As Integer = 7
	Public Const ACPaymentPaymentStatus As Integer = 9
	Public Const ACPaymentCancelReason As Integer = 27
	Public Const ACPaymentCancellationDate As Integer = 28
	Public Const ACPaymentAccountNumber As Integer = 2
	Public Const ACPaymentBankSortCode As Integer = 26
	Public Const ACPaymentBranchCode As Integer = 25
	Public Const ACPaymentBankAccount As Integer = 22
	Public Const ACPaymentBatchReference As Integer = 12
	Public Const ACPaymentTheirReference As Integer = 18
	Public Const ACPaymentOurReference As Integer = 19
	Public Const ACPaymentDocumentReference As Integer = 23
	Public Const ACPaymentPaymentType As Integer = 24
	Public Const ACPaymentMediaType As Integer = 5
	Public Const ACPaymentUser As Integer = 20
	Public Const ACPaymentPayeeName As Integer = 1
	Public Const ACPaymentReverseReasonID As Integer = 29
	Public Const ACPaymentAllowReverseAllocation As Integer = 30
	Public Const ACPaymentReverseAllocationDays As Integer = 31
	
	Public Const ACPaymentTransDetailID As Integer = 32
	Public Const ACPaymentPartyCnt As Integer = 33
	Public Const ACPaymentInsuranceFileCnt As Integer = 34
	Public Const ACPaymentClaimId As Integer = 35
	Public Const ACPaymentTranCurrencyId As Integer = 36
	Public Const ACPaymentBankReconcilationDate As Integer = 38
	Public Const ACPaymentAccountCode As Integer = 40
	Public Const ACPaymentAccountBankBranchCode As Integer = 41
	
	Public Const kPaymentMaintColHIndexClientCode As Integer = 0
	Public Const kPaymentMaintColHIndexPolicyHolder As Integer = 1
	Public Const kPaymentMaintColHIndexPolicyClaimNumber As Integer = 2
    Public Const kPaymentMaintColHIndexAmount As Integer = 3
    'changes as in listview kPaymentMaintColHIndexPaymentDate is not coming at index 4
    Public Const kPaymentMaintColHIndexPaymentDate As Integer = 4
    'Public Const kPaymentMaintColHIndexPaymentDate As Integer = 23
	Public Const kPaymentMaintColHIndexMediaReference As Integer = 5
	Public Const kPaymentMaintColHIndexPaymentStatus As Integer = 6
	Public Const kPaymentMaintColHIndexCancelReason As Integer = 8
	Public Const kPaymentMaintColHIndexCancellationDate As Integer = 9
	Public Const kPaymentMaintColHIndexAccountNumber As Integer = 10
	Public Const kPaymentMaintColHIndexBankSortCode As Integer = 11
	Public Const kPaymentMaintColHIndexBranchCode As Integer = 12
	Public Const kPaymentMaintColHIndexBankAccount As Integer = 13
	Public Const kPaymentMaintColHIndexBatchReference As Integer = 14
	Public Const kPaymentMaintColHIndexTheirReference As Integer = 15
	Public Const kPaymentMaintColHIndexOurReference As Integer = 16
	Public Const kPaymentMaintColHIndexDocumentReference As Integer = 17
	Public Const kPaymentMaintColHIndexPaymentType As Integer = 18
	Public Const kPaymentMaintColHIndexMediaType As Integer = 19
	Public Const kPaymentMaintColHIndexUser As Integer = 20
	Public Const kPaymentMaintColHIndexPayeeName As Integer = 21
    'changes as in listview ReverseReasonID is not coming at index 22
    Public Const kPaymentMaintColHIndexReverseReasonID As Integer = 22
    'Public Const kPaymentMaintColHIndexReverseReasonID As Integer = 31
	Public Const kPaymentMaintColHIndexAllowReverseAllocation As Integer = 23
    'changes as in listview ReverseAllocationDays is not coming at index 33
    Public Const kPaymentMaintColHIndexReverseAllocationDays As Integer = 24
    'Public Const kPaymentMaintColHIndexReverseAllocationDays As Integer = 33
	Public Const kPaymentMaintColHIndexCashListItemID As Integer = 25
	Public Const kPaymentMaintColHIndexTransDetailID As Integer = 26
	Public Const kPaymentMaintColHIndexPartyCnt As Integer = 27
	Public Const kPaymentMaintColHIndexInsuranceFileCnt As Integer = 28
	Public Const kPaymentMaintColHIndexClaimId As Integer = 29
	Public Const kPaymentMaintColHIndexCurrencyId As Integer = 30
	Public Const kPaymentMaintColHIndexBankReconcilationDate As Integer = 7
	
	'Constants CancelPayment
	Public Const ACCancelPaymentPolicyClaimNo As Integer = 15
	Public Const ACCancelPaymentDocumentRef As Integer = 14
	Public Const ACCancelPaymentDocumentType As Integer = 7
	Public Const ACCancelPaymentAmount As Integer = 5
	Public Const ACCancelPaymentTransactionDate As Integer = 2
	Public Const ACCancelPaymentPolicyNumber As Integer = 8
	
	Public Const kCancelPaymentColHIndexPolicyClaimNo As Integer = 0
	Public Const kCancelPaymentColHIndexDocumentRef As Integer = 2
	Public Const kCancelPaymentColHIndexDocumentType As Integer = 3
	Public Const kCancelPaymentColHIndexAmount As Integer = 1
	Public Const kCancelPaymentColHIndexTransactionDate As Integer = 4
	
	Public Const ACAllowReverse As Integer = 60
	Public Const ACReverseDays As Integer = 61
	Public Const ACEventTypeId As Integer = 0
	Public Const ACHasPaymentAuthority As Integer = 10
	Public Const ACPaymentCurrencyId As Integer = 16
	Public Const ACPaymentAmt As Integer = 11
	
	' Cancel Payment of Event Type
	Public Const ACEventCode As String = "CNCLPMT"
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 250
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCompanyID As Integer
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.

    'archana 123 todolist 
    'Public g_oBusiness As bACTPaymentMaintenance.Form
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oACTCashListBusiness As bACTFindCashListItem.Form
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oACTDocumentReversal As bACTDocumentReversal.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_obACTCurrencyConvert As bACTCurrencyConvert.Form
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_obACTFindTransaction As bACTFindTransaction.Business
	
	Public g_iCurrencyID As Integer
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 51000
	

	Public Sub Main()
		
	End Sub
	Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)
		
		Try 
			
			Dim lColumnHeaderIndex As Integer
			Dim m_lReturn As gPMConstants.PMEReturnCode
			
			lColumnHeaderIndex = ColumnHeader.Index + 1 - 1
			
			With ListView
				
				
				Select Case lColumnHeaderIndex
					Case kPaymentMaintColHIndexAmount
                        ListViewHelper.SetSortedProperty(ListView, False)
                        If (ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending) Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If

						'Use the special sort function for numerics
                        ListViewFunc.ListViewSortByValue(ListView, lColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(ListView))
						
						' If date column clicked, then sort by date sort column
					Case kPaymentMaintColHIndexBankReconcilationDate, kPaymentMaintColHIndexCancellationDate, kPaymentMaintColHIndexPaymentDate
                        If (ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending) Then
                            m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=SortOrder.Descending), gPMConstants.PMEReturnCode)
                        Else
                            m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=SortOrder.Ascending), gPMConstants.PMEReturnCode)
                        End If
                        'm_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
						
						'        Case kPaymentMaintColHIndexBankSortCode, kPaymentMaintColHIndexAccountNumber, kPaymentMaintColHIndexBankAccount
						'            m_lReturn& = ListViewSortByStringValue(v_oListView:=ListView, _
						''                               v_iSourceColumn:=lColumnHeaderIndex, _
						''                               v_iDirection:=(.SortOrder + 1) Mod 2)
					Case ListViewHelper.GetSortKeyProperty(ListView)
						' Set sort order opposite of
						' current direction.
                        If (ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending) Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If
					Case Else
						' Sort by this column (ascending).
						ListViewHelper.SetSortedProperty(ListView, False)
						
						' Turn off sorting so that the list
						' is not sorted twice
                        If (ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending) Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If
						ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
						ListViewHelper.SetSortedProperty(ListView, True)
				End Select
				
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module
