Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 1st September 1997
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
    'Developer Guide No.19
    Private Const vbFormCode As Integer = 0
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sNavigatorTitle As String = ""
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lCashListID As Integer
	Private m_lCashListTypeID As Integer
	'eck140600
	Private m_iCashListCompanyID As Integer
	

	Private m_oCashList As iACTCashList.Interface_Renamed

	Private m_oCashListItem As iACTCashListItem.Interface_Renamed
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTFindCashList.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the business object.
	Public m_vSearchData( ,  ) As Object
	
    Private m_vSourceArray As Object
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	Public ReadOnly Property CashlistID() As Integer
		Get
			' Return the Cash List ID
			Return m_lCashListID
		End Get
	End Property
	
	Public ReadOnly Property CashListTypeID() As Integer
		Get
			Return m_lCashListTypeID
		End Get
	End Property
	'eck140600
	Public ReadOnly Property CashListCompanyID() As Integer
		Get
			Return m_iCashListCompanyID
		End Get
	End Property
	'eck040500
	Public WriteOnly Property SourceArray() As Object()
		Set(ByVal Value() As Object)
			
			' Set the valid sources for the user
			m_vSourceArray = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
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
		Dim iCashListStatusID, iCashListTypeID As Integer
		Dim sCashlistRef As String = ""
		Dim iCompanyID, iBankAccountID, iCurrencyID As Integer
        Dim dtStartDate, dtEndDate As Object
		Dim cControlTotal As Decimal
		Dim lItemCount As Integer
		
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
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (Begin) *}
			iCompanyID = g_iCompanyID
			
			' Type: If any list item other than (Any) selected
			If uctType.ItemId > 0 Then
				iCashListTypeID = uctType.ItemId
			Else
				' Must be (Any) so send -1
				iCashListTypeID = -1
			End If
			
			' Status: If any list item other than (Any) selected
			If uctStatus.ItemId > 0 Then
				iCashListStatusID = uctStatus.ItemId
			Else
				' Must be (Any) so send -1
				iCashListStatusID = -1
			End If
			
			' Reference
			sCashlistRef = txtReference.Text.Trim()
			
			' Bank: If any list item other than (Any) selected
			If uctBankAccount.Id > 0 Then
				iBankAccountID = uctBankAccount.Id
			Else
				' Must be (Any) so send -1
				iBankAccountID = -1
			End If
			
			' Currency: If any list item other than (Any) selected
			If uctCurrency.CurrencyId > 0 Then
				iCurrencyID = uctCurrency.CurrencyId
			Else
				' Must be (Any) so send -1
				iCurrencyID = -1
			End If
			
			' Date From
			If Information.IsDate(txtDateFrom.Text) Then
				dtStartDate = CDate(txtDateFrom.Text)
			Else
				dtStartDate = #12/30/1899#
			End If
			
			' Date To
			If Information.IsDate(txtDateTo.Text) Then
				dtEndDate = CDate(txtDateTo.Text)
			Else
				dtEndDate = #12/30/1899#
			End If
			
			' Total Amount
			Dim dbNumericTemp As Double
			If Double.TryParse(txtTotalAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				cControlTotal = CDbl(txtTotalAmount.Text)
			Else
				cControlTotal = -1
			End If
			
			' Total Items
			'eck040601 replace cInt with cLng
			Dim dbNumericTemp2 As Double
			If Double.TryParse(txtTotalItems.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
				lItemCount = CInt(txtTotalItems.Text)
			Else
				lItemCount = -1
			End If
			
			
			

			m_lReturn = g_oBusiness.SearchDetails(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, iCompanyID:=iCompanyID, vCashlistStatusID:=iCashListStatusID, vCashlistTypeID:=iCashListTypeID, vCashlistRef:=sCashlistRef, vBankAccountID:=iBankAccountID, vCurrencyID:=iCurrencyID, vStartDate:=dtStartDate, vEndDate:=dtEndDate, vControlTotal:=cControlTotal, vItemCount:=lItemCount)
			
			' {* USER DEFINED CODE (End) *}
			
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
        Dim iCashListTypeID, iCashListStatusID, iBankAccountID As Integer
        'Const ACFindImage As String = "FindImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Clear the search details.
			lvwSearchDetails.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vSearchData) Then
				Return result
			End If
			
			' Assign the details to the interface.
			
			For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' ************************************************************
				' Enter your code here to assign the all of the interface
				' details from the search data storage, using the FormatField
				' function for any type conversion.
				'
				' Example:-
				'
				'    ' Assign the details to the first column.
				'    Set oListItem = lvwSearchDetails.ListItems.Add(, , _
				''        Trim$(m_vSearchData(ACName, lRow&)), , ACFindImage)
				'
				'    ' Assign details to other the columns
				'    oListItem.SubItems(1) = Trim$(m_vSearchData(ACCode, lRow&))
				'
				'
				' NOTE: Replace this section with your new code.
				' ************************************************************
				'ACCashListID = 0
				'ACCashListStatusID = 1
				'ACCashListTypeID = 2
				'ACCashListRef = 3
				'ACCompanyID = 4
				'ACBankAccountID = 5
				'ACCurrencyID = 6
				'ACListDate = 7
				'ACControlTotal = 8
				'ACItemCount = 9
				
				' Assign the details to the first column.
				' Column 1 Reference
				'eck050500
				If ValidSource(vSource:=m_vSearchData(ACCompanyID, lRow)) Then
					'eck050500
					

					oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACCashListRef, lRow)).Trim(), "")
					
					' Assign details to other the columns
					' Column 2 Type
					iCashListTypeID = CInt(m_vSearchData(ACCashListTypeID, lRow))
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = uctType.ItemDescription(iCashListTypeID).Trim()
					' Column 3 Status
					iCashListStatusID = CInt(m_vSearchData(ACCashListStatusID, lRow))
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = uctStatus.ItemDescription(iCashListStatusID).Trim()
					' Column 4 Date
					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACListDate, lRow)).Trim()
					' Column 5 Bank
					iBankAccountID = CInt(m_vSearchData(ACBankAccountID, lRow))
					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = uctBankAccount.Description(iBankAccountID).Trim()
					
					' Columns 6, 7 & 8 are currently suppressed (via width = 0)
					'   as totalling has been disabled
					
					' Column 6 Currency
					'iCurrencyID = CInt(m_vSearchData(ACCurrencyID, lRow&))
					'oListItem.SubItems(5) = uctCurrency.CurrencyName(iCurrencyID)
					'oListItem.SubItems(5) = uctCurrency.CurrencyCode(iCurrencyID)
					' Column 7 Total Amount
					'Use CurrencyConvert here to get right number of dec. places, if reinstated
					'oListItem.SubItems(6) = Trim$(m_vSearchData(ACControlTotal, lRow&))
					' Column 8 Total Items
					'oListItem.SubItems(7) = Trim$(m_vSearchData(ACItemCount, lRow&))
					
					' Column 9 Date Sort
					ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CDate(m_vSearchData(ACListDate, lRow)).ToString("yyyyMMdd")
					
					' {* USER DEFINED CODE (End) *}
					
					' Set the tag property with the index of
					' the search data storage.
					oListItem.Tag = CStr(lRow)
				End If
			Next lRow
			
			' Select the first item.
			lvwSearchDetails.Items.Item(0).Selected = True
			
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
	' ***************************************************************** '
	Public Function DataToProperties() As Integer
		
		Dim result As Integer = 0
		Dim lSelectedItem As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the selected item's tag, so we can use this
			' as the index to the search data storage details.

			lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
			
			' Update the property members.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the details from the search
			' data storage to the property members.
			'
			' Example:-
			'
			' m_sName$ = Trim$(m_vSearchData(ACName, lSelectedItem&))
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			m_lCashListID = CInt(m_vSearchData(ACCashListID, lSelectedItem))
			m_lCashListTypeID = CInt(m_vSearchData(ACCashListTypeID, lSelectedItem))
			'eck140600
			m_iCashListCompanyID = CInt(m_vSearchData(ACCompanyID, lSelectedItem))
			
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
	
	'' ***************************************************************** '
	'' Name: DisplayLookupDetails
	''
	'' Description: Displays all of the lookup details using the lookup
	''              values/details.
	''
	'' ***************************************************************** '
	'Public Function DisplayLookupDetails() As Long
	'
	'    On Error GoTo Err_DisplayLookupDetails
	'
	'    DisplayLookupDetails = PMTrue
	'
	'    ' Get the lookup values.
	'
	'    m_lReturn& = GetLookupValues()
	'
	'    ' Check for errors.
	'    If (m_lReturn& <> PMTrue) Then
	'        DisplayLookupDetails = PMFalse
	'        Exit Function
	'    End If
	'
	'    ' Get all of the lookup details.
	'
	'    ' {* USER DEFINED CODE (Begin) *}
	'
	'    ' ************************************************************
	'    ' Enter your code here to retreive all of the lookup
	'    ' descriptions for a given lookup type.
	'    ' The GetLookupDetails function will allow you to do this.
	'    '
	'    ' Example:-
	'    '
	'    '    m_lReturn& = GetLookupDetails( _
	''    '        sLookupTable:=PMLookupCodeName, _
	''    '        ctlLookup:=cmbCodeName)
	'    '
	'    '    ' Check for errors.
	'    '    If (m_lReturn& <> PMTrue) Then
	'    '        DisplayLookupDetails = PMFalse
	'    '        Exit Function
	'    '    End If
	'    '
	'    ' NOTE: Replace this section with your new code.
	'    ' ************************************************************
	'
	'    ' {* USER DEFINED CODE (End) *}
	'
	'    Exit Function
	'
	'Err_DisplayLookupDetails:
	'
	'    ' Error Section
	'
	'    DisplayLookupDetails = PMError
	'
	'    ' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to display the lookup details", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="DisplayLookupDetails", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'    Exit Function
	'
	'End Function
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	'eck090500
	' ********************************************************************************* '
	' Name: Private Function                                                            '
	'                                                                                   '
	' Description: Checks that the transaction is for one of the branches being paid    '
	'                                                                                   '
	' ********************************************************************************* '
	Private Function ValidSource(ByVal vSource As Object) As Boolean
		Dim result As Boolean = False
		If Not Information.IsArray(m_vSourceArray) Then
			Return True
		End If
		For i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

			If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
				result = True
			End If
		Next i
		Return result
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
			
			
			' Update the interface details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the details from the
			' property members to the interface.
			'
			' Example:-
			'
			' txtName.Text = Trim$(m_sName$)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			Select Case (m_lNavigate)
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = True
					
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = False
					
				Case Else
					cmdNavigate.Visible = False
			End Select
			
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
			
			' Set the column widths for the search list.
			lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(720))
			lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(720))
			lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(720))
			lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(720))
			' Bank
			lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(940))
			'
			'Columns 6,7 & 8 suppressed as control totalling not initially
			'  required. Current use has CashLists containing single items.
			' Currency
			lvwSearchDetails.Columns.Item(5).Width = CInt(0) ' was 580
			lvwSearchDetails.Columns.Item(6).Width = CInt(0) ' was 720
			' Items
			lvwSearchDetails.Columns.Item(7).Width = CInt(0) ' was 270
			lvwSearchDetails.Columns.Item(8).Width = CInt(0) '
			
			' {* USER DEFINED CODE (Begin) *}
			
			' CF080499
			m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
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
			

			sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Display the message.
			iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			
			' Check message result.
			If iMsgResult = System.Windows.Forms.DialogResult.No Then
				' Don't continue with the clear.
				Return result
			End If
			
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
			txtReference.Text = ""
			txtDateFrom.Text = ""
			txtDateTo.Text = ""
			txtTotalAmount.Text = ""
			txtTotalItems.Text = ""
			
			uctType.ItemId = 0
			uctStatus.ItemId = 0
			uctCurrency.CurrencyId = 0
			uctBankAccount.Id = 0
			
			' Set focus to the search details.
			txtReference.Focus()
			
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 1)
			
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
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtReference
			m_ctlTabFirstLast(ACControlEnd, 0) = txtDateTo
			m_ctlTabFirstLast(ACControlStart, 1) = uctBankAccount
			m_ctlTabFirstLast(ACControlEnd, 1) = txtTotalItems
			
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
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Dim sAnyText As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			' {* USER DEFINED CODE (Begin) *}
			
			'    lvwSearchDetails.ColumnHeaders(1).Text = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACListTitle1, _
			''        iDataType:=PMResString)
			'
			'    lvwSearchDetails.ColumnHeaders(2).Text = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACListTitle2, _
			''        iDataType:=PMResString)
			
			' Add (Any)entries to user controls Combos

			sAnyText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			sAnyText = "Any"
			
			uctType.FirstItem = sAnyText
			uctStatus.FirstItem = sAnyText
			uctCurrency.FirstItem = sAnyText
			uctBankAccount.FirstItem = sAnyText
			
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
			
			cmdOK.Enabled = Not bDisable
			cmdEdit.Enabled = Not bDisable
			
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
	' Name: CallCashListForm
	'
	' Description: Call Cash List Form in Add or Edit mode
	'
	' ***************************************************************** '
	Private Function CallCashListForm(ByRef iTask As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lSelectedIndex As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_m_oCashList As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oCashList, sClassName:="iACTCashList.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			m_oCashList = temp_m_oCashList
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set Form to Add or Edit
			m_lReturn = CType(m_oCashList.SetProcessModes(vTask:=iTask), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If iTask = gPMConstants.PMEComponentAction.PMEdit Then
				' Get selected list view item Cash List ID
				m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Pass ID to Cash List

				m_oCashList.CashlistID = m_lCashListID
			End If
			

			m_lReturn = m_oCashList.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
			End If
			
			' If form not OK ie Cancelled return status

			If m_oCashList.Status <> gPMConstants.PMEReturnCode.PMOK Then

				result = m_oCashList.Status
			End If
			
			If iTask = gPMConstants.PMEComponentAction.PMAdd Then
				' Return newly created Cash List ID

				m_lCashListID = m_oCashList.CashlistID
				
			Else
				' Save the currently selected item list index
				lSelectedIndex = lvwSearchDetails.FocusedItem.Index + 1
				
				' Refresh any changes to the List Details from the database
				' This is a bit of a fudge.
				' Could update the list array directly from returned CashList properties
				m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
				
				' Reset to currently selected item
				lvwSearchDetails.Items.Item(lSelectedIndex - 1).Selected = True
				
			End If
			

            m_oCashList.Dispose()

            m_oCashList = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Cash List Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CallCashListForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: CallCashListItemForm
	'
	' Description: Call Cash List Item Form in Add or Edit mode
	'
	' ***************************************************************** '
	Private Function CallCashListItemForm(ByRef iTask As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_m_oCashListItem As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oCashListItem, sClassName:="iACTCashListItem.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			m_oCashListItem = temp_m_oCashListItem
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set Form to Add or Edit
			m_lReturn = CType(m_oCashListItem.SetProcessModes(vTask:=iTask), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Pass Cash List ID

			m_oCashListItem.CashlistID = m_lCashListID
			

			m_lReturn = m_oCashListItem.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
			End If
			

            m_oCashListItem.Dispose()

            m_oCashListItem = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Cash List Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CallCashListItemForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'' ***************************************************************** '
	'' Name: GetLookupValues
	''
	'' Description: Gets all of the lookup values, ready to be used by
	''              the lookup function.
	''
	'' ***************************************************************** '
	'Private Function GetLookupValues() As Long
	'
	'    On Error GoTo Err_GetLookupValues
	'
	'    GetLookupValues = PMTrue
	'
	'    ' Gets all of the lookup values.
	'
	'    ' Get all of the lookup values.
	'    m_lReturn& = m_oBusiness.GetLookupValues( _
	''        iLookupType:=PMLookupAll, _
	''        vTableArray:=m_vLookupValues, _
	''        iLanguageID:=g_iLanguageID%, _
	''        vResultArray:=m_vLookupDetails)
	'
	'    ' Check for errors.
	'    If (m_lReturn& <> PMTrue) Then
	'        GetLookupValues = PMFalse
	'
	'        ' Log Error.
	'        LogMessagePopup _
	''            iType:=PMLogError, _
	''            sMsg:="Failed to get the lookup values from the business object", _
	''            vApp:=ACApp, _
	''            vClass:=ACClass, _
	''            vMethod:="GetLookupValues"
	'
	'        Exit Function
	'    End If
	'
	'    Exit Function
	'
	'Err_GetLookupValues:
	'
	'    ' Error Section.
	'
	'    GetLookupValues = PMError
	'
	'    ' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to get all of the lookup values", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="GetLookupValues", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'    Exit Function
	'
	'End Function
	'
	'' ***************************************************************** '
	'' Name: GetLookupDetails
	''
	'' Description: Gets all of the lookup details using the lookup
	''              values, then assigns them to the control passed.
	''
	'' ***************************************************************** '
	'Private Function GetLookupDetails( _
	''    sLookupTable As String, _
	''    ctlLookup As Control) As Long
	'
	'Dim lRow As Long
	'Dim lCntr As Long
	'Dim bFoundMatch As Boolean
	'
	'' Lookup value contants.
	'Const ACValueTableName = 0
	'Const ACValueID = 1
	'Const ACValueStartPos = 2
	'Const ACValueNumber = 3
	'
	'' Lookup detail contants.
	'Const ACDetailKey = 0
	'Const ACDetailDesc = 1
	'
	'    On Error GoTo Err_GetLookupDetails
	'
	'    GetLookupDetails = PMTrue
	'
	'    ' Get the lookup values.
	'
	'    bFoundMatch = False
	'
	'    For lRow& = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
	'        ' Check for a match of the table name.
	'        If (Trim$(m_vLookupValues(ACValueTableName, lRow&)) = _
	''        Trim$(sLookupTable$)) Then
	'            ' Found a match
	'            bFoundMatch = True
	'            Exit For
	'        End If
	'    Next lRow&
	'
	'    ' Check if there has been a table match.
	'    If (bFoundMatch = False) Then
	'        GetLookupDetails = PMFalse
	'
	'        ' Log Error.
	'        LogMessage _
	''            iType:=PMLogOnError, _
	''            sMsg:="Failed to get details for the table, " & sLookupTable$, _
	''            vApp:=ACApp, _
	''            vClass:=ACClass, _
	''            vMethod:="GetLookupDetails"
	'
	'        Exit Function
	'    End If
	'
	'    ' Using the lookup values, populate the control with
	'    ' the details from the lookup details array.
	'
	'    For lCntr& = m_vLookupValues(ACValueStartPos, lRow&) To _
	''    (m_vLookupValues(ACValueStartPos, lRow&) + m_vLookupValues(ACValueNumber, lRow&)) - 1
	'        ' Add the details to the control.
	'        ctlLookup.AddItem m_vLookupDetails(ACDetailDesc, lCntr&)
	'        ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr&)
	'
	'        ' Check if this is the selected index.
	'        If (m_vLookupValues(ACValueID, lRow&) = _
	''        m_vLookupDetails(ACDetailKey, lCntr&)) Then
	'            ctlLookup.ListIndex = ctlLookup.NewIndex
	'        End If
	'    Next lCntr&
	'
	'    ' Check if the selected index is blank. If so,
	'    ' we set the controls index to zero.
	'    If (m_vLookupValues(ACValueID, lRow&) = "") Then
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

				sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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

				sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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
	' PRIVATE Methods (End)
	
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		' Fire up the help screen
        'Developer Guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

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
			m_oGeneral = New iACTFindCashList.General()
			
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
			
			'    ' Set the status for the business object.
			'BB    m_lReturn& = g_oBusiness.SetStatus( _
			''        sProcessStatus:=m_sProcessStatus$, _
			''        sMapStatus:=m_sMapStatus$, _
			''        sStepStatus:=m_sStepStatus$)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
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
			
			' Check if the search contains more or equal
			' to the miniumum search length.
			
			' {* USER DEFINED CODE (Begin) *}
			
			If txtReference.Text.Trim().Length < ACMinSearchLength Then
				' Because of the search length, we can't
				' continue with the search.
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
			
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
		
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				'BB        If (.Tab < cmdNext.Count) Then
				'            cmdNext(.Tab).Default = True
				'        Else
				'            cmdOK.Default = True
				'        End If
				
				VB6.SetDefault(cmdOK, True)
				
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
	
	Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
		
		' Click event of the New Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Call Cash List header details form to Add
			m_lReturn = CType(CallCashListForm(iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
			
			' If Cash List Form Error or Cancelled don't continue
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = m_lReturn
				Exit Sub
			End If
			
			' Call Cash List Items details object to Add and Edit items
			m_lReturn = CType(CallCashListItemForm(iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = m_lReturn
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		' Click event of the Edit Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Call Cash List header details object to Edit
			m_lReturn = CType(CallCashListForm(iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
					m_lErrorNumber = m_lReturn
				End If
				Exit Sub
			End If
			
			' Call Cash List Items details object to Add or Edit items
			m_lReturn = CType(CallCashListItemForm(iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
					m_lErrorNumber = m_lReturn
				End If
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		
		' LostFocus Event for the search details
		
		Try 
			
			' Set the default button.
			VB6.SetDefault(cmdFindNow, True)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
				
				'Change the sort key if column is date
				If ColumnHeader.Index + 1 - 1 = 3 Then
					ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
					If ListViewHelper.GetSortKeyProperty(lvwSearchDetails) <> 8 Then
						ListViewHelper.SetSortKeyProperty(lvwSearchDetails, 8)
						ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
					Else
						ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
					End If
					ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
					
					' If current sort column header is
					' pressed.
				ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails)) Then 
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
					ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
					ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	Private Sub txtReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Enter
		iPMFunc.SelectText(txtReference)
	End Sub
	
	Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter
		
		' Check date.
		iPMValidate.CheckDateGotFocus(txtDateFrom)
		
		' Hightlight any text.
		iPMFunc.SelectText(txtDateFrom)
		
	End Sub
	
	Private Sub txtDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Leave
		
		' Check date.
		iPMValidate.CheckDateLostFocus(txtDateFrom)
		
	End Sub
	Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
		
		' Check date.
		iPMValidate.CheckDateGotFocus(txtDateTo)
		
		' Hightlight any text.
		iPMFunc.SelectText(txtDateTo)
		
	End Sub
	
	Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave
		
		' Check date.
		iPMValidate.CheckDateLostFocus(txtDateTo)
		
	End Sub
	Private Sub txtTotalAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalAmount.Enter
		iPMFunc.SelectText(txtTotalAmount)
	End Sub
	Private Sub txtTotalAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalAmount.Leave
		'BB Temporary fix until we have proper validation routines
		txtTotalAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtTotalAmount.Text)
	End Sub
	Private Sub txtTotalItems_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalItems.Enter
		iPMFunc.SelectText(txtTotalItems)
	End Sub
	Private Sub txtTotalItems_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalItems.Leave
		'BB Temporary fix until we have proper validation routines
		txtTotalItems.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=txtTotalItems.Text)
	End Sub
	
	' PRIVATE Events (End)
End Class
