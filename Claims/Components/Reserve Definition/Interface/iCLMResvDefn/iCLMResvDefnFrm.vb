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
	' Form Name     : frmInterface
	' Description   : Main interface.
	' Date          : 05/08/2000
	' Author        : SK
	' Edit History  :
	' RKS 01/12/2005 PN25979 Adding IsExcess field
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"
	
	Private Const ACColumn1 As Integer = 1
	Private Const ACColumn2 As Integer = 2
	Private Const ACColumn3 As Integer = 3
	Private Const ACColumn4 As Integer = 4
	Private Const ACColumn5 As Integer = 5
	Private Const ACColumn6 As Integer = 6
	Private Const ACColumn7 As Integer = 7
	
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
	
	'This variable is use to store the orignal value of the
	'text box when a value is selected from the listview
	'Its used for allowing the user to modify the text box value
	'back to its old value.
	Private m_sResvTypeNameTxtBx As String = ""
	
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMResvDefn.General
	
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	'flag set to True if the record can be modified or deleted
	'Private m_lModify As Boolean
	Private m_lModify As Integer
	Private m_bResvTypeChanged As Boolean
	
	
	
	'Variable for Underwriting/Broking
	Private m_lSiriusUnderWritingBroking As String = ""
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the GetResvTypCount method of business object.
	Public m_vSearchData As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vArray( ,  ) As Object
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = DisableInterface(bDisable:=True)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			''    m_lReturn& = g_oBusiness.GetResvTypCount(r_vResultArray:=m_vSearchData, _
			'''                                                    v_lclm_id:=ClaimId)
			''
			''    'Check for errors
			''    If (m_lReturn& <> PMTrue) Then
			''        ' Failed to get details.
			''        GetBusiness = PMFalse
			''        Exit Function
			''    End If
			
			

			m_lReturn = g_oBusiness.GetReserveTypes(r_vResultArray:=m_vTotalArray)
			' Check for errors
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				m_lReturn = DisableInterface(bDisable:=False)
				
				Return result
				
			ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then 
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'populate the list view for each tab
			'Assign Values to Interface
			m_lReturn = DataToInterfaceSumm()
			Me.Refresh()
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'After loading & populating all the listviews
			'set the default to the 1st tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)

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
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
			SSTabHelper.SetSelectedIndex(tabMainTab, iLstVwNo)
            'developer guide no. 233
            ContainerHelper.LoadControl(Me, lvwResvDefn.ToString, iLstVwNo)
            lvwResvDefn.Parent = tabMainTab
            lvwResvDefn.Visible = True
            lvwResvDefn.Top = VB6.TwipsToPixelsY(ACListViewTop)
            lvwResvDefn.Left = VB6.TwipsToPixelsX(ACListViewLeft)
            lvwResvDefn.Height = VB6.TwipsToPixelsY(ACListViewHeight)
            lvwResvDefn.Width = VB6.TwipsToPixelsX(ACListViewWidth)
			
			
			' Clear the search details.
            lvwResvDefn.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vArray) Then
				Return result
			End If
			
            '    m_lReturn& = ListView_GridLines(lvwResvDefn(iLstVwNo))
            'developer guide no.302
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwResvDefn.Handle.ToInt32(), v_vShowGridLines:=True)
            lvwResvDefn.GridLines = True
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Assign the details to the interface.
			For lRow As Integer = m_vArray.GetLowerBound(1) To m_vArray.GetUpperBound(1)
				
                SSTabHelper.SetTabCaption(tabMainTab, iLstVwNo, "" & iLstVwNo + 1 & " -  " & CStr(m_vArray(ACColDesc, lRow)).Trim())
				
				' Assign the details to the first column.
				' Column 1 Claim Type
                oListItem = lvwResvDefn.Items.Add(CStr(m_vArray(ACColPerilDesc, lRow)).Trim())
				
				' Assign details to other the columns
				' Column 2 Claim Ref
                oListItem.SubItems.Add(1).Text = CStr(m_vArray(ACColInitResv, lRow)).Trim()
				
				
				' Column 4 RiskIndex
                oListItem.SubItems.Add(2).Text = CStr(m_vArray(ACColPdtoDt, lRow)).Trim()
				' Column 5 Product Code
				
                oListItem.SubItems.Add(3).Text = CStr(m_vArray(ACColRevResv, lRow)).Trim()
				

				curCurrresv = CDec(CStr(m_vArray(ACColInitResv, lRow)).Trim()) - CDec(CStr(m_vArray(ACColPdtoDt, lRow)).Trim()) + CDec(CStr(m_vArray(ACColRevResv, lRow)).Trim())
				
				
                oListItem.SubItems.Add(4).Text = CStr(curCurrresv).Trim()
				
                oListItem.SubItems.Add(5).Text = CStr(m_vArray(ACColSumIns, lRow)).Trim()
                oListItem.SubItems.Add(6).Text = CStr(m_vArray(ACColAvg, lRow)).Trim()
				
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwResvDefn.Items.Item(iLstVwNo).Selected = True
                    ' Refresh the initial results.

                    lvwResvDefn.Refresh()
				End If
			Next lRow
			
            ' Select the first item.

            lvwResvDefn.Items.Item(0).Selected = True


			
			' Enable the interface now that the search has completed.
			m_lReturn = DisableInterface(bDisable:=False)
			
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
		Const ACColResvTypeID As Integer = 0
		Const ACColResvType As Integer = 1
		Const ACColDesc As Integer = 2
		Const ACColInclTot As Integer = 3
		Const ACColIsExcess As Integer = 4
		Const ACColIs_Indemnity As Integer = 5
		Const ACColIs_Expense As Integer = 6
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			
			' Clear the search details.
            lvwResvDefn.Items.Clear()

			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vTotalArray) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)

				' Assign the details to the first column.
				' Column 1 Reserve Type
                oListItem = lvwResvDefn.Items.Add(m_vTotalArray(ACColResvType, lRow).Trim())
				
				' Assign details to other the columns
				' Column 2 Description
                oListItem.SubItems.Add(1).Text = CStr(m_vTotalArray(ACColDesc, lRow)).Trim()
				
				
				If CStr(m_vTotalArray(ACColInclTot, lRow)).Trim() = "False" Then
					
					' Column 3 Include In Total
                    oListItem.SubItems.Add(2).Text = "No"
					
				ElseIf CStr(m_vTotalArray(ACColInclTot, lRow)).Trim() = "True" Then 
					
					' Column 3 Include In Total
                    oListItem.SubItems.Add(2).Text = "Yes"
				End If
				
				If CStr(m_vTotalArray(ACColIsExcess, lRow)).Trim() = "1" Then
					
					' Column 4 Is Excess
                    oListItem.SubItems.Add(3).Text = "Yes"
					
				Else
					' Column 4 Is Excess
                    oListItem.SubItems.Add(3).Text = "No"
				End If
				
				If CStr(m_vTotalArray(ACColIs_Indemnity, lRow)).Trim() = "1" Then
					
					' Column 5 Is Indemnity
                    oListItem.SubItems.Add(4).Text = "Yes"
					
				Else
					' Column 5 Is Indemnity
                    oListItem.SubItems.Add(4).Text = "No"
				End If
				
				If CStr(m_vTotalArray(ACColIs_Expense, lRow)).Trim() = "1" Then
					
					' Column 6 Is Expense
                    oListItem.SubItems.Add(5).Text = "Yes"
					
				Else
					' Column 6 Is Expense
                    oListItem.SubItems.Add(5).Text = "No"
				End If
				
				' Set the tag property with Reserve Type ID
				oListItem.Tag = CStr(m_vTotalArray(ACColResvTypeID, lRow)).Trim()

				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
                    lvwResvDefn.Items.Item(0).Selected = True
					
					' Refresh the initial results.
                    lvwResvDefn.Refresh()

				End If
				'    DoEvents
			Next lRow
			
			'    DoEvents
			
			
			' Select the first item.
			'    lvwResvDefn.ListItems(1).Selected = True
			
			' Enable the interface now that the search has completed.
			m_lReturn = DisableInterface(bDisable:=False)
			
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

            lSelectedItem = Convert.ToString(lvwResvDefn.Items.Item(lvwResvDefn.FocusedItem.Index).Tag)
			
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

            lvwResvDefn.Columns.Insert(ACColumn1 - 1, "", 94)
            lvwResvDefn.Columns.Insert(ACColumn2 - 1, "", 94)
            lvwResvDefn.Columns.Insert(ACColumn3 - 1, "", 94)
            lvwResvDefn.Columns.Insert(ACColumn4 - 1, "", 94)
            lvwResvDefn.Columns.Insert(ACColumn5 - 1, "", 94)
            lvwResvDefn.Columns.Insert(ACColumn6 - 1, "", 94)

			
			''    If m_lSiriusUnderWritingBroking = ACUnderWriting Then
			
			''Set the column widths

            lvwResvDefn.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwResvDefn.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(1750))
            lvwResvDefn.Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(1550))
            lvwResvDefn.Columns.Item(ACColumn4 - 1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwResvDefn.Columns.Item(ACColumn5 - 1).Width = CInt(VB6.TwipsToPixelsX(1550))
            lvwResvDefn.Columns.Item(ACColumn6 - 1).Width = CInt(VB6.TwipsToPixelsX(1000))


			
			''        'Set the Allignment of the columns to right justified
			''        lvwResvDefn.ColumnHeaders(ACColumn1).Alignment = 1
			''        lvwResvDefn.ColumnHeaders(ACColumn2).Alignment = 2
			''        lvwResvDefn.ColumnHeaders(ACColumn3).Alignment = 1
			
			''    End If
			
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Update the interface details with the
			' property members.
			m_lReturn = PropertiesToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = SetFirstLastControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			'-----------------------------------------------------
			cmdButton(ACAdd).Enabled = False
			cmdButton(ACModify).Enabled = False
			cmdButton(ACDelete).Enabled = False
			YesNoCheck1.CheckState = CheckState.Checked 'default YES
			chkIsExcess.CheckState = CheckState.Unchecked 'default NO
			'-----------------------------------------------------
			
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
			'lvwResvDefn.Items.Clear()
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
			'm_lReturn = DisableInterface(bDisable:=True)
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
			
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtReserveType
			m_ctlTabFirstLast(ACControlEnd, 0) = txtReserveType
			
			
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
            'Developer Guide No. :243
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

            'Developer Guide No. :243
            Label1(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblReserveType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            Label1(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            Label1(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblncludeInTotal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            Label1(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClbIsExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            Label1(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblIsIndemnity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            Label1(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblIsExpense, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdButton(ACAdd).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdButton(ACModify).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACModButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            cmdButton(ACDelete).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. :232,243
            lvwResvDefn.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
            'Developer Guide No. :232,243
            lvwResvDefn.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			''    If m_lSiriusUnderWritingBroking = ACUnderWriting Then
            'Developer Guide No. :232,243
            lvwResvDefn.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 232
            lvwResvDefn.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 232
            lvwResvDefn.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 232
            lvwResvDefn.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' Date :15/07/2000
	'
	' Edit History:SK
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			tabMainTab.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 300)
			tabMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1020)
			
			cmdButton(ACAdd).Left = Me.Width - VB6.TwipsToPixelsX(1665)
			cmdButton(ACModify).Left = Me.Width - VB6.TwipsToPixelsX(1665)
			cmdButton(ACDelete).Left = Me.Width - VB6.TwipsToPixelsX(1665)
			
			cmdHelp.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 1275)
			cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 870)
			
			cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 2475)
			cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 870)
			
			cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 3675)
			cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 870)
			
            'developer guide no. 233
            lvwResvDefn.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 2130)
            lvwResvDefn.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 3300)
			
			Return result
		
        Catch
            Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	' PRIVATE Methods (End)
	
	Private Sub chkIsExcess_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsExcess.CheckStateChanged
		
		
		Select Case m_lModify
			Case 0
				
				'if an item in the listview is NOT selected
				'ie. your trying to Add a new item to the database
				cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACModify).Enabled = False
				cmdButton(ACDelete).Enabled = False
				
			Case 1
				'if an item in the listview IS selected
				'ie. your trying to Modify or Delete an item from the database
				cmdButton(ACModify).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACDelete).Enabled = True
				cmdButton(ACAdd).Enabled = False
				
			Case 2
				If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
					cmdButton(ACModify).Enabled = True
				Else
					cmdButton(ACModify).Enabled = False
					cmdButton(ACAdd).Enabled = False
				End If
				
				cmdButton(ACDelete).Enabled = True
				
				
		End Select
	End Sub
	
	Private Sub chkIsExpense_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsExpense.CheckStateChanged
		If chkIsExpense.CheckState = CheckState.Checked Then
			chkIsIndemnity.CheckState = CheckState.Unchecked
		End If
		
		
		Select Case m_lModify
			Case 0
				
				'if an item in the listview is NOT selected
				'ie. your trying to Add a new item to the database
				cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACModify).Enabled = False
				cmdButton(ACDelete).Enabled = False
				
			Case 1
				'if an item in the listview IS selected
				'ie. your trying to Modify or Delete an item from the database
				cmdButton(ACModify).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACDelete).Enabled = True
				cmdButton(ACAdd).Enabled = False
				
			Case 2
				If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
					cmdButton(ACModify).Enabled = True
				Else
					cmdButton(ACModify).Enabled = False
					cmdButton(ACAdd).Enabled = False
				End If
				
                cmdButton(ACDelete).Enabled = True
				
		End Select
	End Sub
	
	Private Sub chkIsIndemnity_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsIndemnity.CheckStateChanged
		If chkIsIndemnity.CheckState = CheckState.Checked Then
			chkIsExpense.CheckState = CheckState.Unchecked
		End If

		Select Case m_lModify
			Case 0
				
				'if an item in the listview is NOT selected
				'ie. your trying to Add a new item to the database
				cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACModify).Enabled = False
				cmdButton(ACDelete).Enabled = False
				
			Case 1
				'if an item in the listview IS selected
				'ie. your trying to Modify or Delete an item from the database
				cmdButton(ACModify).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""
				
				cmdButton(ACDelete).Enabled = True
				cmdButton(ACAdd).Enabled = False
				
			Case 2
				If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
					cmdButton(ACModify).Enabled = True
				Else
					cmdButton(ACModify).Enabled = False
					cmdButton(ACAdd).Enabled = False
				End If
				
                cmdButton(ACDelete).Enabled = True
		End Select
	End Sub
	
	Private Sub cmdButton_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdButton_2.Click, _cmdButton_1.Click, _cmdButton_0.Click
		Dim Index As Integer = Array.IndexOf(cmdButton, eventSender)
		
		Try 
            Dim vresultarray(,) As Object
			Dim lDelCount As Integer

			Dim iVal As Integer
			Dim lRecordCount As Integer
			Dim sMsg1, sMsg2, sMsgHeading As String
			Dim iValIsExcess, iValIs_Indemnity, iValIs_Expense As Integer
			
            'Developer Guide No.: 243
            sMsg1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            sMsg2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            sMsgHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

			If YesNoCheck1.CheckState = CheckState.Unchecked Then
				iVal = 0
			ElseIf YesNoCheck1.CheckState = CheckState.Checked Then 
				iVal = 1
			End If

			If chkIsExcess.CheckState = CheckState.Unchecked Then
				iValIsExcess = 0
			ElseIf chkIsExcess.CheckState = CheckState.Checked Then 
				iValIsExcess = 1
			End If
			
			If chkIsIndemnity.CheckState = CheckState.Unchecked Then
				iValIs_Indemnity = 0
			ElseIf chkIsIndemnity.CheckState = CheckState.Checked Then 
				iValIs_Indemnity = 1
			End If
			
			If chkIsExpense.CheckState = CheckState.Unchecked Then
				iValIs_Expense = 0
			ElseIf chkIsExpense.CheckState = CheckState.Checked Then 
				iValIs_Expense = 1
			End If

			Select Case Index
				Case ACAdd
					
					'Check if the reserve Type is not being used by any claim

					m_lReturn = g_oBusiness.ChkResvTypeNameExists(lRecordCount, txtReserveType.Text.Trim())
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						'            Update = PMFalse
						Exit Sub
					End If
					
					If lRecordCount < 1 Then
						' Add Item
						g_lReserveTypeID = 0

						m_lReturn = g_oBusiness.EditAdd(1, vReserveTypeID:=g_lReserveTypeID, vDescription:=txtDescription.Text.Trim(), vIncludeInTotal:=iVal, vName:=txtReserveType.Text.Trim(), vIsExcess:=iValIsExcess, vIs_Indemnity:=iValIs_Indemnity, vIs_Expense:=iValIs_Expense)
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							'                Update = PMFalse
							Exit Sub
						End If
					Else
						MessageBox.Show(sMsg1 & txtReserveType.Text.Trim().ToUpper() & " " & sMsg2, sMsgHeading, MessageBoxButtons.OK)
						Exit Sub
						
					End If

				Case ACModify
					
					'only check if the NAME text box already exists in the database if
					'the Orignal NAME text box value is not the same as the current value for the same record
					If m_sResvTypeNameTxtBx.Trim() <> txtReserveType.Text.Trim() Then
						'Check if the reserve Type is not being used by any claim

						m_lReturn = g_oBusiness.ChkResvTypeNameExists(lRecordCount, txtReserveType.Text.Trim())
					End If
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						'            Update = PMFalse
						Exit Sub
					End If
					
                    If lRecordCount < 1 Then 'if zero records are returned, then Update
                        ' Add Item

                        'developer guide no. 233
                        g_lReserveTypeID = Convert.ToString(lvwResvDefn.FocusedItem.Tag)

                        m_lReturn = g_oBusiness.EditUpdate(1, vReserveTypeID:=g_lReserveTypeID, vDescription:=txtDescription.Text.Trim(), vIncludeInTotal:=iVal, vName:=txtReserveType.Text.Trim(), vIsExcess:=iValIsExcess, vIs_Indemnity:=iValIs_Indemnity, vIs_Expense:=iValIs_Expense)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            '                Update = PMFalse
                            Exit Sub
                        End If
                    Else
                        MessageBox.Show(sMsg1 & txtReserveType.Text.Trim().ToUpper() & " " & sMsg2, sMsgHeading, MessageBoxButtons.OK)
                        Exit Sub

                    End If
					
					'        g_lReserveTypeID = lvwResvDefn.SelectedItem.Tag
					'        m_lReturn = g_oBusiness.EditUpdate(1, vReserveTypeID:=g_lReserveTypeID, vDescription:=Trim$(txtDescription), _
					''                      vIncludeInTotal:=iVal, vName:=Trim$(txtReserveType), vIsExcess:=iValIsExcess)
					
				Case ACDelete

                    'g_lReserveTypeID = Convert.ToString(lvwResvDefn.FocusedItem.Tag)
                    g_lReserveTypeID = Convert.ToString(lvwResvDefn.FocusedItem.Tag)
					
					'****************Start of Code Change Reserve Type as vallidation is has shifted to interfaec
					'Pandu 20-10-2000 Bug Under Client Server Mode
					'Check if the reserve Type is not being used by any claim

					m_lReturn = g_oBusiness.GetClmForResvType(lDelCount, vresultarray, g_lReserveTypeID)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						'Update = PMFalse
						Exit Sub
					End If
					
					If lDelCount >= 1 Then
						
                        MessageBox.Show("Reserve Type is being used by a Peril type.", "Invalid Action", MessageBoxButtons.OK)
                        txtReserveType.Select()
                        txtReserveType.Focus()
                        Exit Sub
						
					End If
					
					'****************end of Code Change Reserve Type as vallidation is has shifted to interfaec
                    m_lReturn = g_oBusiness.Editdelete(1, vReserveTypeID:=g_lReserveTypeID)
            End Select

			m_lReturn = g_oBusiness.Update

			g_oBusiness.ClearColl()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			m_lReturn = UpdListview(Index)

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			txtReserveType.Text = ""
			txtReserveType.Focus()
			txtDescription.Text = ""
			YesNoCheck1.CheckState = CheckState.Unchecked
			chkIsExcess.CheckState = CheckState.Unchecked
			chkIsIndemnity.CheckState = CheckState.Unchecked
			chkIsExpense.CheckState = CheckState.Unchecked
			'-----------------------------------------------------
			cmdButton(ACAdd).Enabled = False
			cmdButton(ACModify).Enabled = False
			YesNoCheck1.CheckState = CheckState.Checked 'default YES
			cmdButton(ACDelete).Enabled = False
			'-----------------------------------------------------

            If lvwResvDefn.Items.Count < 1 Then
                cmdButton(ACDelete).Enabled = False
            End If

            m_bResvTypeChanged = False
            'm_lModify = False
            m_lModify = 0

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdButton click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdButton_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	Private Function UpdListview(ByVal Button As Integer) As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		Dim sVal As String = ""
		Dim sValIsExcess, sValIs_Indemnity, sValIs_Expense As String
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		If YesNoCheck1.CheckState = CheckState.Unchecked Then
			sVal = "No"
		ElseIf YesNoCheck1.CheckState = CheckState.Checked Then 
			sVal = "Yes"
		End If
		
		If chkIsExcess.CheckState = CheckState.Unchecked Then
			sValIsExcess = "No"
		ElseIf chkIsExcess.CheckState = CheckState.Checked Then 
			sValIsExcess = "Yes"
		End If
		
		If chkIsIndemnity.CheckState = CheckState.Unchecked Then
			sValIs_Indemnity = "No"
		ElseIf chkIsIndemnity.CheckState = CheckState.Checked Then 
			sValIs_Indemnity = "Yes"
		End If
		
		If chkIsExpense.CheckState = CheckState.Unchecked Then
			sValIs_Expense = "No"
		ElseIf chkIsExpense.CheckState = CheckState.Checked Then 
			sValIs_Expense = "Yes"
		End If
		
		
		Select Case Button
			Case ACAdd
				
				' Assign the details to the first column.
				' Column 1 Reserve Type
                'developer guide no. 233
                oListItem = lvwResvDefn.Items.Add(txtReserveType.Text.Trim())
				
				' Assign details to other the columns
				' Column 2 Description
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDescription.Text.Trim()
				
				' Column 3 Include In Total
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sVal
				
				' Column 4 Is Excess
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sValIsExcess
				
				'Column 5 Is Indemnity
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sValIs_Indemnity
				
				'Column 6 Is Indemnity
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = sValIs_Expense
				
				' Set the tag property with Reserve Type ID

				oListItem.Tag = g_oBusiness.ReserveTypeID
				
				' Refresh the initial results.
                'developer guide no. 233
                lvwResvDefn.Refresh()

			Case ACModify
				
                'developer guide no. 233
                lvwResvDefn.Items.RemoveAt(lvwResvDefn.FocusedItem.Index)
				' Assign the details to the first column.
				' Column 1 Reserve Type

                'oListItem = lvwResvDefn.Items.Add(txtReserveType.Text.Trim())
                oListItem = lvwResvDefn.Items.Add(txtReserveType.Text.Trim())
				
				' Assign details to other the columns
				' Column 2 Description
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDescription.Text.Trim()
				
				' Column 3 Include In Total
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sVal
				
				' Column 3 Is Excess
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sValIsExcess
				
				'Column 5 Is Indemnity
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sValIs_Indemnity
				
				'Column 6 Is Indemnity
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = sValIs_Expense
				
				' Set the tag property with Reserve Type ID

                oListItem.Tag = g_oBusiness.ReserveTypeID
                ' Refresh the initial results.
                lvwResvDefn.Refresh()
            Case ACDelete

                lvwResvDefn.Items.RemoveAt(lvwResvDefn.FocusedItem.Index)
                ' Refresh the initial results.
                lvwResvDefn.Refresh()
        End Select
		
		Return result
	End Function
	' ***************************************************************** '
	' Name:         FormActivate
	' Description:  Loads all required details of the form
	' Date:         15/07/00
	' Edit History: SK
	' ***************************************************************** '
	
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
			m_oGeneral = New iCLMResvDefn.General()
			
			' Create an instance of the interface object.
			'    Set m_oInterface = New iCLMResvDefn.Interface
			
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
			
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
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
			
			'Zero because the form will load in the ADD mode
			m_lModify = 0
			
			'Set the UnderWriting/Broking Constant
			m_lReturn = iPMFunc.getUnderwritingOrAgency(m_lSiriusUnderWritingBroking)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
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
			m_lReturn = m_oGeneral.GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
            'develope guide no.303
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwResvDefn.Handle.ToInt32(), v_vShowRowSelect:=1)
            lvwResvDefn.FullRowSelect = True

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If

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

            'developer guide no. 19 (no solution)
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

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
            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
                tabMainTab.Focus()
            End If
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
			
			m_lReturn = ResizeInterface()
			'    Me.Refresh
		
		Catch 
            Exit Sub
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	' Name          :lvwResvDefn_Click
	' Description   :Fill the Claim Reference,Policy No.,Client Short Name
	'                   in Text Box for the listitem clicked
	' Date          :11/07/00
	' Edit History  :SK
	' ***************************************************************** '

    Private Sub lvwResvDefn_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwResvDefn.Click

        Dim sindex As Integer
        If lvwResvDefn.Items.Count > 0 Then
            m_lModify = 1
            sindex = Convert.ToString(lvwResvDefn.FocusedItem.Tag)
            'used to hold the orignal NAME txtbox value
            m_sResvTypeNameTxtBx = lvwResvDefn.FocusedItem.Text
            txtReserveType.Text = lvwResvDefn.FocusedItem.Text

            txtDescription.Text = lvwResvDefn.FocusedItem.SubItems(1).Text

            'developer guide no. 52
            If lvwResvDefn.FocusedItem.SubItems(2).Text = "No" Then

                YesNoCheck1.CheckState = CheckState.Unchecked
                'developer guide no. 52
            ElseIf lvwResvDefn.FocusedItem.SubItems(2).Text = "Yes" Then
                YesNoCheck1.CheckState = CheckState.Checked
            End If
            'developer guide no. 52
            If lvwResvDefn.FocusedItem.SubItems(3).Text = "No" Then

                chkIsExcess.CheckState = CheckState.Unchecked

                'developer guide no. 52
            ElseIf lvwResvDefn.FocusedItem.SubItems(3).Text = "Yes" Then
                chkIsExcess.CheckState = CheckState.Checked
            End If
            'developer guide no. 52
            If lvwResvDefn.FocusedItem.SubItems(4).Text = "No" Then

                chkIsIndemnity.CheckState = CheckState.Unchecked
                'developer guide no. 52
            ElseIf lvwResvDefn.FocusedItem.SubItems(4).Text = "Yes" Then
                chkIsIndemnity.CheckState = CheckState.Checked
            End If
            'developer guide no. 52
            If lvwResvDefn.FocusedItem.SubItems(5).Text = "No" Then
                chkIsExpense.CheckState = CheckState.Unchecked
                'developer guide no. 52
            ElseIf lvwResvDefn.FocusedItem.SubItems(5).Text = "Yes" Then
                chkIsExpense.CheckState = CheckState.Checked
            End If
            '------------------------------------------------------------------------
            cmdButton(ACDelete).Enabled = True
            cmdButton(ACModify).Enabled = False
            cmdButton(ACAdd).Enabled = False
            '                m_lModify = True
            m_lModify = 2
            '------------------------------------------------------------------------

            VB6.SetDefault(cmdOK, True)

        End If

    End Sub

    ' ***************************************************************** '
    ' Name: lvwResvDefn_ColumnClick
    '
    ' Description:Sort the Details of List View as per the column clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwResvDefn.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwResvDefn.Columns(eventArgs.Column)

        Try
            'Commented below code and handled sorting through the below function.
            ListViewFunc.SortListView(lvControl:=lvwResvDefn, eventArgs:=eventArgs)

            'With lvwResvDefn
            '    ' If date column clicked, then sort by date sort column
            '    If ColumnHeader.Index + 1 - 1 = 4 Then
            '        ListViewHelper.SetSortedProperty(lvwResvDefn, False)
            '        If ListViewHelper.GetSortKeyProperty(lvwResvDefn) <> 5 Then
            '            ListViewHelper.SetSortKeyProperty(lvwResvDefn, 5)
            '            ListViewHelper.SetSortOrderProperty(lvwResvDefn, SortOrder.Ascending)
            '        Else

            '            ListViewHelper.SetSortOrderProperty(lvwResvDefn, (ListViewHelper.GetSortOrderProperty(lvwResvDefn) + 1) Mod 2)
            '        End If

            '        ListViewHelper.SetSortedProperty(lvwResvDefn, True)

            '        ' If current sort column header is
            '        ' pressed.

            '    ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwResvDefn)) Then
            '        ' Set sort order opposite of
            '        ' current direction.

            '        ListViewHelper.SetSortOrderProperty(lvwResvDefn, (ListViewHelper.GetSortOrderProperty(lvwResvDefn) + 1) Mod 2)
            '        ListViewHelper.SetSortedProperty(lvwResvDefn, True) 'Line Added
            '    Else
            '        ' Sort by this column (ascending).

            '        ListViewHelper.SetSortedProperty(lvwResvDefn, False)

            '        ' Turn off sorting so that the list
            '        ' is not sorted twice

            '        ListViewHelper.SetSortOrderProperty(lvwResvDefn, SortOrder.Ascending)
            '        ListViewHelper.SetSortKeyProperty(lvwResvDefn, ColumnHeader.Index + 1 - 1)
            '        ListViewHelper.SetSortedProperty(lvwResvDefn, True)
            '    End If
            'End With

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwResvDefn_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: lvwResvDefn_DblClick
    '
    ' Description:Move to the next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwResvDefn.DoubleClick

    End Sub

    ' ***************************************************************** '
    ' Name: lvwResvDefn_GotFocus
    '
    ' Description:Set Ok Button a default
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwResvDefn.Enter
        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can select with Enter key.
            '    cmdFindNow.Default = False
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwResvDefn_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name:lvwResvDefn_KeyDown
    '
    ' Description:Set Command Button Ok as Not Default on Pressing Enter Key
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwResvDefn.KeyDown

        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

     
    End Sub

    ' ***************************************************************** '
    ' Name:lvwResvDefn_KeyPress
    '
    ' Description:Fill the Policy Number in Text Box when enter button is
    '               pressed when focus is  on list item
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwResvDefn.KeyPress

        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)


        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: lvwResvDefn_LostFocus
    '
    ' Description:Set find now as default
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwResvDefn_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwResvDefn.Leave
        ' LostFocus Event for the search details
        Try
            ' Set the default button.
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwResvDefn_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
    ' RKS 01/12/2005 PN25979 Adding IsExcess field
    ' **************************************************************** '
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
            m_lReturn = m_oGeneral.ProcessCommand()

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
            m_lReturn = m_oGeneral.ProcessCommand()

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


    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Select Case m_lModify
            Case 0

                'if an item in the listview is NOT selected
                'ie. your trying to Add a new item to the database
                cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""

                cmdButton(ACModify).Enabled = False
                cmdButton(ACDelete).Enabled = False

            Case 1 'if an item in the listview IS selected
                'ie. your trying to Modify or Delete an item from the database
                If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
                    '        cmdButton(ACModify).Enabled = True
                    '    Else
                    cmdButton(ACModify).Enabled = False
                    cmdButton(ACAdd).Enabled = False
                End If

                cmdButton(ACDelete).Enabled = True

            Case 2
                If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
                    cmdButton(ACModify).Enabled = True
                    If m_bResvTypeChanged Then
                        cmdButton(ACAdd).Enabled = True
                    End If
                Else
                    cmdButton(ACModify).Enabled = False
                    cmdButton(ACAdd).Enabled = False
                End If

                cmdButton(ACDelete).Enabled = True


        End Select
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        iPMFunc.SelectText(txtDescription)
    End Sub

    Private Sub txtReserveType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReserveType.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If


        Select Case m_lModify
            Case 0
                'if an item in the listview is NOT selected
                'ie. your trying to add a new item to the database
                cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""

                cmdButton(ACModify).Enabled = False
                cmdButton(ACDelete).Enabled = False

            Case 1
                'if an item in the listview IS selected
                'ie. your trying to Modify or Delete an item from the database
                cmdButton(ACModify).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""

                cmdButton(ACDelete).Enabled = True
                cmdButton(ACAdd).Enabled = False

            Case 2

                m_bResvTypeChanged = True

                If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
                    cmdButton(ACModify).Enabled = True
                    cmdButton(ACAdd).Enabled = True
                Else
                    cmdButton(ACModify).Enabled = False
                    cmdButton(ACAdd).Enabled = False
                End If
                cmdButton(ACDelete).Enabled = True
        End Select
    End Sub

    Private Sub txtReserveType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReserveType.Enter
        iPMFunc.SelectText(txtReserveType)
    End Sub

    Private Sub txtReserveType_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtReserveType.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim bOK As Boolean

        '1-10
        If KeyAscii > 47 And KeyAscii < 58 Then
            bOK = True
        End If

        'A-Z
        If KeyAscii > 64 And KeyAscii < 91 Then
            bOK = True
        End If

        'a-z
        If KeyAscii > 96 And KeyAscii < 123 Then
            bOK = True
        End If

        'Backspace or Space or Ctrl+C or Ctrl+V
        If KeyAscii = 8 Or KeyAscii = 32 Or KeyAscii = 3 Or KeyAscii = 22 Then
            bOK = True
        End If

        If Not bOK Then
            'type nothing
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub YesNoCheck1_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles YesNoCheck1.CheckStateChanged



        Select Case m_lModify
            Case 0

                'if an item in the listview is NOT selected
                'ie. your trying to Add a new item to the database
                cmdButton(ACAdd).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""

                cmdButton(ACModify).Enabled = False
                cmdButton(ACDelete).Enabled = False

            Case 1
                'if an item in the listview IS selected
                'ie. your trying to Modify or Delete an item from the database
                cmdButton(ACModify).Enabled = txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> ""

                cmdButton(ACDelete).Enabled = True
                cmdButton(ACAdd).Enabled = False

            Case 2
                If txtReserveType.Text.Trim() <> "" And txtDescription.Text.Trim() <> "" Then
                    cmdButton(ACModify).Enabled = True
                Else
                    cmdButton(ACModify).Enabled = False
                    cmdButton(ACAdd).Enabled = False
                End If
                cmdButton(ACDelete).Enabled = True
        End Select
    End Sub
End Class
