Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmInterface      iCLMFinSumm.frm
	
	' Description   : Main interface.
	' Date          : 05/08/2000
	' Author        : SK
	' Edit History  :
	' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	Private Const ACColumn1 As Integer = 1
	Private Const ACColumn2 As Integer = 2
	Private Const ACColumn3 As Integer = 3
	Private Const ACColumn4 As Integer = 4
	Private Const ACColumn5 As Integer = 5
	Private Const ACColumn6 As Integer = 6
	Private Const ACColumn7 As Integer = 7
	
	'Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	Private Const ACColumn8 As Integer = 8
	'End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
	
	'Constants for Defining Width of Columns in List View
	
	Private Const ColWidthBroking As Integer = 1700
	Private Const ACColWidthUnderWriting As Integer = 1300
	
	Private Const ACListViewTop As Integer = 390
	Private Const ACListViewLeft As Integer = 120
	Private Const ACListViewHeight As Integer = 3585
	Private Const ACListViewWidth As Integer = 10050
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' Variables for Financial Summary
	Private m_lClaimId As Integer


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMFinSumm.General
	
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	
	'Variable for Underwriting/Broking
	Private m_lSiriusUnderWritingBroking As String = ""
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	'variable added 'Gaurav
	'-----------------------------------------------------Claim Summary
	Private m_lIsRecovery As Integer
	'-----------------------------------------------------Claim Summary
	' Stores the search data from the GetResvTypCount method of business object.
	Public m_vSearchData As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vArray( ,  ) As Object
	'Stores the data from the GetPerilsForRecovery method of the business object.
	Public m_vRecoveryArray( ,  ) As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vTotalArray( ,  ) As Object
	
	
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

    Public Property ClaimId() As Integer
        Get

            'Return Claimid
            Return m_lClaimId

        End Get
        Set(ByVal Value As Integer)

            'Set Claim Number
            m_lClaimId = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name:         GetBusiness
    '
    ' Description:  Retrieves the details from the business object.
    '
    ' Date:         11/07/00
    '
    ' Edit History: SK

    '
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			Dim iTabNo, iTabCount, iLstVwIdx As Integer
			
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Start - PM Bug 40, 01/11/00 6:00 pm, SK
			'Add the Salvage & TPRecovery tabs initially as they will always be required
			
			'-----------------------------------------------------Claim Summary
			'Value of tabMainTab is incrimented for Payment Tab
			SSTabHelper.SetTabs(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) + 4) '(+Salvage+TPRecovery+Payments)
			''(Saurabh) Changed 3 to 4
			'-----------------------------------------------------Claim Summary
			
			'End - PM Bug 40, 01/11/00 6:00 pm, SK
			
			'It retrieves the distinct Reserve Types for the specified Claim ID.
			'It is used to determine the number of tabs for the screen

			m_lReturn = g_oBusiness.GetResvTypCount(r_vResultArray:=m_vSearchData, v_lclm_id:=ClaimId)
			
			If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And Not (m_lReturn = gPMConstants.PMEReturnCode.PMNotFound) Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
				'Gets the Total for all the Perils for each Resereve Type
				'for the specified Claim_Id

				m_lReturn = g_oBusiness.GetPerilTotals(r_vResultArray:=m_vTotalArray, v_lclaim_id:=ClaimId)
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to get details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				'Add all the Tab
				'Start - PM Bug 40, 01/11/00 6:00 pm, SK
				'    tabMainTab.Tabs = UBound(m_vSearchData, 2) + 4      '(Total+1+Salvage+TPRecovery)

				SSTabHelper.SetTabs(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) + m_vSearchData.GetUpperBound(1) + 1) '(Total+1)     SK-01/11/00
				'End - PM Bug 40, 01/11/00 6:00 pm, SK
				
				
				'populate the list view for each tab
				'Assign Values to Interface
				m_lReturn = CType(DataToInterfaceSumm(), gPMConstants.PMEReturnCode)
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to get details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				
				'get the Number of tabs to be added (which is equal
				'to the row count of the resultset)

				iTabCount = m_vSearchData.GetUpperBound(1)
				
				
				For iTabNo = 0 To iTabCount
					
					'        'Add a Tab
					'        tabMainTab.Tabs = tabMainTab.Tabs + 1          'SK
					
					


                    'developer guide no.98
                    m_lReturn = g_oBusiness.GetPerilsForReserve(r_vResultArray:=m_vArray, v_lclaim_id:=ClaimId, v_lReserve_type_id:=m_vSearchData(0, iTabNo))
					' Check for errors
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						' Failed to get details.
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					
					
					'populate the list view for each tab
					'Assign Values to Interface
                    m_lReturn = CType(DataToInterface(iTabNo + 1), gPMConstants.PMEReturnCode)
                    'm_lReturn = CType(DataToInterface(iTabNo), gPMConstants.PMEReturnCode)
					
				Next iTabNo
			End If
			
			'Load & Populate the Salvage & TP Recovery tabs
			
			'repeat twice since 2 tabs are to be loaded
            iLstVwIdx = 0

			
			'-----------------------------------------------------Claim Summary
			
			For iTabNoRec As Integer = 0 To 3
				
				'        'Add a Tab
				'        tabMainTab.Tabs = tabMainTab.Tabs + 1
				
				'        tabMainTab.TabCaption(tabMainTab.Tabs) = "&" & tabMainTab.Tabs + 1 + 1 & " -  Salvage"
				

				m_lReturn = g_oBusiness.GetPerilsForRecovery(r_vResultArray:=m_vRecoveryArray, v_lclaim_id:=ClaimId, v_lIs_Salvage:=iTabNoRec)
				
				If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And Not (m_lReturn = gPMConstants.PMEReturnCode.PMNotFound) Then
					' Failed to get details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				'Add a Tab
				
				'Start - PM Bug 40, 01/11/00 6:00 pm, SK
				'tabMainTab.Tabs = tabMainTab.Tabs + 1          'Commented
				'Start - PM Bug 40, 01/11/00 6:00 pm, SK
				
				iLstVwIdx += 1
				
				'populate the list view for each tab
				'Assign Values to Interface
				m_lReturn = CType(DataToInterfaceRec(iTabNo + iLstVwIdx), gPMConstants.PMEReturnCode)
				
			Next iTabNoRec
			
			
			
			
			'After loading & populating all the listviews
			'set the default to the 1st tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			
			m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Check the return values.
			
			Select Case (m_lReturn)
				Case gPMConstants.PMEReturnCode.PMTrue
					' Found search details.
				Case gPMConstants.PMEReturnCode.PMNotFound
					' No search details found.
				Case Else
					' Failed to get details.
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
					
					Return result
			End Select
			
			'    iTabCnt = tabMainTab.Tabs
			For iTabCnt As Integer = 0 To SSTabHelper.GetTabCount(tabMainTab) - 1
                'call API to select the entire row
                'developer guide no.303
                'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwFinSumm(iTabCnt).Handle.ToInt32(), v_vShowRowSelect:=1), gPMConstants.PMEReturnCode)
                lvwFinSumm(iTabCnt).FullRowSelect = True
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '	Return gPMConstants.PMEReturnCode.PMFalse
                'End If
			Next iTabCnt
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
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
	' Date:11/07/00
	'
	' Edit History:SK
	'
	'
	' ***************************************************************** '
	
	Public Function DataToInterface(ByRef iLstVwNo As Integer) As Integer
		
		Dim result As Integer = 0
        Dim oListItem As ListViewItem
		Dim curCurrresv As Decimal
		
		
		'Const ACFindImage = "FindImage"
		Const ACColPerilDesc As Integer = 8
		Const ACColInitResv As Integer = 2
		Const ACColPdtoDt As Integer = 3
		Const ACColRevResv As Integer = 4
		Const ACColSumIns As Integer = 5
		Const ACColAvg As Integer = 6
		Const ACColDesc As Integer = 0
		'DC170602
        'Const ACRevisedEntered As Integer = 9
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
            SSTabHelper.SetSelectedIndex(tabMainTab, iLstVwNo)
            'Developer Guide No.95
            ContainerHelper.LoadControl(Me, "lvwFinSumm", iLstVwNo, True)

            For i As Integer = 0 To lvwFinSumm(0).Columns.Count - 1
                lvwFinSumm(iLstVwNo).Columns.Insert(i, lvwFinSumm(0).Columns(i).Text)
                lvwFinSumm(iLstVwNo).Columns(i).Width = lvwFinSumm(0).Columns(i).Width
            Next
            ListViewHelper.SetColumnHeaderIconsProperty(lvwFinSumm(iLstVwNo), ListViewHelper.GetColumnHeaderIconsProperty(lvwFinSumm(0)))

            lvwFinSumm(iLstVwNo).Parent = tabMainTab.TabPages(iLstVwNo)
			lvwFinSumm(iLstVwNo).Visible = True
			''    lvwFinSumm(iLstVwNo).Top = ACListViewTop              'SK
			''    lvwFinSumm(iLstVwNo).Left = ACListViewLeft
			''    lvwFinSumm(iLstVwNo).Height = ACListViewHeight
			''    lvwFinSumm(iLstVwNo).Width = ACListViewWidth
			
            'developer guide no.305
            lvwFinSumm(iLstVwNo).Top = VB6.TwipsToPixelsY(120) '120
			lvwFinSumm(iLstVwNo).Left = VB6.TwipsToPixelsX(120)
            lvwFinSumm(iLstVwNo).Width = tabMainTab.Width - (2 * VB6.TwipsToPixelsX(120)) - VB6.TwipsToPixelsX(150) '240
            If VB6.PixelsToTwipsY(tabMainTab.Height) - (VB6.PixelsToTwipsY(tabMainTab.ItemSize.Height) * 2 + 300) > 0 Then '240
                lvwFinSumm(iLstVwNo).Height = tabMainTab.Height - tabMainTab.ItemSize.Height * 2 - VB6.TwipsToPixelsY(300)
            Else
                lvwFinSumm(iLstVwNo).Height = VB6.TwipsToPixelsY(1)
            End If
			
			
			' Clear the search details.
			lvwFinSumm(iLstVwNo).Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vArray) Then
				Return result
			End If
			
            '    m_lReturn& = ListView_GridLines(lvwFinSumm(iLstVwNo))
            'developer guide no.302
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwFinSumm(iLstVwNo).Handle.ToInt32(), v_vShowGridLines:=True), gPMConstants.PMEReturnCode)
            lvwFinSumm(iLstVwNo).GridLines = True

			
			' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            'End If
			
			
			' Assign the details to the interface.
			For lRow As Integer = m_vArray.GetLowerBound(1) To m_vArray.GetUpperBound(1)
				
                SSTabHelper.SetTabCaption(tabMainTab, iLstVwNo, "" & iLstVwNo + 1 & " -  " & CStr(m_vArray(ACColDesc, lRow)).Trim())
				
				' Assign the details to the first column.
				oListItem = lvwFinSumm(iLstVwNo).Items.Add(CStr(m_vArray(ACColPerilDesc, lRow)).Trim())
				
				
				
				' Assign details to other the columns
				If (CStr(m_vArray(ACColInitResv, lRow)).Trim()) = "" Then m_vArray(ACColInitResv, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vArray(ACColInitResv, lRow)).Trim())

				If (CStr(m_vArray(ACColPdtoDt, lRow)).Trim()) = "" Then m_vArray(ACColPdtoDt, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vArray(ACColPdtoDt, lRow)).Trim())
				
				If (CStr(m_vArray(ACColRevResv, lRow)).Trim()) = "" Then m_vArray(ACColRevResv, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vArray(ACColRevResv, lRow)).Trim())
				
				'DC170602 - Start - for broking calculate Current Reserve differently
				
				curCurrresv = CDec(CStr(m_vArray(ACColInitResv, lRow)).Trim()) - CDec(CStr(m_vArray(ACColPdtoDt, lRow)).Trim()) + CDec(CStr(m_vArray(ACColRevResv, lRow)).Trim())
				
				
				'DC170602 - End
				
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(curCurrresv).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vArray(ACColSumIns, lRow)).Trim())
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vArray(ACColAvg, lRow)).Trim())
				
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
					lvwFinSumm(iLstVwNo).Items.Item(0).Selected = True
					
					' Refresh the initial results.
					lvwFinSumm(iLstVwNo).Refresh()
				End If
			Next lRow
			
			' Select the first item.
			lvwFinSumm(iLstVwNo).Items.Item(0).Selected = True
			
			' Enable the interface now that the search has completed.
			m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DataToInterfaceSumm
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	' Date:11/07/00
	'
	' Edit History:SK
	'
	'
	' ***************************************************************** '
	Public Function DataToInterfaceSumm() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem

		'Const ACFindImage = "FindImage"
		Const ACColPerilDesc As Integer = 1
		Const ACColInitResv As Integer = 2
		Const ACColPdtoDt As Integer = 3
		Const ACColRevResv As Integer = 4
		Const ACColCurResv As Integer = 5
		Const ACColSumIns As Integer = 6
		Const ACColAvg As Integer = 7
		''Const ACColDesc = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            'developer guide no.305
            lvwFinSumm(0).Top = VB6.TwipsToPixelsX(120)
			lvwFinSumm(0).Left = VB6.TwipsToPixelsX(120)
            lvwFinSumm(0).Width = tabMainTab.Width - (2 * VB6.TwipsToPixelsX(120)) - VB6.TwipsToPixelsX(150) '240
            If VB6.PixelsToTwipsY(tabMainTab.Height) - (VB6.PixelsToTwipsY(tabMainTab.ItemSize.Height) * 2 + 300) > 0 Then '240
                lvwFinSumm(0).Height = tabMainTab.Height - tabMainTab.ItemSize.Height * 2 - VB6.TwipsToPixelsY(300)
            Else
                lvwFinSumm(0).Height = VB6.TwipsToPixelsY(1)
            End If
			
			
			' Clear the search details.
			lvwFinSumm(0).Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vTotalArray) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)
				
				
				' Assign the details to the first column.
				' Column 1 Claim Type
				oListItem = lvwFinSumm(0).Items.Add(CStr(m_vTotalArray(ACColPerilDesc, lRow)).Trim())
				
				' Assign details to other the columns
				If (CStr(m_vTotalArray(ACColInitResv, lRow)).Trim()) = "" Then m_vTotalArray(ACColInitResv, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColInitResv, lRow)).Trim())
				
				If (CStr(m_vTotalArray(ACColPdtoDt, lRow)).Trim()) = "" Then m_vTotalArray(ACColPdtoDt, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColPdtoDt, lRow)).Trim())
				
				If (CStr(m_vTotalArray(ACColRevResv, lRow)).Trim()) = "" Then m_vTotalArray(ACColRevResv, lRow) = 0
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColRevResv, lRow)).Trim())
				
				
				'DC060802 -start
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColCurResv, lRow)).Trim())
				
				
				'DC060802 -end
				
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColSumIns, lRow)).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vTotalArray(ACColAvg, lRow)).Trim())
				
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
					lvwFinSumm(0).Items.Item(0).Selected = True
					
					' Refresh the initial results.
					lvwFinSumm(0).Refresh()
				End If
			Next lRow
			
			' Select the first item.
			lvwFinSumm(0).Items.Item(0).Selected = True
			
			' Enable the interface now that the search has completed.
			m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterfaceSumm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToProperties
	'
	' Description: Updates the property member from the search data
	'              storage.
	' Date:15/07/00
	'
	' Edit History:SK
	'
	' ***************************************************************** '
	Public Function DataToProperties() As Integer
		
		Dim result As Integer = 0
		Dim lSelectedItem As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the selected item's tag, so we can use this
			' as the index to the search data storage details.

			lSelectedItem = Convert.ToString(lvwFinSumm(0).Items.Item(lvwFinSumm(0).FocusedItem.Index).Tag)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	' Edit History :SK
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		
		Dim result As Integer = 0

		Try 
			
			
			' Update the interface details.
			
			'    txtClaimRef.Text = Trim(m_sClaimRef$)
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
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
	' Date :15/07/2000
	'
	' Edit History : SK
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			
			'Add the columns to the listview
            lvwFinSumm(0).Columns.Insert(ACColumn1 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn2 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn3 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn4 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn5 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn6 - 1, "")
            lvwFinSumm(0).Columns.Insert(ACColumn7 - 1, "")
			
            lvwFinSumm(0).Columns.Insert(ACColumn8 - 1, "") 'Saurabh Tech Spec QBENZCR004 Claims Recovery Reinsurance
			
			
			''    If m_lSiriusUnderWritingBroking = ACUnderWriting Then
			
			''Set the column widths
            lvwFinSumm(0).Columns.Item(ACColumn1 - 1).Width = 80 'CInt(VB6.TwipsToPixelsX(975))
            lvwFinSumm(0).Columns.Item(ACColumn2 - 1).Width = 95 'CInt(VB6.TwipsToPixelsX(1150))
            lvwFinSumm(0).Columns.Item(ACColumn3 - 1).Width = 83 'CInt(VB6.TwipsToPixelsX(930))
            lvwFinSumm(0).Columns.Item(ACColumn4 - 1).Width = 110 'CInt(VB6.TwipsToPixelsX(1400))
            lvwFinSumm(0).Columns.Item(ACColumn5 - 1).Width = 110 'CInt(VB6.TwipsToPixelsX(1400))
            lvwFinSumm(0).Columns.Item(ACColumn6 - 1).Width = 90 'CInt(VB6.TwipsToPixelsX(1040))
            lvwFinSumm(0).Columns.Item(ACColumn7 - 1).Width = 90 'CInt(VB6.TwipsToPixelsX(1105))
            lvwFinSumm(0).Columns.Item(ACColumn8 - 1).Width = 95 'CInt(VB6.TwipsToPixelsX(1105)) 'Saurabh Tech Spec QBENZCR004 Claims Recovery Reinsurance
			
			
			'Set the Allignment of the columns to right justified
			lvwFinSumm(0).Columns.Item(ACColumn2 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn3 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn4 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn5 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn6 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn7 - 1).TextAlign = HorizontalAlignment.Right
			lvwFinSumm(0).Columns.Item(ACColumn8 - 1).TextAlign = HorizontalAlignment.Right ' Saurabh Tech Spec QBENZCR004 Claims Recovery Reinsurance
			
			''    End If
			
			
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
			
			
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
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
	' Date :15/07/2000
	'
	' Edit History :SK
	' ***************************************************************** '

	'Private Function ClearInterface() As Integer
		'
		'Dim result As Integer = 0
		'Dim iMsgResult As DialogResult
		'Dim sMessage, sTitle As String
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Check if the user still wishes to clear
			' the interface.
			'

			'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'

			'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			' Display the message.
			'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			'
			' Check message result.
			'If iMsgResult = System.Windows.Forms.DialogResult.No Then
				' Don't continue with the clear.
				'Return result
			'End If
			'
			' Clear the interface details.
			'
			' Clear the search data array.
			'm_vSearchData = Nothing
			'
			' Clear the search list details.
			'lvwFinSumm(0).Items.Clear()
			'
			' Clear the search status bar.
			'    stbstatus.SimpleText = ""
			'
			'
			'
			' Set to the first tab.
			'SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			'
			' Set focus to the search details.
			'   txtClaimRef.SetFocus
			'
			' Set the default button.
			'    cmdFindNow.Default = True
			'
			'
			' Disable parts of the interface, so the
			' user can now only enter a new search
			'm_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' Date :15/07/2000
	'
	' Edit History :SK
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
			
			
			m_ctlTabFirstLast(ACControlStart, 0) = cmdOK
			m_ctlTabFirstLast(ACControlEnd, 0) = cmdOK
			
			
			''    If m_lSiriusUnderWritingBroking = ACUnderWriting Then
			
			m_ctlTabFirstLast(ACControlStart, 1) = cmdHelp
			m_ctlTabFirstLast(ACControlEnd, 1) = cmdHelp
			
			''    End If
			
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
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
	' Date :15/07/2000
	'
	' Edit History :SK
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
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            lvwFinSumm(0).Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwFinSumm(0).Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            lvwFinSumm(0).Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            lvwFinSumm(0).Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwFinSumm(0).Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwFinSumm(0).Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwFinSumm(0).Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
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
	' Date :15/07/2000
	'
	' Edit History :SK
	'
	' ***************************************************************** '
	Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cmdOK.Enabled = Not bDisable
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name:         FormActivate
	' Description:  Loads all required details of the form
	' Date:         15/07/00
	' Edit History: SK
	' ***************************************************************** '
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			Try 
				
				'-----------------------------------------------------Claim Summary
				m_lIsRecovery = 0
				
				' Check if we have had an error so far.
				' Possibly creating the business object.
				If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
					' We have already encountered an error,
					' so we MUST exit now.
					Exit Sub
				End If
				
                '    m_lReturn& = ListView_GridLines(lvwFinSumm(0))
                'developer guide no.302
                'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwFinSumm(0).Handle.ToInt32(), v_vShowGridLines:=True), gPMConstants.PMEReturnCode)
                lvwFinSumm(0).GridLines = True
				
				' Check for errors.
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '	m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                'End If
				
				Exit Sub
			
			Catch excep As System.Exception
				
				
				
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Activate interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Exit Sub
			End Try
		End If
	End Sub
	
	' ***************************************************************** '
	' Name: FormIntialise
	'
	' Description: Intialise all required details of the form
	'
	' Date:15/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMFinSumm.General()
			
			' Create an instance of the interface object.
			'    Set m_oInterface = New iCLMFinSumm.Interface
			
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
			
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
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
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name:         FormLoad
	' Description:  Loads all required details of the form
	' Date:         15/07/00
	' Edit History: SK
	' ***************************************************************** '

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
			
			' Validate fields using Forms Control
			
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
			
			
			
			' {* USER DEFINED CODE (End) *}
			
			'SINCE GetInterfaceDetails's GetBusiness method is no longer being called
			'from here, we can comment GetInterfaceDetails
			'GetBusiness isbeing called from Form Activate
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

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
		
		
	End Sub
	' ***************************************************************** '
	' Name: Form_Query Unload
	'
	' Description: Store all Property Details before unloading form
	'
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
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
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

          

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try
		
	End Sub
	' ***************************************************************** '
	' Name:Form_KeyDown
	'
	' Description: Determine the Position of Tab and Control on
	'              pressing pageup,pagedown,home,end buttons
	'
	' Date:15/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
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
		
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	' ***************************************************************** '
	' Name:Form_Resize
	'
	' Description: Resize the the controls on form
	'
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
		
		Catch 
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: tabMainTab_Click
	'
	' Description:Set the Focus on the First control on the relevant Tab Clicked
	'
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				'        If (.Tab < cmdNext.Count) Then
				'            cmdNext(.Tab).Default = True
				'        Else
				'            cmdOK.Default = True
				'        End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				'        DoEvents
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
					m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
				End If
			End With
		
		Catch 
			
			
			
			
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: cmdOK_Click
	'
	' Description:Set Properties of the form on clicking OK Button from the
	'               relevant list item under focus or clicked
	'
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name: cmdCancel_Click
	'
	' Description:Unload the Form
	'
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckDateDiff
	'
	' Description: Checks Whether claim Loss from Date is greater than to date
	'
	' Date:11/07/2000
	'
	' Edit History:SK
	' ***************************************************************** '
	
	Public Function CheckDateDiff(ByRef vFromDate As Object, ByRef vToDate As Object) As Integer
		
		
		Dim nDiffDays As Double = DateAndTime.DateDiff("d", CDate(vFromDate), CDate(vToDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
		
		If nDiffDays < 0 Then
			
			Return gPMConstants.PMEReturnCode.PMFalse
		Else
			
			Return gPMConstants.PMEReturnCode.PMTrue
		End If
		
	End Function
	
	' ***************************************************************** '
	' Name: DisplayMessage
	'
	' Description: Display the Suitable Message
	'
	' Date:11/07/00
	'
	' Edit History:SK
	
	' ***************************************************************** '

	'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
		'
		'Static sMessage As String = ""
		'
		'Try 
			'
			'

			'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			'
			' Display the status message.
			'
			'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	
	' ***************************************************************** '
	' Name: DataToInterfaceRec
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	' Date:11/07/00
	'
	' Edit History:SK
	'Last Modified: By Vivek And Gaurav
	'To Display User control (uctCLMListPayments) on Payments Tab
	' ***************************************************************** '
	
	Public Function DataToInterfaceRec(ByRef iLstVwNo As Integer) As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		Dim curCurrresv As Decimal
		
		Dim sTabCap As String = ""
		
		
		
		Const ACColPerilDesc As Integer = 1
		Const ACColInitResv As Integer = 2
		Const ACColRcvtoDt As Integer = 3
		Const ACColRevResv As Integer = 4
		
		'Const ACColSumIns = 5
		'Const ACColAvg = 6
		'Const ACColDesc = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
            SSTabHelper.SetSelectedIndex(tabMainTab, iLstVwNo)
            ContainerHelper.LoadControl(Me, "lvwFinSumm", iLstVwNo, True)
           
            For i As Integer = 0 To lvwFinSumm(0).Columns.Count - 1
                lvwFinSumm(iLstVwNo).Columns.Insert(i, lvwFinSumm(0).Columns(i).Text)
                lvwFinSumm(iLstVwNo).Columns(i).Width = lvwFinSumm(0).Columns(i).Width
            Next
            ListViewHelper.SetColumnHeaderIconsProperty(lvwFinSumm(iLstVwNo), ListViewHelper.GetColumnHeaderIconsProperty(lvwFinSumm(0)))

            '-----------------------------------------------------Claim Summary
            If m_lIsRecovery <> 2 And m_lIsRecovery <> 3 Then
                lvwFinSumm(iLstVwNo).Parent = tabMainTab.TabPages(iLstVwNo)
                lvwFinSumm(iLstVwNo).Visible = True
                uctCLMListPaymentsC1.Visible = False
                'Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            ElseIf m_lIsRecovery = 3 And m_lIsRecovery <> 2 Then
                lvwFinSumm(iLstVwNo).Visible = False
                uctCLMListReceiptsC1.Visible = True
                uctCLMListReceiptsC1.Parent = tabMainTab.TabPages(iLstVwNo)
                'Set objUser = tabMainTab
                'developer guide no.305
                uctCLMListReceiptsC1.Top = VB6.TwipsToPixelsY(120) '120
                uctCLMListReceiptsC1.Left = VB6.TwipsToPixelsX(120)
                uctCLMListReceiptsC1.Width = tabMainTab.Width - (2 * VB6.TwipsToPixelsX(120)) - VB6.TwipsToPixelsX(150) '240
                'objUser.Visible = True
                uctCLMListReceiptsC1.CountColumn = 10
                uctCLMListReceiptsC1.ColumnCaption(0) = "Receipt_id"
                uctCLMListReceiptsC1.ColumnCaption(1) = "Date"
                uctCLMListReceiptsC1.ColumnCaption(2) = "Party Received"
                uctCLMListReceiptsC1.ColumnCaption(3) = "Payer"
                uctCLMListReceiptsC1.ColumnCaption(4) = "Amount"
                uctCLMListReceiptsC1.ColumnCaption(5) = "TaxAmount"
                uctCLMListReceiptsC1.ColumnCaption(6) = "Currency"
                uctCLMListReceiptsC1.ColumnCaption(7) = "Loss Amount"
                uctCLMListReceiptsC1.ColumnCaption(8) = "Base Amount"
                uctCLMListReceiptsC1.ClaimId = m_lClaimId
                uctCLMListReceiptsC1.SalvageAndTPRecoveryReceipts = 1
                uctCLMListReceiptsC1.GetBusiness()
                'End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            Else
                lvwFinSumm(iLstVwNo).Visible = False
                uctCLMListPaymentsC1.Visible = True
                uctCLMListPaymentsC1.Parent = tabMainTab.TabPages(iLstVwNo)

                'Set objUser = tabMainTab
                'developer guide no.305
                uctCLMListPaymentsC1.Top = VB6.TwipsToPixelsY(120) '120
                uctCLMListPaymentsC1.Left = VB6.TwipsToPixelsX(120)
                uctCLMListPaymentsC1.Width = tabMainTab.Width - (2 * VB6.TwipsToPixelsX(120)) - VB6.TwipsToPixelsX(150) '240
                'objUser.Visible = True

                uctCLMListPaymentsC1.CountColumn = 9
                uctCLMListPaymentsC1.ColumnCaption(0) = "Payment_id"
                uctCLMListPaymentsC1.ColumnCaption(1) = "Date"
                uctCLMListPaymentsC1.ColumnCaption(2) = "Party Paid"
                uctCLMListPaymentsC1.ColumnCaption(3) = "Payee"
                uctCLMListPaymentsC1.ColumnCaption(4) = "Amount"
                uctCLMListPaymentsC1.ColumnCaption(5) = "TaxAmount"
                uctCLMListPaymentsC1.ColumnCaption(6) = "Currency"
                uctCLMListPaymentsC1.ColumnCaption(7) = "Loss Amount"
                uctCLMListPaymentsC1.ColumnCaption(8) = "Base Amount"
                uctCLMListPaymentsC1.ClaimId = m_lClaimId
                uctCLMListPaymentsC1.ReserveID = 0
                uctCLMListPaymentsC1.GetBusiness()
            End If
            '-----------------------------------------------------Claim Summary

            'developer guide no.305
            lvwFinSumm(iLstVwNo).Top = VB6.TwipsToPixelsY(120) '120
            lvwFinSumm(iLstVwNo).Left = VB6.TwipsToPixelsX(120)
            lvwFinSumm(iLstVwNo).Width = tabMainTab.Width - (2 * VB6.TwipsToPixelsX(120)) - VB6.TwipsToPixelsX(150) '240
            If VB6.PixelsToTwipsY(tabMainTab.Height) - (VB6.PixelsToTwipsY(tabMainTab.ItemSize.Height) * 2 + 300) > 0 Then '240
                lvwFinSumm(iLstVwNo).Height = tabMainTab.Height - tabMainTab.ItemSize.Height * 2 - VB6.TwipsToPixelsY(300)
            Else
                lvwFinSumm(iLstVwNo).Height = VB6.TwipsToPixelsY(1)
            End If

            ' Clear the search details.
            lvwFinSumm(iLstVwNo).Items.Clear()


            '    m_lReturn& = ListView_GridLines(lvwFinSumm(iLstVwNo))
            'developer guide no.302
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwFinSumm(iLstVwNo).Handle.ToInt32(), v_vShowGridLines:=True), gPMConstants.PMEReturnCode)
            lvwFinSumm(iLstVwNo).GridLines = True

            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            'End If

            'remove the Sum Insured & Average columns
            lvwFinSumm(iLstVwNo).Columns.RemoveAt(ACColumn7 - 1)
            lvwFinSumm(iLstVwNo).Columns.RemoveAt(ACColumn6 - 1)


            'Change tha Column header "Paid to date" to "Recieved to date"


            lvwFinSumm(iLstVwNo).Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Increase the width of the "Recieved to date" column
            lvwFinSumm(iLstVwNo).Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))


            '-----------------------------------------------------Claim Summary
            If m_lIsRecovery = 0 Then

                sTabCap = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitleRec, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                m_lIsRecovery += 1

            ElseIf m_lIsRecovery = 1 Then

                sTabCap = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitleSal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                m_lIsRecovery += 1
                'Added payment Tab
            ElseIf m_lIsRecovery = 2 Then

                sTabCap = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitlePay, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                'Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                m_lIsRecovery += 1
            ElseIf m_lIsRecovery = 3 Then

                sTabCap = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitleReceipt, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                'End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            End If
            'ACTabTitlePay - Public Const added for Payment tab
            'from resource file with value 1402
            '-----------------------------------------------------Claim Summary


            SSTabHelper.SetTabCaption(tabMainTab, iLstVwNo, "" & iLstVwNo + 1 & " -  " & sTabCap)

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vRecoveryArray) Then
                Return result
            End If


            ' Assign the details to the interface.
            For lRow As Integer = m_vRecoveryArray.GetLowerBound(1) To m_vRecoveryArray.GetUpperBound(1)


                ' Assign the details to the first column.
                oListItem = lvwFinSumm(iLstVwNo).Items.Add(CStr(m_vRecoveryArray(ACColPerilDesc, lRow)).Trim())

                ' Assign details to other the columns
                If (CStr(m_vRecoveryArray(ACColInitResv, lRow)).Trim()) = "" Then m_vRecoveryArray(ACColInitResv, lRow) = 0
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vRecoveryArray(ACColInitResv, lRow)).Trim())

                If (CStr(m_vRecoveryArray(ACColRcvtoDt, lRow)).Trim()) = "" Then m_vRecoveryArray(ACColRcvtoDt, lRow) = 0
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vRecoveryArray(ACColRcvtoDt, lRow)).Trim())

                If (CStr(m_vRecoveryArray(ACColRevResv, lRow)).Trim()) = "" Then m_vRecoveryArray(ACColRevResv, lRow) = 0
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vRecoveryArray(ACColRevResv, lRow)).Trim())


                curCurrresv = CDec(CStr(m_vRecoveryArray(ACColInitResv, lRow)).Trim()) - CDec(CStr(m_vRecoveryArray(ACColRcvtoDt, lRow)).Trim()) + CDec(CStr(m_vRecoveryArray(ACColRevResv, lRow)).Trim())


                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(curCurrresv).Trim())


                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwFinSumm(iLstVwNo).Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwFinSumm(iLstVwNo).Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwFinSumm(iLstVwNo).Items.Item(0).Selected = True

            ' Enable the interface now that the search has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterfaceRec", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' Date :15/07/2000
	'
	' Edit History:SK
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Try 
			
			
			'''*************************** fine code
			''
			''    tabMainTab.Width = Width - 345         '+ 1053.87
			''    tabMainTab.Height = Height - 930       '   566.719
			''
			''    cmdHelp.Left = Width - 1200         '2265
			''    cmdHelp.Top = Height - 750
			''
			''    cmdCancel.Left = Width - 2265
			''    cmdCancel.Top = Height - 750
			''
			''    cmdOK.Left = Width - 3315
			''    cmdOK.Top = Height - 750
			''
			''
			''    lvwFinSumm(0).Width = Width - 570
			''    lvwFinSumm(0).Height = Height - 1455
			'''*************************** fine code
			''
			''Dim itab As Integer
			'''    tabMainTab.Top = 0
			'''    tabMainTab.Left = 0
			'''    tabMainTab.Width = Me.Width - 240
			'''    tabMainTab.Height = Me.Height - 720
			''
			'''    Command1.Top = Me.Height - Command1.Height - 300
			'''    Text2.Top = Command1.Top - 50
			''    For itab = 0 To lvwFinSumm.UBound
			''        lvwFinSumm(itab).Top = tabMainTab.TabHeight * tabMainTab.Rows + 120
			''        lvwFinSumm(itab).Left = 120
			''        lvwFinSumm(itab).Width = tabMainTab.Width - (tabMainTab.Rows * 120) - 150
			''    If tabMainTab.Height - (tabMainTab.TabHeight * tabMainTab.Rows + 240) > 0 Then       '240
			''        lvwFinSumm(itab).Height = tabMainTab.Height - (tabMainTab.TabHeight * tabMainTab.Rows + 240)
			''    Else
			''        lvwFinSumm(itab).Height = 1
			''    End If
			''    Next itab
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class