Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "iACTFindPaymentMaintenance"
    Public b_RunChecked As Boolean = False
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
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
	
	Public Const ACReceiptsDocumentRef As Integer = 6
	Public Const ACReceiptsClientCode As Integer = 8
	Public Const ACReceiptsClientName As Integer = 9
	Public Const ACReceiptsPolicyNumber As Integer = 10
	Public Const ACReceiptsMediaType As Integer = 11
	Public Const ACReceiptsMediaReference As Integer = 12
	Public Const ACReceiptsChequeStatus As Integer = 14
	
	'Public Const ACPaymentCashListItemID = 0
	'Public Const ACPaymentClientCode = 15
	'Public Const ACPaymentPolicyHolder = 16
	'Public Const ACPaymentPolicyClaimNumber = 17
	'Public Const ACPaymentAmount = 8
	'Public Const ACPaymentPaymentDate = 21
	'Public Const ACPaymentMediaReference = 7
	'Public Const ACPaymentPaymentStatus = 9
	'Public Const ACPaymentCancelReason = 27
	'Public Const ACPaymentCancellationDate = 28
	'Public Const ACPaymentAccountNumber = 2
	'Public Const ACPaymentBankSortCode = 26
	'Public Const ACPaymentBranchCode = 25
	'Public Const ACPaymentBankAccount = 22
	'Public Const ACPaymentBatchReference = 12
	'Public Const ACPaymentTheirReference = 18
	'Public Const ACPaymentOurReference = 19
	'Public Const ACPaymentDocumentReference = 23
	'Public Const ACPaymentPaymentType = 24
	'Public Const ACPaymentMediaType = 5
	'Public Const ACPaymentUser = 20
	'Public Const ACPaymentPayeeName = 1
	'Public Const ACPaymentReverseReasonID = 29
	'Public Const ACPaymentAllowReverseAllocation = 30
	'Public Const ACPaymentReverseAllocationDays = 31
	'
	'Public Const ACPaymentTransDetailID = 32
	'Public Const ACPaymentPartyCnt = 33
	'Public Const ACPaymentInsuranceFileCnt = 34
	'Public Const ACPaymentClaimId = 35
	'Public Const ACPaymentTranCurrencyId = 36
	'Public Const ACPaymentBankReconcilationDate = 38
	'Public Const ACPaymentAccountCode = 40
	'Public Const ACPaymentAccountBankBranchCode = 41
	
	Public Const ACMaintainMediaTypeStatusSentForClearance As Integer = 1
	Public Const ACMaintainMediaTypeStatusCleared As Integer = 2
	Public Const ACMaintainMediaTypeStatusBounced As Integer = 3
	
	
	Public Const kReceiptMaintColHIndexDocumentRef As Integer = 1
	Public Const kReceiptMaintColHIndexBranch As Integer = 2
	Public Const kReceiptMaintColHIndexClientCode As Integer = 3
	Public Const kReceiptMaintColHIndexClientName As Integer = 4
	Public Const kReceiptMaintColHIndexPolicyNumber As Integer = 5
	Public Const kReceiptMaintColHIndexMediaType As Integer = 6
	Public Const kReceiptMaintColHIndexMediaReference As Integer = 7
	Public Const kReceiptMaintColHIndexDrawnBankName As Integer = 8
	Public Const kReceiptMaintColHIndexMediaTypeStatus As Integer = 9
	Public Const kReceiptMaintColHIndexCashlistitemId As Integer = 10
	Public Const kReceiptMaintColHIndexMediaTypeId As Integer = 11
	Public Const kReceiptMaintColHIndexMediaTypeStatusId As Integer = 12
	Public Const kReceiptMaintColHIndexInsuranceFileId As Integer = 13
	Public Const kReceiptMaintColHIndexUpdatedDate As Integer = 14
	Public Const kReceiptMaintColHIndexComments As Integer = 15
	
	
	
	Public Enum enuSelMediaTypeStatusFields
		enuSelMTSF_CASHLISTITEM_ID = 0
		enuSelMTSV_INSURANCEFILE_ID = 1
		enuSelMTSV_MEDIATYPE_ID = 2
		enuSelMTSV_MEDIATYPE_CODE = 3
		enuSelMTSV_MEDIATYPESTATUS_ID = 4
		enuSelMTSV_MEDIATYPESTATUS_CODE = 5
		enuSelMTSV_DOCUMENT_REF = 6
		enuSelMTSV_BRANCH = 7
		enuSelMTSV_CLIENT_CODE = 8
		enuSelMTSF_CLIENT_NAME = 9
		enuSelMTSV_POLICY_NUMBER = 10
		enuSelMTSV_MEDIATYPE = 11
		enuSelMTSV_MEDIAREFERENCE = 12
		enuSelMTSV_DRAWN_BANK_NAME = 13
		enuSelMTSV_MEDIATYPESTATUS = 14
		enuSelMTSV_NUMBER_OF_FIELDS = 15
	End Enum
	
	
	Public Enum enuUpdMediaTypeStatusFields
		enuUpdMTSF_CASHLISTITEM_ID = 0
		enuUpdMTSV_MEDIATYPE_ID = 1
		enuUpdMTSV_MEDIATYPESTATUS_ID = 2
		enuUpdMTSV_COMMENTS = 3
		enuUpdMTSV_USER_ID = 4
		enuUpdMTSV_DATE_MODIFIED = 5
		enuUpdMTSV_INSURANCEFILE_ID = 6
		enuUpdMTSV_DOCUMENT_REF = 7
		enuUpdMTSV_MAX_INDEX = enuUpdMediaTypeStatusFields.enuUpdMTSV_DOCUMENT_REF
	End Enum
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
    'developer guide no. 50
    'developer guide no. 107
    <ThreadStatic()> _
    Public objFrmInterface As frmInterface
    'developer guide no. 107
    <ThreadStatic()> _
    Public objfrmUpdateMediaTypeStatus As frmUpdateMediaTypeStatus
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
   
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oACTMaintainMediaTypeStatus As bACTMaintainMediaTypeStatus.Form
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oACTDocumentReversal As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_obACTCurrencyConvert As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_obACTFindTransaction As Object
	
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
            b_RunChecked = True
			With ListView
				
				Select Case lColumnHeaderIndex
                    'Case kReceiptMaintColHIndexBranch, kReceiptMaintColHIndexClientName, kReceiptMaintColHIndexPolicyNumber
                    '                   ListViewHelper.SetSortedProperty(ListView, False)
                    '	ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                    '	'Use the special sort function for numerics
                    '                   'ListViewFunc.ListViewSortByValue(ListView, lColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(ListView))

                    '                   ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                    '                   ListViewHelper.SetSortedProperty(ListView, True)
                    '	' If date column clicked, then sort by date sort column
					Case kReceiptMaintColHIndexUpdatedDate
						
                        m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
						
						'        Case kPaymentMaintColHIndexBankSortCode, kPaymentMaintColHIndexAccountNumber, kPaymentMaintColHIndexBankAccount
						'            m_lReturn& = ListViewSortByStringValue(v_oListView:=ListView, _
						''                               v_iSourceColumn:=lColumnHeaderIndex, _
						''                               v_iDirection:=(.SortOrder + 1) Mod 2)
					Case ListViewHelper.GetSortKeyProperty(ListView)
						' Set sort order opposite of
						' current direction.
                        'ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                        If ListViewHelper.GetSortOrderProperty(ListView) = 1 Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If
                        ListViewHelper.SetSortedProperty(ListView, True)
                    Case Else
                        ' Sort by this column (ascending).
                        'ListViewHelper.SetSortedProperty(ListView, False)

                        ' Turn off sorting so that the list
                        ' is not sorted twice
                        ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                        ListViewHelper.SetSortedProperty(ListView, True)

                End Select
                b_RunChecked = False
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
