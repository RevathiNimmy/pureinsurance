Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06 May 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTAllocate"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	Public g_iUserID As Integer
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	' Calling Application
	Public g_sCallingAppName As String = ""
	' Company ID
	Public g_iCompanyID As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	'eck170500
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle As Integer = 101
	Public Const ACTabTitleCount As Integer = 2
	
	' ColumnHeaders
	Public Const ACListTitle As Integer = 110
	'eck090500
	Public Const ACListTitleCount As Integer = 18
	
	' Labels
	Public Const ACDocumentRef As Integer = 130
	Public Const ACDocTypeGroup As Integer = 131
	Public Const ACDocumentType As Integer = 132
	Public Const ACPeriod As Integer = 133
	Public Const ACDateFrom As Integer = 134
	Public Const ACDateTo As Integer = 135
	Public Const ACCurrency As Integer = 136
	Public Const ACCurrencyAmount As Integer = 137
	Public Const ACTolerance As Integer = 138
	Public Const ACInsuranceRef As Integer = 139
	Public Const ACOperatorName As Integer = 140
	Public Const ACPurchaseInvoiceNo As Integer = 141
	Public Const ACPurchaseOrderNo As Integer = 142
	Public Const ACDepartment As Integer = 143
	Public Const ACSpare As Integer = 144
	Public Const ACAccountCode As Integer = 150
	Public Const ACContactName As Integer = 151
	Public Const ACTelephone As Integer = 152
	Public Const ACAccountBalance As Integer = 153
	Public Const ACAccountName As Integer = 154
	Public Const ACBalanceOption As Integer = 155
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	Public Const ACAllocateButton As Integer = 208
	Public Const ACMarkButton As Integer = 209
	'CMG/PB 05082002: For Multi-Branch
	Public Const ACSubBranchLabel As Integer = 210
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACAllTypes As Integer = 308
	Public Const ACYes As Integer = 309
	Public Const ACNo As Integer = 310
	Public Const ACPrimaryForAllocation As Integer = 311
	Public Const ACSecondaryForAllocation As Integer = 312
	
	' Menus
	
	
	' Constants for the lookup table array indexes.
	Public Const ACLDocumentType As Integer = 0
	Public Const ACLDocTypeGroup As Integer = 1
	Public Const ACLMax As Integer = 1
	
	' Constants for data array indexes.
	Public Const ACIMarkedStatus As Integer = 0
	Public Const ACIDocumentRef As Integer = ACIMarkedStatus + 1
	Public Const ACIDocumentId As Integer = ACIDocumentRef + 1
	Public Const ACIDocumentSequence As Integer = ACIDocumentId + 1
	Public Const ACIAccountingDate As Integer = ACIDocumentSequence + 1
	Public Const ACIPeriodName As Integer = ACIAccountingDate + 1
	Public Const ACICurrencyAmount As Integer = ACIPeriodName + 1
	Public Const ACIDocumentTypeId As Integer = ACICurrencyAmount + 1
	Public Const ACIDocTypeGroupId As Integer = ACIDocumentTypeId + 1
	Public Const ACIInsuranceRef As Integer = ACIDocTypeGroupId + 1
	Public Const ACIOperatorName As Integer = ACIInsuranceRef + 1
	Public Const ACIPurchaseOrderNo As Integer = ACIOperatorName + 1
	Public Const ACIPurchaseInvoiceNo As Integer = ACIPurchaseOrderNo + 1
	Public Const ACIDepartment As Integer = ACIPurchaseInvoiceNo + 1
	Public Const ACISpare As Integer = ACIDepartment + 1
	Public Const ACIAccountShortCode As Integer = ACISpare + 1
	Public Const ACIAccountId As Integer = ACIAccountShortCode + 1
	Public Const ACICurrencyId As Integer = ACIAccountId + 1
	Public Const ACITransDetailId As Integer = ACICurrencyId + 1
	Public Const ACIBaseAmount As Integer = ACITransDetailId + 1
	Public Const ACIPrimarySettled As Integer = ACIBaseAmount + 1
	Public Const ACIDocumentDate As Integer = ACIPrimarySettled + 1
	Public Const ACISourceID As Integer = ACIDocumentDate + 1
	Public Const ACIMatchAmount As Integer = ACISourceID + 1
	Public Const ACIMatchDate As Integer = ACIMatchAmount + 1
	Public Const ACIMarkedAmount As Integer = ACIMatchDate + 1
	
	
	
	
	
	
	' Constants for List View index.
	Public Const ACListSourceID As Integer = 0
	Public Const ACListDocumentRef As Integer = 1
	Public Const ACListAccountingDate As Integer = 2
	Public Const ACListDocumentDate As Integer = 3
	Public Const ACListCurrencyAmount As Integer = 4
	Public Const ACListPrimarySettled As Integer = 5
	Public Const ACListOSCurrencyAmount As Integer = 6
	Public Const ACListMarkedAmount As Integer = 7
	Public Const ACListMatchDate As Integer = 8
	Public Const ACListDocumentTypeId As Integer = 9
	Public Const ACListInsuranceRef As Integer = 10
	Public Const ACListOperatorName As Integer = 11
	Public Const ACListPeriodName As Integer = 12
	Public Const ACListDocTypeGroupId As Integer = 13
	Public Const ACListSpare As Integer = 14
	Public Const ACListPurchaseInvoiceNo As Integer = 15
	Public Const ACListPurchaseOrderNo As Integer = 16
	Public Const ACListDepartment As Integer = 17
	Public Const ACListMatchAmount As Integer = 18
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sYes As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sNo As String = ""
	
	
	' Icon
	Public Const ACIconCheck As String = "check"
	Public Const ACIconBlank As Integer = 0
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' Public instances of the business objects.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object 'FindTrabsaction business
	
	' CTAF 291099 - Put these in g_oBusiness now
	'Public g_oCurrencyConvert As Object
	'Public g_oPeriod As Object
	'Public g_oAccount As Object
	'Public g_oAllocationCalculate As Object
	'Public g_oSirConfig As Object
	
    ' Variables to store the lookup values/details.

	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	Private m_vLedgers As Object
	
	' Stores the return value for a function call.
	Private m_lReturn As Integer
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 13000
	
	' CTAF 130300
	Public Const ACTFindTransBalanceOption As String = "BalanceOption"
	
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

			m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocumentType) = gACTLibrary.ACTLookupDocumentType

			m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocTypeGroup) = gACTLibrary.ACTLookupDocTypeGroup
			
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

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Public Function GetLookupDetails(ByRef lLookupRow As Integer, ByRef ctlLookup As ComboBox, Optional ByVal vAllTypes As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.


            Dim sLookupDesc As String = ""
            If Not Information.IsNothing(vAllTypes) Then

                If CBool(vAllTypes) Then
                    ' First entry is all types (don't care)

                    sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Developer Guide No.29
                    ctlLookup.Items.Add(New VB6.ListBoxItem(sLookupDesc, 0))

                End If
            End If




            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Add the details to the control.

                'Developer Guide No.29
                Dim newIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr), 0))

                ' Check if this is the selected index.


                If m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow).Equals(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) Then


                    'Developer Guide No.28
                    ctlLookup.SelectedIndex = newIndex
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.

            If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow)) = "" Then

                'Developer Guide No.28
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
				Select Case ColumnHeader.Index + 1 - 1
					Case ACListAccountingDate
                        'Developer guide no.170
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
					Case ACListDocumentDate
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
					Case ACListCurrencyAmount, ACListOSCurrencyAmount
                        m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
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
