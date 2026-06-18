Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Public Const ScreenHelpID As Integer = 44000
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctListPolicyControl"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	Public Const ACClientCode As Integer = 103
	Public Const ACStatus As Integer = 104
	
	Public Const ACListTitleProductName As Integer = 112
	Public Const ACListTitlePolicyNumber As Integer = 113
	Public Const ACListTitlePolicyType As Integer = 114
	Public Const ACListTitleRiskType As Integer = 115
	Public Const ACListTitleRegarding As Integer = 116
	Public Const ACListTitleRenewalDate As Integer = 117
	Public Const ACListTitleInsurer As Integer = 118
	Public Const ACListTitlePremium As Integer = 119
	Public Const ACListTitlePolicyStatus As Integer = 120
	
	Public Const ACListTitleStartDate As Integer = 121
	Public Const ACListTitleEDI As Integer = 122
	'RWH(10/04/2001)
	Public Const ACListTitleAgent As Integer = 123
	Public Const ACListTitleRiskTypeDescription As Integer = 124
	'SJ 18/10/2004 - start
	Public Const ACListTitleStoredInd As Integer = 125
	'SJ 18/10/2004 - end
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	'SB 31/03/98 defect 37
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	' Menus
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsFileId As Integer = 0
	Public Const ACIInsFileSourceId As Integer = 1
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
    Public Const ACIInsFileType As Integer = 5
    Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsuredSourceId As Integer = 9
	Public Const ACILastModified As Integer = 10
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	Public Const ACIProductID As Integer = 13
	Public Const ACIProductCode As Integer = 14
	Public Const ACIProductName As Integer = 15
	Public Const ACILeadAgentCnt As Integer = 16
	Public Const ACIDateCreated As Integer = 17
	Public Const ACIStatus As Integer = 18
	'ECK 16/7/99
	Public Const ACIInsurer As Integer = 19
	Public Const ACIPremium As Integer = 20
	'Tomo010200
	Public Const ACIPolicyTypeId As Integer = 21
	Public Const ACIPolicyType As Integer = 22
	Public Const ACIGeminiPolicyStatus As Integer = 23
	'Tomo200300
	'MSS240701
	Public Const ACIRiskDesc As Integer = 23
	Public Const ACIGeminiIIEdi As Integer = 25
	
	Public Const ACIInsuranceFileTypeId As Integer = 26
	Public Const ACIExpiryDate As Integer = 27
    Public Const ACIEventDescription As Integer = 41 'Gaurav Changed
 
	'SJ 20/02/2004 - start
	Public Const ACIAlternateReference As Integer = 30
	Public Const ACIUnderwritingBranchInd As Integer = 31
	'SJ 20/02/2004 - end
	'SJ 18/10/2004 - start
	Public Const ACIStoredInd As Integer = 32
    'SJ 18/10/2004 - end
   

	Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"
	
	'Constants for the columns
	Public Const ACAColumnPolicyNumber As Integer = 1
	Public Const ACAColumnRegarding As Integer = 2
	Public Const ACAColumnRenewalDate As Integer = 3
	Public Const ACAColumnStartDate As Integer = 3 'Also used for start date if GII
	Public Const ACAColumnInsurer As Integer = 4
	Public Const ACAColumnRiskTypeDescription As Integer = 5
	Public Const ACAColumnPremium As Integer = 6
	Public Const ACAColumnPolicyStatus As Integer = 7
	Public Const ACAColumnPolicyType As Integer = 8
	Public Const ACAColumnRiskType As Integer = 9
	Public Const ACAColumnGeminiEDI As Integer = 10
	Public Const ACAColumnEventDescription As Integer = 11 'Gaurav Change
	
	
	Public Const ACUColumnPolicyNumber As Integer = 1
	Public Const ACUColumnPolicyType As Integer = 2
	Public Const ACUColumnRiskType As Integer = 3
	Public Const ACUColumnRegarding As Integer = 4
	Public Const ACUColumnRenewalDate As Integer = 5
	Public Const ACUColumnInsurer As Integer = 6
	Public Const ACUColumnPremium As Integer = 7
	Public Const ACUColumnPolicyStatus As Integer = 8
	Public Const ACUColumnRiskTypeDescription As Integer = 9
	Public Const ACUColumnGeminiEDI As Integer = 10
	Public Const ACUColumnEventDescription As Integer = 10 'Gaurav Change
	
	'SJ 18/10/2004 - start
	Public Const ACGColumnPolicyNumber As Integer = 1
	Public Const ACGColumnStoredInd As Integer = 2
	Public Const ACGColumnRegarding As Integer = 3
	Public Const ACGColumnRenewalDate As Integer = 4
	Public Const ACGColumnStartDate As Integer = 3 'Also used for start date if GII
	Public Const ACGColumnInsurer As Integer = 5
	Public Const ACGColumnRiskTypeDescription As Integer = 6
	Public Const ACGColumnPremium As Integer = 7
	Public Const ACGColumnPolicyStatus As Integer = 8
	Public Const ACGColumnPolicyType As Integer = 9
	Public Const ACGColumnRiskType As Integer = 10
	Public Const ACGColumnGeminiEDI As Integer = 11
	Public Const ACGColumnEventDescription As Integer = 12 'Gaurav Change
	'SJ 18/10/2004 - end
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Constants for Date and Date Sort Column
	Public Const ACDateColumn As Integer = 4
	Public Const ACDateSortColumn As Integer = 6
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'eck220500
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	'Public g_oBusiness As bSIRFindInsurance.Form
	'eck190500
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'Public do we have a link to Gemini
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bPMGeminiLink As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bPMSwiftLink As Boolean
	
	'DJM 15/10/2003
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'DJM 15/10/2003 : Copied and modified from same function for Find Transaction.
	Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader, ByVal v_sUnderwritingOrAgency As String)
		
		Try 
			
			With ListView
				
				If v_sUnderwritingOrAgency = "A" Then
					Select Case ColumnHeader.Index + 1 - 1
						Case ACAColumnRenewalDate - 1
                            'NIIT - Replaced with the Migrated code 1144 
                            'm_lReturn = CType(ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                            m_lReturn = CType(ListViewFunc.ListViewSortByDate(ListView, ColumnHeader.Index + 1 - 1, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
							
                        Case ACAColumnPremium - 1
                            'NIIT - Replaced with the Migrated code 1144 
                            'm_lReturn = CType(ListViewSortByValue(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                            m_lReturn = CType(ListViewFunc.ListViewSortByValue(ListView, ColumnHeader.Index + 1 - 1, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
							
						Case ListViewHelper.GetSortKeyProperty(ListView)
							' Set sort order opposite of
							' current direction.
							ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						Case Else
							' Sort by this column (ascending).
							ListViewHelper.SetSortedProperty(ListView, False)
							ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
							ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)
							ListViewHelper.SetSortedProperty(ListView, True)
					End Select
				Else
					Select Case ColumnHeader.Index + 1 - 1
						Case ACUColumnRenewalDate - 1
                            'NIIT - Replaced with the Migrated code 1144 
                            'm_lReturn = CType(ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                            m_lReturn = CType(ListViewFunc.ListViewSortByDate(ListView, ColumnHeader.Index + 1 - 1, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
							
                        Case ACUColumnPremium - 1
                            'NIIT - Replaced with the Migrated code 1144 
                            'm_lReturn = CType(ListViewSortByValue(v_oListView:=ListView, v_iSourceColumn:=ColumnHeader.Index + 1 - 1, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
                            m_lReturn = CType(ListViewFunc.ListViewSortByValue(ListView, ColumnHeader.Index + 1 - 1, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2), gPMConstants.PMEReturnCode)
							
						Case ListViewHelper.GetSortKeyProperty(ListView)
							' Set sort order opposite of
							' current direction.
							ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						Case Else
							' Sort by this column (ascending).
							ListViewHelper.SetSortedProperty(ListView, False)
							ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
							ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)
							ListViewHelper.SetSortedProperty(ListView, True)
					End Select
				End If
				
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module