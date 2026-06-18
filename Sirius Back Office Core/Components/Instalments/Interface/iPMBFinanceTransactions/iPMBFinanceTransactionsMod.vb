Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02/09/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBFinanceTransactions"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	Public Const ACListTitle3 As Integer = 104
	Public Const ACListTitle4 As Integer = 105
	Public Const ACListTitle5 As Integer = 106
	Public Const ACListTitle6 As Integer = 107
	Public Const ACListTitle7 As Integer = 108
	Public Const ACListTitle8 As Integer = 109
	Public Const ACListTitle9 As Integer = 110
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACSelectButton As Integer = 204
	Public Const ACDrillButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACNoSelected As Integer = 308
	Public Const ACDiffCurrencies As Integer = 309
	Public Const ACDiffInsRefs As Integer = 310
	
	' Menus
	' Constants for the lookup table array indexes.
	Public Const ACLDocumentType As Integer = 0
	Public Const ACLDocTypeGroup As Integer = 1
	Public Const ACLMax As Integer = 1
	
	' Constants for the search data array indexes.
	Public Const ACListSourceID As Integer = 0
	Public Const ACListInsuranceRef As Integer = 1
	Public Const ACListDocumentRef As Integer = 2
	Public Const ACListAccountingDate As Integer = 3
	Public Const ACListDocumentDate As Integer = 4
	Public Const ACListPeriodName As Integer = 5
	Public Const ACListCurrencyAmount As Integer = 6
	Public Const ACListOSCurrencyAmount As Integer = 7
	Public Const ACListDocumentTypeId As Integer = 8
	Public Const ACListTransDetailId As Integer = 9
	
	' Icon
	Public Const ACIconCheck As String = "check"
	'Public Const ACIconBlank = 0
	Public Const ACIconBlank As String = "blank" '--(RC)
	
	' Constants for data array from bACTFindTransaction.
	Public Const ACIDocumentRef As Integer = 0
	Public Const ACIAccountingDate As Integer = 1
	Public Const ACIPeriodName As Integer = 2
	Public Const ACICurrencyAmount As Integer = 3
	Public Const ACIPrimarySettled As Integer = 4
	Public Const ACIMatchedCurrencyAmount As Integer = 5
	Public Const ACIDocumentTypeId As Integer = 6
	Public Const ACIDocTypeGroupId As Integer = 7
	Public Const ACIInsuranceRef As Integer = 8
	Public Const ACIOperatorName As Integer = 9
	Public Const ACIPurchaseInvoiceNo As Integer = 10
	Public Const ACIPurchaseOrderNo As Integer = 11
	Public Const ACIDepartment As Integer = 12
	Public Const ACISpare As Integer = 13
	Public Const ACIAccountShortCode As Integer = 14
	Public Const ACIAccountId As Integer = 15
	Public Const ACICurrencyID As Integer = 16
	Public Const ACITransDetailId As Integer = 17
	Public Const ACIBaseAmount As Integer = 18
	Public Const ACIDocumentSequence As Integer = 19
	Public Const ACIDocumentDate As Integer = 20
	Public Const ACISourceID As Integer = 21
	Public Const ACIMatchAmount As Integer = 22
	Public Const ACIMatchDate As Integer = 23
	Public Const ACIReason As Integer = 24
	Public Const ACIInsuredName As Integer = 25
	Public Const ACIInsuredAccount As Integer = 26
	Public Const ACIFlag As Integer = 27
	Public Const ACIDocInsuranceFileCnt As Integer = 28
	Public Const ACIDocDocumentID As Integer = 29
	Public Const ACIAuditSetID As Integer = 30
	Public Const ACIAuditSetUserID As Integer = 31
	Public Const ACITransCurrencyBaseXRate As Integer = 32
	Public Const ACIPayeeName As Integer = 33
	Public Const ACIAlternateReference As Integer = 34
	Public Const ACIPolicyTypeId As Integer = 35
	Public Const ACIComment As Integer = 36
	Public Const ACINotReported As Integer = 37
	Public Const ACIUnderwritingYear As Integer = 38
	Public Const ACIMediaType As Integer = 39
	Public Const ACICurrencyText As Integer = 40
	Public Const ACIAmountCurrencyText As Integer = 41
	Public Const ACIAmountCurrencyID As Integer = 42
	Public Const ACIAccountCurrencyID As Integer = 43
	Public Const ACIAccountAmount As Integer = 44
	Public Const ACIOutstandingBaseAmount As Integer = 45
	Public Const ACIOutstandingAccountAmount As Integer = 46
	Public Const ACIAmountUpdated As Integer = 47
	Public Const ACIOutstandingTransAmount As Integer = 48
    Public Const ACIDocumentType As Integer = 62 'Added by Nitesh for (PN-72036)Dt-24-05-2010
    Public Const ACIMinimumCashListID As Integer = 68
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
    ' Company ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCompanyID As Integer
	'
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
	

    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As bACTFindTransaction.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oOrionLink As bSirOrionLink.Form
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPremiumFinance As bSIRPremiumFinance.Business
	' Instance of SolutionConfig

    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oSirConfig As bSIRSolutionConfig.Business
	'EK 130300
    ' Public instance of account business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oAccount As Object
	
	' Stores the return value for a function call.
	Private m_lReturn As Integer
	' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object = Nothing
    Private m_vLookupDetails(,) As Object = Nothing
	Private m_vLedgers As Object
	' ***************************************************************** '
	' Name: GetLookupRow
	'
	' Description: Converts a lookup table name to its matching row index
	'              in the table of lookup values.
	'              May be used to indirect GetLookupDetails, GetLookupDesc.
	'              Returns -1 if no match found
	'
	' ***************************************************************** '
	Public Function GetLookupRow(ByRef sLookupTable As String) As Integer
		
		Dim result As Integer = 0
		Dim lRow As Integer
		Dim bFoundMatch As Boolean
		
		Try 
			
			result = -1
			
			bFoundMatch = False
			

			For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
				' Check for a match of the table name.

                If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
			Next lRow
			
			If bFoundMatch Then
				result = lRow
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to match lookup table", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' Global application functions
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'
	' ***************************************************************** '
	Public Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			

			ReDim m_vLookupValues(3, ACLMax)
			
			' Setup Lookup Table Names

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocumentType) = ACTConst.ACTLookupDocumentType

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocTypeGroup) = ACTConst.ACTLookupDocTypeGroup
			
			' Do not supply a key
            For i As Integer = 0 To ACLMax
                m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
            Next i
			
			' Get all of the lookup values with the correct
			' effective date.

			m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
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
	'Public Function GetLookupDetails( _
	''    lLookupRow As Long, _
	''    ctlLookup As Control, _
	''    Optional ByVal vAllTypes As Variant) As Long
	'
	'Dim lCntr As Long
	'
	'    On Error GoTo Err_GetLookupDetails
	'
	'    GetLookupDetails = PMTrue
	'
	'    ' Check if there has been a table match.
	'    If (lLookupRow& = -1) Then
	'        GetLookupDetails = PMFalse
	'        Exit Function
	'    End If
	'
	'    ' Using the lookup values, populate the control with
	'    ' the details from the lookup details array.
	'
	'    If (IsMissing(vAllTypes) = False) Then
	'        If CBool(vAllTypes) Then
	'            ' First entry is all types (don't care)
	'            Dim sLookupDesc As String
	'            sLookupDesc = GetResData( _
	''                iLangID:=g_iLanguageID%, _
	''                lID:=ACAllTypes, _
	''                iDataType:=PMResString)
	'            ctlLookup.AddItem sLookupDesc
	'            ctlLookup.ItemData(ctlLookup.NewIndex) = -1
	'        End If
	'    End If
	'
	'    For lCntr& = m_vLookupValues(PMLookupStartPos, lLookupRow&) To _
	''    (m_vLookupValues(PMLookupStartPos, lLookupRow&) + m_vLookupValues(PMLookupNumOfItems, lLookupRow&)) - 1
	'        ' Add the details to the control.
	'        ctlLookup.AddItem m_vLookupDetails(PMLookupCaption, lCntr&)
	'        ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(PMLookupID, lCntr&)
	'
	'        ' Check if this is the selected index.
	'        If (m_vLookupValues(PMLookupKey, lLookupRow&) = _
	''        m_vLookupDetails(PMLookupID, lCntr&)) Then
	'            ctlLookup.ListIndex = ctlLookup.NewIndex
	'        End If
	'    Next lCntr&
	'
	'    ' Check if the selected index is blank. If so,
	'    ' we set the controls index to zero.
	'    If (m_vLookupValues(PMLookupKey, lLookupRow&) = "") Then
	'        ctlLookup.ListIndex = 0
	'    End If
	'
	'    Exit Function
	'
	'Err_GetLookupDetails:
	'
	'    ' Error Section.
	'
	'    GetLookupDetails = PMError
	'
	'    ' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to get all of the lookup details", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="GetLookupDetails", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'    Exit Function
	'
	'End Function
	
	' ***************************************************************** '
	' Name: GetLookupDesc
	'
	' Description: Gets a description string for a given lookup set
	'              and lookup id.
	'
	' ***************************************************************** '
	Public Function GetLookupDesc(ByRef lLookupRow As Integer, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer
		
		Dim result As Integer = 0

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if there has been a table match.
			If lLookupRow = -1 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Using the lookup values, populate the lookup
			' string from the lookup details array when the
			' lookup ID has been matched.
			



            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Check for a match on the ID.

                If CInt(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.

                    sLookupDesc = CStr(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr)).Trim()

                    Exit For
                End If
            Next lCntr
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	
	' ***************************************************************** '
	' Name: OnColumnClick
	'
	' Description: Called by a form's listview column click event
	'
	' ***************************************************************** '
	Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)
		
		Try 
			
			With ListView
				
				' If date column clicked, then sort by date sort column
				'eck010800
				Select Case ColumnHeader.Index + 1 - 1
					Case ACListAccountingDate
						'If (ColumnHeader.Index - 1 = ACListAccountingDate) Then
                        'developer guide no. 						
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						
					Case ACListDocumentDate
                        'developer guide no. 170
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						'eck240800 also sort for OSAmount
					Case ACListCurrencyAmount, ACListOSCurrencyAmount
                        'developer guide no. 170
                        m_lReturn = ListViewFunc.ListViewSortByStringValue(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						'        ElseIf (ColumnHeader.Index - 1 = .SortKey) Then
					Case ListViewHelper.GetSortKeyProperty(ListView)
						' Set sort order opposite of
						' current direction.
						ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
					Case Else
						' Sort by this column (ascending).
						ListViewHelper.SetSortedProperty(ListView, False)
						
						' Turn off sorting so that the list
						' is not sorted twice
						ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
						ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)
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
    
End Module
