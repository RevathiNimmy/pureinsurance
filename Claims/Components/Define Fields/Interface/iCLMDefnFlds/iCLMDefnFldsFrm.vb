Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No: 129
Imports SharedFiles



Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmInterface
	' Description   : Main interface.
	' Date          : 05/08/2000
	' Author        : SK
	' Edit History  :
	' 20020404 PSL Add Tab id combo and tab caption column to listview
	' ***************************************************************** '
	
	
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
	Private Const ACColumn8 As Integer = 8
	
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
	
	' Variables for Define Feilds Screen-05/09/2000
	Private m_lMode As Integer
	Private m_lTypeId As Integer
	Private m_sTypeName As String = ""
	Private m_bCaptionChanged As Boolean
	
	'TN20010501 Start
	Private m_lDataMode As gPMConstants.PMEComponentAction '0 - risk and 1 - peril
	'TN20010501 End
	
	'This variable is used to hold the value of the Display Order
	'which is used while checking te maximum display order permitable.
	'The value is assigned to it while the form is loading & whenever
	'a new value is being added to the database
	Private m_vDisplayOrder As Double
	
	'These variable is use to store the orignal value of the
	'text box when a value is selected from the listview
	'Its used for allowing the user to modify the text box value
	'back to its old value.
	Private m_sCaptionTxtBx As String = ""
	Private m_vDispOrdTxtBx As String = ""
	Private m_sDescTxtBx As String = ""
	Private m_iTypeCbo As Integer
	Private m_iLookUpCbo As Integer
	Private m_bMandatory As Boolean
	Private m_bReadOnly As Boolean
	
	Private m_bListViewClick As Boolean
	Private m_bFormLoad As Boolean
	
	
	
	'Declare an instance of the general interface object.
	Private m_oGeneral As iCLMDefnFlds.General
	
	
	'Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	'flag set to
	'0 -if an item in the listview is NOT selected
	'ie. your trying to add a new item to the database
	'1 _if an item in the listview IS selected
	'ie. your trying to Modify or Delete an item from the database
	
	Private m_lModify As Integer
	
	'Variable for Underwriting/Broking
	Private m_sSiriusUnderWritingBroking As String = ""
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the GetResvTypCount method of business object.
	Public m_vSearchData As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vArray( ,  ) As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vTotalArray( ,  ) As Object
	

    'Developer Guide No: 7
    Private Const vbFormCode As Integer = 0

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
	
	'TN20010501 End
	

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
	Public Property TypeName() As String
		Get
			
			Return m_sTypeName
			
		End Get
		Set(ByVal Value As String)
			
			m_sTypeName = Value
			
		End Set
	End Property
	Public Property TypeId() As Integer
		Get
			
			'Return TypeId
			Return m_lTypeId
			
		End Get
		Set(ByVal Value As Integer)
			
			'Set Claim Number
			m_lTypeId = Value
			
		End Set
	End Property
	Public Property Mode() As Integer
		Get
			
			Return m_lMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lMode = Value
			
		End Set
	End Property
	
	'TN20010501 Start
	
	Public Property DataMode() As Integer
		Get
			Return m_lDataMode
		End Get
		Set(ByVal Value As Integer)
			m_lDataMode = Value
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
			
			Dim lRecordsFound As gPMConstants.PMEReturnCode
			
			Const ACColDispOrd As Integer = 4
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Disable parts of the interface while
			' a search is in progress.
			'    m_lReturn& = DisableInterface( _
			'bDisable:=True)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'TN20010501 Start -- use DataMode instead of Mode
			'If Mode = ACRiskMode Then
			If DataMode = ACRiskMode Then
				

				m_lReturn = g_oBusiness.GetRiskDataDefn(r_vResultArray:=m_vTotalArray, v_lRiskTypeId:=TypeId, r_lRecordsFound:=lRecordsFound)
				
				'ElseIf Mode = ACPerilMode Then
			ElseIf Mode = ACPerilMode Then 
				'TN20010501 End
				

				m_lReturn = g_oBusiness.GetPerilDataDefn(r_vResultArray:=m_vTotalArray, v_lPerilTypeId:=TypeId, r_lRecordsFound:=lRecordsFound)
				
			End If
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound Then
				' Failed to get details.
				'        GetBusiness = PMFalse
				txtDispOrd.Text = CStr(1)
				m_vDisplayOrder = 1
				Return result
			End If
			
			
			
			'Set the default value of the display order text box to ONE more
			'than the maximum existing display order.
			'We get this value from the last row of the result array as
			'the Select query has ORDER BY DISPLAY_ORDER clause
			txtDispOrd.Text = CStr(Conversion.Val(CStr(m_vTotalArray(ACColDispOrd, m_vTotalArray.GetUpperBound(1)))) + 1)
			'Also assign the same value to this variable
			m_vDisplayOrder = Conversion.Val(CStr(m_vTotalArray(ACColDispOrd, m_vTotalArray.GetUpperBound(1)))) + 1
			
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
			'tabMainTab.Tab = 0
			
			
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


            'Developer Guide No. : 233
            ContainerHelper.LoadControl(Me, lvwDefnFlds.ToString(), iLstVwNo)
            lvwDefnFlds.Parent = tabMainTab
            lvwDefnFlds.Visible = True
            lvwDefnFlds.Top = VB6.TwipsToPixelsY(ACListViewTop)
            lvwDefnFlds.Left = VB6.TwipsToPixelsX(ACListViewLeft)
            lvwDefnFlds.Height = VB6.TwipsToPixelsY(ACListViewHeight)
            lvwDefnFlds.Width = VB6.TwipsToPixelsX(ACListViewWidth)
			
			
			' Clear the search details.
            lvwDefnFlds.Items.Clear()

			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vArray) Then
				Return result
			End If
			
            '    m_lReturn& = ListView_GridLines(lvwDefnFlds(iLstVwNo))

            m_lReturn = SetExtraListViewProperties(v_hWndList:=CType(lvwDefnFlds.Items(iLstVwNo).ToString(), Int32), v_vShowGridLines:=True)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Assign the details to the interface.
			For lRow As Integer = m_vArray.GetLowerBound(1) To m_vArray.GetUpperBound(1)
				
				SSTabHelper.SetTabCaption(tabMainTab, iLstVwNo, "&" & iLstVwNo + 1 & " -  " & CStr(m_vArray(ACColDesc, lRow)).Trim())
				
				' Assign the details to the first column.
                ' Column 1 Claim Type
                oListItem = lvwDefnFlds.Items.Add(CStr(m_vArray(ACColPerilDesc, lRow)).Trim())

				' Assign details to other the columns
				' Column 2 Claim Ref
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vArray(ACColInitResv, lRow)).Trim()
				
				
				' Column 4 RiskIndex
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vArray(ACColPdtoDt, lRow)).Trim()
				' Column 5 Product Code
				
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vArray(ACColRevResv, lRow)).Trim()
				
				
				curCurrresv = CDec(CStr(m_vArray(ACColInitResv, lRow)).Trim()) - CDec(CStr(m_vArray(ACColPdtoDt, lRow)).Trim()) + CDec(CStr(m_vArray(ACColRevResv, lRow)).Trim())
				
				
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(curCurrresv).Trim()
				
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vArray(ACColSumIns, lRow)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vArray(ACColAvg, lRow)).Trim()
				
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.

                    lvwDefnFlds.Items.Item(0).Selected = True

					' Refresh the initial results.
                    lvwDefnFlds.Refresh()
				End If
			Next lRow
			
            ' Select the first item.
            lvwDefnFlds.Items.Item(0).Selected = True

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
		Dim iType As Integer
		
		
		
		'Const ACFindImage = "FindImage"
		
		Const ACColDataDefnId As Integer = 0
		Const ACColCaption As Integer = 1
		Const ACColDesc As Integer = 2
		Const ACColType As Integer = 3
		Const ACColDispOrd As Integer = 4
		Const ACColMand As Integer = 5
		Const ACColReadOnly As Integer = 6
        'Const ACColClmPrtyTypeId As Integer = 7
		Const ACColClmPrtyTypeDesc As Integer = 8
        'Const ACColClmLookupId As Integer = 9
		Const ACColClmLookupName As Integer = 10
        'Const ACColTab As Integer = 11
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'make the tab on which the listview is to be put as the current tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			
			' Clear the search details.
			lvwDefnFlds.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vTotalArray) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)
				
				
				' Assign the details to the first column.
				' Column 1 Caption
				oListItem = lvwDefnFlds.Items.Add(CStr(m_vTotalArray(ACColCaption, lRow)).Trim())
				
				' Assign details to other the columns
				' Column 2 Description
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vTotalArray(ACColDesc, lRow)).Trim()
				
				iType = CInt(CStr(m_vTotalArray(ACColType, lRow)).Trim())
				
				Select Case iType
					Case ACText
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Text"
					Case ACInteger
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Integer"
					Case ACDate
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Date"
					Case ACYesNo
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes/No"
					Case ACLookUp
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Lookup"
					Case ACParty
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Party"
						
						'DC140302
					Case ACTabName
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Screen Title"
						
				End Select
				
				
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vTotalArray(ACColDispOrd, lRow)).Trim()
				
				'if Type=LookUp then populate from the Lookup Table Name column of the array
				'into the Party/Lookup column of the listview
				If StringsHelper.ToDoubleSafe(CStr(m_vTotalArray(ACColType, lRow)).Trim()) = ACLookUp Then
					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vTotalArray(ACColClmLookupName, lRow)).Trim()
				ElseIf StringsHelper.ToDoubleSafe(CStr(m_vTotalArray(ACColType, lRow)).Trim()) = ACParty Then 
					'if Type=Party then populate from the Party Desc column of the array
					'into the Party/Lookup column of the listview
					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vTotalArray(ACColClmPrtyTypeDesc, lRow)).Trim()
				End If
				
				
				If CStr(m_vTotalArray(ACColMand, lRow)).Trim() = "False" Then
					
					' Column 3 Include In Total
					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "No"
					
				ElseIf CStr(m_vTotalArray(ACColMand, lRow)).Trim() = "True" Then 
					
					' Column 3 Include In Total
					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "Yes"
				End If
				
				
				If CStr(m_vTotalArray(ACColReadOnly, lRow)).Trim() = "False" Then
					
					' Column 3 Include In Total
					ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "No"
					
				ElseIf CStr(m_vTotalArray(ACColReadOnly, lRow)).Trim() = "True" Then 
					
					' Column 3 Include In Total
					ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "Yes"
				End If
				
				'DC110603 -ISS4415 - to not use Tab for now
				'PSL added the Tab col
				'oListItem.SubItems(7) = m_vTotalArray(ACColTab, lRow&)
				
				' Set the tag property with Reserve Type ID
				oListItem.Tag = CStr(m_vTotalArray(ACColDataDefnId, lRow)).Trim()
				
				
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
					lvwDefnFlds.Items.Item(0).Selected = True
					
					' Refresh the initial results.
					lvwDefnFlds.Refresh()
				End If
				'    DoEvents
			Next lRow
			
			'    DoEvents
			
			
			' Select the first item.
			'    lvwDefnFlds.ListItems(1).Selected = True
			
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

			lSelectedItem = Convert.ToString(lvwDefnFlds.Items.Item(lvwDefnFlds.FocusedItem.Index).Tag)
			
			
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
		Dim sMode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'TN20010501 Start
			'If Mode = ACRiskMode Then
			If DataMode = ACRiskMode Then
				


                'Developer Guide No:243
                sMode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				'ElseIf Mode = ACPerilMode Then
			ElseIf DataMode = ACPerilMode Then 
				'TN20010501 End
				


                'Developer Guide No:243
                sMode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
			End If
			
			'Update the interface details.
			Me.Text = Me.Text & " " & sMode & " / " & TypeName
			
			
			'    txtTypeName.Text = Trim(m_sModeName$)
			
			Return result
		
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
			
			'KB 17032003 PN Issue 2460
			'For Broking we want to default the field to manual
			'we already have a global variable to contain this
			m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sSiriusUnderWritingBroking)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Add the columns to the listview
			lvwDefnFlds.Columns.Insert(ACColumn1 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn2 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn3 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn4 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn5 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn6 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn7 - 1, "", 94)
			lvwDefnFlds.Columns.Insert(ACColumn8 - 1, "", 94)
			
			''    If m_sSiriusUnderWritingBroking = ACUnderWriting Then
			
			''Set the column widths
			lvwDefnFlds.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
			lvwDefnFlds.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
			lvwDefnFlds.Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(600))
			lvwDefnFlds.Columns.Item(ACColumn4 - 1).Width = CInt(VB6.TwipsToPixelsX(1100))
			lvwDefnFlds.Columns.Item(ACColumn5 - 1).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwDefnFlds.Columns.Item(ACColumn6 - 1).Width = CInt(VB6.TwipsToPixelsX(800))
			lvwDefnFlds.Columns.Item(ACColumn7 - 1).Width = CInt(VB6.TwipsToPixelsX(850))
			
			
			'Set the Allignment of the columns to right justified
			'        lvwDefnFlds.ColumnHeaders(ACColumn3).Alignment = lvwColumnCenter
			lvwDefnFlds.Columns.Item(ACColumn4 - 1).TextAlign = HorizontalAlignment.Center
			''        lvwDefnFlds.ColumnHeaders(ACColumn3).Alignment = 1
			
			''    End If
			
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			PopulateTypeCbo()
			
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
			
			'TN20010402 Start
			cmdAdd.Enabled = Mode = gPMConstants.PMEComponentAction.PMAdd
			
			cmdModify.Enabled = False
			cmdDelete.Enabled = False
			
			'KB PN 2460 end
			optMandatory(0).Checked = True
			
			'TN20010402 End
			
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
			'lvwDefnFlds.Items.Clear()
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
			'   txtTypeName.SetFocus
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
			
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtCaption
			m_ctlTabFirstLast(ACControlEnd, 0) = txtCaption
			
			
			''    If m_sSiriusUnderWritingBroking = ACUnderWriting Then
			
			m_ctlTabFirstLast(ACControlStart, 1) = txtCaption
			m_ctlTabFirstLast(ACControlEnd, 1) = txtCaption
			
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
			

            'Developer Guide No: 243
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
			


            'Developer Guide No: 243
            lblCaption.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'Developer Guide No: 243
            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            lblMandatory.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            lblDispOrd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblDispOrd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            lblLookUp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No: 243
            lblReadOnly.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblReadOnly, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'RWH(10/04/2001) RSA#229


            'Developer Guide No:243
            lblManualEntry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACManualEntry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            'Developer Guide No:243
            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            cmdModify.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACModButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No:243
            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            'Developer Guide No:243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			



            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn1 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn2 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn3 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn4 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDispOrd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn5 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyLookup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn6 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            lvwDefnFlds.Columns.Item(ACColumn7 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReadOnly, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC110603 -ISS4415 - to not use Tab for now
            'lvwDefnFlds.ColumnHeaders(ACColumn8).Text = "Tab"

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

    'Private Function ResizeInterface() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '*************************** fine code
    '
    'tabMainTab.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 315)
    'tabMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1005)
    '
    'cmdAdd.Left = Me.Width - VB6.TwipsToPixelsX(1800)
    'cmdModify.Left = Me.Width - VB6.TwipsToPixelsX(1800)
    'cmdDelete.Left = Me.Width - VB6.TwipsToPixelsX(1800)
    '
    'cmdHelp.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 1290)
    'cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 795)
    '
    'cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 2490)
    'cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 795)
    '
    'cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 3690)
    'cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 795)
    '
    'lvwDefnFlds.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 585)
    'lvwDefnFlds.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 4005)
    '*************************** fine code
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    '
    '
    'Return gPMConstants.PMEReturnCode.PMError
    'End Try
    '
    'End Function
    ' PRIVATE Methods (End)

    Private Function ButtonClick(ByRef Index As Integer) As Integer

        Try

            Dim iMandatory, iReadOnly As Integer
            Dim lRecordCount As Integer
            Dim sMsg1, sMsg2, sMsg3, sMsg4, sMsg5, sMsg6, sMsg7, sMsgZero As String

            Dim bCheckMandatory As Boolean

            Dim sMsgHeading As String = ""
            'Developer Guide No 17
            Dim vClmPrtyTypeID As Object
            Dim vClmLookupID As Object
            Dim vDispOrd As Object
            Dim lRecordsFound As gPMConstants.PMEReturnCode
            Dim iType As Integer
            Dim lFeildsCount As Integer



            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No:243
            sMsg1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No:243
            sMsg2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No:243
            sMsg3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No:243
            sMsg7 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No:243
            sMsgHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            '**********Bugid 2 Added Message For Not Allowing Zeros in Display Order-Pandu*****



            'Developer Guide No:243
            sMsgZero = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDefFieldsZeroMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            '**********Bugid 2  End of Change 17-10-2000********************************

            'TN20010402 Start
            'RWH(10/04/2001) Control 0 is ReadOnly.
            iMandatory = optMandatory(1).Checked
            iReadOnly = optMandatory(0).Checked
            'TN20010402 End


            vClmLookupID = Nothing

            vClmPrtyTypeID = Nothing

            'DC140302
            If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Or VB6.GetItemData(cboType, cboType.SelectedIndex) = ACTabName Then

                vDispOrd = Nothing
                txtDispOrd.Text = ""
            Else
                vDispOrd = txtDispOrd.Text.Trim()
            End If

            'if in the TYPE combo Lookup is selected then pass
            'the value from the cboLookup to vClmLookupID
            If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACLookUp Then
                vClmLookupID = VB6.GetItemData(cboLookUp, cboLookUp.SelectedIndex)

                vClmPrtyTypeID = Nothing
            ElseIf VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then
                'else pass the value from the cboLookup to vClmPrtyTypeID

                vClmLookupID = Nothing
                vClmPrtyTypeID = VB6.GetItemData(cboLookUp, cboLookUp.SelectedIndex)
                If txtDispOrd.Text.Trim() = "" Then

                    vDispOrd = Nothing
                End If
            End If



            Select Case Index
                Case ACAdd

                    bCheckMandatory = CheckMandatory()

                    If Not bCheckMandatory Then

                        Exit Function

                    End If

                    'force a lost focus event
                    Application.DoEvents()

                    'Check mandatory controls have been entered into.
                    m_lReturn = m_oFormFields.CheckMandatoryControls()

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Function
                    End If

                    If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then
                        iType = 6
                    Else
                        iType = 0
                    End If

                    'Check if the Caption does not already exist
                    'TN20010501 Start
                    'm_lReturn& = g_oBusiness.ChkCaptionExists(lRecordCount, Trim$(txtCaption), TypeId, Mode, iType)

                    m_lReturn = g_oBusiness.ChkCaptionExists(lRecordCount, txtCaption.Text.Trim(), TypeId, DataMode, iType)
                    'TN20010501 End

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Function
                    End If

                    If lRecordCount > 0 Then
                        MessageBox.Show(sMsg1 & txtCaption.Text.Trim().ToUpper() & " " & sMsg2, sMsgHeading, MessageBoxButtons.OK)
                        Exit Function
                    End If



                    If txtDispOrd.Text.Trim() <> "" Then
                        'check if the display order is not greater than
                        'the maximum permissable display order
                        If Conversion.Val(txtDispOrd.Text.Trim()) <= m_vDisplayOrder Then
                            '******Bugid 2  Commented By pandu Change Start 16-10-2000 ******

                            'Check to see if the same display order is not being added again
                            '                m_lReturn& = g_oBusiness.ChkDispOrdExists(lRecordCount, Trim$(txtDispOrd), TypeId, Mode)
                            '
                            '                If (m_lReturn& <> PMTrue) Then
                            '                    Exit Function
                            '                End If
                            '******Bugid 2  Commented By pandu Change end 16-10-2000 ******
                        Else
                            MessageBox.Show(sMsg7 & CStr(m_vDisplayOrder), sMsgHeading, MessageBoxButtons.OK)
                            txtDispOrd.Focus()
                            Exit Function
                        End If

                        '******Bugid 2  Added by Pandu For Displaying Message For Zero*****

                        If Conversion.Val(txtDispOrd.Text.Trim()) = 0 Then

                            MessageBox.Show(sMsgZero, sMsgHeading, MessageBoxButtons.OK)
                            txtDispOrd.Focus()
                            Exit Function
                        End If

                        '*****Bugid 2  End of Change by Pandu********************************

                    End If


                    If lRecordCount > 0 Then
                        MessageBox.Show(sMsg3 & txtDispOrd.Text.Trim().ToUpper() & " " & sMsg2, sMsgHeading, MessageBoxButtons.OK)
                        txtDispOrd.Focus()
                        Exit Function
                    End If


                    ''        If cboType.ItemData(cboType.ListIndex) = ACLookUp Then
                    '''            vClmLookupID = cboLookUp.ItemData(cboLookUp.ListIndex)
                    ''            'Check if the Caption does not already exist
                    ''            m_lReturn& = g_oBusiness.ChkCaptionExists(lRecordCount, vClmLookupID, TypeId, Mode)
                    ''
                    ''            If (m_lReturn& <> PMTrue) Then
                    ''                Exit Function
                    ''            End If
                    ''
                    ''            If lRecordCount > 0 Then
                    ''                MsgBox sMsg1 & UCase(Trim$(txtCaption)) & " " & sMsg2, vbOKOnly, sMsgHeading
                    ''                Exit Function
                    ''            End If
                    ''        End If


                    If lRecordCount < 1 Then
                        ' Add Item
                        g_lDataDefnID = 0

                        'TN20010501 - change mode to datamode

                        'ED 07102002 - If Tab is not visible then Underwriting and 'Tab'
                        '              values not supplied for user defined data
                        If Not cboTab.Visible Then

                            m_lReturn = g_oBusiness.EditAdd(1, vDataDefnID:=g_lDataDefnID, vDescription:=txtDescription.Text.Trim(), vMandatory:=iMandatory, vCaption:=txtCaption.Text.Trim(), vRiskTypeID:=TypeId, vType:=VB6.GetItemData(cboType, cboType.SelectedIndex), vDispOrd:=vDispOrd, vReadOnly:=iReadOnly, vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vMode:=DataMode)

                        Else

                            m_lReturn = g_oBusiness.EditAdd(1, vDataDefnID:=g_lDataDefnID, vDescription:=txtDescription.Text.Trim(), vMandatory:=iMandatory, vCaption:=txtCaption.Text.Trim(), vRiskTypeID:=TypeId, vType:=VB6.GetItemData(cboType, cboType.SelectedIndex), vDispOrd:=vDispOrd, vReadOnly:=iReadOnly, vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vTabID:=VB6.GetItemData(cboTab, cboTab.SelectedIndex), vTabCaption:=cboTab.Text, vMode:=DataMode)

                        End If
                        'ED 07102002 - End
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            '                Exit Function
                        End If
                        '        Else
                        '            MsgBox sMsg1 & UCase(Trim$(txtCaption)) & " " & sMsg2, vbOKOnly, sMsgHeading
                        '                Exit Function

                    End If



                Case ACModify


                    bCheckMandatory = CheckMandatory()

                    If Not bCheckMandatory Then

                        Exit Function

                    End If
                    '--------
                    'only check if the Caption text box already exists in the database if
                    'the Orignal Caption text box value is not the same as the current value for the same record
                    If m_sCaptionTxtBx.Trim().ToUpper() <> txtCaption.Text.Trim().ToUpper() Then
                        If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then
                            iType = 6
                        Else
                            iType = 0
                        End If
                        'Check if the Field is not being used by any claim
                        'TN20010501 Start
                        'm_lReturn& = g_oBusiness.ChkCaptionExists(lRecordCount, Trim$(txtCaption), TypeId, Mode, iType)

                        m_lReturn = g_oBusiness.ChkCaptionExists(lRecordCount, txtCaption.Text.Trim(), TypeId, DataMode, iType)
                        'TN20010501 End

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Function
                        End If

                        If lRecordCount > 0 Then
                            'The Caption <  > already exists (in the database)
                            MessageBox.Show(sMsg1 & txtCaption.Text.Trim().ToUpper() & " " & sMsg2, sMsgHeading, MessageBoxButtons.OK)
                            Exit Function
                        End If
                    End If

                    '--------------------------------SK-07/11/2000--------------------------------------

                    g_lDataDefnID = Convert.ToString(lvwDefnFlds.FocusedItem.Tag)

                    'if the type is not party
                    If (m_iTypeCbo + 1) <> ACParty Then
                        '            If cboType.ItemData(cboType.ListIndex) <> ACParty Then

                        '            If cboType.ItemData(cboType.ListIndex) <> ACParty Then
                        'Check if the reserve Type is not being used by any claim
                        'TN20010501 - change mode to datamode

                        m_lReturn = g_oBusiness.ChkDataDefnIDExists(lFeildsCount, v_lDataDefnID:=g_lDataDefnID, iMode:=DataMode)
                    Else
                        'For Party
                        '                m_lReturn& = g_oBusiness.ChkDataDefnIDForPartyExists(lFeildsCount, _
                        'v_lTypeID:=TypeId, _
                        'v_lPrtyTypeID:=vClmPrtyTypeID, _
                        'iMode:=Mode)
                        'TN20010501 - change mode to datamode

                        m_lReturn = g_oBusiness.ChkDataDefnIDForPartyExists(lFeildsCount, v_lTypeID:=TypeId, v_lPrtyTypeID:=(m_iLookUpCbo + 1), iMode:=DataMode)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Function
                    End If

                    '
                    If VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACParty And VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACLookUp Then
                        m_iLookUpCbo = -1
                    End If
                    'only if the Type OR Lookup Combo values have been changed then
                    'then give message

                    If cboType.SelectedIndex <> m_iTypeCbo Or cboLookUp.SelectedIndex <> m_iLookUpCbo Then


                        'Developer Guide No:243
                        sMsg4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        'Developer Guide No:243
                        sMsg5 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        'TN20010501 Start
                        'If Mode = ACRiskMode Then
                        If DataMode = ACRiskMode Then

                            'Developer Guide No:243
                            sMsg6 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



                            'ElseIf Mode = ACPerilMode Then
                        ElseIf DataMode = ACPerilMode Then
                            'TN20010501 End


                            'Developer Guide No:243
                            sMsg6 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        End If

                        If cboType.SelectedIndex <> m_iTypeCbo Then

                            'if any child records exist then
                            '                If lFeildsCount > 1 Then
                            If lFeildsCount > 0 Then
                                'Cannot Modify <Caption>, because it is used in Risk/Peril Type
                                MessageBox.Show(sMsg4 & "Type" & ", " & sMsg5 & sMsg6, sMsgHeading, MessageBoxButtons.OK)
                                Exit Function
                            End If
                        ElseIf cboLookUp.SelectedIndex <> m_iLookUpCbo Then
                            'if any child records exist then
                            '                If lFeildsCount > 1 Then
                            If lFeildsCount > 0 Then
                                'Cannot Modify <Caption>, because it is used in Risk/Peril Type
                                MessageBox.Show(sMsg4 & "Party/Lookup" & ", " & sMsg5 & sMsg6, sMsgHeading, MessageBoxButtons.OK)
                                Exit Function
                            End If
                        End If

                    End If
                    '--------------------------------SK-07/11/2000--------------------------------------

                    If VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACParty Then

                        'only check if the Display Order text box value already exists in the database if
                        'the Orignal Display Order text box value is not the same as the current value for the same record
                        'if type combo has changed from Party to something else Then
                        If (m_iTypeCbo + 1) = ACParty And cboType.SelectedIndex <> m_iTypeCbo Then
                            '            If cboType.ListIndex <> m_iTypeCbo Then
                            If m_vDispOrdTxtBx.Trim() <> txtDispOrd.Text.Trim() Then
                                'check if the display order is not greater than
                                'the maximum permissable display order
                                If Conversion.Val(txtDispOrd.Text.Trim()) < m_vDisplayOrder + 1 Then
                                    'If Val(Trim$(txtDispOrd)) <= m_vDisplayOrder Then       'SK-06/11/2000

                                Else
                                    MessageBox.Show(sMsg7 & CStr(m_vDisplayOrder - 1), sMsgHeading, MessageBoxButtons.OK)
                                    '                        MsgBox sMsg7 & m_vDisplayOrder, vbOKOnly, sMsgHeading           'SK-06/11/2000
                                    txtDispOrd.Focus()
                                    Exit Function
                                End If

                                '******Bugid 2  Added by Pandu For Displaying Message For Zero*****

                                If Conversion.Val(txtDispOrd.Text.Trim()) = 0 Then

                                    MessageBox.Show(sMsgZero, sMsgHeading, MessageBoxButtons.OK)
                                    txtDispOrd.Focus()
                                    Exit Function
                                End If

                                '*****Bugid 2  End of Change by Pandu********************************

                            End If

                        Else

                            If m_vDispOrdTxtBx.Trim() <> txtDispOrd.Text.Trim() Then
                                'check if the display order is not greater than
                                'the maximum permissable display order
                                If Conversion.Val(txtDispOrd.Text.Trim()) < m_vDisplayOrder Then
                                    'If Val(Trim$(txtDispOrd)) <= m_vDisplayOrder Then       'SK-06/11/2000

                                Else
                                    MessageBox.Show(sMsg7 & CStr(m_vDisplayOrder - 1), sMsgHeading, MessageBoxButtons.OK)
                                    '                        MsgBox sMsg7 & m_vDisplayOrder, vbOKOnly, sMsgHeading           'SK-06/11/2000
                                    txtDispOrd.Focus()
                                    Exit Function
                                End If

                                '******Bugid 2  Added by Pandu For Displaying Message For Zero*****

                                If Conversion.Val(txtDispOrd.Text.Trim()) = 0 Then

                                    MessageBox.Show(sMsgZero, sMsgHeading, MessageBoxButtons.OK)
                                    txtDispOrd.Focus()
                                    Exit Function
                                End If

                                '*****Bugid 2  End of Change by Pandu********************************

                            End If


                        End If

                        'SK-07/11/2000-Commented
                        ''                If lRecordCount > 0 Then
                        ''                    'The Display Order <  > already exists
                        ''                    MsgBox sMsg3 & UCase(Trim$(txtDispOrd)) & " " & sMsg2, vbOKOnly, sMsgHeading
                        ''                    txtDispOrd.SetFocus
                        ''                    Exit Function
                        ''                End If
                    End If


                    If lRecordCount < 1 Then 'if zero records are returned, then Update
                        ' Add Item


                        g_lDataDefnID = Convert.ToString(lvwDefnFlds.FocusedItem.Tag)
                        'TN20010501 - change mode to datamode


                        'ED 07102002 - If Tab is not visible then Underwriting and 'Tab'
                        '              values not supplied for userdefined data
                        If Not cboTab.Visible Then

                            m_lReturn = g_oBusiness.EditUpdate(1, vDataDefnID:=g_lDataDefnID, vDescription:=txtDescription.Text.Trim(), vMandatory:=iMandatory, vCaption:=txtCaption.Text.Trim(), vRiskTypeID:=TypeId, vType:=VB6.GetItemData(cboType, cboType.SelectedIndex), vDispOrd:=vDispOrd, vReadOnly:=iReadOnly, vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vMode:=DataMode)

                        Else

                            m_lReturn = g_oBusiness.EditUpdate(1, vDataDefnID:=g_lDataDefnID, vDescription:=txtDescription.Text.Trim(), vMandatory:=iMandatory, vCaption:=txtCaption.Text.Trim(), vRiskTypeID:=TypeId, vType:=VB6.GetItemData(cboType, cboType.SelectedIndex), vDispOrd:=vDispOrd, vReadOnly:=iReadOnly, vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vTabID:=VB6.GetItemData(cboTab, cboTab.SelectedIndex), vTabCaption:=cboTab.Text, vMode:=DataMode)


                        End If
                        'ED 07102002 - End

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Function
                        End If
                        '        Else
                        '            MsgBox sMsg1 & UCase(Trim$(txtCaption)) & " " & sMsg2, vbOKOnly, sMsgHeading
                        '                Exit Function

                    End If


                Case ACDelete


                    g_lDataDefnID = Convert.ToString(lvwDefnFlds.FocusedItem.Tag)

                    If VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACParty Then
                        'Check if the reserve Type is not being used by any claim
                        'TN20010501 - change mode to datamode

                        m_lReturn = g_oBusiness.ChkDataDefnIDExists(lRecordCount, v_lDataDefnID:=g_lDataDefnID, iMode:=DataMode)
                    Else
                        'TN20010501 - change mode to datamode

                        m_lReturn = g_oBusiness.ChkDataDefnIDForPartyExists(lRecordCount, v_lTypeID:=TypeId, v_lPrtyTypeID:=vClmPrtyTypeID, iMode:=DataMode)

                    End If


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Function
                    End If


                    'Developer Guide No:243
                    sMsg4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    'Developer Guide No:243
                    sMsg5 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACResvTypeNameMsg5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    'TN20010501 Start
                    'If Mode = ACRiskMode Then
                    If DataMode = ACRiskMode Then

                        'Developer Guide No:243
                        sMsg6 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        'ElseIf Mode = ACPerilMode Then
                    ElseIf DataMode = ACPerilMode Then
                        'TN20010501 End


                        'Developer Guide No:243
                        sMsg6 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    End If


                    If lRecordCount < 1 Then
                        '            g_lDataDefnID = lvwDefnFlds.SelectedItem.Tag
                        'TN20010501 - change mode to datamode

                        m_lReturn = g_oBusiness.EditDelete(1, vDataDefnID:=g_lDataDefnID, vMode:=DataMode)
                    Else
                        'Cannot Delete <Caption>, because it is used in Risk/Peril Type
                        MessageBox.Show(sMsg4 & m_sCaptionTxtBx.Trim().ToUpper() & ", " & sMsg5 & sMsg6, sMsgHeading, MessageBoxButtons.OK)
                        Exit Function
                    End If

            End Select


            m_lReturn = g_oBusiness.Update


            g_oBusiness.ClearColl()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'CMG/PB This was an invisible error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdButton click failed, failed to update business", vApp:=ACApp, vClass:=ACClass, vMethod:="ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                'End CMG
                Exit Function
            End If

            'Pandu Commented this Bugid 2

            'm_lReturn = UpdListview(Index)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If


            If lvwDefnFlds.Items.Count < 1 Then
                cmdDelete.Enabled = False
            End If

            txtCaption.Text = ""
            txtDescription.Text = ""

            'txtDispOrd.Text = Val(txtDispOrd.Text) + 1

            'Only if the type is PARTY, then populate the txtCaption with
            'the selected value from the cboLookup
            If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then
                txtCaption.Text = VB6.GetItemString(cboLookUp, cboLookUp.SelectedIndex)
            End If

            'DC140302
            If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACTabName Then
                txtCaption.Text = "Screen Title"
            End If

            'We check because when 'TYPE=PARTY' then txtCaption is disabled
            '& we will not be able to SetFocus to it
            If txtCaption.Enabled Then
                txtCaption.Focus()
            End If
            'm_lModify = False
            m_lModify = 0

            '************************Start-Get the Maximum Display Order**************************
            'TN20010501 Start
            'If Mode = ACRiskMode Then
            If DataMode = ACRiskMode Then

                m_lReturn = g_oBusiness.GetRiskDataDefn(r_vResultArray:=m_vTotalArray, v_lRiskTypeId:=TypeId, r_lRecordsFound:=lRecordsFound)

                'ElseIf Mode = ACPerilMode Then
            ElseIf DataMode = ACPerilMode Then
                'TN20010501 End


                m_lReturn = g_oBusiness.GetPerilDataDefn(r_vResultArray:=m_vTotalArray, v_lPerilTypeId:=TypeId, r_lRecordsFound:=lRecordsFound)

            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Failed to get details.
                '        GetBusiness = PMFalse
                txtDispOrd.Text = CStr(1)
                'Exit Function 'Commented this line pandu
            End If


            Const ACColDispOrd As Integer = 4
            'DC140302
            If VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACParty And VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACTabName Then
                'Set the default value of the display order text box to ONE more
                'than the maximum existing display order.
                'We get this value from the last row of the result array as
                'the Select query has ORDER BY DISPLAY_ORDER clause
                If Information.IsArray(m_vTotalArray) Then
                    txtDispOrd.Text = CStr(Conversion.Val(CStr(m_vTotalArray(ACColDispOrd, m_vTotalArray.GetUpperBound(1)))) + 1)
                    m_vDisplayOrder = Conversion.Val(CStr(m_vTotalArray(ACColDispOrd, m_vTotalArray.GetUpperBound(1)))) + 1
                Else
                    m_vDisplayOrder -= 1
                End If
            End If

            m_bCaptionChanged = False


            '************************End-Get the Maximum Display Order**************************

            'Added by Pandu 17-10-2000 refresh the List view Bugid 2 **********

            lvwDefnFlds.Items.Clear()

            m_lReturn = DataToInterfaceSumm()

            'Me.Refresh

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '****************End of Changes Bugid 2 ****************************

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdButton click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Exit Function

        End Try



    End Function

    'Private Function UpdListview(ByVal Button As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim sVal, sReadOnly As String
    'Dim nCnt As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'TN20010402 Start
    'RWH(10/04/2001) Control 0 is ReadOnly.
    'If Not optMandatory(1).Checked Then
    'sVal = "No"
    'Else
    'sVal = "Yes"
    'End If
    '
    'If Not optMandatory(0).Checked Then
    'sReadOnly = "No"
    'Else
    'sReadOnly = "Yes"
    'End If
    'TN20010402 End
    '
    '
    'Select Case Button
    'Case ACAdd
    '
    ' Assign the details to the first column.
    ' Column 1 Reserve Type
    'oListItem = lvwDefnFlds.Items.Add(txtCaption.Text.Trim())
    '
    ' Assign details to other the columns
    ' Column 2 Description
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDescription.Text.Trim()
    '
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = VB6.GetItemString(cboType, cboType.SelectedIndex)
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDispOrd.Text
    '
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 4).Text = VB6.GetItemString(cboLookUp, cboLookUp.SelectedIndex)
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 5).Text = sVal
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sReadOnly
    '
    ' Set the tag property with Reserve Type ID

    'oListItem.Tag = g_oBusiness.DataDefnID
    '
    ' Refresh the initial results.
    'lvwDefnFlds.Refresh()
    '
    '
    'Case ACModify
    '
    'lvwDefnFlds.Items.RemoveAt(lvwDefnFlds.FocusedItem.Index)
    ' Assign the details to the first column.
    ' Column 1 Reserve Type
    'oListItem = lvwDefnFlds.Items.Add(txtCaption.Text.Trim())
    '
    ' Assign details to other the columns
    ' Column 2 Description
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDescription.Text.Trim()
    '
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = VB6.GetItemString(cboType, cboType.SelectedIndex)
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDispOrd.Text
    '
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 4).Text = VB6.GetItemString(cboLookUp, cboLookUp.SelectedIndex)
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 5).Text = sVal
    ' Column 3 Include In Total
    'ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sReadOnly
    ' Set the tag property with Reserve Type ID

    'oListItem.Tag = g_oBusiness.DataDefnID
    '
    ' Refresh the initial results.
    'lvwDefnFlds.Refresh()
    '
    'Case ACDelete
    '
    'lvwDefnFlds.Items.RemoveAt(lvwDefnFlds.FocusedItem.Index)
    '
    ' Refresh the initial results.
    'lvwDefnFlds.Refresh()
    '
    '
    'End Select
    '
    'Return result
    'End Function

    Private Sub cboLookUp_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLookUp.SelectedIndexChanged

        'Only if the type is PARTY, then populate the txtCaption with
        'the selected value from the cboLookup
        If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then
            txtCaption.Text = VB6.GetItemString(cboLookUp, cboLookUp.SelectedIndex)
            txtDispOrd.Text = ""
        End If

    End Sub

    Private Sub cboType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboType.SelectedIndexChanged
        Dim vResultArray As Object

        cboLookUp.Items.Clear()

        '    On Error GoTo Err_cboType_Click
        'if LOOKUP is selected in the TYPE combo
        If VB6.GetItemData(cboType, cboType.SelectedIndex) = ACLookUp Then

            'changed here pandu 24-10-2000

            txtDispOrd.Enabled = True
            'DC140302
            txtDispOrd.BackColor = SystemColors.Window 'white

            'Get the Lookup val;ues from the database

            m_lReturn = g_oBusiness.SelLookupTables(vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '            cboType_Click = PMFalse
                Exit Sub
            End If

            'Populate the Risk Type combo using the above returned records

            m_lReturn = PopulateLookupCbo(vResultArray)

            '    'Used to revert back to the previous combo value
            '    m_iRskTypeCboIndex = cboRskType.ListIndex

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        SetInterfaceDefaults = PMFalse
                Exit Sub
            End If

            cboLookUp.Enabled = True
            cboLookUp.BackColor = SystemColors.Window
            'change the label captions dynamically

            'Developer Guide No.: 243
            lblLookUp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblLookup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Enable the caption txtbox &
            'Leave it blank, populate it in a later function with the
            'the values from the list view
            txtCaption.Enabled = True

            '        txtCaption.Text = ""

            txtCaption.BackColor = SystemColors.Window 'white

            If txtDispOrd.Text = "" Then
                txtDispOrd.Text = CStr(m_vDisplayOrder)
            End If



        ElseIf VB6.GetItemData(cboType, cboType.SelectedIndex) = ACParty Then

            'Changed here Pandu 24-10-2000

            txtDispOrd.Enabled = False
            'DC140302
            txtDispOrd.BackColor = SystemColors.Window 'white

            'if PARTY is selected in the TYPE combo

            m_lReturn = g_oBusiness.SelPartyTypes(vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '            cboType_Click = PMFalse
                Exit Sub
            End If

            'Populate the Risk Type combo using the above returned records

            m_lReturn = PopulateLookupCbo(vResultArray)

            '    'Used to revert back to the previous combo value
            '    m_iRskTypeCboIndex = cboRskType.ListIndex

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        SetInterfaceDefaults = PMFalse
                Exit Sub
            End If

            'Disable the caption txtbox &
            'Populate it with the value in the cboLookup
            txtCaption.Enabled = False
            txtCaption.BackColor = SystemColors.Control 'grey
            txtCaption.Text = VB6.GetItemString(cboLookUp, cboLookUp.SelectedIndex)
            txtDispOrd.Text = ""

            cboLookUp.Enabled = True
            cboLookUp.BackColor = SystemColors.Window 'white

            'change the label captions dynamically

            lblLookUp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC140302
        ElseIf VB6.GetItemData(cboType, cboType.SelectedIndex) = ACTabName Then

            txtDispOrd.Enabled = False

            'Disable the caption txtbox &
            'Populate it with the value in the cboLookup
            txtCaption.Enabled = False
            txtCaption.BackColor = SystemColors.Control 'grey
            txtCaption.Text = "Screen Title"
            txtDispOrd.Text = ""
            txtDispOrd.BackColor = SystemColors.Control 'grey
            optMandatory(1).Enabled = False
            optMandatory(2).Enabled = False
            optMandatory(0).Enabled = False
            cboLookUp.Enabled = False
            cboLookUp.BackColor = SystemColors.Control 'grey
        Else
            'DC24/03/2004 PN11187 -show all as enabled
            optMandatory(1).Enabled = True
            optMandatory(2).Enabled = True
            optMandatory(0).Enabled = True
            'DC140302
            txtDispOrd.BackColor = SystemColors.Window 'white

            'DC150801 do not allow mandatory option if yes/no selected
            optMandatory(1).Enabled = Not (VB6.GetItemData(cboType, cboType.SelectedIndex) = ACYesNo)

            'changed here pandu 24-10-2000

            txtDispOrd.Enabled = True

            'If anything apart from PARTY/LOOKUP IS SELECTED
            cboLookUp.Enabled = False
            cboLookUp.BackColor = SystemColors.Control 'grey

            'Enable the caption txtbox &
            'Leave it blank, populate it in a later function with the
            'the values from the list view
            txtCaption.Enabled = True

            '        txtCaption.Text = ""

            txtCaption.BackColor = SystemColors.Window 'white

            If txtDispOrd.Text = "" Then
                txtDispOrd.Text = CStr(m_vDisplayOrder)
            End If


        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Try

            m_lReturn = ButtonClick(ACAdd)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAdd click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try

            m_lReturn = ButtonClick(ACDelete)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDelete click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdModify_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdModify.Click

        Try


            m_lReturn = ButtonClick(ACModify)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdModify click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdModify_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
    Private Function CheckMandatory() As Boolean

        Dim result As Boolean = False
        Try


            If txtCaption.Text.Trim() = "" Then ' Requirement text box
                DisplayMessage(ACMandatoryFieldMsg, Mid(txtCaption.Name, 4))
                result = False
                txtCaption.Focus()
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtDescription.Text.Trim() = "" Then ' Requirement text box
                DisplayMessage(ACMandatoryFieldMsg, Mid(txtDescription.Name, 4))
                result = False
                txtDescription.Focus()
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            'DC140302
            If VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACParty And VB6.GetItemData(cboType, cboType.SelectedIndex) <> ACTabName Then
                If txtDispOrd.Text.Trim() = "" Then ' Requirement text box
                    DisplayMessage(ACMandatoryFieldMsg, Mid(txtDispOrd.Name, 4))
                    result = False
                    txtDispOrd.Focus()
                    Return result
                Else
                    '   If all the Mandatory fields are having values SET the CheckMandatory = True
                    result = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for Mandatory Fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            m_oGeneral = New iCLMDefnFlds.General()

            ' Create an instance of the interface object.
            '    Set m_oInterface = New iCLMDefnFlds.Interface


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


            'Set the UnderWriting/Broking Constant
            'm_sSiriusUnderWritingBroking = ACBroking

            'm_sSiriusUnderWritingBroking = ACUnderWriting



            ''    m_sSiriusUnderWritingBroking = g_oBackofficelink.Sirius_Product

            'Zero because the form will load in the ADD mode
            m_lModify = 0

            'Validate fields using Forms Control,
            'using FormFields object
            m_lReturn = SetFieldValidation()

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

            'Variable set to true when the form has just loaded
            m_bFormLoad = True



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

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwDefnFlds.Handle.ToInt32(), v_vShowRowSelect:=1)
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

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
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

            ' Terminate the business object

		g_oBusiness.Dispose()

           

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
		m_oFormFields.Dispose()

         

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

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


        'Try 
        '
        'm_lReturn& = ResizeInterface()
        '    Me.Refresh
        '
        'Catch 
        '
        '
        'Exit Sub
        'End Try

    End Sub


    ' ***************************************************************** '
    ' Name          :lvwDefnFlds_Click
    ' Description   :Fill the Claim Reference,Policy No.,Client Short Name
    '                   in Text Box for the listitem clicked
    ' Date          :11/07/00
    ' Edit History  :SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDefnFlds.Click
        Dim sindex As Integer


        If lvwDefnFlds.Items.Count > 0 Then
            '        m_bListViewClick = True
            '        m_lModify = True
            m_lModify = 1


            sindex = Convert.ToString(lvwDefnFlds.FocusedItem.Tag)

            'used to hold the orignal NAME txtbox value
            m_sCaptionTxtBx = lvwDefnFlds.FocusedItem.Text

            txtCaption.Text = lvwDefnFlds.FocusedItem.Text
            'Developer Guide No. :52
            txtDescription.Text = lvwDefnFlds.FocusedItem.SubItems(1).Text

            'Developer Guide No. :52
            m_sDescTxtBx = lvwDefnFlds.FocusedItem.SubItems(1).Text

            For i As Integer = 0 To cboType.Items.Count - 1
                'Developer Guide No. :52
                If lvwDefnFlds.FocusedItem.SubItems(2).Text.Trim() = VB6.GetItemString(cboType, i) Then
                    cboType.SelectedIndex = i
                    m_iTypeCbo = i
                    Exit For
                End If
            Next i

            'used to hold the orignal DISPLAY txtbox value

            'Developer Guide No. :52
            m_vDispOrdTxtBx = lvwDefnFlds.FocusedItem.SubItems(3).Text
            txtDispOrd.Text = lvwDefnFlds.FocusedItem.SubItems(3).Text


            If cboLookUp.Items.Count <> 0 Then
                For j As Integer = 0 To cboLookUp.Items.Count - 1
                    'Developer Guide No. :52
                    If lvwDefnFlds.FocusedItem.SubItems(4).Text.Trim() = VB6.GetItemString(cboLookUp, j) Then
                        cboLookUp.SelectedIndex = j
                        m_iLookUpCbo = j
                        Exit For
                    End If
                Next j
            End If

            'TN20010402 Start
            'RWH(10/04/2001) ReadOnly is SubItem 6.

            'Developer Guide No. :52
            If lvwDefnFlds.FocusedItem.SubItems(6).Text = "No" Then
                optMandatory(0).Checked = False
                m_bMandatory = False
            Else
                optMandatory(0).Checked = True
                m_bMandatory = True
            End If

            'RWH(10/04/2001) Mandatory is SubItem 5.

            'Developer Guide No. :52
            If lvwDefnFlds.FocusedItem.SubItems(5).Text = "No" Then
                optMandatory(1).Checked = False
                m_bReadOnly = False
            Else
                optMandatory(1).Checked = True
                m_bReadOnly = True
            End If

            'RWH(10/04/2001) New option so neither Mandatory nor
            'ReadOnly need be selected enabling free manual entry.
            If Not (m_bMandatory Or m_bReadOnly) Then
                optMandatory(2).Checked = True
            End If

            '                If lvwDefnFlds.SelectedItem.SubItems(5) = "No" Then
            '                    optMandatory(0).Value = False
            '                    chkMandatory.Value = 0
            '                    m_bMandatory = False
            '                ElseIf lvwDefnFlds.SelectedItem.SubItems(5) = "Yes" Then
            '                    chkMandatory.Value = 1
            '                    m_bMandatory = True
            '                End If
            '
            '                If lvwDefnFlds.SelectedItem.SubItems(6) = "No" Then
            '                    chkReadOnly.Value = 0
            '                    m_bReadOnly = False
            '                ElseIf lvwDefnFlds.SelectedItem.SubItems(6) = "Yes" Then
            '                    chkReadOnly.Value = 1
            '                    m_bReadOnly = True
            '                End If
            'TN20010402 End

            '------------------------------------------------------------------------
            '                m_lModify = True
            m_lModify = 2
            '------------------------------------------------------------------------
            'Developer Guide No. :52
            ComboFindText(cboTab, lvwDefnFlds.FocusedItem.SubItems(7).Text)

            VB6.SetDefault(cmdOK, True)

        End If

    End Sub

    ' ***************************************************************** '
    ' Name: lvwDefnFlds_ColumnClick
    '
    ' Description:Sort the Details of List View as per the column clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwDefnFlds.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwDefnFlds.Columns(eventArgs.Column)
        Try

            With lvwDefnFlds

                ' If date column clicked, then sort by date sort column
                If ColumnHeader.Index + 1 - 1 = 4 Then
                    ListViewHelper.SetSortedProperty(lvwDefnFlds, False)
                    If ListViewHelper.GetSortKeyProperty(lvwDefnFlds) <> 5 Then
                        ListViewHelper.SetSortKeyProperty(lvwDefnFlds, 5)
                        ListViewHelper.SetSortOrderProperty(lvwDefnFlds, SortOrder.Ascending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwDefnFlds, (ListViewHelper.GetSortOrderProperty(lvwDefnFlds) + 1) Mod 2)
                    End If
                    ListViewHelper.SetSortedProperty(lvwDefnFlds, True)

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwDefnFlds)) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwDefnFlds, (ListViewHelper.GetSortOrderProperty(lvwDefnFlds) + 1) Mod 2)
                    ListViewHelper.SetSortedProperty(lvwDefnFlds, True)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwDefnFlds, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwDefnFlds, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwDefnFlds, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwDefnFlds, True)
                End If
            End With

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDefnFlds_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: lvwDefnFlds_DblClick
    '
    ' Description:Move to the next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDefnFlds.DoubleClick
        ''    ' Double click event for the search details.
        ''
        ''    On Error GoTo Err_lvwDefnFldsDblClick
        ''
        ''    ' Check if there are any items available.
        ''    If (lvwDefnFlds.ListItems.Count = 0) Then
        ''        Exit Sub
        ''    End If
        ''
        ''    ' Set the interface status.
        ''    m_lStatus& = PMOK
        ''
        ''    ' Process the next set of actions.
        ''    m_lReturn& = m_oGeneral.ProcessCommand()
        ''
        ''    ' Check the return value.
        ''    If (m_lReturn& = PMTrue) Then
        ''        ' Everything OK, so we can hide the interface.
        ''        Me.Hide
        ''    End If
        ''
        ''Exit Sub
        ''
        ''
        ''Err_lvwDefnFldsDblClick:
        ''
        ''    ' Log Error.
        ''    LogMessage _
        '''        iType:=PMLogOnError, _
        '''        sMsg:="Failed to process the double click event", _
        '''        vApp:=ACApp, _
        '''        vClass:=ACClass, _
        '''        vMethod:="lvwDefnFlds_DblClick", _
        '''        vErrNo:=Err.Number, _
        '''        vErrDesc:=Err.Description
        ''
        ''Exit Sub

    End Sub

    ' ***************************************************************** '
    ' Name: lvwDefnFlds_GotFocus
    '
    ' Description:Set Ok Button a default
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDefnFlds.Enter
        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can select with Enter key.
            '    cmdFindNow.Default = False
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDefnFlds_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try


    End Sub

    ' ***************************************************************** '
    ' Name:lvwDefnFlds_KeyDown
    '
    ' Description:Set Command Button Ok as Not Default on Pressing Enter Key
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwDefnFlds.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        '' Dim I As Integer
        '' Dim J As Integer
        '' Dim sindex As Long
        ''
        ''    If (KeyCode <> 13) Then
        ''        cmdOK.Default = False
        ''    End If
        ''
        ''
        '''on pressing down arrow or up arrow key
        ''If (KeyCode = 38) Or (KeyCode = 40) Then
        ''
        ''
        ''    'check if there are any items in the listview
        ''    If (lvwDefnFlds.ListItems.Count > 0) Then
        '''        m_lModify = True
        ''        m_lModify = 1
        '''        m_bListViewClick = True
        ''
        ''        'Down Arrow click
        ''        If (KeyCode = 40) Then
        ''            'only move down if the current item is not the last item
        ''            If lvwDefnFlds.SelectedItem.Index < lvwDefnFlds.ListItems.Count Then
        ''
        ''                'used to hold the orignal NAME txtbox value
        ''                m_sCaptionTxtBx = lvwDefnFlds.SelectedItem.Text
        ''
        ''                'move to the next item
        ''                txtCaption.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).Text
        ''                txtDescription.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(1)
        ''
        ''                For I = 0 To cboType.ListCount - 1
        ''                    If Trim$(lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(2)) = cboType.List(I) Then
        ''                        cboType.ListIndex = I
        ''                        Exit For
        ''                    End If
        ''                Next I
        ''
        ''                If cboLookUp.ListCount <> 0 Then
        ''                    For J = 0 To cboLookUp.ListCount - 1
        ''
        ''                        If Trim$(lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(4)) = cboLookUp.List(J) Then
        ''                            cboLookUp.ListIndex = J
        ''                            Exit For
        ''                        End If
        ''                    Next J
        ''                End If
        ''
        ''                m_vDispOrdTxtBx = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(3)
        ''                txtDispOrd.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(3)
        ''
        ''                If lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(5) = "No" Then
        ''                    chkMandatory.Value = 0
        ''                ElseIf lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(5) = "Yes" Then
        ''                    chkMandatory.Value = 1
        ''                End If
        ''
        ''                If lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(6) = "No" Then
        ''                    chkReadOnly.Value = 0
        ''                ElseIf lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index + 1).SubItems(6) = "Yes" Then
        ''                    chkReadOnly.Value = 1
        ''                End If
        ''            End If
        ''
        ''        'Up arrow click
        ''        ElseIf (KeyCode = 38) Then
        ''            'only move up if the current item is not the first item
        ''            If lvwDefnFlds.SelectedItem.Index > 1 Then
        ''                'move to the previous item
        ''
        ''                'used to hold the orignal NAME txtbox value
        ''                m_sCaptionTxtBx = lvwDefnFlds.SelectedItem.Text
        ''
        ''                txtCaption.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).Text
        ''                txtDescription.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(1)
        ''                txtDispOrd.Text = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(3)
        ''                m_vDispOrdTxtBx = lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(3)
        ''
        ''
        ''                 For I = 0 To cboType.ListCount - 1
        ''                    If Trim$(lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(2)) = cboType.List(I) Then
        ''                        cboType.ListIndex = I
        ''                        Exit For
        ''                    End If
        ''                Next I
        ''
        ''                If cboLookUp.ListCount <> 0 Then
        ''                    For J = 0 To cboLookUp.ListCount - 1
        ''
        ''                        If Trim$(lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(4)) = cboLookUp.List(J) Then
        ''                            cboLookUp.ListIndex = J
        ''                            Exit For
        ''                        End If
        ''                    Next J
        ''                End If
        ''
        ''
        ''
        ''                If lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(5) = "No" Then
        ''                    chkMandatory.Value = 0
        ''                ElseIf lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(5) = "Yes" Then
        ''                    chkMandatory.Value = 1
        ''                End If
        ''
        ''                If lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(6) = "No" Then
        ''                    chkReadOnly.Value = 0
        ''                ElseIf lvwDefnFlds.ListItems(lvwDefnFlds.SelectedItem.Index - 1).SubItems(6) = "Yes" Then
        ''                    chkReadOnly.Value = 1
        ''                End If
        ''            End If
        ''
        ''        End If
        ''
        '''------------------------------------------------------------------------
        ''        cmdDelete.Enabled = True
        ''        cmdModify.Enabled = False
        ''        cmdAdd.Enabled = False
        '''        m_lModify = True
        ''        m_lModify = 1
        '' '------------------------------------------------------------------------
        ''
        ''        cmdOK.Default = True
        ''
        ''    End If
        ''End If
    End Sub

    ' ***************************************************************** '
    ' Name:lvwDefnFlds_KeyPress
    '
    ' Description:Fill the Policy Number in Text Box when enter button is
    '               pressed when focus is  on list item
    '
    ' Date:11/07/00
    '
    ' Edit History:SK
    ' ***************************************************************** '

    Private Sub lvwDefnFlds_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwDefnFlds.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)


        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name:         lvwDefnFlds_LostFocus
    ' Description:  Set find now as default
    ' Date:         11/07/00
    ' Edit History: SK
    ' ***************************************************************** '
    Private Sub lvwDefnFlds_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDefnFlds.Leave
        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDefnFlds_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub lvwDefnFlds_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwDefnFlds.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Mode <> gPMConstants.PMEComponentAction.PMView Then
            If Me.lvwDefnFlds.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdModify.Enabled = False
            Else
                cmdAdd.Enabled = True 'CMG/PB bug fix 1865 dont disable add, no need
                cmdModify.Enabled = True
                cmdDelete.Enabled = True
            End If
        End If
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

            '     'force a lost focus event
            '    DoEvents
            '
            '     'Check mandatory controls have been entered into.
            '    m_lReturn = m_oFormFields.CheckMandatoryControls
            '
            '    ' Check for errors
            '    If m_lReturn <> PMTrue Then
            '      Exit Sub
            '    End If

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.

                'DC030701 -start -generate merge codes for user defined screens
                'no time to mess.
                'for now generates merge codes for both risk and peril.
                'will revisit when got more time.

                'TN20010820 - start
                If DataMode = 0 Then

                    m_lReturn = g_oBusiness.ApplyMergeCodes(0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                Else

                    m_lReturn = g_oBusiness.ApplyMergeCodes(1)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If
                'TN20010820 - end

                'DC030701 -end

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

    ' ***************************************************************** '
    ' Name: DisplayMessage
    '
    ' Description: Display the Suitable Message
    '
    ' Date:11/07/00
    '
    ' Edit History:SK

    ' ***************************************************************** '
    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try


            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtCaption_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCaption.Enter
        iPMFunc.SelectText(txtCaption)
    End Sub

    Private Sub txtCaption_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCaption.KeyPress
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

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            ' Pass control and required settings to FormControl
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCaption, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Error checking
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Error checking
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboType, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Error checking
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                   ctlControl:=txtDispOrd, _
            ''                   lFieldType:=PMLong, _
            ''                   lMandatory:=PMMandatory)
            '
            '    'Error checking
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If

            'commented this code cos cboLookup can be empty
            'if TYPE selected is not Party/Lookup
            '     m_lReturn = m_oFormFields.AddNewFormField( _
            'ctlControl:=frmInterface.cboLookUp, _
            'lFieldType:=PMCombo, _
            'lMandatory:=PMMandatory)

            'Error checking
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If


            'TN20010402 Start
            '     m_lReturn = m_oFormFields.AddNewFormField( _
            ''                   ctlControl:=frmInterface.chkMandatory, _
            ''                   lFieldType:=PMCheckBox, _
            ''                   lMandatory:=PMMandatory)
            '
            '    'Error checking
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '
            '
            '     m_lReturn = m_oFormFields.AddNewFormField( _
            ''                   ctlControl:=frmInterface.chkReadOnly, _
            ''                   lFieldType:=PMCheckBox, _
            ''                   lMandatory:=PMMandatory)
            '
            '    'Error checking
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            'TN20010402 End


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name:     PopulateTypeCbo (Private)
    '
    ' Description:  Depending upon the Primary Cause Selected Secondary
    '               causes are filled into the combo
    '
    ' ***************************************************************** '
    Private Function PopulateTypeCbo() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboType.Items.Clear()
            'get the upper bound of the no. of rows
            Dim cboType_NewIndex As Integer = -1


            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))

            VB6.SetItemData(cboType, cboType_NewIndex, 1)



            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboInteger, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))

            VB6.SetItemData(cboType, cboType_NewIndex, 2)



            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))


            VB6.SetItemData(cboType, cboType_NewIndex, 3)



            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboYesNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))


            VB6.SetItemData(cboType, cboType_NewIndex, 4)



            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboLookUp, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))


            VB6.SetItemData(cboType, cboType_NewIndex, 5)

            If Not (m_lDataMode = gPMConstants.PMEComponentAction.PMAdd) Then


                'Developer Guide No:243
                cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))


                VB6.SetItemData(cboType, cboType_NewIndex, 6)
            End If

            'DC140302


            'Developer Guide No:243
            cboType_NewIndex = cboType.Items.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACcboTabName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
            VB6.SetItemData(cboType, cboType_NewIndex, 7)


            'Default select the first item of the combo
            cboType.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopulateTypeCbo", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTypeCbo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name:     PopulateLookupCbo (Private)
	'
	' Description:  Depending upon the Type Selected Lookup combo is filled
	'
	' ***************************************************************** '
	Private Function PopulateLookupCbo(ByRef vResultArray( ,  ) As Object) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cboLookUp.Items.Clear()
			'get the upper bound of the no. of rows
			For ncount As Integer = 0 To vResultArray.GetUpperBound(1)
				
				''        If vResultArray(1, ncount) = cboLookUp.ItemData(cboLookUp.ListIndex) Then
				
				Dim cboLookUp_NewIndex As Integer = -1

				cboLookUp_NewIndex = cboLookUp.Items.Add(CStr(vResultArray(1, ncount)))

				VB6.SetItemData(cboLookUp, cboLookUp_NewIndex, CInt(vResultArray(0, ncount)))
				
				''        End If
			Next 
			
			'Default select the first item of the combo
			cboLookUp.SelectedIndex = 0
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopulateLookupCbo", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLookupCbo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
		iPMFunc.SelectText(txtDescription)
		
	End Sub
	
	Private Sub txtDispOrd_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDispOrd.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		
		If m_bFormLoad Then
			m_bFormLoad = False
		End If
		
		'm_lModify = 2
		
		
	End Sub
	
	Private Sub txtDispOrd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDispOrd.Enter
		iPMFunc.SelectText(txtDispOrd)
	End Sub
	
	Private Sub txtDispOrd_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDispOrd.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		Dim bOK As Boolean
		
		'1-10
		If KeyAscii > 47 And KeyAscii < 58 Then
			bOK = True
		End If
		
		''A-Z
		'If KeyAscii > 64 And KeyAscii < 91 Then
		'     bOK = True
		'End If
		'
		''a-z
		'If KeyAscii > 96 And KeyAscii < 123 Then
		'        bOK = True
		'End If
		
		'Backspace or Space or Ctrl+C or Ctrl+V
		If KeyAscii = 8 Or KeyAscii = 32 Then
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
	
	Private Sub txtDispOrd_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDispOrd.Leave
		'm_lReturn = m_oFormFields.LostFocus(txtDispOrd)
	End Sub
	
	Function LoadTabCombo() As Integer
		
		Dim result As Integer = 0
		Dim r As gPMConstants.PMEReturnCode
        Dim vResultsArray(,) As Object
		Dim lRecordCount As Integer
		result = True
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			r = g_oBusiness.ListClaimTabs(vResultsArray, lRecordCount)
			
			'ISS1745 Logica-CMG(SJP) 17/02/2003
			'start
			' Check for errors
			Select Case r
				Case gPMConstants.PMEReturnCode.PMTrue
					' Continue
				Case gPMConstants.PMEReturnCode.PMNotFound
					' Failed to get details.
					Return result
				Case Else
					' Failed to get details.
					Throw New Exception()
			End Select
			'end
			
			'DC110603 -ISS4415 not used for now
			'    cboTab.Clear
			'    For lRow = LBound(vResultsArray, 2) To UBound(vResultsArray, 2)
			'        cboTab.AddItem vResultsArray(ACColTabName, lRow)
			'        cboTab.ItemData(cboTab.ListCount - 1) = Val(vResultsArray(ACColTabID, lRow))
			'    Next
			
			'sj 25/09/2002 - start
			'   cboTab.ListIndex = 0
			'sj 25/09/2002 - end
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed load tab combo", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadTabCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Class